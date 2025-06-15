using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.QueryService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class ReportTestDataCollection : ObservableCollection<ReportTestData>
    {
        public ReportTestDataCollection()
        {

        }


        public bool GenerateTestData(string regNo)
        {
            DataTable dtTable = ManagerQueryService.Current.QueryServiceRegistration.ReportTestData(regNo);

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        ReportTestData testData = new ReportTestData();
                        testData.REG_NO = dtTable.Rows[i]["REG_NO"].ToString();
                        testData.TEST_DATE_TIME = dtTable.Rows[i]["TEST_DATE_TIME"].ToString();

                        testData.NUMBER_OF_SPECIMEN = dtTable.Rows[i]["NUMBER_OF_SPECIMEN"].ToString();
                        testData.SUMMARY_DESCRIPTION = dtTable.Rows[i]["SUMMARY_DESCRIPTION"].ToString();

                        testData.CHAMBER_TC = dtTable.Rows[i]["CHAMBER_TC"].ToString();
                        testData.FAN_FLOWRATE = dtTable.Rows[i]["FAN_FLOWRATE"].ToString();

                        testData.ABSORBANCE = dtTable.Rows[i]["ABSORBANCE"].ToString();
                        testData.TRANSMISSION = dtTable.Rows[i]["TRANSMISSION"].ToString();                        

                        testData.TEST_DURATION = dtTable.Rows[i]["TEST_DURATION"].ToString();
                        testData.FLAMEOUT_SECOND = dtTable.Rows[i]["FLAMEOUT_SECOND"].ToString();

                        testData.MAXIMUM_ABSORBANCE = dtTable.Rows[i]["MAXIMUM_ABSORBANCE"].ToString();
                        testData.MINIMUM_TRANSMISSION = dtTable.Rows[i]["MINIMUM_TRANSMISSION"].ToString();

                        testData.MINIMUM_TRANSMISSION_SECOND = dtTable.Rows[i]["MINIMUM_TRANSMISSION_SECOND"].ToString();
                                                                         
                        testData.START_CHAMBER_TEMPERATURE = dtTable.Rows[i]["START_CHAMBER_TEMPERATURE"].ToString();

                        #region Test
                        testData.CHAMBER_TC_LIST.Clear();
                        foreach (var item in testData.CHAMBER_TC.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            testData.CHAMBER_TC_LIST.Add(Convert.ToDouble(item));
                        }

                        testData.FAN_FLOWRATE_LIST.Clear();
                        foreach (var item in testData.FAN_FLOWRATE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            testData.FAN_FLOWRATE_LIST.Add(Convert.ToDouble(item));
                        }

                        testData.ABSORBANCE_LIST.Clear();
                        foreach (var item in testData.ABSORBANCE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            testData.ABSORBANCE_LIST.Add(Convert.ToDouble(item));
                        }

                        testData.TRANSMISSION_LIST.Clear();
                        foreach (var item in testData.TRANSMISSION.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            testData.TRANSMISSION_LIST.Add(Convert.ToDouble(item));
                        }
                        #endregion

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


    public class ReportTestData : BaseModel
    {
        public ReportTestData()
        {
            this.ABSORBANCE_LIST = new List<double>();
            this.CHAMBER_TC_LIST = new List<double>();
            this.FAN_FLOWRATE_LIST = new List<double>();
            this.TRANSMISSION_LIST = new List<double>();
        }

        public string REG_NO { get; set; }
        public string TEST_DATE_TIME { get; set; }

        public string NUMBER_OF_SPECIMEN { get; set; }
        public string SUMMARY_DESCRIPTION { get; set; }

        public string ABSORBANCE { get; set; }
        public string CHAMBER_TC { get; set; }
        public string FAN_FLOWRATE { get; set; }        
        public string TRANSMISSION { get; set; }
        public string START_CHAMBER_TEMPERATURE { get; set; }

        public string TEST_DURATION { get; set; }
        public string FLAMEOUT_SECOND { get; set; }
        public string MAXIMUM_ABSORBANCE { get; set; }
        public string MINIMUM_TRANSMISSION { get; set; }        
        public string MINIMUM_TRANSMISSION_SECOND { get; set; }        

        public List<double> ABSORBANCE_LIST { get; set; }
        public List<double> CHAMBER_TC_LIST { get; set; }
        public List<double> FAN_FLOWRATE_LIST { get; set; }
        public List<double> TRANSMISSION_LIST { get; set; }        
    }
}
