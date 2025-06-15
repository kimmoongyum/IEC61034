using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.Setting;
using System.Data;

namespace FTSolutions.IEC61034.Common.QueryService
{
    public class QueryServiceTest : BaseQueryService
    {
        public QueryServiceTest()
        {

        }



        //###################################################################
        //  Public
        //###################################################################

        public DataTable SearchTestInfoSEQ(string regNo, string serialNo)
        {
            string sql = "SELECT SEQ FROM TEST_INFO WHERE REG_NO = '" + regNo + "' AND SERIAL_NO = '" + serialNo + "' ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }


        public DataTable SearchTestInfo(string regNo)
        {
            string sql = "SELECT * FROM TEST_INFO WHERE REG_NO = '" + regNo + "'";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }

        public void DeleteTestInfo(string regNo, string seq)
        {
            string sql = " DELETE FROM TEST_DATA WHERE REG_NO = '" + regNo + "' AND TEST_INFO_SEQ = '" + seq + "'";

            this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);

            sql = " DELETE FROM TEST_INFO WHERE REG_NO = '" + regNo + "' AND SEQ = '" + seq + "'";

            this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }


        public void InsertTestInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " INSERT INTO TEST_INFO (REG_NO, TEST_DATE_TIME, NUMBER, TOTAL_NUMBER, ";
            sql += " CABLE_TYPE, CABLE_DIAMETER, CABLE_MAJOR_AXIS, CABLE_MINOR_AXIS, TEST_PIECES_COUNT, SERIAL_NO) ";
            sql += " VALUES(:reg_no, :test_date_time, :number, :total_number, ";
            sql += " :cable_type, :cable_diameter, :cable_major_axis, :cable_minor_axis, :test_pieces_count, :serial_no) ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void InsertTestData(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " INSERT INTO TEST_DATA (TEST_INFO_SEQ, REG_NO, START_CHAMBER_TEMPERATURE, CHAMBER_TC, FAN_FLOWRATE, TRANSMISSION, ";
            sql += " ABSORBANCE, MAX_ABSORBANCE, MAX_ABSORBANCE0, MINIMUM_TRANSMISSION, MINIMUM_TRANSMISSION_SECOND, MAXIMUM_ABSORBANCE, FLAMEOUT_SECOND, TEST_DURATION, SUMMARY_DESCRIPTION) ";
            sql += " VALUES(:test_info_seq, :reg_no, :start_chamber_temperature, :chamber_tc, :fan_flowrate, :transmission, ";
            sql += " :absorbance, :max_absorbance, :max_absorbance0, :minimum_transmission, :minimum_transmission_second, :maximum_absorbance, :flameout_second, :test_duration, :summary_description) ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public DataTable SearchUpdateTestResult(string regNo)
        {
            string sql = " SELECT I.SEQ, I.REG_NO, I.NUMBER || '/' || I.TOTAL_NUMBER AS TEST_COUNT, I.THICKNESS, I.INITIAL_MASS, D.FINAL_MASS, D.TRAY_MASS ";
            sql += " FROM 	TEST_INFO AS I ";
            sql += "        INNER JOIN TEST_DATA AS D ON I.SEQ = D.TEST_INFO_SEQ ";
            sql += " WHERE I.REG_NO = '" + regNo + "'";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }
    }
}
