using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Data;

namespace FTSolutions.IEC61034.Common.QueryService
{
    public class QueryServiceRegistration : BaseQueryService
    {
        public QueryServiceRegistration()
        {

        }



        //###################################################################
        //  Public
        //###################################################################

        public DataTable SearchRegistrationList(string year, string month, string searchCondition, string filter)
        {
            string sql = " SELECT   R.*, ifnull(MAX(T.NUMBER), 0) AS NUMBER, ifnull(MAX(T.TOTAL_NUMBER), 0) AS TOTAL_NUMBER, ";
            sql += "                ifnull(MAX(T.NUMBER), 0) || '/' || ifnull(MAX(T.TOTAL_NUMBER), 0) AS TEST_COUNT";
            sql += "       FROM 	REGIST_INFO R  ";
            sql += "                LEFT OUTER JOIN TEST_INFO T ON R.REG_NO = T.REG_NO ";
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
     
            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }


        public int GetNextRegOrder(string regDate)
        {
            string sql = "SELECT ifnull(max(REG_ORDER), 0) + 1 FROM REGIST_INFO WHERE REG_DATE = '" + regDate + "'";

            return Convert.ToInt32(this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql).Rows[0][0]);
        }

        public void InsertRegistrationInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " INSERT INTO REGIST_INFO (REG_NO, REG_DATE, REG_ORDER, CUST_NO, SPONSOR_NAME, SPONSOR_ADDR, SUPPLIER_NAME, SUPPLIER_ADDR, PRODUCT_NAME, PRODUCT_DESC, ";
            sql += " OPERATOR_NAME, MANAGER_NAME, USE_SPECIMEN_CONDITION, CONDITION_TEMPERATURE, CONDITION_HUMIDITY, ";
            sql += " CABLE_TYPE, CABLE_DESC, CABLE_DIAMETER, CABLE_MAJOR_AXIS, CABLE_MINOR_AXIS, TEST_PIECES_COUNT, STATUS) ";
            sql += " VALUES(:regNo, :regDate, :regOrder, :custNo, :sponsorName, :sponsorAddr, :supplierName, :supplierAddr, :productName, :productDesc, ";
            sql += " :operatorName, :managerName, :useSpecimenCondition, :conditionTemperature, :conditionHumidity, ";
            sql += " :cableType, :cableDesc, :cableDiameter, :cableMajorAxis, :cableMinorAxis, :testPiecesCount, 'R') ";
         
            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }


        public void UpdateRegistrationInfo(SQLiteParamInfoCollection paramInfo)                                                                                                                     
        {
            string sql = " UPDATE REGIST_INFO ";
            sql += " SET REG_DATE = :regDate, REG_ORDER = :regOrder, CUST_NO = :custNo, SPONSOR_NAME = :sponsorName, SPONSOR_ADDR = :sponsorAddr, ";
            sql += "    SUPPLIER_NAME = :supplierName, SUPPLIER_ADDR = :supplierAddr, PRODUCT_NAME = :productName, PRODUCT_DESC = :productDesc, ";
            sql += "    OPERATOR_NAME = :operatorName, MANAGER_NAME = :managerName, USE_SPECIMEN_CONDITION = :useSpecimenCondition, CONDITION_TEMPERATURE = :conditionTemperature, CONDITION_HUMIDITY = :conditionHumidity, ";
            sql += "    CABLE_TYPE = :cableType, CABLE_DESC = :cableDesc, CABLE_DIAMETER = :cableDiameter, ";
            sql += "    CABLE_MAJOR_AXIS = :cableMajorAxis, CABLE_MINOR_AXIS = :cableMinorAxis, TEST_PIECES_COUNT = :testPiecesCount, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE SEQ = :seq AND REG_NO = :regNo ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void DeleteRegistrationInfo(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " DELETE FROM REGIST_INFO WHERE SEQ = :seq ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }

        public void UpdateTestStatus(SQLiteParamInfoCollection paramInfo)
        {
            string sql = " UPDATE REGIST_INFO ";
            sql += " SET STATUS = :status, UPDATE_DT = datetime('now', 'localtime') ";
            sql += " WHERE SEQ = :seq AND REG_NO = :regNo ";

            this.ExecuteNonSelectQuery(SettingManager.ConnectionStringSystemDB(), sql, paramInfo);
        }


        public DataTable ReportTestData(string regNo)
        {
            string sql = " SELECT R.*, I.*, I.NUMBER || ' / ' || I.TOTAL_NUMBER AS NUMBER_OF_SPECIMEN, D.* FROM REGIST_INFO R ";
            sql += " LEFT OUTER JOIN TEST_INFO I ON R.REG_NO = I.REG_NO ";
            sql += " LEFT OUTER JOIN TEST_DATA D ON I.REG_NO = D.REG_NO AND I.SEQ = D.TEST_INFO_SEQ ";
            sql += " WHERE R.REG_NO = '" + regNo + "' ORDER BY I.NUMBER ";

            return this.ExecuteSelectQuery(SettingManager.ConnectionStringSystemDB(), sql);
        }       
    }
}
