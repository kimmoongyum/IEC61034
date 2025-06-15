using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Helper;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Result;
using FTSolutions.IEC61034.Common.Setting;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_QualificationProperties : BaseIEC61034ViewModel
    {
        public vmPopup_QualificationProperties()
        {
            this.QualificationItem_Properties = new QualificationProperty();
            this.RegistrationInfo = new TypeQualificationRegistration();
        }




        //###################################################################
        //  Command
        //###################################################################



        //###################################################################
        //  Property
        //###################################################################

        private QualificationProperty _qualificationItem_Properties;
        public QualificationProperty QualificationItem_Properties
        {
            get { return _qualificationItem_Properties; }
            set
            {
                if (this._qualificationItem_Properties != value)
                {
                    this._qualificationItem_Properties = value;
                    this.RaisePropertyChanged(nameof(QualificationItem_Properties));
                }
            }
        }

        private TypeQualificationRegistration _registrationInfo;
        public TypeQualificationRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                _registrationInfo = value;
                this.RaisePropertyChanged(nameof(RegistrationInfo));
            }
        }

        #region Toluene Radiobutton.
        private bool _isToluene4;
        public bool IsToluene4
        {
            get { return _isToluene4; }
            set
            {
                if (this._isToluene4 != value)
                {
                    this._isToluene4 = value;
                    this.RaisePropertyChanged(nameof(IsToluene4));
                }

                if (value)
                {
                    this.RegistrationInfo.TOLUENE_CONTENT = IEC61034Const.KEY_TOLUENE_4;
                }
            }
        }

        private bool _isToluene10;
        public bool IsToluene10
        {
            get { return _isToluene10; }
            set
            {
                if (this._isToluene10 != value)
                {
                    this._isToluene10 = value;
                    this.RaisePropertyChanged(nameof(IsToluene10));
                }

                if (value)
                {
                    this.RegistrationInfo.TOLUENE_CONTENT = IEC61034Const.KEY_TOLUENE_10;
                }
            }
        }
        #endregion

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            if (this.CallerMenu == MenuKind.QUALIFICATION_REGISTRATION)
            {
                //this.QualificationItem_Properties.Number = this.GetNumberOfTest(this.RegistrationInfo.NUMBER).ToString();

                //if (this.QualificationItem_Properties.TotalNumber == null || this.QualificationItem_Properties.TotalNumber.Trim().Length < 1)
                //{
                //    this.QualificationItem_Properties.TotalNumber = "1";
                //}
            }
            else if(this.CallerMenu == MenuKind.QUALIFICATION_BLANK_TEST)
            {
                this.RegistrationInfo.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo);
                this.QualificationItem_Properties.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties);
            }
            else
            {
                this.QualificationItem_Properties.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties);

                if (this.CallerMenu == MenuKind.TEST_SUMMARY)
                {
                    //this.QualificationItem_Properties.Number = this.GetNumberOfTest(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.Number).ToString();
                }
            }
           
            if(this.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_4)
            {
                this.IsToluene4 = true;
                this.IsToluene10 = false;
            }
            else if(this.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_10)
            {
                this.IsToluene4 = false;
                this.IsToluene10 = true;
            }

            this.QualificationItem_Properties.SetNow();
            this.QualificationItem_Properties.SetQualificationInfo(this.RegistrationInfo);
        }

        public override void Dispose()
        {
            base.Dispose();
        }


        protected override void ExecuteNextClick(object obj)
        {
            base.ExecuteNextClick(obj);

            this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo.CopyValueFrom(this.RegistrationInfo);
            this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties.CopyValueFrom(this.QualificationItem_Properties);

            string targetMenu = IEC61034Const.MENU_BLANK_TEST_KEY;

            BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.QUALIFICATION_REGISTRATION;
            };

            this.CloseWindow();

            popup.ShowDialog();
        }

        protected override bool ExecuteCancelClick(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_popup", "msg_quit_menu");

            if (result)
            {
                if (this.SESSION_MANAGER.IEC61034_DataSetTest != null)
                {
                    this.SESSION_MANAGER.IEC61034_DataSetTest.ClearAll();
                }

                this.CloseWindow();
            }

            return result;
        }

        public override bool IsValid()
        {
            return this.QualificationItem_Properties.IsValid();
        }



        //###################################################################
        //  EventHandler
        //###################################################################



        //###################################################################
        //  Private
        //###################################################################

        private int GetNumberOfTest(string prevNumberOfTest)
        {
            int number;

            if (int.TryParse(prevNumberOfTest, out number))
            {
                return number + 1;
            }

            return 1;
        }
    }
}
