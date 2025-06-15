using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;
using System.Windows.Media.Animation;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_Qualification))]
    public partial class Popup_Qualification : BaseIEC61034Popup
    {
        public Popup_Qualification()
        {
            InitializeComponent();
        }
    }
}
