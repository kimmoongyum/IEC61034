using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_QualificationProperties))]
    public partial class Popup_QualificationProperties : BaseIEC61034Popup
    {
        public Popup_QualificationProperties()
        {
            InitializeComponent();
        }
    }
}
