using Geta.EPi.Find.Extensions.Sample.Models.ViewModels;

namespace Geta.EPi.Find.Extensions.Sample.Business
{
    /// <summary>
    /// Defines a method which may be invoked by PageContextActionFilter allowing controllers
    /// to modify common layout properties of the view model.
    /// </summary>
    interface IModifyLayout
    {
        void ModifyLayout(LayoutModel layoutModel);
    }
}
