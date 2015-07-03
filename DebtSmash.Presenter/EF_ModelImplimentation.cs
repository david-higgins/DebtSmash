using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.Threading;

namespace DebtSmash.Presenter
{
    class DebtContext : DbContext
    {
        public DebtContext(String conn) : base(conn) { }
        public DbSet<Debt> debts { get; set; }
    }
    class EFDebtUpdateList : UpdateBindingList<Debt>
    {
        readonly DebtContext ctx;
        bool loadedup = false;
        public EFDebtUpdateList(DebtContext ctx)
        {
            this.ctx = ctx;
            IList<Debt> debts = new List<Debt>(ctx.debts);
            foreach (var c in debts) Add(c);
            loadedup = true;
        }
        Object rlock = new object();
        Debt removed;
        protected override void RemoveItem(int index)
        {
            Monitor.Enter(rlock);
            removed = this[index];
            base.RemoveItem(index);
        }
        protected override void OnListChanged(System.ComponentModel.ListChangedEventArgs e)
        {
            if (!loadedup) return;
            switch (e.ListChangedType)
            {
                case System.ComponentModel.ListChangedType.ItemAdded:
                    ctx.debts.Add(this[e.NewIndex]);
                    break;
                case System.ComponentModel.ListChangedType.ItemChanged:
                    ctx.Entry(this[e.NewIndex]).CurrentValues.SetValues(this[e.NewIndex]);
                    break;
                case System.ComponentModel.ListChangedType.ItemDeleted:
                    ctx.debts.Remove(removed);
                    removed = null;
                    Monitor.Exit(rlock);
                    break;
            }
            ctx.SaveChanges();
            base.OnListChanged(e);
        }
    }
    class EF_ModelImplimentation : IModel
    {
        readonly EFDebtUpdateList edl;
        readonly DebtContext debt_context;
        public EF_ModelImplimentation(String connString)
        {
            debt_context = new DebtContext(connString);
            edl = new EFDebtUpdateList(debt_context);
        }
        public IUpdateList<Debt> models { get { return edl; } }
    }
}
