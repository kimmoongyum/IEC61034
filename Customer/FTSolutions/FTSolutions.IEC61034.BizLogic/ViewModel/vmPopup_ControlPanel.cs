using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.GlobalType.SysType;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_ControlPanel : BaseIEC61034ViewModel
    {
        private DispatcherTimer _timerCheckState;

#if IS_LOCAL
        VirtualFactory _factory = new VirtualFactory(MenuKind.BLANK_TEST);
        private int _factoryIndex = 0;
#endif

        public vmPopup_ControlPanel()
        {
            int capacity = IEC61034Const.CAPACITY_600;

            this.SeriesFan = new ChartSeriesInfo(capacity);
            this.SeriesTransmission = new ChartSeriesInfo(capacity);
            this.SeriesChamberTemperature = new ChartSeriesInfo(capacity);

            ApplyInverterCommand = new DelegateCommand((o) => ExecuteApplyInverterCommand(o));       
            ClearChartCommand = new DelegateCommand((o) => ExecuteClearChartCommand(o));
            ChangeFilter = new DelegateCommand((o) => ExecuteChangeFilter(o));
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand ClearChartCommand { get; private set; }
        public ICommand ApplyInverterCommand { get; private set; }
        public ICommand ChangeFilter { get; private set; }

        //###################################################################
        //  Property
        //###################################################################

        #region SereiesInfo collections.
        public ChartSeriesInfo SeriesFan { get; set; }
        public ChartSeriesInfo SeriesTransmission { get; set; }
        public ChartSeriesInfo SeriesChamberTemperature { get; set; }
        #endregion
        
        
        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
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

        protected override bool ExecuteCancelClick(object obj)
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

        public void ExecuteClearChartCommand(object obj)
        {
            this.Clear();
        }

        public void ExecuteApplyInverterCommand(object obj)
        {
            double flowrate;

            if (double.TryParse(obj.ToString(), out flowrate))
            {
                this.DEVICE_MANAGER.AnalogOutput.WriteInverter(flowrate);
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

        //###################################################################
        //  Private
        //###################################################################

        private void InitializeTimer()
        {
            this._timerCheckState = new DispatcherTimer();
            this._timerCheckState.Interval = TimeSpan.FromMilliseconds(IEC61034Const.MEASURING_INTERVAL);
            this._timerCheckState.Tick += (s, e) =>
            {
#if IS_LOCAL
                double chamberTC;
                double transmission;

                chamberTC = _factory.ChamberTCData[this._factoryIndex];
                transmission = _factory.TransmissionData[this._factoryIndex];

                this._factoryIndex++;

                this.SeriesFan.AddPoint(this.DEVICE_MANAGER.AnalogInput.FanFlowrate);
                this.SeriesTransmission.AddPoint(transmission);
                this.SeriesChamberTemperature.AddPoint(chamberTC);
#else
                //Console.WriteLine(string.Format("{0} : {1} : {2}", this.DEVICE_MANAGER.AnalogInput.FanFlowrate, this.DEVICE_MANAGER.AnalogInput.Transmission, this.DEVICE_MANAGER.AnalogInput.ChamberTemperature));
                this.SeriesFan.AddPoint(this.DEVICE_MANAGER.AnalogInput.FanFlowrate);
                this.SeriesTransmission.AddPoint(this.DEVICE_MANAGER.AnalogInput.Transmission);
                this.SeriesChamberTemperature.AddPoint(this.DEVICE_MANAGER.AnalogInput.ChamberTemperature);
#endif
            };

            this._timerCheckState.Start();
        }

        private void Clear()
        {
#if IS_LOCAL
            this._factoryIndex = 0;
#endif
            this.SeriesFan.Clear();
            this.SeriesTransmission.Clear();
            this.SeriesChamberTemperature.Clear();
        }
    }
}
