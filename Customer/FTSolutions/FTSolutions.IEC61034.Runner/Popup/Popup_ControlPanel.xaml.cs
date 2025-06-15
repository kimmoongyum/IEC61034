using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;
using System.Windows;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_ControlPanel))]
    public partial class Popup_ControlPanel : BaseIEC61034Popup
    {
        public Popup_ControlPanel()
        {
            InitializeComponent();
        }
    }
}
