using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.GlobalType.SysType;
using eccFramework.SharedLib.Utility.Services;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.Common.Standard
{
    public abstract class baseStandard : BaseModel
    {
#if IS_LOCAL
        protected int _factoryIndex = 0;
        protected VirtualFactory _factory = null;
        //protected VirtualFactory _factory = new VirtualFactory(MenuKind.TEST);        
#endif

        public Action StopAction;

        public baseStandard()
        {
            this.IsTest = false;
            this.IsQualification = false;
            this.IsValidResult = false;

            //this.IsFlameout = false;
            this.MaxAbsorbance = 0;
            this.MinTransmission = 100;
            this.CurrentAbsorbance = 0;
            this.CurrentTransmission = 0;
            this.CorrectedAbsorbance = 0;

            this.MinTransmissionSecond = 0;

            //this.FlameoutSecond = 0;
            this.StartChamberTemperature = 0;
           
            this.SeriesChamberTC = new ChartSeriesInfo();
            this.SeriesFanFlowrate = new ChartSeriesInfo();
            this.SeriesAbsorbance = new ChartSeriesInfo();      // Ao
            this.SeriesTransmission = new ChartSeriesInfo();

            this.SeriesMaxAbsorbance = new ChartSeriesInfo();   // Am
            this.SeriesMaxAbsorbance0 = new ChartSeriesInfo();   // Max Ao
            this.SeriesTransmissionVoltage = new ChartSeriesInfo();

            this.TargetAbsorbanceMin = IEC61034Const.DEFAULT_TOLUENE4_MIN_VALUE;
            this.TargetAbsorbanceMax = IEC61034Const.DEFAULT_TOLUENE4_MAX_VALUE;

            this.MaxTestDurationMinute = IEC61034Const.DEFAULT_MAX_TEST_DURATION;
        }

        public baseStandard(string type)
        {
            if (type.Equals("TEST", StringComparison.OrdinalIgnoreCase))
            {
                _factory = new VirtualFactory(MenuKind.TEST);

                this.IsTest = true;
                this.IsQualification = false;

                this.StandardDesc = "IEC61034";
            }
            else if (type.Equals("QUALIFICATION", StringComparison.OrdinalIgnoreCase))
            {
                double minAbsorbance = 0;
                double maxAbsorbance = 0;
                string tempValue = string.Empty;

                if (SessionManager.Current.IEC61034_DataSetQualification.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_4)
                {
                    _factory = new VirtualFactory(MenuKind.QUALIFICATION, IEC61034Const.KEY_TOLUENE_4);                    

                    tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TOLUENE_4_ABSORBANCE_MIN);
                    if (tempValue == null || tempValue == string.Empty)
                    {
                        minAbsorbance = IEC61034Const.DEFAULT_TOLUENE4_MIN_VALUE;
                    }
                    else
                    {
                        minAbsorbance = Convert.ToDouble(tempValue);
                    }

                    tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TOLUENE_4_ABSORBANCE_MAX);
                    if (tempValue == null || tempValue == string.Empty)
                    {
                        maxAbsorbance = IEC61034Const.DEFAULT_TOLUENE4_MAX_VALUE;
                    }
                    else
                    {
                        maxAbsorbance = Convert.ToDouble(tempValue);
                    }

                    this.TargetAbsorbanceMin = minAbsorbance;
                    this.TargetAbsorbanceMax = maxAbsorbance;
                }
                else if (SessionManager.Current.IEC61034_DataSetQualification.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_10)
                {
                    _factory = new VirtualFactory(MenuKind.QUALIFICATION, IEC61034Const.KEY_TOLUENE_10);

                    tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TOLUENE_10_ABSORBANCE_MIN);
                    if (tempValue == null || tempValue == string.Empty)
                    {
                        minAbsorbance = IEC61034Const.DEFAULT_TOLUENE10_MIN_VALUE;
                    }
                    else
                    {
                        minAbsorbance = Convert.ToDouble(tempValue);
                    }

                    tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TOLUENE_10_ABSORBANCE_MAX);
                    if (tempValue == null || tempValue == string.Empty)
                    {
                        maxAbsorbance = IEC61034Const.DEFAULT_TOLUENE10_MAX_VALUE;
                    }
                    else
                    {
                        maxAbsorbance = Convert.ToDouble(tempValue);
                    }

                    this.TargetAbsorbanceMin = minAbsorbance;
                    this.TargetAbsorbanceMax = maxAbsorbance;
                }

                this.StandardDesc = IEC61034Const.QUALIFICATION_STANDARD_TYPE;

                this.IsTest = false;
                this.IsQualification = true;
            }

            this.IsValidResult = false;

            //this.IsFlameout = false;
            this.MaxAbsorbance = 0;
            this.MinTransmission = 100;
            this.CurrentAbsorbance = 0;
            this.CurrentTransmission = 0;
            this.CorrectedAbsorbance = 0;
            this.MinTransmissionSecond = 0;

            //this.FlameoutSecond = 0;
            this.StartChamberTemperature = 0;

            this.SeriesChamberTC = new ChartSeriesInfo();
            this.SeriesFanFlowrate = new ChartSeriesInfo();
            this.SeriesAbsorbance = new ChartSeriesInfo();
            this.SeriesTransmission = new ChartSeriesInfo();

            this.SeriesMaxAbsorbance = new ChartSeriesInfo();
            this.SeriesMaxAbsorbance0 = new ChartSeriesInfo();
            this.SeriesTransmissionVoltage = new ChartSeriesInfo();

            this.MaxTestDurationMinute = IEC61034Const.DEFAULT_MAX_TEST_DURATION;
        }



        //###################################################################
        //  Property
        //###################################################################

        private string _standardDesc;
        public string StandardDesc
        {
            get { return _standardDesc; }
            set
            {
                if (this._standardDesc != value)
                {
                    this._standardDesc = value;
                }

                this.RaisePropertyChanged(nameof(StandardDesc));
            }
        }

        private int _maxTestDurationMinute;
        public int MaxTestDurationMinute
        {
            get { return _maxTestDurationMinute; }
            set
            {
                if (this._maxTestDurationMinute != value)
                {
                    this._maxTestDurationMinute = value;
                }

                this.RaisePropertyChanged(nameof(MaxTestDurationMinute));
            }
        }

        

        private int _finishTimeSecond;
        public int FinishTimeSecond
        {
            get { return _finishTimeSecond; }
            set
            {
                if (this._finishTimeSecond != value)
                {
                    this._finishTimeSecond = value;
                }

                this.RaisePropertyChanged(nameof(FinishTimeSecond));
            }
        }

        private int _inverter;
        public int Inverter
        {
            get { return _inverter; }
            set
            {
                if (this._inverter != value)
                {
                    this._inverter = value;
                }

                this.RaisePropertyChanged(nameof(Inverter));
            }
        }


        public ChartSeriesInfo SeriesTransmission { get; set; }
        public ChartSeriesInfo SeriesAbsorbance { get; set; }
        public ChartSeriesInfo SeriesChamberTC { get; set; }
        public ChartSeriesInfo SeriesFanFlowrate { get; set; }

        public ChartSeriesInfo SeriesMaxAbsorbance { get; set; }
        public ChartSeriesInfo SeriesMaxAbsorbance0 { get; set; }
        public ChartSeriesInfo SeriesTransmissionVoltage { get; set; }


        //private int _flameoutSecond;
        //public int FlameoutSecond
        //{
        //    get { return _flameoutSecond; }
        //    set
        //    {
        //        if (this._flameoutSecond != value)
        //        {
        //            this._flameoutSecond = value;
        //        }

        //        this.RaisePropertyChanged(nameof(FlameoutSecond));
        //    }
        //}

        private double _startChamberTemperature;
        public double StartChamberTemperature
        {
            get { return _startChamberTemperature; }
            set
            {
                if (this._startChamberTemperature != value)
                {
                    this._startChamberTemperature = value;
                    this.RaisePropertyChanged(nameof(StartChamberTemperature));
                }
            }
        }

        private double _maxAbsorbance;
        public double MaxAbsorbance
        {
            get { return _maxAbsorbance; }
            set
            {
                if (this._maxAbsorbance != value)
                {
                    this._maxAbsorbance = value;
                    this.RaisePropertyChanged(nameof(MaxAbsorbance));
                }
            }
        }

        private double _minTransmission;
        public double MinTransmission
        {
            get { return _minTransmission; }
            set
            {
                if (this._minTransmission != value)
                {
                    this._minTransmission = value;
                    this.RaisePropertyChanged(nameof(MinTransmission));
                }
            }
        }

        private int _minTransmissionSecond;
        public int MinTransmissionSecond
        {
            get { return _minTransmissionSecond; }
            set
            {
                if (this._minTransmissionSecond != value)
                {
                    this._minTransmissionSecond = value;
                    this.RaisePropertyChanged(nameof(MinTransmissionSecond));
                }
            }
        }

        private double _currentFanFlowrate;
        public double CurrentFanFlowrate
        {
            get { return _currentFanFlowrate; }
            set
            {
                if (this._currentFanFlowrate != value)
                {
                    this._currentFanFlowrate = value;
                    this.RaisePropertyChanged(nameof(CurrentFanFlowrate));
                }
            }
        }

        private double _currentChamberTC;
        public double CurrentChamberTC
        {
            get { return _currentChamberTC; }
            set
            {
                if (this._currentChamberTC != value)
                {
                    this._currentChamberTC = value;
                    this.RaisePropertyChanged(nameof(CurrentChamberTC));
                }
            }
        }

        private double _currentAbsorbance;
        public double CurrentAbsorbance
        {
            get { return _currentAbsorbance; }
            set
            {
                if (this._currentAbsorbance != value)
                {
                    this._currentAbsorbance = value;
                    this.RaisePropertyChanged(nameof(CurrentAbsorbance));
                }
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
                    this.RaisePropertyChanged(nameof(CurrentTransmission));
                }
            }
        }

        private double _correctedAbsorbance;
        public double CorrectedAbsorbance
        {
            get { return _correctedAbsorbance; }
            set
            {
                if (this._correctedAbsorbance != value)
                {
                    this._correctedAbsorbance = value;
                    this.RaisePropertyChanged(nameof(CorrectedAbsorbance));
                }
            }
        }

        private double _targetAbsorbanceMin;
        public double TargetAbsorbanceMin
        {
            get { return _targetAbsorbanceMin; }
            set
            {
                if (this._targetAbsorbanceMin != value)
                {
                    this._targetAbsorbanceMin = value;
                    this.RaisePropertyChanged(nameof(TargetAbsorbanceMin));
                }
            }
        }

        private double _targetAbsorbanceMax;
        public double TargetAbsorbanceMax
        {
            get { return _targetAbsorbanceMax; }
            set
            {
                if (this._targetAbsorbanceMax != value)
                {
                    this._targetAbsorbanceMax = value;
                    this.RaisePropertyChanged(nameof(TargetAbsorbanceMax));
                }
            }
        }

        //private bool _isFlameout;
        //public bool IsFlameout
        //{
        //    get { return _isFlameout; }
        //    set
        //    {
        //        if (this._isFlameout != value)
        //        {
        //            this._isFlameout = value;
        //        }

        //        this.RaisePropertyChanged(nameof(IsFlameout));
        //    }
        //}

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

        private bool _isAutoStop;
        public bool IsAutoStop
        {
            get { return _isAutoStop; }
            set
            {
                if (this._isAutoStop != value)
                {
                    this._isAutoStop = value;
                    this.RaisePropertyChanged(nameof(IsAutoStop));
                }
            }
        }

        private bool _isManualStop;
        public bool IsManualStop
        {
            get { return _isManualStop; }
            set
            {
                if (this._isManualStop != value)
                {
                    this._isManualStop = value;
                    this.RaisePropertyChanged(nameof(IsManualStop));
                }
            }
        }
        //###################################################################
        //  Abstract
        //###################################################################



        //###################################################################
        //  Virtual
        //###################################################################

        public virtual void Start()
        {
            DeviceManager.Current.StartDevice(this.Inverter);
        }

        public virtual void Stop()
        {
            DeviceManager.Current.StopDevice();
        }

        public virtual void Clear()
        {
#if IS_LOCAL
            this._factoryIndex = 0;
#endif

            //this.IsFlameout = false;
            this.IsValidResult = false;
            

            this.SeriesAbsorbance.Clear();
            this.SeriesTransmission.Clear();
            this.SeriesChamberTC.Clear();
            this.SeriesFanFlowrate.Clear();

            this.SeriesMaxAbsorbance.Clear();
            this.SeriesMaxAbsorbance0.Clear();
            this.SeriesTransmissionVoltage.Clear();

            this.MaxAbsorbance = 0;
            this.MinTransmission = 100;

            this.CurrentChamberTC = 0;
            this.CurrentFanFlowrate = 0;
            this.CurrentAbsorbance = 0;
            this.CurrentTransmission = 0;
            this.CorrectedAbsorbance = 0;
            this.MinTransmissionSecond = 0;

            //this.FlameoutSecond = 0;
            this.StartChamberTemperature = 0;
        }

        public virtual void CheckTestData()
        {
            double transmissionFlow, absorbanceFlow, chamberTC, fanFlowrate;

#if IS_LOCAL
            transmissionFlow = _factory.TransmissionData[this._factoryIndex];
            absorbanceFlow = _factory.AbsorbanceData[this._factoryIndex];
            chamberTC = _factory.ChamberTCData[this._factoryIndex];
            fanFlowrate = GetRandomFanFlowrate();

            this._factoryIndex++;
#else
            chamberTC = DeviceManager.Current.AnalogInput.ChamberTemperature;            
            fanFlowrate = DeviceManager.Current.AnalogInput.FanFlowrate;
            absorbanceFlow = DeviceManager.Current.AnalogInput.Absorbance;
            transmissionFlow = DeviceManager.Current.AnalogInput.Transmission;            
#endif

            this.CurrentChamberTC = chamberTC;
            this.CurrentFanFlowrate = fanFlowrate;
            this.CurrentAbsorbance = absorbanceFlow;
            this.CurrentTransmission = transmissionFlow;

            this.SeriesChamberTC.AddPoint(chamberTC);
            this.SeriesTransmission.AddPoint(transmissionFlow);
            this.SeriesAbsorbance.AddPoint(absorbanceFlow);
            this.SeriesFanFlowrate.AddPoint(fanFlowrate);

            this.MaxAbsorbance = this.SeriesAbsorbance.GetValueList().Max();
            this.MinTransmission = this.SeriesTransmission.GetValueList().Min(v => v <= 0 ? 0.0001 : v);
            
            int minindex = this.SeriesTransmission.GetValueList().IndexOf(this.MinTransmission) - 1;
            if (minindex < 0) minindex = 0;
            this.MinTransmissionSecond = minindex;

            this.SeriesTransmissionVoltage.AddPoint(DeviceManager.Current.AnalogInput.TransmissionVoltage);

            double max_absorbance = LightMeasurementUtils.CalculateMaxAbsorbance(DbChannel.AI_LIGHT_PHOTODIODE.MaxVoltage, this.SeriesTransmissionVoltage.GetValueList());            
            this.SeriesMaxAbsorbance.AddPoint(max_absorbance);

            double max_absorbance0 = this.SeriesAbsorbance.GetValueList().Max();
            this.SeriesMaxAbsorbance0.AddPoint(max_absorbance0);

            if (this.IsQualification)
            {
                double chamberVolume = 0;
                double lightPathLength = 0;
                string tempValue = string.Empty;

                tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_CHAMBER_VOLUME);
                if (tempValue == null || tempValue == string.Empty)
                {
                    chamberVolume = IEC61034Const.DEFAULT_CHAMBER_VOLUME;
                }
                else
                {
                    chamberVolume = Convert.ToDouble(tempValue);
                }

                tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_LIGHT_PATH);
                if (tempValue == null || tempValue == string.Empty)
                {
                    lightPathLength = IEC61034Const.DEFAULT_LIGHT_PATH;
                }
                else
                {
                    lightPathLength = Convert.ToDouble(tempValue);
                }

                if (SessionManager.Current.IEC61034_DataSetQualification.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_4) { 
                    this.CorrectedAbsorbance = LightMeasurementUtils.CalculateCorrectedAbsorbance(max_absorbance, 4.0, chamberVolume, lightPathLength);

                    if (this.CorrectedAbsorbance >= this.TargetAbsorbanceMin && this.CorrectedAbsorbance <= this.TargetAbsorbanceMax)
                    {
                        this.IsValidResult = true;
                    }
                    else
                    {
                        this.IsValidResult = false;
                    }
                }
                else if (SessionManager.Current.IEC61034_DataSetQualification.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_10)
                {
                    this.CorrectedAbsorbance = LightMeasurementUtils.CalculateCorrectedAbsorbance(max_absorbance, 10.0, chamberVolume, lightPathLength);

                    if (this.CorrectedAbsorbance >= this.TargetAbsorbanceMin && this.CorrectedAbsorbance <= this.TargetAbsorbanceMax)
                    {
                        this.IsValidResult = true;
                    }
                    else
                    {
                        this.IsValidResult = false;
                    }
                }
            }
            else
            {
                /*
                if (this.IsFlameout)
                {
                    int stableDuration = 0;
                    double stableTolerance = 0;
                    string tempValue = string.Empty;

                    tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TRANSMISSION_STABILITY_DURATION);
                    if (tempValue == null || tempValue == string.Empty)
                    {
                        stableDuration = IEC61034Const.FLAMEOUT_STABILIZATION_DURATION;
                    }
                    else
                    {
                        stableDuration = Convert.ToInt16(tempValue);
                    }

                    tempValue = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_TRANSMISSION_STABILITY_TOLERANCE);
                    if (tempValue == null || tempValue == string.Empty)
                    {
                        stableTolerance = IEC61034Const.TRANSMISSION_STABILITY_TOLERANCE;
                    }
                    else
                    {
                        stableTolerance = Convert.ToDouble(tempValue);
                    }

                    // 불꽃이 꺼진 후 5분 경과 
                    if (this.SeriesTransmission.SeriesCollection.Count > this.FlameoutSecond + stableDuration * 60)
                    {
                        var recentValues = this.SeriesTransmission.GetValueListByLastN(stableDuration * 60);
                        if ((recentValues != null) && (recentValues.Count >= stableDuration * 60))
                        {
                            if (LightMeasurementUtils.CheckTransmissionStabilized(recentValues, stableTolerance, stableDuration))
                            {
                                if (this.IsAutoStop && this.StopAction != null)
                                {
                                    Console.WriteLine(string.Format("불꺼진후 안정화 종료"));
                                    this.StopAction();
                                    return;
                                }
                            }
                        }
                    }
                }
                */
            }

            Console.WriteLine(string.Format("{0} : {1}", this.IsAutoStop.ToString(), this.MaxTestDurationMinute));

            if (this.IsAutoStop)
            {
                if (this.SeriesTransmission.SeriesCollection.Count > (this.MaxTestDurationMinute * 60) + 1)
                {
                    if (this.StopAction != null)
                    {
                        this.StopAction();
                        return;
                    }
                }
            }

#if IS_LOCAL
            if (this._factoryIndex >= _factory.TransmissionData.Count)
            {
                this._factoryIndex = this._factoryIndex - 10;

                if (this.IsAutoStop && this.StopAction != null)
                {
                    this.StopAction();
                    return;
                }

                return;
            }
#endif
        }



        //###################################################################
        //  Public
        //###################################################################



        //###################################################################
        //  private
        //###################################################################

#if IS_LOCAL
        private Random random = new Random();

        public double GetRandomFanFlowrate()
        {
            Random rand = new Random();
            double value = rand.NextDouble() * (15.0 - 7.0) + 7.0;
            return Math.Round(value, 2);
        }
#endif
    }
}
