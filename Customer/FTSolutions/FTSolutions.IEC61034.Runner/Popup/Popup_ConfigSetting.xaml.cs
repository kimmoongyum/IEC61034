using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.BizLogic.ViewModel;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_ConfigSetting))]
    public partial class Popup_ConfigSetting : BaseIEC61034Popup
    {
        public Popup_ConfigSetting()
        {
            InitializeComponent();
        }
    }
}
