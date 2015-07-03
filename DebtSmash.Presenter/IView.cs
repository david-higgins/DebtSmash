using System;
using System.Collections.Generic;
using System.Text;

namespace DebtSmash.Presenter
{
    interface IView
    {
        String GetConnectionString(String[] others);
        void HeresTheDebt(IUpdateList<Debt> debt);
    }
}
