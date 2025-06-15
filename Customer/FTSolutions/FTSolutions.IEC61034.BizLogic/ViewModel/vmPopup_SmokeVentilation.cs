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
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_SmokeVentilation : BaseIEC61034ViewModel
    {
        private DispatcherTimer _timerCheckState;

#if IS_LOCAL
        VirtualFactory _factory = new VirtualFactory(MenuKind.BLANK_TEST);
        private int _factoryIndex = 0;
#endif

        public vmPopup_SmokeVentilation()
        {
            int capacity = IEC61034Const.CAPACITY_600;

            this.SeriesTransmission = new ChartSeriesInfo(capacity);

            ChangeFilter = new DelegateCommand((o) => ExecuteChangeFilter(o));
        }

        //###################################################################
        //  Command
        //###################################################################

        public ICommand ChangeFilter { get; private set; }

        //###################################################################
        //  Property
        //###################################################################

        private double _averageTransmission;
        public double AverageTransmission
        {
            get { return _averageTransmission; }
            set
            {
                if (this._averageTransmission != value)
                {
                    this._averageTransmission = value;
                }

                this.RaisePropertyChanged(nameof(AverageTransmission));
            }
        }

        private double _currentTransmission;
        public double CurrentTransmission
        {
            get { return _currentTransmission; }
            set
            {
                if (this._currentTransmission != value)
                {
                    this._currentTransmission = value;
                }

                this.RaisePropertyChanged(nameof(CurrentTransmission));
            }
        }

        #region Standard
        private bool _is61034;
        public bool Is61034
        {
            get { return _is61034; }
            set
            {
                if (this._is61034 != value)
                {
                    this._is61034 = value;
                    this.RaisePropertyChanged(nameof(Is61034));
                }
            }
        }
        #endregion

        #region Title
        private string _iec61034_Title;
        public string IEC61034_TITLE
        {
            get { return _iec61034_Title; }
            set
            {
                this._iec61034_Title = value;
                this.RaisePropertyChanged(nameof(IEC61034_TITLE));
            }
        }
        #endregion
        

        public ChartSeriesInfo SeriesTransmission { get; set; }

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.OPEN_IN_DAMPER);
            Thread.Sleep(200);
            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.OPEN_OUT_DAMPER);

            this.IEC61034_TITLE = this.SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_IEC61034);

            this.Clear();

            this.InitializeTimer();
        }


        //###################################################################
        //  EventHandler
        //###################################################################
        public void ExecuteTickTimerCommand(object obj)
        {
            if (obj is DependencyPropertyChangedEventArgs)
            {
                this.CheckVentilationData();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.CLOSE_OUT_DAMPER);
            Thread.Sleep(200);
            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.CLOSE_IN_DAMPER);

            if (this._timerCheckState != null)
            {
                this._timerCheckState.Stop();
                this._timerCheckState = null;
            }
        }

        protected override bool ExecuteCloseClick(object obj)
        {
            this.CloseWindow();

            return true;
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
                CheckVentilationData();
            };

            this._timerCheckState.Start();
        }

        private void Clear()
        {
#if IS_LOCAL
            this._factoryIndex = 0;
#endif
            this.SeriesTransmission.Clear();
        }

        public virtual void CheckVentilationData()
        {
            double transmission;

#if IS_LOCAL
            transmission = _factory.TransmissionData[this._factoryIndex];

            this.CurrentTransmission = transmission;

            this._factoryIndex++;
#else
            transmission = DeviceManager.Current.AnalogInput.Transmission;
#endif
            this.CurrentTransmission = transmission;

            this.SeriesTransmission.AddPoint(transmission);

            int targetDataCount = IEC61034Const.TRANSMISSION_STABILIZATION_DATA_COUNT;

            this.AverageTransmission = Math.Round(this.SeriesTransmission.GetValueListByLastN(targetDataCount).Average(), 2, MidpointRounding.AwayFromZero);

            if (this.SeriesTransmission.SeriesCollection.Count >= targetDataCount)
            {
            }
            else
            {
            }
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
    }
}
