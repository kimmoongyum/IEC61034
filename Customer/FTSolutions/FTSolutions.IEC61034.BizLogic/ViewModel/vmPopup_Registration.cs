using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Utility.Helper;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.QueryService;
using FTSolutions.IEC61034.Common.Setting;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_Registration : BaseIEC61034ViewModel
    {
        public vmPopup_Registration()
        {
            CloseVisibility = System.Windows.Visibility.Visible;

            this.YearDataSource = new List<string>();
            this.MonthDataSource = new List<string>();

            this.RegistrationList = new TypeRegistrationCollection();

            YearChanged = new DelegateCommand((o) => ExecuteRefreshList(o));
            MonthChanged = new DelegateCommand((o) => ExecuteRefreshList(o));
            SearchCommand = new DelegateCommand((o) => ExecuteRefreshList(o));

            ReportCommand = new DelegateCommand((o) => ExecuteReportCommand(o));

            FilterCommand = new DelegateCommand((o) => ExecuteFilterCommand(o));
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand YearChanged { get; private set; }
        public ICommand MonthChanged { get; private set; }
        public ICommand SearchCommand { get; private set; }

        public ICommand ReportCommand { get; private set; }

        public ICommand FilterCommand { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        public List<string> YearDataSource { get; set; }
        public List<string> MonthDataSource { get; set; }

        public string CurrentYear { get; set; }
        public string CurrentMonth { get; set; }
        public string SearchCondition { get; set; }

        private TypeRegistrationCollection _registrationList;
        public TypeRegistrationCollection RegistrationList
        {
            get { return _registrationList; }
            set
            {
                _registrationList = value;
                this.RaisePropertyChanged(nameof(RegistrationList));
            }
        }

        private TypeRegistration _currentRegistrationInfo;
        public TypeRegistration CurrentRegistrationInfo
        {
            get { return _currentRegistrationInfo; }
            set
            {
                _currentRegistrationInfo = value;
                this.RaisePropertyChanged(nameof(CurrentRegistrationInfo));
            }
        }




        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            this.InitializeData();
            this.BindingRegistration();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override bool ExecuteCloseClick(object obj)
        {
            this.CloseWindow();

            return true;
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        public void ExecuteRefreshList(object obj)
        {
            this.BindingRegistration();
        }

        public void ExecuteReportCommand(object obj)
        {
            if (this.CurrentRegistrationInfo.STATUS.Equals(UILabelConst.CODE_T))
            {
                try
                {
                    this.DEVICE_MANAGER.Stop();

                    ReportTestDataCollection reportDataCollection = new ReportTestDataCollection();
                    bool dataResult = reportDataCollection.GenerateTestData(this.CurrentRegistrationInfo.REG_NO);

                    if (!dataResult)
                    {
                        this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_report_no_data");
                        return;
                    }

                    #region Create Report Source.
                    Assembly asm = Assembly.GetEntryAssembly();

                    string reportSource = "IEC61034_TestResult_V1";

                    string reportSourcefile = string.Format("{0}.Report.{1}.xlsx", asm.GetName().Name, reportSource);

                    string directory = Environment.CurrentDirectory + "\\temp";
                    DirectoryInfo di = new DirectoryInfo(directory);

                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    #endregion

                    foreach (ReportTestData reportData in reportDataCollection)
                    {
                        Stream fileStream = asm.GetManifestResourceStream(reportSourcefile);                        

                        #region Generating Workbook and Worksheet.
                        string tempFileName = string.Format(@"{0}\{1}.xlsx", directory, Guid.NewGuid().ToString());

                        ReportHelper.SaveStreamToFile(tempFileName, fileStream);
                        Application xlApp = new Application();
                        Workbook wb = xlApp.Workbooks.Open(tempFileName);

                        if (wb == null)
                        {
                            this.ShowMessageKey(MessageButtonType.OK, "msg_error", "msg_report_error_excel");
                            return;
                        }

                        xlApp.ScreenUpdating = false;
                        xlApp.Visible = false;
                        #endregion

                        this.GenerateReport(reportData, wb);

                        #region Generate Report File.
                        string today = DateTime.Now.ToString("yyyy.MM.dd");
                        string year = today.Substring(0, 4);
                        string month = today.Substring(5, 2);

                        string tempDir = SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_REPORT_PATH) + "/" + year + "/" + month + "/" + today;
                        DirectoryInfo diSave = new DirectoryInfo(tempDir);
                        if (!diSave.Exists)
                        {
                            diSave.Create();
                        }

                        string keyNo = this.CurrentRegistrationInfo.CUST_NO.Trim().Length > 0 ? this.CurrentRegistrationInfo.CUST_NO : this.CurrentRegistrationInfo.REG_NO;
                        string testTime = Convert.ToDateTime(this.CurrentRegistrationInfo.UPDATE_DT).ToString("yyyyMMdd_HHmmss");
                        string saveTime = DateTime.Now.ToString("HHmmss");
                        string sponsorName = this.CurrentRegistrationInfo.SPONSOR_NAME.Replace("[", "(").Replace("]", ")").Replace("/", "_").Replace("\\", "_");
                        string testName = this.CurrentRegistrationInfo.PRODUCT_NAME.Replace("[", "(").Replace("]", ")").Replace("/", "_").Replace("\\", "_");
                        string testNo = reportData.NUMBER_OF_SPECIMEN.Replace(" ", "").Replace("/", "of");

                        string reportPath = String.Format("{0}\\{1}_{2}_{3}_{4}_{5}_{6}.xlsx", tempDir, keyNo, sponsorName, testName, testNo, testTime, saveTime);

                        wb.SaveAs(reportPath);
                        wb.Close();

                        fileStream.Close();

                        File.Delete(tempFileName);

                        xlApp.Workbooks.Open(reportPath);

                        xlApp.ScreenUpdating = true;
                        xlApp.Visible = true;
                        xlApp.UserControl = true;
                        #endregion

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);

                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);
                    }

                    this.ShowMessageKey(MessageButtonType.OK, "key_complete", "msg_report_created");
                }
                catch (Exception ex)
                {
                    this.ShowMessageKey(MessageButtonType.OK, "msg_error", "msg_report_error");
                }
                finally
                {
                    this.DEVICE_MANAGER.Run();
                }
            }
            else
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "popup_registration_report_msg");
            }
        }

        public void ExecuteFilterCommand(object obj)
        {
            this.BindingRegistration(obj.ToString());
        }




        //###################################################################
        //  Private
        //###################################################################

        private void InitializeData()
        {
            this.YearDataSource.Clear();
            this.MonthDataSource.Clear();

            int currentYear = DateTime.Now.Year;

            this.YearDataSource.Add(UILabelConst.ITEM_ALL);

            for (int i = IEC61034Const.START_REG_YEAR; i <= currentYear; i++)
            {
                this.YearDataSource.Add(i.ToString());
            }

            this.MonthDataSource.Add(UILabelConst.ITEM_ALL);

            for (int i = 1; i <= 12; i++)
            {
                this.MonthDataSource.Add(i.ToString().PadLeft(2, '0'));
            }

            this.CurrentYear = currentYear.ToString();
            this.CurrentMonth = UILabelConst.ITEM_ALL;
        }

        private void BindingRegistration(string filter = "ALL")
        {
            System.Data.DataTable dtResult = ManagerQueryService.Current.QueryServiceRegistration.SearchRegistrationList(
                this.CurrentYear, this.CurrentMonth, this.SearchCondition, filter);

            TypeRegistrationCollection result = new TypeRegistrationCollection();

            if (dtResult != null)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    result.Add(new TypeRegistration(row));
                }
            }

            this.RegistrationList.Clear();
            this.RegistrationList = result;
        }


        private void GenerateReport(ReportTestData reportData, Workbook wb)
        {
            Worksheet wsReport = wb.Sheets["Report"];
            Worksheet wsRawData = wb.Sheets["RawData"];

            #region Write General Information
            wsReport.Range["H2"].Value = reportData.REG_NO;
            wsReport.Range["P2"].Value = this.CurrentRegistrationInfo.CUST_NO;
            wsReport.Range["H3"].Value = SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_LABORATORY);
            wsReport.Range["H4"].Value = IEC61034Const.DEFAULT_STANDARD_TYPE;
            wsReport.Range["H5"].Value = reportData.TEST_DATE_TIME;
            wsReport.Range["H6"].Value = this.CurrentRegistrationInfo.SPONSOR_NAME;
            wsReport.Range["H7"].Value = this.CurrentRegistrationInfo.SPONSOR_ADDR;
            wsReport.Range["H8"].Value = this.CurrentRegistrationInfo.SUPPLIER_NAME;
            wsReport.Range["H9"].Value = this.CurrentRegistrationInfo.SUPPLIER_ADDR;
            wsReport.Range["H10"].Value = this.CurrentRegistrationInfo.PRODUCT_NAME;
            wsReport.Range["H11"].Value = this.CurrentRegistrationInfo.PRODUCT_DESC;

            wsReport.Range["H13"].Value = this.CurrentRegistrationInfo.OPERATOR_NAME;
            wsReport.Range["P13"].Value = this.CurrentRegistrationInfo.MANAGER_NAME;

            bool useSpecimenCondition = this.CurrentRegistrationInfo.USE_SPECIMEN_CONDITION;

            wsReport.Range["H14"].Value = useSpecimenCondition ? "Yes" : "No";
            wsReport.Range["H15"].Value = useSpecimenCondition ? this.CurrentRegistrationInfo.CONDITION_TEMPERATURE : "N/A";
            wsReport.Range["P15"].Value = useSpecimenCondition ? this.CurrentRegistrationInfo.CONDITION_HUMIDITY : "N/A";

            wsReport.Range["H17"].Value = reportData.NUMBER_OF_SPECIMEN;
            
            wsReport.Range["H18"].Value = this.CurrentRegistrationInfo.CABLE_TYPE;
            if(this.CurrentRegistrationInfo.CABLE_TYPE.Equals("ROUND", StringComparison.OrdinalIgnoreCase) == true)
            {
                wsReport.Range["P18"].Value = this.CurrentRegistrationInfo.CABLE_DIAMETER;

                wsReport.Range["H19"].Value = "N/A";
                wsReport.Range["P19"].Value = "N/A";
            }
            else
            {
                wsReport.Range["P18"].Value = this.CurrentRegistrationInfo.CABLE_DIAMETER;

                wsReport.Range["H19"].Value = this.CurrentRegistrationInfo.CABLE_MINOR_AXIS;
                wsReport.Range["P19"].Value = this.CurrentRegistrationInfo.CABLE_MAJOR_AXIS;
            }

            wsReport.Range["H20"].Value = this.CurrentRegistrationInfo.TEST_PIECES_COUNT;

            if (IniConfig.IsDisplyRPTTestCondition)
            {
                wsReport.Range["H23"].Value = reportData.START_CHAMBER_TEMPERATURE;
            }
            else
            {
                wsReport.Range["B22"].Value = string.Empty;

                wsReport.Range["B23"].Value = string.Empty;
                wsReport.Range["H23"].Value = string.Empty;
            }
            

            wsReport.Range["H26"].Value = reportData.TEST_DURATION;
            wsReport.Range["P26"].Value = reportData.FLAMEOUT_SECOND;

            wsReport.Range["H27"].Value = reportData.MAXIMUM_ABSORBANCE;
            wsReport.Range["P27"].Value = reportData.MINIMUM_TRANSMISSION_SECOND;

            wsReport.Range["H28"].Value = reportData.MINIMUM_TRANSMISSION;
            #endregion

            
            #region Define variables.
            int TEST_DATA_COLUMN_COUNT = 4;
            object[,] testRawData = new object[reportData.TRANSMISSION_LIST.Count, TEST_DATA_COLUMN_COUNT];
            #endregion

            #region Test Data.
            for (var i = 0; i < reportData.TRANSMISSION_LIST.Count; i++)
            {
                testRawData[i, 0] = i;
                testRawData[i, 1] = reportData.CHAMBER_TC_LIST[i];
                testRawData[i, 2] = reportData.TRANSMISSION_LIST[i];
                testRawData[i, 3] = reportData.ABSORBANCE_LIST[i];
            }
            #endregion
    
            this.WriteDataSetOnExcel(wsRawData, testRawData, 3, 1, TEST_DATA_COLUMN_COUNT);
        }

        private void WriteDataSetOnExcel(Worksheet ws, object[,] data, int startRowIndex, int startColumnIndex, int columnCount)
        {
            int rowCount = data.Length / columnCount;

            var startCell = (Range)ws.Cells[startRowIndex, startColumnIndex];
            var endCell = (Range)ws.Cells[rowCount + startRowIndex - 1, columnCount + startColumnIndex - 1];

            ws.Range[startCell, endCell].Value2 = data;
        }



        //###################################################################
        //  Public
        //###################################################################

        public void NewPopup_EndEventHandler(TypeRegistration regInfo)
        {
            this.BindingRegistration();
        }
    }
}
