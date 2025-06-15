using eccFramework.SharedLib.Core.Device.DAQ;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;

namespace FTSolutions.IEC61034.Common.Device
{
    public class AnalogInFAN : BaseIEC61034Device
    {
        private SetFANInfo _setFunc;

        NIAnalogInput _daq = null;

        public AnalogInFAN(SetFANInfo setFunc)
        {
            this._setFunc = setFunc;
        }

        //###################################################################
        //  Property
        //###################################################################

        public double Flowrate { get; set; }


        //###################################################################
        //  Public
        //###################################################################

        public override void Run()
        {
            try
            {
                if (_daq == null)
                {
                    _daq = new NIAnalogInput(AnalysisData, IEC61034Const.SAMPLING_RATE, IEC61034Const.BUFFER_SIZE, IEC61034Const.SAMPLE_PER_CHANNEL, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);

                    _daq.AddChannel(DbChannel.AI_INVERTER.Address, DbChannel.AI_INVERTER.Channel, AITerminalConfiguration.Differential, DbChannel.AI_INVERTER.MinVoltage, DbChannel.AI_INVERTER.MaxVoltage, AIVoltageUnits.Volts);
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
            _daq.StopAnalogInputContinuous();
        }

        public override void Clear()
        {
            this.Flowrate = 0;
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
                        this.Flowrate = this.ConvertVoltageToTargetValue(voltage, DbChannel.AI_INVERTER, 2);
                        this.AppendLog($"FanFlowrate:{voltage}^{this.Flowrate},");
                        //Console.WriteLine(string.Format("Transmission : {0} -> {1} : {2}", voltage, Transmission, this.ConvertTargetValueToVoltage(Transmission, DbChannel.AI_PHOTODIODE)));
                        break;
                }
            }

            this._setFunc(this.Flowrate);

            this.WriteDevice(this.LogBuilder.ToString());
        }
    }
}
