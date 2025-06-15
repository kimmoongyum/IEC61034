using eccFramework.SharedLib.Core.Attributes;
using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Lego.Brick;
using FTSolutions.IEC61034.BizLogic.ViewModel.Utility;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using System;
using System.Linq;

namespace FTSolutions.IEC61034.Runner.Popup.Utility
{
    [TargetViewModel(typeof(vmPopup_New_QualificationRegistration))]
    public partial class Popup_New_QualificationRegistration : BaseIEC61034Popup
    {
        public event Action<bool, TypeQualificationRegistration> EndEventHandler;

        public Popup_New_QualificationRegistration()
        {
            InitializeComponent();

            this.Loaded += Popup_New_QualificationRegistration_Loaded;

            this.RegistrationInfo = new TypeQualificationRegistration();
        }

        public Popup_New_QualificationRegistration(TypeQualificationRegistration regInfo) : this()
        {
            this.RegistrationInfo = regInfo;
        }



        //###################################################################
        //  Property
        //###################################################################

        TypeQualificationRegistration RegistrationInfo { get; set; }



        //###################################################################
        //  EventHandler
        //###################################################################

        private void Popup_New_QualificationRegistration_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            vmPopup_New_QualificationRegistration vm = this.DataContext as vmPopup_New_QualificationRegistration;

            if (vm != null && this.RegistrationInfo.SEQ != null && this.RegistrationInfo.SEQ.Trim().Length > 0)
            {
                vm.SetRegistrationInfo(this.RegistrationInfo);
            }
        }

        private void Apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EndEventHandler != null)
            {
                vmPopup_New_QualificationRegistration vm = this.DataContext as vmPopup_New_QualificationRegistration;

                if (vm != null)
                {
                    if (vm.CheckValidation())
                    {
                        vm.SaveQualificationRegstrationInfo();

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
                vmPopup_New_QualificationRegistration vm = this.DataContext as vmPopup_New_QualificationRegistration;

                if (vm.QualificationInfoDataCollection != null && vm.QualificationInfoDataCollection.Count > 0)
                {
                    //this.ShowMessageKeyBox(MessageButtonType.OK, "msg_warning", "msg_delete_warning");
                    //return;
                    foreach(QualificationInfoData data in vm.QualificationInfoDataCollection.ToList())
                    {
                        if(data != null)
                        {
                            vm.DeleteQualificationInfo2(data.REG_NO, data.SEQ);
                        }
                    }
                }

                if (vm != null)
                {
                    if (this.ShowMessageKeyBox(MessageButtonType.YesNo, "msg_confirm", "popup_registration_delete_confirm"))
                    {
                        vm.DeleteQualificationRegstrationInfo();

                        this.EndEventHandler(true, null);
                    }
                }
            }

            this.Close();
        }

        private void Delete_QualificationInfo(object sender, System.Windows.RoutedEventArgs e)
        {
            eccButton btn = e.OriginalSource as eccButton;

            if (btn != null)
            {
                QualificationInfoData data = btn.DataContext as QualificationInfoData;
                vmPopup_New_QualificationRegistration vm = this.DataContext as vmPopup_New_QualificationRegistration;

                if (data != null && vm != null)
                {
                    vm.DeleteQualificationInfo(data.REG_NO, data.SEQ);

                    this.EndEventHandler(true, null);
                }
            }
        }
    }
}
