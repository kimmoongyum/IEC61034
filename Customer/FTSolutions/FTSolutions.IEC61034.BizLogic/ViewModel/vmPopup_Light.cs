using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.Protocol;
using eccFramework.SharedLib.GlobalType.SysType;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_Light : BaseIEC61034ViewModel
    {
        private DispatcherTimer _timerCheckState;

        public vmPopup_Light()
        {
            int capacity = IEC61034Const.CAPACITY_600;

            this.SeriesTransmission = new ChartSeriesInfo(capacity);
            this.SeriesDummyMin = new ChartSeriesInfo(capacity);

            this.RegistrationInfo = new TypeRegistration();
            this.QualificationRegistrationInfo = new TypeQualificationRegistration();

            LaserClearCommand = new DelegateCommand((o) => ExecuteLaserClearCommand(o));
            LaserDarkCommand = new DelegateCommand((o) => ExecuteLaserDarkCommand(o));

            ChangeFilter = new DelegateCommand((o) => ExecuteChangeFilter(o));
        }


        //###################################################################
        //  Command
        //###################################################################

        public ICommand LaserClearCommand { get; private set; }
        public ICommand LaserDarkCommand { get; private set; }

        public ICommand ChangeFilter { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        private double _smokeTransmission;
        public double Transmission
        {
            get { return _smokeTransmission; }
            set
            {
                if (this._smokeTransmission != value)
                {
                    this._smokeTransmission = value;
                }

                this.RaisePropertyChanged(nameof(Transmission));
            }
        }

        
        public ChartSeriesInfo SeriesTransmission { get; set; }
        public ChartSeriesInfo SeriesDummyMin { get; set; }


        private TypeRegistration _registrationInfo;
        public TypeRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                if (this._registrationInfo != value)
                {
                    this._registrationInfo = value;
                    this.RaisePropertyChanged(nameof(RegistrationInfo));
                }
            }
        }

        private TypeQualificationRegistration _qualificationRegistrationInfo;
        public TypeQualificationRegistration QualificationRegistrationInfo
        {
            get { return _qualificationRegistrationInfo; }
            set
            {
                if (this._qualificationRegistrationInfo != value)
                {
                    this._qualificationRegistrationInfo = value;
                    this.RaisePropertyChanged(nameof(RegistrationInfo));
                }
            }
        }

        private bool _isTest;
        public bool IsTest
        {
            get { return _isTest; }
            set
            {
                if (this._isTest != value)
                {
                    this._isTest = value;
                }

                this.RaisePropertyChanged(nameof(IsTest));
            }
        }

        private bool _isCalibration;
        public bool IsCalibration
        {
            get { return _isCalibration; }
            set
            {
                if (this._isCalibration != value)
                {
                    this._isCalibration = value;
                }

                this.RaisePropertyChanged(nameof(IsCalibration));
            }
        }


        private bool _isQualification;
        public bool IsQualification
        {
            get { return _isQualification; }
            set
            {
                if (this._isQualification != value)
                {
                    this._isQualification = value;
                }

                this.RaisePropertyChanged(nameof(IsQualification));
            }
        }

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            base.VMLoaded();

            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.DO_LIGHT_ON);
            Thread.Sleep(200);
            //this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_ON);
            this.DEVICE_MANAGER.ExecuteNDFilterCommand("CLEAR");

            if (this.CallerMenu == MenuKind.MAIN)
            {
                this.IsTest = false;
                this.IsCalibration = true;
                this.IsQualification = false;

                this.RegistrationInfo = null;
                this.QualificationRegistrationInfo = null;
                //this.RegistrationInfo.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo);
            }
            else if (this.CallerMenu == MenuKind.REGISTRATION)
            {
                this.IsTest = true;
                this.IsCalibration = false;
                this.IsQualification = false;

                this.QualificationRegistrationInfo = null;
                this.RegistrationInfo.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo);
            }
            else if (this.CallerMenu == MenuKind.QUALIFICATION_REGISTRATION)
            {
                this.IsTest = false;
                this.IsCalibration = false;
                this.IsQualification = true;

                this.RegistrationInfo = null;
                this.QualificationRegistrationInfo.CopyValueFrom(this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo);
            }
            else
            {
                Console.WriteLine(string.Format("vmPopup_LightCalibration -> {0}", this.CallerMenu.ToString()));
            }

            this.InitializeTimer();
        }

        public override void Dispose()
        {
            base.Dispose();            

            if (this._timerCheckState != null)
            {
                this._timerCheckState.Stop();
                this._timerCheckState = null;
            }
        }

        protected override void ExecuteBackClick(object obj)
        {
            base.ExecuteBackClick(obj);

            /*
            string targetMenu = IEC61034Const.MENU_BLANK_TEST_KEY;
            BasePopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.TEST;
            };

            this.CloseWindow();

            popup.ShowDialog();
            */

            if (this.CallerMenu == MenuKind.REGISTRATION || this.CallerMenu == MenuKind.TEST)
            {
                string targetMenu = IEC61034Const.MENU_BLANK_TEST_KEY;

                BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
                popup.Owner = this.Owner;

                popup.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = this.CallerMenu;
                };

                this.CloseWindow();

                popup.ShowDialog();
            }
            else if (this.CallerMenu == MenuKind.QUALIFICATION_REGISTRATION || this.CallerMenu == MenuKind.QUALIFICATION)
            {
                string targetMenu = IEC61034Const.MENU_BLANK_TEST_KEY;

                BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
                popup.Owner = this.Owner;

                popup.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = this.CallerMenu;
                };

                this.CloseWindow();

                popup.ShowDialog();

            }
        }

        protected override void ExecuteNextClick(object obj)
        {
            base.ExecuteNextClick(obj);

            /*
            string targetMenu = IEC61034Const.MENU_TEST_KEY;
            BasePopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.TEST;
            };

            this.CloseWindow();

            popup.ShowDialog();
            */

            if (this.CallerMenu == MenuKind.REGISTRATION)
            {
                string targetMenu = IEC61034Const.MENU_TEST_KEY;

                BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
                popup.Owner = this.Owner;

                popup.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = this.CallerMenu;
                };

                this.CloseWindow();

                popup.ShowDialog();
            }
            else if (this.CallerMenu == MenuKind.QUALIFICATION_REGISTRATION)
            {
                string targetMenu = IEC61034Const.MENU_QUALIFICATION_KEY;

                BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
                popup.Owner = this.Owner;

                popup.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = this.CallerMenu;
                };

                this.CloseWindow();

                popup.ShowDialog();
            }
        }

        protected override bool ExecuteCancelClick(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_popup", "msg_quit_menu");

            if (result)
            {
                this.CloseWindow();
            }

            return result;
        }

        protected override bool ExecuteCloseClick(object obj)
        {
            this.CloseWindow();
            return true;
        }

        public override bool IsValid()
        {
            return true;
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        

        //###################################################################
        //  Public
        //###################################################################

        public void ExecuteLaserClearCommand(object obj)
        {
            //this.DEVICE_MANAGER.SetLightSpan(DbChannel.AI_LIGHT_PHOTODIODE, this.DEVICE_MANAGER.AnalogInMeasure.LightPhotoDiode);
            //this.SESSION_MANAGER.CalibrationProcess.IsGoodLightSpan = true;
            SaveSpan();
        }

        public void ExecuteLaserDarkCommand(object obj)
        {
            //this.DEVICE_MANAGER.SetLightZero(DbChannel.AI_LIGHT_PHOTODIODE, this.DEVICE_MANAGER.AnalogInMeasure.LightPhotoDiode);
            //this.SESSION_MANAGER.CalibrationProcess.IsGoodLightZero = true;
            SaveZero();
        }

        public void ExecuteChangeFilter(object obj)
        {
            switch (obj.ToString())
            {
                case "CLEAR": this.DEVICE_MANAGER.ExecuteNDFilterCommand("CLEAR"); break;
                case "DARK": this.DEVICE_MANAGER.ExecuteNDFilterCommand("DARK"); break;
                case "#1": this.DEVICE_MANAGER.ExecuteNDFilterCommand("#1"); break;
                case "#2": this.DEVICE_MANAGER.ExecuteNDFilterCommand("#2"); break;
                case "#3": this.DEVICE_MANAGER.ExecuteNDFilterCommand("#3"); break;
                case "#4": this.DEVICE_MANAGER.ExecuteNDFilterCommand("#4"); break;
                case "#5": this.DEVICE_MANAGER.ExecuteNDFilterCommand("#5"); break;
                case "#6": this.DEVICE_MANAGER.ExecuteNDFilterCommand("#6"); break;
                default: this.DEVICE_MANAGER.ExecuteNDFilterCommand("CLEAR"); break;
            }
        }



        //###################################################################
        //  Private
        //###################################################################

        private void InitializeTimer()
        {
            this._timerCheckState = new DispatcherTimer();
            this._timerCheckState.Interval = TimeSpan.FromMilliseconds(IEC61034Const.MEASURING_INTERVAL);
            this._timerCheckState.Tick += (s, e) =>
            {
                this.Transmission = this.DEVICE_MANAGER.AnalogInput.Transmission;

                this.SeriesTransmission.AddPoint(this.Transmission);
            };

            this._timerCheckState.Start();
        }

        private void Clear()
        {
            this.SeriesTransmission.Clear();
        }

        // min voltage, min value를 저장해야 함
        private void SaveZero()
        {
            double voltage = 0;
#if IS_LOCAL
            voltage = 0.09;
#else
            voltage = this.DEVICE_MANAGER.AnalogInput.TransmissionVoltage;
            //voltage = this.DEVICE_MANAGER.AnalogInput.AnalogIn.TransmissionVoltage;
#endif
            SetLightZero(DbChannel.AI_LIGHT_PHOTODIODE, voltage);
        }

        // max voltage, max value를 저장해야 함
        private void SaveSpan()
        {
            double voltage = 0;
#if IS_LOCAL
            voltage = 4.99;
#else
            voltage = this.DEVICE_MANAGER.AnalogInput.TransmissionVoltage;
            //voltage = this.DEVICE_MANAGER.AnalogInput.AnalogIn.TransmissionVoltage;
#endif
            SetLightSpan(DbChannel.AI_LIGHT_PHOTODIODE, voltage);

        }

        public void SetLightZero(ChannelInfo channel, double voltage)
        {
            ZeroSpanMethod zeroSpanMethod = new ZeroSpanMethod();
            zeroSpanMethod.SetZero(voltage, channel.GradientVoltage, channel.InterceptVoltage);

            SettingManager.WriteChannelMinVoltageInfoInDatabase(channel.Channel, voltage.ToString(), zeroSpanMethod.Gradient.ToString(), zeroSpanMethod.Intercept.ToString());
            SettingManager.ReadChannelDatabase();
        }

        public void SetLightSpan(ChannelInfo channel, double voltage)
        {
            ZeroSpanMethod zeroSpanMethod = new ZeroSpanMethod();
            zeroSpanMethod.SetSpan(voltage, channel.GradientVoltage, channel.InterceptVoltage);

            SettingManager.WriteChannelMaxVoltageInfoInDatabase(channel.Channel, voltage.ToString(), zeroSpanMethod.Gradient.ToString(), zeroSpanMethod.Intercept.ToString());
            SettingManager.ReadChannelDatabase();
        }
    }
}
