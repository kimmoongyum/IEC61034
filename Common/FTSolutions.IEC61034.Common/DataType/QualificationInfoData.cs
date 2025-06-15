using eccFramework.SharedLib.GlobalType.Core;
using FTSolutions.IEC61034.Common.QueryService;
using System.Collections.ObjectModel;
using System.Data;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class QualificationInfoDataCollection : ObservableCollection<QualificationInfoData>
    {
        public QualificationInfoDataCollection()
        {

        }



        public bool GenerateQualificationInfoData(string regNo)
        {
            DataTable dtTable = ManagerQueryService.Current.QueryServiceQualification.SearchQualificationInfo(regNo);

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        QualificationInfoData testData = new QualificationInfoData();
                        testData.REG_NO = dtTable.Rows[i]["REG_NO"].ToString();
                        testData.SEQ = dtTable.Rows[i]["SEQ"].ToString();

                        testData.CREATE_DT = dtTable.Rows[i]["CREATE_DT"].ToString();

                        this.Add(testData);
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }

    public class QualificationInfoData : BaseNotifyProperty
    {
        public QualificationInfoData()
        {

        }

        public string REG_NO { get; set; }
        public string SEQ { get; set; }

        public string CREATE_DT { get; set; }
    }
}
