using System;
using System.Collections.Generic;
using System.Text;

namespace DebtSmash.Presenter
{
    class DebtSmashPresenter
    {
        static IView mView;
        static IModel mModel = new MockModelImplimentation();
        public static void Present(IView view)
        {
            mView = view;
            view.HeresTheDebt(mModel.models);
        }
    }
}
