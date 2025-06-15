using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Runner.Popup.Utility;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_ChannelSetting))]
    public partial class Popup_ChannelSetting : BaseIEC61034Popup
    {
        public Popup_ChannelSetting()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Popup_New popup = new Popup_New();
            popup.Owner = this.Owner;
            popup.EndEventHandler += Popup_EndEventHandler;

            popup.ShowDialog();
        }

        private void Popup_EndEventHandler(bool arg1, string arg2)
        {
            if (arg1)
            {
                ((vmPopup_ChannelSetting)this.DataContext).ExecuteNewCommand(arg2);
            }
        }
    }
}
