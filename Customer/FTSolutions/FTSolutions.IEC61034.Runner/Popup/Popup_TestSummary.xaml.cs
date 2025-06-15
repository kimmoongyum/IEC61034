using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_TestSummary))]
    public partial class Popup_TestSummary : BaseIEC61034Popup
    {
        public Popup_TestSummary()
        {
            InitializeComponent();
        }
    }
}
