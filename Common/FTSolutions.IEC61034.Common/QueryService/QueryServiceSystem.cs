using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.Setting;
using System.Data;

namespace FTSolutions.IEC61034.Common.QueryService
{
    public class QueryServiceSystem : BaseQueryService
    {
        public QueryServiceSystem()
        {

        }



        //###################################################################
        //  Public
        //###################################################################

        #region ConfigSetting.
        public DataTable SearchConfigSetting()
        {
            string sql = "SELECT * FROM CONFIG_SETTING ORDER BY DISPLAY_INDEX, KEY ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }

        public DataTable SearchCalibrationValue()
        {
            string sql = "SELECT * FROM CONFIG_SETTING WHERE \"KEY\" IN ('ND_POLYNOMIAL', 'ND_1ST_TERM', 'ND_2ND_TERM', 'ND_3RD_TERM', 'ND_4TH_TERM', 'ND_INTERCEPT') ORDER BY DISPLAY_INDEX ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }

        public void UpdateConfigSetting(SQLiteParamInfoBatch paramBatchInfo)
        {
            string sql = " UPDATE CONFIG_SETTING ";
            sql += " SET VALUE = :value, DESC = :desc, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE KEY = :key ";

            this.ExecuteNonSelectBatchQuery(SettingManager.ConnectionStringSystemDB(), sql, paramBatchInfo);
        }

        public void UpdateConfigSingleValue(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE CONFIG_SETTING ";
            sql += " SET VALUE = :value, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE KEY = :key ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }
        #endregion

        #region ChannelSetting.
        public DataTable SearchChannelSetting()
        {
            string sql = "SELECT * FROM CHANNEL_SETTING ORDER BY DISPLAY_INDEX, ADDRESS ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }

        public void UpsertChannelSetting(SQLiteParamInfoBatch paramBatchInfo)
        {
            string sql = " INSERT INTO CHANNEL_SETTING (CHANNEL, ADDRESS, MIN_VOLTAGE, MAX_VOLTAGE, GRADIENT_VOLTAGE, INTERCEPT_VOLTAGE, MIN_VALUE, MAX_VALUE, GRADIENT_VALUE, INTERCEPT_VALUE, DESC) ";
            sql += " VALUES(:channel, :address, :minVoltage, :maxVoltage, :gradientVoltage, :interceptVoltage, :minValue, :maxValue, :gradientValue, :interceptValue, :desc) ";
            sql += " ON CONFLICT(CHANNEL) ";
            sql += " DO UPDATE ";
            sql += " SET ADDRESS = :address, DESC = :desc, MIN_VOLTAGE = :minVoltage, MAX_VOLTAGE = :maxVoltage, ";
            sql += "     GRADIENT_VOLTAGE = :gradientVoltage, INTERCEPT_VOLTAGE = :interceptVoltage, MIN_VALUE = :minValue, MAX_VALUE = :maxValue, GRADIENT_VALUE = :gradientValue, INTERCEPT_VALUE = :interceptValue, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE CHANNEL = :channel ";

            this.ExecuteNonSelectBatchQuery(SettingManager.ConnectionStringSystemDB(), sql, paramBatchInfo);
        }


        public void UpdateChannelMinVoltageInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE CHANNEL_SETTING ";
            sql += " SET MIN_VOLTAGE = :minVoltage, GRADIENT_VOLTAGE = :gradientVoltage, INTERCEPT_VOLTAGE = :interceptVoltage ";
            sql += " WHERE CHANNEL = :channel ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void UpdateChannelMaxVoltageInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE CHANNEL_SETTING ";
            sql += " SET MAX_VOLTAGE = :maxVoltage, GRADIENT_VOLTAGE = :gradientVoltage, INTERCEPT_VOLTAGE = :interceptVoltage ";
            sql += " WHERE CHANNEL = :channel ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void UpdateChannelMinVoltageInfo2(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE CHANNEL_SETTING ";
            sql += " SET MIN_VOLTAGE = :minVoltage, GRADIENT_VOLTAGE = :gradientVoltage, INTERCEPT_VOLTAGE = :interceptVoltage, GRADIENT_VALUE = :gradientValue, INTERCEPT_VALUE = :interceptValue ";
            sql += " WHERE CHANNEL = :channel ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void UpdateChannelMaxVoltageInfo2(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE CHANNEL_SETTING ";
            sql += " SET MAX_VOLTAGE = :maxVoltage, GRADIENT_VOLTAGE = :gradientVoltage, INTERCEPT_VOLTAGE = :interceptVoltage, GRADIENT_VALUE = :gradientValue, INTERCEPT_VALUE = :interceptValue ";
            sql += " WHERE CHANNEL = :channel ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }
        #endregion

        #region ND-Filter.
        public DataTable SearchNDFilters()
        {
            string sql = "SELECT * FROM ND_FILTER ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }

        public void UpdateNDFilters(SQLiteParamInfoBatch paramBatchInfo)
        {
            string sql = " UPDATE ND_FILTER ";
            sql += " SET CERTI_TRANSMISSION = :certi, MEASURE_TRANSMISSION = :measure, ERROR_TRANSMISSION = :error, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE FILTER_NO = :no ";

            this.ExecuteNonSelectBatchQuery(SettingManager.ConnectionStringSystemDB(), sql, paramBatchInfo);
        }

        public void UpdateCalibrationValue(SQLiteParamInfoBatch paramBatchInfo)
        {
            string sql = " UPDATE CONFIG_SETTING ";
            sql += " SET ND_POLYNOMIAL = :nd_polynomial, ND_1ST_TERM  = :nd_1st_term, ND_2ND_TERM = :nd_2nd_term, ND_3RD_TERM = :md_3rd_term, ND_4TH_TERM = :md_4th_term, ND_INTERCEPT = :md_omtercept, UPDATE_DT = datetime('now', 'localtime') ";

            this.ExecuteNonSelectBatchQuery(SettingManager.ConnectionStringSystemDB(), sql, paramBatchInfo);
        }
        #endregion
    }
}
