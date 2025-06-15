using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_QualificationSummary))]
    public partial class Popup_QualificationSummary : BaseIEC61034Popup
    {
        public Popup_QualificationSummary()
        {
            InitializeComponent();
        }
    }
}
