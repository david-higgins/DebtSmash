using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DebtSmash.Presenter
{
    public class UpdateBindingList<T> : BindingList<T>, IUpdateList<T>
    {
        public virtual void Update(int index)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }
    }
}
