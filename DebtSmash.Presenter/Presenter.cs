using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text;
using MySql.Data.Entity;

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

            mView = view;
            String cs;
            List<String> css = new List<string>();
            if(File.Exists(csf)) css.AddRange(File.ReadAllLines(csf));
            cs = view.GetConnectionString(css.ToArray());
            if (!css.Contains(cs)) css.Add(cs);
            File.WriteAllLines(csf, css.ToArray());
            mModel = new EF_ModelImplimentation(cs);
            view.HeresTheDebt(mModel.models);
        }
    }
}
