using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Data;

namespace FTSolutions.IEC61034.Common.QueryService
{
    public class QueryServiceQualification : BaseQueryService
    {
        public QueryServiceQualification()
        {

        }



        //###################################################################
        //  Public
        //###################################################################

        public DataTable SearchQualificationInfoSEQ(string regNo, string serialNo)
        {
            string sql = "SELECT SEQ FROM QUALIFICATION_INFO WHERE REG_NO = '" + regNo + "' AND SERIAL_NO = '" + serialNo + "' ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }


        public DataTable SearchQualificationInfo(string regNo)
        {
            string sql = "SELECT * FROM QUALIFICATION_INFO WHERE REG_NO = '" + regNo + "'";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }

        public void DeleteQualificationInfo(string regNo, string seq)
        {
            string sql = " DELETE FROM QUALIFICATION_DATA WHERE REG_NO = '" + regNo + "' AND TEST_INFO_SEQ = '" + seq + "'";

            this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);

            sql = " DELETE FROM QUALIFICATION_INFO WHERE REG_NO = '" + regNo + "' AND SEQ = '" + seq + "'";

            this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }


        public void InsertQualificationInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " INSERT INTO QUALIFICATION_INFO (REG_NO, TEST_DATE_TIME, TOLUENE_CONTENT, TARGET_ABSORBANCE_MIN, TARGET_ABSORBANCE_MAX, SERIAL_NO) ";
            sql += " VALUES(:reg_no, :test_date_time, :toluene_content, :target_absorbance_min, :target_absorbance_max, :serial_no) ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void InsertQualificationData(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " INSERT INTO QUALIFICATION_DATA (TEST_INFO_SEQ, REG_NO, START_CHAMBER_TEMPERATURE, CHAMBER_TC, FAN_FLOWRATE, TRANSMISSION, ";
            sql += " ABSORBANCE, MAX_ABSORBANCE, MAX_ABSORBANCE0, CORRECTED_ABSORBANCE, MINIMUM_TRANSMISSION, MINIMUM_TRANSMISSION_SECOND, MAXIMUM_ABSORBANCE, FLAMEOUT_SECOND, TEST_DURATION, SUMMARY_DESCRIPTION) ";
            sql += " VALUES(:qualification_info_seq, :reg_no, :start_chamber_temperature, :chamber_tc, :fan_flowrate, :transmission, ";
            sql += " :absorbance, :max_absorbance, :max_absorbance0, :corrected_absorbance, :minimum_transmission, :minimum_transmission_second, :maximum_absorbance, :flameout_second, :test_duration, :summary_description) ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public DataTable SearchUpdateQualificationResult(string regNo)
        {
            string sql = " SELECT I.SEQ, I.REG_NO, I.THICKNESS, I.INITIAL_MASS, D.FINAL_MASS, D.TRAY_MASS ";
            sql += " FROM 	QUALIFICATION_INFO AS I ";
            sql += "        INNER JOIN QUALIFICATION_DATA AS D ON I.SEQ = D.TEST_INFO_SEQ ";
            sql += " WHERE I.REG_NO = '" + regNo + "'";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }
    }
}
