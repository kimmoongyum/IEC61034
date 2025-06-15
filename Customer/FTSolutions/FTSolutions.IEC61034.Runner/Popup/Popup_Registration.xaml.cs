using eccFramework.SharedLib.Core.Attributes;
using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Runner.Popup.Utility;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FTSolutions.IEC61034.Runner.Popup
{
    [TargetViewModel(typeof(vmPopup_Registration))]
    public partial class Popup_Registration : BaseIEC61034Popup
    {
        public Popup_Registration()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Popup_New_Registration popup = new Popup_New_Registration();
            popup.Owner = this.Owner;
            popup.EndEventHandler += Popup_EndEventHandler;

            popup.ShowDialog();
        }

        private void lstRegistration_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ((System.Windows.Controls.ListBox)e.Source).SelectedItem;

            UIElement elem = (UIElement)lstRegistration.InputHitTest(e.GetPosition(lstRegistration));

            while (elem != lstRegistration)
            {
                if (elem is ListBoxItem)
                {
                    TypeRegistration selectedItem = ((ListBoxItem)elem).Content as TypeRegistration;

                    if (selectedItem != null)
                    {
                        Popup_New_Registration popup = new Popup_New_Registration(selectedItem);
                        popup.Owner = this.Owner;
                        popup.EndEventHandler += Popup_EndEventHandler;

                        popup.ShowDialog();
                    }

                    return;
                }

                elem = (UIElement)VisualTreeHelper.GetParent(elem);
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            vmPopup_Registration vm = this.DataContext as vmPopup_Registration;

            if (vm != null && vm.CurrentRegistrationInfo != null)
            {
                vm.IsPopupOwner = true;

                this.Close();
                this.SendSign(IEC61034Const.MENU_REGISTER_MANAGER, IEC61034Const.SIGN_KEY_TEST, vm.CurrentRegistrationInfo);
            }
        }


        private void Popup_EndEventHandler(bool needRefresh, TypeRegistration regInfo)
        {
            if (needRefresh)
            {
                ((vmPopup_Registration)this.DataContext).NewPopup_EndEventHandler(regInfo);
            }
        }
    }
}
