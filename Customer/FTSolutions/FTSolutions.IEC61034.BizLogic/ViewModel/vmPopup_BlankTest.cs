using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.SysType;
using eccFramework.SharedLib.Utility;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Result;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_BlankTest : BaseIEC61034ViewModel
    {
        private DispatcherTimer _timerCheckState;

#if IS_LOCAL
        VirtualFactory _factory = new VirtualFactory(MenuKind.BLANK_TEST);
        private int _factoryIndex = 0;
#endif

        public vmPopup_BlankTest()
        {
            this.RegistrationInfo = new TypeRegistration();
            this.QualificationRegistrationInfo = new TypeQualificationRegistration();   
                
            ApplyInverterCommand = new DelegateCommand((o) => ExecuteApplyInverterCommand(o));
            ChangeFilter = new DelegateCommand((o) => ExecuteChangeFilter(o));

            int capacity = IEC61034Const.CAPACITY_600;

            this.SeriesChamberTemperature = new ChartSeriesInfo(capacity);
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand ApplyInverterCommand { get; private set; }
        public ICommand ChangeFilter { get; private set; }

        //###################################################################
        //  Property
        //###################################################################

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

        private double _averageTemperature;
        public double AverageTemperature
        {
            get { return _averageTemperature; }
            set
            {
                if (this._averageTemperature != value)
                {
                    this._averageTemperature = value;
                }

                this.RaisePropertyChanged(nameof(AverageTemperature));
            }
        }

        private double _maxDeviation;
        public double MaxDeviation
        {
            get { return _maxDeviation; }
            set
            {
                if (this._maxDeviation != value)
                {
                    this._maxDeviation = value;
                }

                this.RaisePropertyChanged(nameof(MaxDeviation));
            }
        }

        private double _drift;
        public double Drift
        {
            get { return _drift; }
            set
            {
                if (this._drift != value)
                {
                    this._drift = value;
                }

                this.RaisePropertyChanged(nameof(Drift));
            }
        }

        private double _chamberTemperature;
        public double ChamberTemperature
        {
            get { return _chamberTemperature; }
            set
            {
                if (this._chamberTemperature != value)
                {
                    this._chamberTemperature = value;
                }

                this.RaisePropertyChanged(nameof(ChamberTemperature));
            }
        }

        private bool _isValidResult;
        public bool IsValidResult
        {
            get { return _isValidResult; }
            set
            {
                if (this._isValidResult != value)
                {
                    this._isValidResult = value;
                }

                this.RaisePropertyChanged(nameof(IsValidResult));
            }
        }

        private double _startChamberTemperature;
        public double StartChamberTemperature
        {
            get { return _startChamberTemperature; }
            set
            {
                if (this._startChamberTemperature != value)
                {
                    this._startChamberTemperature = value;
                }

                this.RaisePropertyChanged(nameof(StartChamberTemperature));
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
        

        public ChartSeriesInfo SeriesChamberTemperature { get; set; }

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            base.VMLoaded();

            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.DO_FAN_ON);

            if(this.CallerMenu == MenuKind.MAIN)
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
                Console.WriteLine(string.Format("vmPopup_BlankTest -> {0}", this.CallerMenu.ToString()));
            }

            this.Clear();

            this.InitializeTimer();
            
            this.StartChamberTemperature = this.DEVICE_MANAGER.AnalogInput.ChamberTemperature;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.DO_FAN_OFF);

            if (this._timerCheckState != null)
            {
                this._timerCheckState.Stop();
                this._timerCheckState = null;
            }
        }


        protected override void ExecuteNextClick(object obj)
        {
            base.ExecuteNextClick(obj);

#if !IS_LOCAL
            if (this.IsValidResult == false)
            {
                //this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "popup_blank_test_invalid_temperature");      
                bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_invalid_temperature", "popup_blank_test_invalid_temperature_continue");
                if (!result)
                {
                    return;
                }
            }
#endif

            string targetMenu = IEC61034Const.MENU_LIGHT_CALIBRATION_KEY;

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

        protected override void ExecuteBackClick(object obj)
        {
            base.ExecuteBackClick(obj);

            if (this.CallerMenu == MenuKind.REGISTRATION)
            {
                string targetMenu = IEC61034Const.MENU_TEST_PROPERTY_KEY;

                BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
                popup.Owner = this.Owner;

                popup.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = MenuKind.BLANK_TEST;
                };

                this.CloseWindow();

                popup.ShowDialog();
            }
            else if(this.CallerMenu == MenuKind.QUALIFICATION_REGISTRATION)
            {
                string targetMenu = IEC61034Const.MENU_QUALIFICATION_PROPERTY_KEY;

                BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
                popup.Owner = this.Owner;

                popup.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = MenuKind.QUALIFICATION_BLANK_TEST;
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

        public void ExecuteApplyInverterCommand(object obj)
        {
            double flowrate;

            if (double.TryParse(obj.ToString(), out flowrate))
            {
                this.DEVICE_MANAGER.AnalogOutput.WriteInverter(flowrate);
            }
        }


        //###################################################################
        //  Puplic
        //###################################################################
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
                CheckBlankTestData();
            };

            this._timerCheckState.Start();
        }

        private void Clear()
        {
#if IS_LOCAL
            this._factoryIndex = 0;
#endif
            this.SeriesChamberTemperature.Clear();

            this.IsValidResult = false;
        }

        public bool CheckStability()
        {
            int targetDataCount = IEC61034Const.TEMPERATURE_STABILIZATION_DATA_COUNT;

            if (this.SeriesChamberTemperature.SeriesCollection.Count < targetDataCount)
            {
                return false;
            }

            return true;
        }

        public virtual void CheckBlankTestData()
        {
            double chamberTC;

#if IS_LOCAL
            chamberTC = _factory.ChamberTCData[this._factoryIndex];      

            this._factoryIndex++;
#else
            chamberTC = DeviceManager.Current.AnalogInput.ChamberTemperature;
#endif

            this.ChamberTemperature = chamberTC;

            this.SeriesChamberTemperature.AddPoint(chamberTC);

            int targetDataCount = IEC61034Const.TEMPERATURE_STABILIZATION_DATA_COUNT;

            List<double> targetValues = this.SeriesChamberTemperature.SeriesCollection.GetValueListByLastN(targetDataCount);

            double avg, maxDev, drift;

            eccMath.CalcStabilization(targetValues, 4, out avg, out maxDev, out drift);

            this.AverageTemperature = avg;
            this.MaxDeviation = maxDev;
            this.Drift = drift;

            bool isValidRange = eccMath.IsInValidRange(IEC61034Const.EXHAUST_FLOW_TARGET, IEC61034Const.EXHAUST_FLOW_VALID_RANGE, this.AverageTemperature);

            if (isValidRange && Math.Abs(this.Drift) < IEC61034Const.EXHAUST_FLOW_DRIFT_VALID_RANGE)
            {
                this.IsValidResult = true;
            }
            else
            {
                this.IsValidResult = false;
            }


            /*
            int minTemperature = 0;
            int maxTemperature = 0;
            string tempValue = string.Empty;

            tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TEMPERATURE_MIN);
            if (tempValue == null || tempValue == string.Empty)
            {
                minTemperature = IEC61034Const.MIN_CHAMBER_TEMPERATURE;
            }
            else
            {
                minTemperature = Convert.ToInt16(tempValue);
            }

            tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TEMPERATURE_MAX);
            if (tempValue == null || tempValue == string.Empty)
            {
                maxTemperature = IEC61034Const.MAX_CHAMBER_TEMPERATURE;
            }
            else
            {
                maxTemperature = Convert.ToInt16(tempValue);
            }

            this.AverageTemperature = Math.Round(this.SeriesChamberTemperature.GetValueListByLastN(targetDataCount).Average(), 2, MidpointRounding.AwayFromZero);

            if (this.SeriesChamberTemperature.SeriesCollection.Count >= targetDataCount)
            {
                if (this.AverageTemperature >= minTemperature && this.AverageTemperature <= maxTemperature)
                {
                    this.IsValidResult = true;
                }
            }
            else
            {
                this.IsValidResult = false;
            }
            */            
        }

#if IS_LOCAL
        private Random random = new Random();

        public double GetRandomTemperature()
        {
            int value = 0;

            value = random.Next(200, 300);

            return value / 10.0;
        }
#endif
    }
}
