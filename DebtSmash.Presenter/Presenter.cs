using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text;
using MySql.Data.Entity;
using System.Threading;


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
        const string csf = "connectionStrings.dat";
        static IView mView;
        static IModel mModel;
        public static void Present(IView view)
        {
            // fast bit do synchronous...
            mView = view;
            String cs;
            List<String> css = new List<string>();
            if(File.Exists(csf)) css.AddRange(File.ReadAllLines(csf));
            cs = view.GetConnectionString(css.ToArray());
            if (!css.Contains(cs)) css.Add(cs);
            File.WriteAllLines(csf, css.ToArray());
            view.loading = true;
            ThreadPool.QueueUserWorkItem(InitData, cs);
        }

        static void InitData(Object cs)
        {
            mModel = new EF_ModelImplimentation(cs as String);
            mView.HeresTheDebt(mModel.models);
            mView.loading = false;
        }
    }
}
