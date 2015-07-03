using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text;

namespace DebtSmash.Presenter
{
    class ConectionString
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public String name { get; set; }
    }
    class CSDB : DbContext
    {
        public DbSet<ConectionString> cstrings { get; set; }
        public List<String> cstring_list
        {
            get
            {
                List<String> ret = new List<string>();
                List<ConectionString> css = new List<ConectionString>(cstrings);
                foreach (var cs in css)
                    ret.Add(cs.name);
                return ret;
            }
        }
    }
    class DebtSmashPresenter
    {
        static IView mView;
        static IModel mModel;
        public static void Present(IView view)
        {
            mView = view;
            String cs;
            using (var db = new CSDB())
            {
                var exist = new List<String>(db.cstring_list);
                cs = view.GetConnectionString(exist.ToArray());
                if (!exist.Contains(cs))
                {
                    db.cstrings.Add(new ConectionString() { name = cs });
                    db.SaveChanges();
                }
            }
            mModel = new EF_ModelImplimentation(cs);
            view.HeresTheDebt(mModel.models);
        }
    }
}
