using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.Threading;

namespace DebtSmash.Presenter
{
    class DebtContext : DbContext
    {
        public DbSet<Debt> debts { get; set; }
    }
    class EFDebtUpdateList : UpdateBindingList<Debt>
    {
        readonly DebtContext ctx;
        bool loadedup = false;
        public EFDebtUpdateList()
        {
            ctx = new DebtContext();
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
        public EF_ModelImplimentation()
        {
            edl = new EFDebtUpdateList();
        }
        public IUpdateList<Debt> models { get { return edl; } }
    }
}
