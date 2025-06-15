using eccFramework.SharedLib.Core.Protocol;
using eccFramework.SharedLib.GlobalType.Protocol;
using System;
using System.Data;

namespace FTSolutions.IEC61034.Common.Base
{
    public class BaseQueryService : BaseSQLite
    {
        public Action<bool, Exception> CheckQueryService;


        public BaseQueryService()
        {

        }



        //###################################################################
        //  Public
        //###################################################################

        public new DataTable ExecuteSelectQuery(string connectionString, string sql)
        {
            DataTable dtResult = null;

            try
            {
                dtResult = base.ExecuteSelectQuery(connectionString, sql);

                if (this.CheckQueryService != null)
                {
                    this.CheckQueryService(true, null);
                }
            }
            catch (Exception ex)
            {
                if (this.CheckQueryService != null)
                {
                    this.CheckQueryService(false, ex);
                }
            }

            return dtResult;
        }

        public new void ExecuteNonSelectQuery(string connectionString, string sql, SQLiteParamInfoCollection paramInfo)
        {
            try
            {
                base.ExecuteNonSelectQuery(connectionString, sql, paramInfo);

                if (this.CheckQueryService != null)
                {
                    this.CheckQueryService(true, null);
                }
            }
            catch (Exception ex)
            {
                if (this.CheckQueryService != null)
                {
                    this.CheckQueryService(false, ex);
                }
            }
        }

        public new void ExecuteNonSelectBatchQuery(string connectionString, string sql, SQLiteParamInfoBatch paramBatchInfo)
        {
            try
            {
                base.ExecuteNonSelectBatchQuery(connectionString, sql, paramBatchInfo);

                if (this.CheckQueryService != null)
                {
                    this.CheckQueryService(true, null);
                }
            }
            catch (Exception ex)
            {
                if (this.CheckQueryService != null)
                {
                    this.CheckQueryService(false, ex);
                }
            }
        }
    }
}
