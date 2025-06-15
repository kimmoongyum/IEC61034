using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;
using System.Windows.Media.Animation;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_Test))]
    public partial class Popup_Test : BaseIEC61034Popup
    {
        public Popup_Test()
        {
            InitializeComponent();
        }
    }
}
