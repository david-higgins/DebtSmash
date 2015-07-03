using System;
using System.Collections.Generic;
using System.Text;

namespace DebtSmash.Presenter
{
    public interface IUpdateList<T> : IList<T>
    {
        // this one changed.
        void Update(int index);
    }

    public interface IModel
    {
        // could impliment with bindinglist etc.
        IUpdateList<Debt> models { get; }
    }
}
