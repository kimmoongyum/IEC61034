using eccFramework.SharedLib.GlobalType.Protocol;
using eccFramework.SharedLib.Utility.InteropService;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.DataType.SessionType;
using FTSolutions.IEC61034.Common.QueryService;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FTSolutions.IEC61034.Common.Setting
{
    public class SettingManager
    {
        public static string ConnectionStringSystemDB()
        {
            string dbFullName = Environment.CurrentDirectory + IniConfig.DatabasePath + IEC61034Const.SYSTEM_DATABASE_NAME;
            return String.Format("Data Source={0};Version=3;", dbFullName);
        }

        //###################################################################
        //   Public
        //###################################################################

        public static void ReadDefaultData()
        {
            ReadConfigIni();

            ReadChannelDatabase();
            ReadConfigDatabase();
            ReadCalibrationValue();

            SessionManager.Current.BindingLogoImage();
        }


        public static void ReadConfigIni()
        {
            string iniConfigPath = String.Format("{0}\\FTSolutions_config.ini", Application.StartupPath);
            IniFile iniConfigFile = new IniFile(iniConfigPath);

            IniConfig.DatabasePath = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.DatabasePath), IniConfig.DatabasePath);

            IniConfig.AllowCustNo = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.AllowCustNo), IniConfig.AllowCustNo);

            IniConfig.AllowDeviceLog = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.AllowDeviceLog), IniConfig.AllowDeviceLog);
            IniConfig.AllowRealtimeLog = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.AllowRealtimeLog), IniConfig.AllowRealtimeLog);
            IniConfig.AllowEventLog = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.AllowEventLog), IniConfig.AllowEventLog);

            IniConfig.AllowIEC61034 = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.AllowIEC61034), IniConfig.AllowIEC61034);

            IniConfig.ChamberTCSecond = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.ChamberTCSecond), IniConfig.ChamberTCSecond);

            IniConfig.DisplyRPTTestCondition = iniConfigFile.ReadValue(IEC61034Const.INI_CONFIG_SECTION, nameof(IniConfig.DisplyRPTTestCondition), IniConfig.DisplyRPTTestCondition);            
        }

        public static void ReadChannelDatabase()
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceSystem.SearchChannelSetting();

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                TypeChannelSettingCollection result = new TypeChannelSettingCollection();
                result.LoadData(dtResult);

                DbChannel.AI_LIGHT_PHOTODIODE = result.Where(t => t.Channel.ToUpper().Equals("AI_LIGHT_PHOTODIODE")).FirstOrDefault();
                DbChannel.AI_CHAMBER_TC = result.Where(t => t.Channel.ToUpper().Equals("AI_CHAMBER_TC")).FirstOrDefault();
                DbChannel.AI_INVERTER = result.Where(t => t.Channel.ToUpper().Equals("AI_INVERTER")).FirstOrDefault();

                DbChannel.AO_INVERTER = result.Where(t => t.Channel.ToUpper().Equals("AO_INVERTER")).FirstOrDefault();

                DbChannel.DI_OUT_DAMPER = result.Where(t => t.Channel.ToUpper().Equals("DI_OUT_DAMPER")).FirstOrDefault();
                DbChannel.DI_IN_DAMPER = result.Where(t => t.Channel.ToUpper().Equals("DI_IN_DAMPER")).FirstOrDefault();

                DbChannel.DI_FILTER_CLEAR = result.Where(t => t.Channel.ToUpper().Equals("DI_FILTER_CLEAR")).FirstOrDefault();
                DbChannel.DI_ND_FILTER_01 = result.Where(t => t.Channel.ToUpper().Equals("DI_ND_FILTER_01")).FirstOrDefault();
                DbChannel.DI_ND_FILTER_02 = result.Where(t => t.Channel.ToUpper().Equals("DI_ND_FILTER_02")).FirstOrDefault();
                DbChannel.DI_ND_FILTER_03 = result.Where(t => t.Channel.ToUpper().Equals("DI_ND_FILTER_03")).FirstOrDefault();
                DbChannel.DI_ND_FILTER_05 = result.Where(t => t.Channel.ToUpper().Equals("DI_ND_FILTER_05")).FirstOrDefault();
                DbChannel.DI_ND_FILTER_07 = result.Where(t => t.Channel.ToUpper().Equals("DI_ND_FILTER_07")).FirstOrDefault();
                DbChannel.DI_ND_FILTER_10 = result.Where(t => t.Channel.ToUpper().Equals("DI_ND_FILTER_10")).FirstOrDefault();
                DbChannel.DI_FILTER_DARK = result.Where(t => t.Channel.ToUpper().Equals("DI_FILTER_DARK")).FirstOrDefault();

                DbChannel.DO_FILTER_CLEAR = result.Where(t => t.Channel.ToUpper().Equals("DO_FILTER_CLEAR")).FirstOrDefault();
                DbChannel.DO_ND_FILTER_01 = result.Where(t => t.Channel.ToUpper().Equals("DO_ND_FILTER_01")).FirstOrDefault();
                DbChannel.DO_ND_FILTER_02 = result.Where(t => t.Channel.ToUpper().Equals("DO_ND_FILTER_02")).FirstOrDefault();
                DbChannel.DO_ND_FILTER_03 = result.Where(t => t.Channel.ToUpper().Equals("DO_ND_FILTER_03")).FirstOrDefault();
                DbChannel.DO_ND_FILTER_05 = result.Where(t => t.Channel.ToUpper().Equals("DO_ND_FILTER_05")).FirstOrDefault();
                DbChannel.DO_ND_FILTER_07 = result.Where(t => t.Channel.ToUpper().Equals("DO_ND_FILTER_07")).FirstOrDefault();
                DbChannel.DO_ND_FILTER_10 = result.Where(t => t.Channel.ToUpper().Equals("DO_ND_FILTER_10")).FirstOrDefault();
                DbChannel.DO_FILTER_DARK = result.Where(t => t.Channel.ToUpper().Equals("DO_FILTER_DARK")).FirstOrDefault();

                DbChannel.DO_LIGHT = result.Where(t => t.Channel.ToUpper().Equals("DO_LIGHT")).FirstOrDefault();
                DbChannel.DO_IGNITOR = result.Where(t => t.Channel.ToUpper().Equals("DO_IGNITOR")).FirstOrDefault();
                DbChannel.DO_FAN = result.Where(t => t.Channel.ToUpper().Equals("DO_FAN")).FirstOrDefault();                
                DbChannel.DO_OUT_DAMPER = result.Where(t => t.Channel.ToUpper().Equals("DO_OUT_DAMPER")).FirstOrDefault();
                DbChannel.DO_IN_DAMPER = result.Where(t => t.Channel.ToUpper().Equals("DO_IN_DAMPER")).FirstOrDefault();
                DbChannel.DO_LAMP = result.Where(t => t.Channel.ToUpper().Equals("DO_LAMP")).FirstOrDefault();
            }
        }

        public static void ReadConfigDatabase()
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceSystem.SearchConfigSetting();

            TypeConfigSettingCollection result = new TypeConfigSettingCollection();

            if (dtResult != null)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    string key = row["KEY"].ToString();
                    string value = row["VALUE"].ToString();

                    result.Add(new TypeConfigSetting(key, value, row["DESC"].ToString()));
                }
            }

            SessionManager.Current.BindingConfigSetting(result);
        }

        public static void WriteConfigDatabase(string key, string value)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("key", DbType.String, key));
            paramInfo.Add(new SQLiteParamInfo("value", DbType.String, value));

            ManagerQueryService.Current.QueryServiceSystem.UpdateConfigSingleValue(paramInfo);
        }


        public static void WriteChannelMinVoltageInfoInDatabase(string channel, string voltage, string gradientVoltage, string interceptVoltage)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("channel", DbType.String, channel));
            paramInfo.Add(new SQLiteParamInfo("minVoltage", DbType.String, voltage));
            paramInfo.Add(new SQLiteParamInfo("gradientVoltage", DbType.String, gradientVoltage));
            paramInfo.Add(new SQLiteParamInfo("interceptVoltage", DbType.String, interceptVoltage));

            ManagerQueryService.Current.QueryServiceSystem.UpdateChannelMinVoltageInfo(paramInfo);
        }

        public static void WriteChannelMaxVoltageInfoInDatabase(string channel, string voltage, string gradientVoltage, string interceptVoltage)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("channel", DbType.String, channel));
            paramInfo.Add(new SQLiteParamInfo("maxVoltage", DbType.String, voltage));
            paramInfo.Add(new SQLiteParamInfo("gradientVoltage", DbType.String, gradientVoltage));
            paramInfo.Add(new SQLiteParamInfo("interceptVoltage", DbType.String, interceptVoltage));

            ManagerQueryService.Current.QueryServiceSystem.UpdateChannelMaxVoltageInfo(paramInfo);
        }

        public static void WriteChannelMinVoltageInfoInDatabase(string channel, string voltage, string gradientVoltage, string interceptVoltage, string gradientValue, string interceptValue)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("channel", DbType.String, channel));
            paramInfo.Add(new SQLiteParamInfo("minVoltage", DbType.String, voltage));
            paramInfo.Add(new SQLiteParamInfo("gradientVoltage", DbType.String, gradientVoltage));
            paramInfo.Add(new SQLiteParamInfo("interceptVoltage", DbType.String, interceptVoltage));
            paramInfo.Add(new SQLiteParamInfo("gradientValue", DbType.String, gradientValue));
            paramInfo.Add(new SQLiteParamInfo("interceptValue", DbType.String, interceptValue));

            ManagerQueryService.Current.QueryServiceSystem.UpdateChannelMinVoltageInfo2(paramInfo);
        }

        public static void WriteChannelMaxVoltageInfoInDatabase(string channel, string voltage, string gradientVoltage, string interceptVoltage, string gradientValue, string interceptValue)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("channel", DbType.String, channel));
            paramInfo.Add(new SQLiteParamInfo("maxVoltage", DbType.String, voltage));
            paramInfo.Add(new SQLiteParamInfo("gradientVoltage", DbType.String, gradientVoltage));
            paramInfo.Add(new SQLiteParamInfo("interceptVoltage", DbType.String, interceptVoltage));
            paramInfo.Add(new SQLiteParamInfo("gradientValue", DbType.String, gradientValue));
            paramInfo.Add(new SQLiteParamInfo("interceptValue", DbType.String, interceptValue));

            ManagerQueryService.Current.QueryServiceSystem.UpdateChannelMaxVoltageInfo2(paramInfo);
        }

        public static void ReadCalibrationValue()
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceSystem.SearchCalibrationValue();

            TypePolynomial polynomial = new TypePolynomial();

            if (dtResult != null)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    string key = row["KEY"].ToString();
                    string value = row["VALUE"].ToString();

                    if (key.Equals(NDFilterKey.ND_POLYNOMIAL.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        polynomial.Polynomial = Convert.ToInt16(value);
                    }

                    if (key.Equals(NDFilterKey.ND_1ST_TERM.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        polynomial.Coefficient1 = Convert.ToDouble(value);
                    }

                    if (key.Equals(NDFilterKey.ND_2ND_TERM.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        polynomial.Coefficient2 = Convert.ToDouble(value);
                    }

                    if (key.Equals(NDFilterKey.ND_3RD_TERM.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        polynomial.Coefficient3 = Convert.ToDouble(value);
                    }

                    if (key.Equals(NDFilterKey.ND_4TH_TERM.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        polynomial.Coefficient4 = Convert.ToDouble(value);
                    }

                    if (key.Equals(NDFilterKey.ND_INTERCEPT.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        polynomial.YIntercept = Convert.ToDouble(value);
                    }
                }
            }

            SessionManager.Current.SetLightPolynomialInfo(polynomial);
        }
    }
}
