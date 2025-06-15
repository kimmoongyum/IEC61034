using eccFramework.SharedLib.Core.Device.DAQ;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.Setting;
using NationalInstruments.DAQmx;
using System;

namespace FTSolutions.IEC61034.Common.Device
{
    public class DeviceAnalogOutput : BaseIEC61034Device
    {
        public event Action<string, string> ShowMessageHandler;

        NIAnalogOutput _daq = null;


        public DeviceAnalogOutput()
        {

        }


        //###################################################################
        //  Public
        //###################################################################

        public void WriteInverter(double value)
        {
            try
            {
                if (this._daq == null)
                {
                    this._daq = new NIAnalogOutput();
                }

                double targetVoltage = this._daq.WriteValue(value, DbChannel.AO_INVERTER, AOVoltageUnits.Volts);

                this.WriteDevice($"[AnalogOut]Inverter:{value}^{targetVoltage}");

                Console.WriteLine(string.Format("[AnalogOut]Inverter -> {0}", value));
            }
            catch (DaqException ex)
            {
                this.ShowMessageHandler("Error", ex.Message);
            }
        }
    }
}
