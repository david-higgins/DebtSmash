using System;
using System.Collections.Generic;
using System.Text;

namespace DebtSmash.Presenter
{
    interface IView
    {
        void HeresTheDebt(IUpdateList<Debt> debt);
    }
}
