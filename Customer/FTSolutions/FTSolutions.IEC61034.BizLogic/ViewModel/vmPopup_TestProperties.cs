using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Helper;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Result;
using FTSolutions.IEC61034.Common.Setting;
using System;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_TestProperties : BaseIEC61034ViewModel
    {
        public vmPopup_TestProperties()
        {
            this.TestItem_Properties = new TestProperty();
            this.RegistrationInfo = new TypeRegistration();
        }




        //###################################################################
        //  Command
        //###################################################################



        //###################################################################
        //  Property
        //###################################################################

        private TestProperty _testItem_Properties;
        public TestProperty TestItem_Properties
        {
            get { return _testItem_Properties; }
            set
            {
                if (this._testItem_Properties != value)
                {
                    this._testItem_Properties = value;
                    this.RaisePropertyChanged(nameof(TestItem_Properties));
                }
            }
        }

        private TypeRegistration _registrationInfo;
        public TypeRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                _registrationInfo = value;
                this.RaisePropertyChanged(nameof(RegistrationInfo));
            }
        }

        #region Cable Radiobutton.
        private bool _isCableRound;
        public bool IsCableRound
        {
            get { return _isCableRound; }
            set
            {
                if (this._isCableRound != value)
                {
                    this._isCableRound = value;
                    this.RaisePropertyChanged(nameof(IsCableRound));
                }

                if (value)
                {
                    this.RegistrationInfo.CABLE_TYPE = "ROUND";
                }
            }
        }

        private bool _isCableFlat;
        public bool IsCableFlat
        {
            get { return _isCableFlat; }
            set
            {
                if (this._isCableFlat != value)
                {
                    this._isCableFlat = value;
                    this.RaisePropertyChanged(nameof(IsCableFlat));
                }

                if (value)
                {
                    this.RegistrationInfo.CABLE_TYPE = "FLAT";
                }
            }
        }
        #endregion

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            if (this.CallerMenu == MenuKind.REGISTRATION)
            {
                this.TestItem_Properties.Number = this.GetNumberOfTest(this.RegistrationInfo.NUMBER).ToString();
                this.TestItem_Properties.TotalNumber = this.RegistrationInfo.TOTAL_NUMBER;

                if (this.TestItem_Properties.TotalNumber == null || this.TestItem_Properties.TotalNumber.Trim().Length < 1)
                {
                    this.TestItem_Properties.TotalNumber = "1";
                }
            }
            else if(this.CallerMenu == MenuKind.BLANK_TEST)
            {
                this.RegistrationInfo.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo);
                this.TestItem_Properties.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties);
            }
            else
            {
                Console.WriteLine(string.Format("vmPopup_TestProperties -> {0}", this.CallerMenu.ToString()));

                this.RegistrationInfo.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo);
                this.TestItem_Properties.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties);

                if (this.CallerMenu == MenuKind.TEST_SUMMARY)
                {
                    this.TestItem_Properties.Number = this.GetNumberOfTest(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.Number).ToString();
                }
            }
           
            if(this.RegistrationInfo.CABLE_TYPE.Equals("ROUND"))
            {
                this.IsCableFlat = false;
                this.IsCableRound = true;
            }
            else if(this.RegistrationInfo.CABLE_TYPE.Equals("FLAT"))
            {
                this.IsCableFlat = true;
                this.IsCableRound = false;
            }

            this.TestItem_Properties.SetNow();
            this.TestItem_Properties.SetTestInfo(this.RegistrationInfo);
        }

        public override void Dispose()
        {
            base.Dispose();
        }


        protected override void ExecuteNextClick(object obj)
        {
            base.ExecuteNextClick(obj);

            this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo.CopyValueFrom(this.RegistrationInfo);
            this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.CopyValueFrom(this.TestItem_Properties);

            string targetMenu = IEC61034Const.MENU_BLANK_TEST_KEY;

            BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.REGISTRATION;
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
            return this.TestItem_Properties.IsValid();
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
