using eccFramework.SharedLib.Core.Attributes;
using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Lego.Brick;
using FTSolutions.IEC61034.BizLogic.ViewModel.Utility;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using System;
using System.Text.RegularExpressions;

namespace FTSolutions.IEC61034.Runner.Popup.Utility
{
    [TargetViewModel(typeof(vmPopup_New_Registration))]
    public partial class Popup_New_Registration : BaseIEC61034Popup
    {
        public event Action<bool, TypeRegistration> EndEventHandler;

        public Popup_New_Registration()
        {
            InitializeComponent();

            this.Loaded += Popup_New_Registration_Loaded;

            this.RegistrationInfo = new TypeRegistration();
        }

        public Popup_New_Registration(TypeRegistration regInfo) : this()
        {
            this.RegistrationInfo = regInfo;
        }



        //###################################################################
        //  Property
        //###################################################################

        TypeRegistration RegistrationInfo { get; set; }



        //###################################################################
        //  EventHandler
        //###################################################################

        private void Popup_New_Registration_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            vmPopup_New_Registration vm = this.DataContext as vmPopup_New_Registration;

            if (vm != null && this.RegistrationInfo.SEQ != null && this.RegistrationInfo.SEQ.Trim().Length > 0)
            {
                vm.SetRegistrationInfo(this.RegistrationInfo);
            }
        }

        private void Apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EndEventHandler != null)
            {
                vmPopup_New_Registration vm = this.DataContext as vmPopup_New_Registration;

                if (vm != null)
                {
                    if (vm.CheckValidation())
                    {
                        vm.SaveRegstrationInfo();

                        this.EndEventHandler(true, vm.RegistrationInfo);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            this.Close();
        }

        private void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EndEventHandler != null)
            {
                this.EndEventHandler(false, null);
            }

            this.Close();
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EndEventHandler != null)
            {
                vmPopup_New_Registration vm = this.DataContext as vmPopup_New_Registration;

                if (vm.TestInfoDataCollection != null && vm.TestInfoDataCollection.Count > 0)
                {
                    this.ShowMessageKeyBox(MessageButtonType.OK, "msg_warning", "msg_delete_warning");
                    return;
                }

                if (vm != null)
                {
                    if (this.ShowMessageKeyBox(MessageButtonType.YesNo, "msg_confirm", "popup_registration_delete_confirm"))
                    {
                        vm.DeleteRegstrationInfo();

                        this.EndEventHandler(true, null);
                    }
                }
            }

            this.Close();
        }

        private void Delete_TestInfo(object sender, System.Windows.RoutedEventArgs e)
        {
            eccButton btn = e.OriginalSource as eccButton;

            if (btn != null)
            {
                TestInfoData data = btn.DataContext as TestInfoData;
                vmPopup_New_Registration vm = this.DataContext as vmPopup_New_Registration;

                if (data != null && vm != null)
                {
                    vm.DeleteTestInfo(data.REG_NO, data.SEQ);

                    this.EndEventHandler(true, null);
                }
            }
        }

        private void setCable_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            eccTextBox textBox = sender as eccTextBox;
            string futureText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            // 정규식: 최대 소수점 첫째 자리까지 허용 (예: 123.4)
            Regex regex = new Regex(@"^\d*([.]\d?)?$");
            e.Handled = !regex.IsMatch(futureText);
        }
    }
}
