using eccFramework.SharedLib.GlobalType.Core;
using FTSolutions.IEC61034.Common.QueryService;
using System.Collections.ObjectModel;
using System.Data;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class TestInfoDataCollection : ObservableCollection<TestInfoData>
    {
        public TestInfoDataCollection()
        {

        }


        public bool GenerateTestInfoData(string regNo)
        {
            DataTable dtTable = ManagerQueryService.Current.QueryServiceTest.SearchTestInfo(regNo);

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        TestInfoData testData = new TestInfoData();
                        testData.REG_NO = dtTable.Rows[i]["REG_NO"].ToString();
                        testData.SEQ = dtTable.Rows[i]["SEQ"].ToString();

                        testData.NUMBER = dtTable.Rows[i]["NUMBER"].ToString();
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

    public class TestInfoData : BaseNotifyProperty
    {
        public TestInfoData()
        {

        }


        public string REG_NO { get; set; }
        public string SEQ { get; set; }

        public string NUMBER { get; set; }
        public string CREATE_DT { get; set; }
    }
}
