using eccFramework.SharedLib.GlobalType.SysType;
using System.Windows.Media;

namespace FTSolutions.IEC61034.Common.QueryService
{
    public class ManagerQueryService
    {
        #region Singleton.
        private static ManagerQueryService _instance;
        private static object m_lock = new object();

        public static ManagerQueryService Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (m_lock)
                    {
                        _instance = new ManagerQueryService();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public ManagerQueryService()
        {
            this.QueryServiceSystem = new QueryServiceSystem();
            this.QueryServiceRegistration = new QueryServiceRegistration();
            this.QueryServiceTest = new QueryServiceTest();

            this.QueryServiceQualificationRegistration = new QueryServiceQualificationRegistration();
            this.QueryServiceQualification = new QueryServiceQualification();            

            this.QueryServiceSystem.CheckQueryService += (status, err) => { StatusDatabase = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
            this.QueryServiceRegistration.CheckQueryService += (status, err) => { StatusDatabase = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
            this.QueryServiceTest.CheckQueryService += (status, err) => { StatusDatabase = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };

            this.QueryServiceQualificationRegistration.CheckQueryService += (status, err) => { StatusDatabase = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
            this.QueryServiceQualification.CheckQueryService += (status, err) => { StatusDatabase = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };

        }



        //###################################################################
        //  Property
        //###################################################################

        public QueryServiceSystem QueryServiceSystem { get; set; }

        public QueryServiceRegistration QueryServiceRegistration { get; set; }

        public QueryServiceQualificationRegistration QueryServiceQualificationRegistration { get; set; }

        public QueryServiceTest QueryServiceTest { get; set; }

        public QueryServiceQualification QueryServiceQualification { get; set; }


        public SolidColorBrush StatusDatabase { get; set; }



        //###################################################################
        //  Public
        //###################################################################

    }
}
