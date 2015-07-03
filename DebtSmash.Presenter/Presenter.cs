using System;
using System.Collections.Generic;
using System.Text;

namespace DebtSmash.Presenter
{
    class DebtSmashPresenter
    {
        static IView mView;
        static IModel mModel;
        public static void Present(IView view)
        {
            mModel = new EF_ModelImplimentation();
            mView = view;
            view.HeresTheDebt(mModel.models);
        }
    }
}
