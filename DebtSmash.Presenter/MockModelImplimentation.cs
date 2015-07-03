using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DebtSmash.Presenter
{
    
    class mdl : UpdateBindingList<Debt>
    {
        public void Add(String name)
        {
            Add(new Debt() { name = name } );
        }
    }

    class MockModelImplimentation : IModel
    {
        UpdateBindingList<Debt> backing = new mdl { "deave is awesome", "but the typos", "ioh the utpys" };
        public IUpdateList<Debt> models { get { return backing; } }
    }
}
