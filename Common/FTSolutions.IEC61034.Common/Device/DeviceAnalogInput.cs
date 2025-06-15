using eccFramework.SharedLib.Core.Device.DAQ;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;

namespace FTSolutions.IEC61034.Common.Device
{
    public delegate void SetINInfo(double flowrate, double transmission, double tcChamber);

    public class DeviceAnalogInput : BaseIEC61034Device
    {
        public event Action<string, string> ShowMessageHandler;

        private SetINInfo _setFunc;
        NIAnalogInput _daq = null;

        public DeviceAnalogInput()
        {
            _setFunc = SetINValue;            
        }


        //###################################################################
        //  Property
        //###################################################################

        private double _absorbance;
        public double Absorbance
        {
            get { return _absorbance; }
            set
            {
                if (this._absorbance != value)
                {
                    this._absorbance = value;
                    this.RaisePropertyChanged(nameof(Absorbance));
                }
            }
        }
        
        private double _transmission;
        public double Transmission
        {
            get { return _transmission; }
            set
            {
                if (this._transmission != value)
                {
                    this._transmission = value;
                    this.RaisePropertyChanged(nameof(Transmission));
                }
            }
        }

        private double _lightTransmission;
        public double LightTransmission
        {
            get { return _lightTransmission; }
            set
            {
                if (this._lightTransmission != value)
                {
                    this._lightTransmission = value;
                    this.RaisePropertyChanged(nameof(LightTransmission));
                }
            }
        }

        private double _correctionTransmission;
        public double CorrectionTransmission
        {
            get { return _correctionTransmission; }
            set
            {
                if (this._correctionTransmission != value)
                {
                    this._correctionTransmission = value;
                    this.RaisePropertyChanged(nameof(CorrectionTransmission));
                }
            }
        }
        

        private double _fanFlowrate;
        public double FanFlowrate
        {
            get { return _fanFlowrate; }
            set
            {
                if (this._fanFlowrate != value)
                {
                    this._fanFlowrate = value;
                    this.RaisePropertyChanged(nameof(FanFlowrate));
                }
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
                    this.RaisePropertyChanged(nameof(ChamberTemperature));
                }
            }
        }

        private double _transmissionVoltage;
        public double TransmissionVoltage
        {
            get { return _transmissionVoltage; }
            set
            {
                if (this._transmissionVoltage != value)
                {
                    this._transmissionVoltage = value;
                    this.RaisePropertyChanged(nameof(TransmissionVoltage));
                }
            }
        }

        private double _lightPhotoDiode;
        public double LightPhotoDiode
        {
            get { return _lightPhotoDiode; }
            set
            {
                if (this._lightPhotoDiode != value)
                {
                    this._lightPhotoDiode = value;
                    this.RaisePropertyChanged(nameof(LightPhotoDiode));
                }
            }
        }

        /*
        private double _coefficient4;
        public double Coefficient4
        {
            get { return _coefficient4; }
            set
            {
                if (this._coefficient4 != value)
                {
                    this._coefficient4 = value;
                    this.RaisePropertyChanged(nameof(Coefficient4));
                }
            }
        }


        private double _coefficient3;
        public double Coefficient3
        {
            get { return _coefficient3; }
            set
            {
                if (this._coefficient3 != value)
                {
                    this._coefficient3 = value;
                    this.RaisePropertyChanged(nameof(Coefficient3));
                }
            }
        }


        private double _coefficient2;
        public double Coefficient2
        {
            get { return _coefficient2; }
            set
            {
                if (this._coefficient2 != value)
                {
                    this._coefficient2 = value;
                    this.RaisePropertyChanged(nameof(Coefficient2));
                }
            }
        }

        private double _coefficient1;
        public double Coefficient1
        {
            get { return _coefficient1; }
            set
            {
                if (this._coefficient1 != value)
                {
                    this._coefficient1 = value;
                    this.RaisePropertyChanged(nameof(Coefficient1));
                }
            }
        }

        private double _yIntercept;
        public double YIntercept
        {
            get { return _yIntercept; }
            set
            {
                if (this._yIntercept != value)
                {
                    this._yIntercept = value;
                    this.RaisePropertyChanged(nameof(YIntercept));
                }
            }
        }
        */
        //###################################################################
        //  Override
        //###################################################################

