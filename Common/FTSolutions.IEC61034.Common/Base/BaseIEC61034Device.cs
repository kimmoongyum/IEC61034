using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Utility.Helper;
using FTSolutions.IEC61034.Common.Setting;
using System;

namespace FTSolutions.IEC61034.Common.Base
{
    public class BaseIEC61034Device : BaseDevice
    {
        public Action<bool, Exception> CheckDAQService;


        public BaseIEC61034Device()
        {

        }



        public override void Clear()
        {

        }

        public override void Run()
        {

        }

        public override void Stop()
        {

        }


        public void WriteDevice(string msg)
        {
            if (IniConfig.IsAllowDeviceLog)
            {
                LogIt.WriteDevice(msg);
            }
        }
    }
}
