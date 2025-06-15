using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_NDFilter))]
    public partial class Popup_NDFilter : BaseIEC61034Popup
    {
        public Popup_NDFilter()
        {
            InitializeComponent();
        }
    }
}
