using System;

namespace FTSolutions.IEC61034.Common.Setting
{
    public class IniConfig
    {
        public static string DatabasePath = "";

        public static string AllowCustNo = "";

        public static string AllowDeviceLog = "";
        public static string AllowRealtimeLog = "";
        public static string AllowEventLog = "";

        public static string AllowIEC61034 = "";

        public static string ChamberTCSecond = "";

        public static string DisplyRPTTestCondition = "";

        public static bool IsAllowCustNo { get { return AllowCustNo.Equals("1"); } }

        public static bool IsAllowDeviceLog { get { return AllowDeviceLog.Equals("1"); } }
        public static bool IsAllowRealtimeLog { get { return AllowRealtimeLog.Equals("1"); } }
        public static bool IsAllowEventLog { get { return AllowEventLog.Equals("1"); } }

        public static bool IsAllowIEC61034 { get { return AllowIEC61034.Equals("1"); } }

        public static int ChamberTCSecondValue { get { return Convert.ToInt16(ChamberTCSecond); } }

        public static bool IsDisplyRPTTestCondition { get { return DisplyRPTTestCondition.Equals("1"); } }
    }
}
