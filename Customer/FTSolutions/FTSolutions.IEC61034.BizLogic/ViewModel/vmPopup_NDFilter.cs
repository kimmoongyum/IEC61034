using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.GlobalType.Protocol;
using eccFramework.SharedLib.GlobalType.SysType;
using eccFramework.SharedLib.Utility.Statistics;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.QueryService;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_NDFilter : BaseIEC61034ViewModel
    {
        private DispatcherTimer _timerCheckState;

        private List<double> _bestCoefficientList;


        public vmPopup_NDFilter()
        {
            int capacity = IEC61034Const.CAPACITY_600;

            this.SeriesTransmission = new ChartSeriesInfo(capacity);
            this.SeriesCorrectionTransmission = new ChartSeriesInfo(capacity);
            this.SeriesDummyMin = new ChartSeriesInfo(capacity);

            this.SeriesCertifiedData = new ChartSeriesInfo();
            this.SeriesInputData = new ChartSeriesInfo();
            this.SeriesLeastSquareMethod = new ChartSeriesInfo();

            LaserClearCommand = new DelegateCommand((o) => ExecuteLaserClearCommand(o));
            LaserDarkCommand = new DelegateCommand((o) => ExecuteLaserDarkCommand(o));

            MeasureCommand = new DelegateCommand((o) => ExecuteMeasureCommand(o));
            CalculateCommand = new DelegateCommand((o) => ExecuteCalculateCommand(o));

            SaveCommand = new DelegateCommand((o) => ExecuteSaveCommand(o));

            ChangeFilter = new DelegateCommand((o) => ExecuteChangeFilter(o));
        }


        //###################################################################
        //  Command
        //###################################################################

        public ICommand LaserClearCommand { get; private set; }
        public ICommand LaserDarkCommand { get; private set; }

        public ICommand MeasureCommand { get; private set; }
        public ICommand CalculateCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand ChangeFilter { get; private set; }


        //###################################################################
        //  Property
        //###################################################################

        private string _polynomial;
        public string POLYNIMIAL
        {
            get { return _polynomial; }
            set
            {
                if (this._polynomial != value)
                {
                    this._polynomial = value;
                    this.RaisePropertyChanged(nameof(POLYNIMIAL));
                }
            }
        }

        private double _coef4;
        public double COEF_4
        {
            get { return _coef4; }
            set
            {
                if (this._coef4 != value)
                {
                    this._coef4 = value;
                    this.RaisePropertyChanged(nameof(COEF_4));
                }
            }
        }

        private double _coef3;
        public double COEF_3
        {
            get { return _coef3; }
            set
            {
                if (this._coef3 != value)
                {
                    this._coef3 = value;
                    this.RaisePropertyChanged(nameof(COEF_3));
                }
            }
        }

        private double _coef2;
        public double COEF_2
        {
            get { return _coef2; }
            set
            {
                if (this._coef2 != value)
                {
                    this._coef2 = value;
                    this.RaisePropertyChanged(nameof(COEF_2));
                }
            }
        }

        private double _coef1;
        public double COEF_1
        {
            get { return _coef1; }
            set
            {
                if (this._coef1 != value)
                {
                    this._coef1 = value;
                    this.RaisePropertyChanged(nameof(COEF_1));
                }
            }
        }

        private double _yIntercept;
        public double Y_INTERCEPT
        {
            get { return _yIntercept; }
            set
            {
                if (this._yIntercept != value)
                {
                    this._yIntercept = value;
                    this.RaisePropertyChanged(nameof(Y_INTERCEPT));
                }
            }
        }


        private bool _isNoCompensation;
        public bool IsNoCompensation
        {
            get { return _isNoCompensation; }
            set
            {
                if (this._isNoCompensation != value)
                {
                    this._isNoCompensation = value;

                    this.ExecuteCalculateCommand("NO");
                }

                this.RaisePropertyChanged(nameof(IsNoCompensation));
            }
        }


        public ChartSeriesInfo SeriesTransmission { get; set; }
        public ChartSeriesInfo SeriesCorrectionTransmission { get; set; }
        public ChartSeriesInfo SeriesDummyMin { get; set; }

        public ChartSeriesInfo SeriesCertifiedData { get; set; }
        public ChartSeriesInfo SeriesInputData { get; set; }
        public ChartSeriesInfo SeriesLeastSquareMethod { get; set; }



        //###################################################################
        //  Override
        //###################################################################


        public override void VMLoaded()
        {
            base.VMLoaded();

            this.InitializeTimer();

            this.POLYNIMIAL = this.SESSION_MANAGER.LightPolynomialInfo.Polynomial.ToString();
        }

        public override void VMContentRendered()
        {
            base.VMContentRendered();

            this.ExecuteCalculateCommand(null);
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
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_popup", "msg_quit_menu");

            if (result)
            {
                this.CloseWindow();
            }

            return result;
        }

        public override bool IsValid()
        {
            return true;
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        public void ExecuteLaserClearCommand(object obj)
        {
            //this.DEVICE_MANAGER.SetLightSpan(DbChannel.AI_LIGHT_PHOTODIODE , this.DEVICE_MANAGER.AnalogInput.LightPhotoDiode);
        }

        public void ExecuteLaserDarkCommand(object obj)
        {
            //this.DEVICE_MANAGER.SetLightZero(DbChannel.AI_LIGHT_PHOTODIODE, this.DEVICE_MANAGER.AnalogInput.LightPhotoDiode);
        }


        public void ExecuteMeasureCommand(object obj)
        {
            TypeNDFilter ndFilter = null;

            if (this.DEVICE_MANAGER.DigitalInput.StatusLightDark)
            { }
            else if (this.DEVICE_MANAGER.DigitalInput.StatusLightFilter1)
            {
                ndFilter = this.SESSION_MANAGER.NDFilterCollection.FilterNo1;
            }
            else if (this.DEVICE_MANAGER.DigitalInput.StatusLightFilter2)
            {
                ndFilter = this.SESSION_MANAGER.NDFilterCollection.FilterNo2;
            }
            else if (this.DEVICE_MANAGER.DigitalInput.StatusLightFilter3)
            {
                ndFilter = this.SESSION_MANAGER.NDFilterCollection.FilterNo3;
            }
            else if (this.DEVICE_MANAGER.DigitalInput.StatusLightFilter4)
            {
                ndFilter = this.SESSION_MANAGER.NDFilterCollection.FilterNo4;
            }
            else if (this.DEVICE_MANAGER.DigitalInput.StatusLightFilter5)
            {
                ndFilter = this.SESSION_MANAGER.NDFilterCollection.FilterNo5;
            }
            else if (this.DEVICE_MANAGER.DigitalInput.StatusLightFilter6)
            {
                ndFilter = this.SESSION_MANAGER.NDFilterCollection.FilterNo6;
            }
            else
            { }

            if (ndFilter != null)
            {
                ndFilter.MEASURE_TRANSMISSION = this.DEVICE_MANAGER.AnalogInput.LightTransmission;
                ndFilter.CalcErrors();
            }
        }

        public void ExecuteCalculateCommand(object obj)
        {
            if (this.POLYNIMIAL == null)
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_select_polynomial");
                return;
            }

            List<PointF> measurePointList = this.SESSION_MANAGER.NDFilterCollection.GetMeasurePointList();

            if (measurePointList.Count != 8)
            {
                //this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_all_nd_filter");
                return;
            }

            this.SeriesCertifiedData.Clear();
            this.SeriesLeastSquareMethod.Clear();
            this.SeriesInputData.Clear();

            foreach (var point in measurePointList)
            {
                this.SeriesCertifiedData.AddPoint(point.X, point.X);
                this.SeriesInputData.AddPoint(point.X, point.Y);
            }

            int degree = Convert.ToInt16(this.POLYNIMIAL);
            this._bestCoefficientList = LeastSquareMethod.GetPolynomialLeastSquaresFit(measurePointList, degree);

            this.Y_INTERCEPT = 0;
            this.COEF_1 = 1;
            this.COEF_2 = 0;
            this.COEF_3 = 0;
            this.COEF_4 = 0;

            if (this.IsNoCompensation)
            {
                this._bestCoefficientList[0] = this.Y_INTERCEPT;
                this._bestCoefficientList[1] = this.COEF_1;

                if (this._bestCoefficientList.Count >= 3)
                {
                    this._bestCoefficientList[2] = this.COEF_2;
                }

                if (this._bestCoefficientList.Count >= 4)
                {
                    this._bestCoefficientList[3] = this.COEF_3;
                }

                if (this._bestCoefficientList.Count >= 5)
                {
                    this._bestCoefficientList[4] = this.COEF_4;
                }
            }
            else
            {
                #region Binding Result.
                if (this.POLYNIMIAL.Equals("1") && this._bestCoefficientList.Count == 2)
                {
                    this.Y_INTERCEPT = this._bestCoefficientList[0];
                    this.COEF_1 = this._bestCoefficientList[1];
                }
                else if (this.POLYNIMIAL.Equals("2") && this._bestCoefficientList.Count == 3)
                {
                    this.Y_INTERCEPT = this._bestCoefficientList[0];
                    this.COEF_1 = this._bestCoefficientList[1];
                    this.COEF_2 = this._bestCoefficientList[2];
                }
                else if (this.POLYNIMIAL.Equals("3") && this._bestCoefficientList.Count == 4)
                {
                    this.Y_INTERCEPT = this._bestCoefficientList[0];
                    this.COEF_1 = this._bestCoefficientList[1];
                    this.COEF_2 = this._bestCoefficientList[2];
                    this.COEF_3 = this._bestCoefficientList[3];
                }
                else if (this.POLYNIMIAL.Equals("4") && this._bestCoefficientList.Count == 5)
                {
                    this.Y_INTERCEPT = this._bestCoefficientList[0];
                    this.COEF_1 = this._bestCoefficientList[1];
                    this.COEF_2 = this._bestCoefficientList[2];
                    this.COEF_3 = this._bestCoefficientList[3];
                    this.COEF_4 = this._bestCoefficientList[4];
                }
                #endregion
            }

            this.DrawChart();
        }

        public void ExecuteSaveCommand(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_save", "msg_save_data");

            if (result)
            {
                this.SaveNDFiltersInfo();
                this.SaveCalibrationValue();

                SettingManager.ReadCalibrationValue();
            }
        }



        //###################################################################
        //  Public
        //###################################################################

        private void DrawChart()
        {
            double minX = this.SeriesInputData.SeriesCollection.MinXValue();
            double maxX = this.SeriesInputData.SeriesCollection.MaxXValue();

            double index = (maxX - minX) / 500;

            if (index <= 0)
            {
                return;
            }

            double currentPoint = minX;

            while (currentPoint <= maxX)
            {
                double yValue = eccFramework.SharedLib.Utility.Statistics.LeastSquareMethod.GetY(_bestCoefficientList, currentPoint);
                this.SeriesLeastSquareMethod.AddPoint(currentPoint, yValue);

                currentPoint += index;
            }
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
                this.SeriesTransmission.AddPoint(this.DEVICE_MANAGER.AnalogInput.LightTransmission);
                this.SeriesCorrectionTransmission.AddPoint(this.DEVICE_MANAGER.AnalogInput.CorrectionTransmission);
            };

            this._timerCheckState.Start();
        }


        private void SaveNDFiltersInfo()
        {
            SQLiteParamInfoBatch paramBatchInfo = new SQLiteParamInfoBatch();

            paramBatchInfo.Add(this.GetNDFiltersInfoParameter(this.SESSION_MANAGER.NDFilterCollection.FilterNo1));
            paramBatchInfo.Add(this.GetNDFiltersInfoParameter(this.SESSION_MANAGER.NDFilterCollection.FilterNo2));
            paramBatchInfo.Add(this.GetNDFiltersInfoParameter(this.SESSION_MANAGER.NDFilterCollection.FilterNo3));
            paramBatchInfo.Add(this.GetNDFiltersInfoParameter(this.SESSION_MANAGER.NDFilterCollection.FilterNo4));
            paramBatchInfo.Add(this.GetNDFiltersInfoParameter(this.SESSION_MANAGER.NDFilterCollection.FilterNo5));
            paramBatchInfo.Add(this.GetNDFiltersInfoParameter(this.SESSION_MANAGER.NDFilterCollection.FilterNo6));

            ManagerQueryService.Current.QueryServiceSystem.UpdateNDFilters(paramBatchInfo);
        }

        private SQLiteParamInfoCollection GetNDFiltersInfoParameter(TypeNDFilter ndFilter)
        {
            SQLiteParamInfoCollection paramInfoCollection = new SQLiteParamInfoCollection();
            paramInfoCollection.Add(new SQLiteParamInfo("no", DbType.String, ndFilter.FILTER_NO));
            paramInfoCollection.Add(new SQLiteParamInfo("certi", DbType.String, ndFilter.CERTI_TRANSMISSION));
            paramInfoCollection.Add(new SQLiteParamInfo("measure", DbType.String, ndFilter.MEASURE_TRANSMISSION));
            paramInfoCollection.Add(new SQLiteParamInfo("error", DbType.String, ndFilter.ERROR_TRANSMISSION));

            return paramInfoCollection;
        }


        private void SaveCalibrationValue()
        {
            SQLiteParamInfoBatch paramBatchInfo = new SQLiteParamInfoBatch();

            paramBatchInfo.Add(this.GetCalibrationValueParameter(NDFilterKey.ND_POLYNOMIAL.ToString(), this.POLYNIMIAL.ToString()));
            paramBatchInfo.Add(this.GetCalibrationValueParameter(NDFilterKey.ND_1ST_TERM.ToString(), this.COEF_1.ToString()));
            paramBatchInfo.Add(this.GetCalibrationValueParameter(NDFilterKey.ND_2ND_TERM.ToString(), this.COEF_2.ToString()));
            paramBatchInfo.Add(this.GetCalibrationValueParameter(NDFilterKey.ND_3RD_TERM.ToString(), this.COEF_3.ToString()));
            paramBatchInfo.Add(this.GetCalibrationValueParameter(NDFilterKey.ND_4TH_TERM.ToString(), this.COEF_4.ToString()));
            paramBatchInfo.Add(this.GetCalibrationValueParameter(NDFilterKey.ND_INTERCEPT.ToString(), this.Y_INTERCEPT.ToString()));

            ManagerQueryService.Current.QueryServiceSystem.UpdateCalibrationValue(paramBatchInfo);
        }

        private SQLiteParamInfoCollection GetCalibrationValueParameter(string key, string value)
        {
            SQLiteParamInfoCollection paramInfoCollection = new SQLiteParamInfoCollection();
            paramInfoCollection.Add(new SQLiteParamInfo("key", DbType.String, key));
            paramInfoCollection.Add(new SQLiteParamInfo("value", DbType.String, value));

            return paramInfoCollection;
        }

    }
}
