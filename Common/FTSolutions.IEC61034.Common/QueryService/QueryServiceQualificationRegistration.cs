using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Data;

namespace FTSolutions.IEC61034.Common.QueryService
{
    public class QueryServiceQualificationRegistration : BaseQueryService
    {
        public QueryServiceQualificationRegistration()
        {

        }


        //###################################################################
        //  Public
        //###################################################################

        public DataTable SearchRegistrationList(string year, string month, string searchCondition, string filter)
        {
            string sql = " SELECT   R.* , ifnull(T.TEST_DATE_TIME, '') AS TEST_DATE_TIME ";
            sql += "       FROM 	QUALIFICATION_REGIST_INFO R  ";
            sql += "                LEFT OUTER JOIN QUALIFICATION_INFO T ON R.REG_NO = T.REG_NO ";
            sql += "       WHERE 1 = 1 ";

            if (!year.Equals(UILabelConst.ITEM_ALL) && !month.Equals(UILabelConst.ITEM_ALL))
            {
                sql += " AND REG_DATE LIKE '" + year + month + "%' ";
            }
            else if (!year.Equals(UILabelConst.ITEM_ALL))
            {
                sql += " AND REG_DATE LIKE '" + year + "%' ";
            }
            else if (!month.Equals(UILabelConst.ITEM_ALL))
            {
                sql += " AND SUBSTR(REG_DATE, 5, 2) = '" + month + "' ";
            }

            if (filter.Equals("R"))
            {
                sql += " AND STATUS = 'R'";
            }
            else if (filter.Equals("T"))
            {
                sql += " AND STATUS = 'T'";
            }

            if (searchCondition != null && searchCondition.Trim().Length > 0)
            {
                sql += " AND (CUST_NAME LIKE '%" + searchCondition + "%' OR TEST_NAME LIKE '%" + searchCondition + "%' ";
                sql += "      OR DESCRIPTION LIKE '%" + searchCondition + "%' )";
            }

            sql += " GROUP BY R.REG_NO ";
            sql += " ORDER BY R.REG_DATE DESC, R.REG_NO DESC";

            Console.WriteLine(string.Format("query -> {0}", sql));

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }


        public int GetNextRegOrder(string regDate)
        {
            string sql = "SELECT ifnull(max(REG_ORDER), 0) + 1 FROM QUALIFICATION_REGIST_INFO WHERE REG_DATE = '" + regDate + "'";

            return Convert.ToInt32(this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql).Rows[0][0]);
        }

        public void InsertRegistrationInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " INSERT INTO QUALIFICATION_REGIST_INFO (REG_NO, REG_DATE, REG_ORDER, USE_SPECIMEN_CONDITION, CONDITION_TEMPERATURE, CONDITION_HUMIDITY, TOLUENE_CONTENT, STATUS) ";
            sql += " VALUES(:regNo, :regDate, :regOrder, :useSpecimenCondition, :conditionTemperature, :conditionHumidity, :tolueneContent, 'R') ";
         
            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }


        public void UpdateRegistrationInfo(SQLiteParamInfoCollection paramInfo)                                                                                                                     
        {
            string sql = " UPDATE QUALIFICATION_REGIST_INFO ";
            sql += " SET REG_DATE = :regDate, REG_ORDER = :regOrder, USE_SPECIMEN_CONDITION = :useSpecimenCondition, CONDITION_TEMPERATURE = :conditionTemperature, CONDITION_HUMIDITY = :conditionHumidity, ";
            sql += "    TOLUENE_CONTENT = :tolueneContent, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE SEQ = :seq AND REG_NO = :regNo ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void DeleteRegistrationInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " DELETE FROM QUALIFICATION_REGIST_INFO WHERE SEQ = :seq ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void UpdateQualificationStatus(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE QUALIFICATION_REGIST_INFO ";
            sql += " SET STATUS = :status, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE SEQ = :seq AND REG_NO = :regNo ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }


        public DataTable ReportQualificationData(string regNo)
        {
            string sql = " SELECT R.*, I.*, D.* FROM QUALIFICATION_REGIST_INFO R ";
            sql += " LEFT OUTER JOIN QUALIFICATION_INFO I ON R.REG_NO = I.REG_NO ";
            sql += " LEFT OUTER JOIN QUALIFICATION_DATA D ON I.REG_NO = D.REG_NO AND I.SEQ = D.TEST_INFO_SEQ ";
            sql += " WHERE R.REG_NO = '" + regNo + "' ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }
    }
}