        public override void Run()
        {
            try
            {
                if (_daq == null)
                {
                    _daq = new NIAnalogInput(AnalysisData, IEC61034Const.SAMPLING_RATE, IEC61034Const.BUFFER_SIZE, IEC61034Const.SAMPLE_PER_CHANNEL, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);

                    _daq.AddChannel(DbChannel.AI_INVERTER.Address, DbChannel.AI_INVERTER.Channel, AITerminalConfiguration.Differential, DbChannel.AI_INVERTER.MinVoltage, DbChannel.AI_INVERTER.MaxVoltage, AIVoltageUnits.Volts);
                    _daq.AddChannel(DbChannel.AI_LIGHT_PHOTODIODE.Address, DbChannel.AI_LIGHT_PHOTODIODE.Channel, AITerminalConfiguration.Differential, DbChannel.AI_LIGHT_PHOTODIODE.MinVoltage, DbChannel.AI_LIGHT_PHOTODIODE.MaxVoltage, AIVoltageUnits.Volts);
                    _daq.AddChannel(DbChannel.AI_CHAMBER_TC.Address, DbChannel.AI_CHAMBER_TC.Channel, AITerminalConfiguration.Differential, DbChannel.AI_CHAMBER_TC.MinVoltage, DbChannel.AI_CHAMBER_TC.MaxVoltage, AIVoltageUnits.Volts);
                }

                _daq.ReadAnalogInputContinuous();

                if (this.CheckDAQService != null)
                {
                    this.CheckDAQService(true, null);
                }
            }
            catch (Exception ex)
            {
                if (this.CheckDAQService != null)
                {
                    this.CheckDAQService(false, ex);
                }
            }
        }

        public override void Stop()
        {
            if(_daq != null)
            {
                _daq.StopAnalogInputContinuous();
            }
        }

        public override void Clear()
        {
            this.Absorbance = 0;
            this.FanFlowrate = 0;
            this.Transmission = 0;
            this.ChamberTemperature = 0;
        }



        //###################################################################
        //  Public
        //###################################################################


        public void SetINValue(double flowrate, double transmission, double tcChamber)
        {
            this.FanFlowrate = flowrate;
            this.Transmission = transmission;
            this.ChamberTemperature = tcChamber;

            this.Absorbance = LightMeasurementUtils.CalculateAbsorbanceFromVoltage(this.TransmissionVoltage);

            //Console.WriteLine(string.Format("흡수율 -> {0}", this.Absorbance));
        }

        //###################################################################
        //  Private
        //###################################################################

        private void AnalysisData(double[,] values)
        {
            List<double> valueList = new List<double>();

            this.ClearLogBuilder();
            this.AppendLog("[AnalogIn]");

            for (int row = 0; row < values.GetLength(0); row++)
            {
                valueList.Clear();

                for (int column = 0; column < values.GetLength(1); column++)
                {
                    valueList.Add(values[row, column]);
                }

                double voltage = GenerateAvgValue(valueList, IEC61034Const.UPPER_REMOVE_PERCENTAGE, IEC61034Const.LOWER_REMOVE_PERCENTAGE);

                switch (row)
                {
                    case 0:
                        this.FanFlowrate = this.ConvertVoltageToTargetValue(voltage, DbChannel.AI_INVERTER, 1);
                        this.AppendLog($"FanFlowrate:{voltage}^{this.FanFlowrate},");

                        //Console.WriteLine(string.Format("FanFlowrate -> {0} : {1}", voltage, this.FanFlowrate));
                        break;
                    case 1:
                        this.LightPhotoDiode = voltage;
                        this.TransmissionVoltage = voltage;
                        this.Transmission = this.ConvertVoltageToTargetValue(voltage, DbChannel.AI_LIGHT_PHOTODIODE, 1);
                        this.LightTransmission = Transmission;
                        this.CorrectionTransmission = CorrectedTransmission(this.Transmission);

                        //this.Transmission = Math.Round(this.ConvertVoltageToTargetValue(voltage, DbChannel.AI_LIGHT_PHOTODIODE.MinVoltage, DbChannel.AI_LIGHT_PHOTODIODE.MaxVoltage, DbChannel.AI_LIGHT_PHOTODIODE.MinValue, DbChannel.AI_LIGHT_PHOTODIODE.MaxValue), 2, MidpointRounding.AwayFromZero);
                        this.AppendLog($"Transmission:{voltage}^{this.Transmission},");

                        //Console.WriteLine(string.Format("Transmission -> {0} : {1}", voltage, this.Transmission));
                        break;
                    case 2:
                        this.ChamberTemperature = this.ConvertVoltageToTargetValue(voltage, DbChannel.AI_CHAMBER_TC, 1);
                        this.AppendLog($"ChamberTemperature:{voltage}^{this.ChamberTemperature},");

                        //Console.WriteLine(string.Format("ChamberTemperature -> {0} : {1}", voltage, this.ChamberTemperature));
                        break;
                }
            }

            this._setFunc(this.FanFlowrate, this.Transmission, this.ChamberTemperature);

            this.WriteDevice(this.LogBuilder.ToString());
        }

        public double CorrectedTransmission(double transmission)
        {
            return Math.Round(SessionManager.Current.LightPolynomialInfo.Coefficient4 * Math.Pow(transmission, 4) + SessionManager.Current.LightPolynomialInfo.Coefficient3 * Math.Pow(transmission, 3) +
                SessionManager.Current.LightPolynomialInfo.Coefficient2 * Math.Pow(transmission, 2) + SessionManager.Current.LightPolynomialInfo.Coefficient1 * transmission + SessionManager.Current.LightPolynomialInfo.YIntercept, 2, MidpointRounding.AwayFromZero);
        }
    }
}
