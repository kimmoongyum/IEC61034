using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.SysType;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Result;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_Qualification : BaseIEC61034ViewModel
    {
        public class TDuration
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public vmPopup_Qualification()
        {
            this.TestItem_Qualification = new Qualification();

            TickTimerCommand = new DelegateCommand((o) => ExecuteTickTimerCommand(o));
            ApplyInverterCommand = new DelegateCommand((o) => ExecuteApplyInverterCommand(o));
            SetFlameoutTimeCommand = new DelegateCommand((o) => ExecuteSetFlameoutTimeCommand(o));

            ChangeFilter = new DelegateCommand((o) => ExecuteChangeFilter(o));

            this.TestItem_Qualification.StopAction += () => 
            {
                this.IsStopChecked = true;
            };
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand TickTimerCommand { get; private set; }
        public ICommand ApplyInverterCommand { get; private set; }
        public ICommand SetFlameoutTimeCommand { get; private set; }

        public ICommand ChangeFilter { get; private set; }

        //###################################################################
        //  Property
        //###################################################################

        public ObservableCollection<TDuration> TestDurations { get; set; }

        private TDuration _selectedDuration;
        public TDuration SelectedDuration
        {
            get { return _selectedDuration; }
            set
            {
                if (_selectedDuration != value)
                {
                    _selectedDuration = value;
                    this.TestItem_Qualification.Standard.MaxTestDurationMinute = Convert.ToInt16(value.Name);
                    this.RaisePropertyChanged(nameof(SelectedDuration));
                }
            }
        }

        private Qualification _testItem_Qualification;
        public Qualification TestItem_Qualification
        {
            get { return _testItem_Qualification; }
            set
            {
                if (this._testItem_Qualification != value)
                {
                    this._testItem_Qualification = value;
                    this.RaisePropertyChanged(nameof(TestItem_Qualification));
                }
            }
        }

        #region Timer Properties.
        private bool _startTimer;
        public bool StartTimer
        {
            get { return _startTimer; }
            set
            {
                if (this._startTimer != value)
                {
                    this._startTimer = value;
                }

                this.RaisePropertyChanged(nameof(StartTimer));
            }
        }

        private bool _stopTimer;
        public bool StopTimer
        {
            get { return _stopTimer; }
            set
            {
                if (this._stopTimer != value)
                {
                    this._stopTimer = value;
                }

                this.RaisePropertyChanged(nameof(StopTimer));
            }
        }

        private bool _isStartChcked;
        public bool IsStartChecked
        {
            get { return _isStartChcked; }
            set
            {
                if (!_isStartChcked && value)
                {
                    this.ExecuteStartCommand(null);
                }

                _isStartChcked = value;
                this.RaisePropertyChanged(nameof(IsStartChecked));
            }
        }

        private bool _isStopChcked;
        public bool IsStopChecked
        {
            get { return _isStopChcked; }
            set
            {
                if (!_isStopChcked && value)
                {
                    this.ExecuteStopCommand(null);
                }

                _isStopChcked = value;
                this.RaisePropertyChanged(nameof(IsStopChecked));
            }
        }
        #endregion

        // IEC60331 에서 시작시 온도를 저장한 이유는 .... 버너 온도가 시작시 온도보다 일정이상 상승하면 Buner Check !!! 알람을 표시 하기 위해서 임
        private double _startChamgerTemperature;
        public double StartChamberTemperature
        {
            get { return _startChamgerTemperature; }
            set
            {
                if (this._startChamgerTemperature != value)
                {
                    this._startChamgerTemperature = value;
                }

                this.RaisePropertyChanged(nameof(StartChamberTemperature));
            }
        }

        private bool _isAlarm;
        public bool IsAlarm
        {
            get { return _isAlarm; }
            set
            {
                if (this._isAlarm != value)
                {
                    this._isAlarm = value;
                }

                this.RaisePropertyChanged(nameof(IsAlarm));
            }
        }

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
            }
        }

        private string _flameOutSecond;
        public string FlameOutSecond
        {
            get { return _flameOutSecond; }
            set
            {
                if (this._flameOutSecond != value)
                {
                    this._flameOutSecond = value;
                    this.RaisePropertyChanged(nameof(FlameOutSecond));
                }
            }
        }

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            base.VMLoaded();

            if(SessionManager.Current.IEC61034_DataSetQualification.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_4)
            {
                this.IsToluene4 = true;
                this.IsToluene10 = false;                
            }
            else if(SessionManager.Current.IEC61034_DataSetQualification.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_10)
            {
                this.IsToluene4 = false;
                this.IsToluene10 = true;
            }
            else
            {
                this.IsToluene4 = false;
                this.IsToluene10 = false;
            }

            this.TestItem_Qualification.InitializeData();

            this.TestItem_Qualification.Standard.IsAutoStop = false;
           
            this.IsStopChecked = true;

            this.TestItem_Qualification.Standard.StartChamberTemperature = 0;

            TestDurations = new ObservableCollection<TDuration>
            {
                new TDuration { Id = 1, Name = "10" },
                new TDuration { Id = 2, Name = "20" },
                new TDuration { Id = 3, Name = "30" },
                new TDuration { Id = 4, Name = "40" },
                new TDuration { Id = 4, Name = "50" },
                new TDuration { Id = 4, Name = "60" },
                new TDuration { Id = 5, Name = "2" }
            };

            this.SelectedDuration = TestDurations[3];

            this.TestItem_Qualification.Standard.IsAutoStop = true;
            this.TestItem_Qualification.Standard.IsManualStop = false;

            this.FlameOutSecond = string.Format("Flame out");
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override void ExecuteNextClick(object obj)
        {
            base.ExecuteNextClick(obj);

            if (this.TestItem_Qualification.Standard == null || this.TestItem_Qualification.Standard.SeriesTransmission == null ||
                this.TestItem_Qualification.Standard.SeriesTransmission.SeriesCollection.Count < 1)
            {
                return;
            }

            this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.CopyValueFrom(this.TestItem_Qualification);

            string targetMenu = IEC61034Const.MENU_QUALIFICATION_SUMMARY_KEY;
            BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.QUALIFICATION;
            };

            this.CloseWindow();

            popup.ShowDialog();
        }

        protected override void ExecuteBackClick(object obj)
        {
            base.ExecuteBackClick(obj);

            string targetMenu = IEC61034Const.MENU_LIGHT_CALIBRATION_KEY;
            BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                //vm.CallerMenu = MenuKind.QUALIFICATION;
                vm.CallerMenu = this.CallerMenu;
            };

            this.CloseWindow();

            popup.ShowDialog();
        }

        protected override bool ExecuteCancelClick(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_popup", "msg_quit_menu");

            if (result)
            {
                this.SESSION_MANAGER.IEC61034_DataSetQualification.ClearAll();

                this.CloseWindow();
            }

            return result;
        }

        public override bool IsValid()
        {
            return this.TestItem_Qualification.Standard != null && this.TestItem_Qualification.Standard.SeriesTransmission != null && 
                this.TestItem_Qualification.Standard.SeriesTransmission.SeriesCollection.Count > 0;
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        public void ExecuteTickTimerCommand(object obj)
        {
            if (obj is DependencyPropertyChangedEventArgs)
            {
                this.TestItem_Qualification.Standard.CheckTestData();                

                int totalSeconds = Convert.ToInt32(((DependencyPropertyChangedEventArgs)obj).NewValue);

                while (this.TestItem_Qualification.Standard.SeriesTransmission.GetIndex - 1 < totalSeconds)
                {
#if IS_LOCAL
                    if (this.StopTimer)
                    {
                        return;
                    }
#endif

                    this.TestItem_Qualification.Standard.CheckTestData();
                }
            }
        }

        public void ExecuteStartCommand(object obj)
        {            
            this.Clear();

            this.StopTimer = false;
            this.StartTimer = true;

            this.TestItem_Qualification.StartQualification();

            this.TestItem_Qualification.Standard.StartChamberTemperature = DEVICE_MANAGER.AnalogInput.ChamberTemperature;
        }

        public void ExecuteStopCommand(object obj)
        {
            this.StartTimer = false;
            this.StopTimer = true;

            this.TestItem_Qualification.Standard.FinishTimeSecond = this.TestItem_Qualification.Standard.SeriesTransmission.SeriesCollection.Count - 1;
            this.TestItem_Qualification.StopQualification();
        }

        public void ExecuteApplyInverterCommand(object obj)
        {
            double flowrate;

            if (double.TryParse(obj.ToString(), out flowrate))
            {
                this.DEVICE_MANAGER.AnalogOutput.WriteInverter(flowrate);
            }
        }

        public void ExecuteSetFlameoutTimeCommand(object obj)
        {
            this.TestItem_Qualification.Standard.IsFlameout = true;

            if (this.TestItem_Qualification.Standard.SeriesTransmission.SeriesCollection.Count - 1 < 0)
            {
                this.TestItem_Qualification.Standard.FlameoutSecond = 0;
            }
            else
            {
                this.TestItem_Qualification.Standard.FlameoutSecond = this.TestItem_Qualification.Standard.SeriesTransmission.SeriesCollection.Count - 1;
            }

            this.FlameOutSecond = string.Format("Flame out\n({0}s)", this.TestItem_Qualification.Standard.FlameoutSecond);
        }


        //###################################################################
        //  Public
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

        private void Clear()
        {
            this.IsAlarm = false;

            this.TestItem_Qualification.Clear();
        }

        public double? GetLastValue(ChartSeriesInfo series)
        {
            var values = series.GetValueList();
            return values.Count > 0 ? values[values.Count - 1] : (double?)null;
        }
    }
}