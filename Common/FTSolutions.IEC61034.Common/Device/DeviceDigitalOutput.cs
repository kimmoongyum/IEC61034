using eccFramework.SharedLib.Core.Device.DAQ;
using eccFramework.SharedLib.GlobalType.Protocol;
using eccFramework.SharedLib.Utility.Services;
using FTSolutions.IEC61034.Common.Base;
using System;

namespace FTSolutions.IEC61034.Common.Device
{
    public class DeviceDigitalOutput : BaseIEC61034Device
    {
        public event Action<string, string> ShowMessageHandler;

        NIDigitalOutput _daq = null;


        public DeviceDigitalOutput()
        {

        }



        //###################################################################
        //  Public
        //###################################################################

        public void WriteDO(ChannelInfo channel, bool isOn)
        {
            if (this._daq == null)
            {
                this._daq = new NIDigitalOutput();
            }

            this.WriteDevice($"[DigitalOut]{channel.Channel}:{channel.Address},IsOn:{isOn}");

            this._daq.ChangeState(channel.Address, isOn);
        }
    }
}
