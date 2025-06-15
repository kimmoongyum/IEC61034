using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.Protocol;
using eccFramework.SharedLib.GlobalType.SysType;
using eccFramework.SharedLib.Utility.Helper;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.QueryService;
using FTSolutions.IEC61034.Common.Result;
using System;
using System.Data;
using System.Windows.Input;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_TestSummary : BaseIEC61034ViewModel
    {
        public vmPopup_TestSummary()
        {
            this.TestItem_TestSummary = new TestSummary();

            SaveNextCommand = new DelegateCommand((o) => ExecuteSaveNextCommand(o));
        }




        //###################################################################
        //  Command
        //###################################################################

        public ICommand SaveNextCommand { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        private TestSummary _testItem_TestSummary;
        public TestSummary TestItem_TestSummary
        {
            get { return _testItem_TestSummary; }
            set
            {
                if (this._testItem_TestSummary != value)
                {
                    this._testItem_TestSummary = value;
                    this.RaisePropertyChanged(nameof(TestItem_TestSummary));
                }
            }
        } 

        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            Test testInfo = this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test;

            //TimeSpan totalSeconds = TimeSpan.FromSeconds(testInfo.Standard.FlameoutSecond);
            //TimeSpan totalSeconds = TimeSpan.FromSeconds(testInfo.FlameoutTime);
            //this.TestItem_TestSummary.FlameoutTime = totalSeconds.ToString(@"hh\:mm\:ss");
            this.TestItem_TestSummary.FlameoutTime = testInfo.FlameoutTime;
            this.TestItem_TestSummary.MaxAbsorbance = testInfo.Standard.MaxAbsorbance;
            this.TestItem_TestSummary.MinTransmission = testInfo.Standard.MinTransmission;
            this.TestItem_TestSummary.StartChamberTemperature = testInfo.Standard.StartChamberTemperature;
            //totalSeconds = TimeSpan.FromSeconds(testInfo.Standard.MinTransmissionSecond);
            //this.TestItem_TestSummary.MinTransmissionSecond = totalSeconds.ToString(@"hh\:mm\:ss");
            this.TestItem_TestSummary.MinTransmissionSecond = testInfo.Standard.MinTransmissionSecond;
        }

        protected override bool ExecuteCancelClick(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_popup", "msg_quit_menu");

            if (result)
            {
                this.SESSION_MANAGER.IEC61034_DataSetTest.ClearAll();

                this.CloseWindow();
            }

            return result;
        }

        public override bool IsValid()
        {
            return this.TestItem_TestSummary.IsValid();
        }



        //###################################################################
        //  EventHandler
        //###################################################################
        
        public void ExecuteSaveNextCommand(object obj)
        {
            this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestSummary.CopyValueFrom(this.TestItem_TestSummary);

            this.DatabaseUpdate();

            this.SESSION_MANAGER.IEC61034_DataSetTest.ClearAll();

            string targetMenu = IEC61034Const.MENU_SMOKE_VENTILATION_KEY;
            BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.TEST_SUMMARY;
            };

            this.SESSION_MANAGER.IEC61034_DataSetTest.ContinueTest();

            this.CloseWindow();

            popup.ShowDialog();
        }



        //###################################################################
        //  Private
        //###################################################################

        private void DatabaseUpdate()
        {
            try
            {
                this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestSummary.CopyValueFrom(this.TestItem_TestSummary);

                string serialNo = Guid.NewGuid().ToString();

                this.InsertTestInfo(serialNo);

                string testInfoSEQ = this.GetTestInfoSEQ(this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo.REG_NO, serialNo);
                this.InsertTestData(testInfoSEQ);

                this.UpdateRegistStatus(this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo.SEQ, this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo.REG_NO);

                this.ShowMessageKey(MessageButtonType.OK, "msg_title_save", "msg_save_complete");
            }
            catch (Exception ex)
            {
                this.WriteEvent(LogType.Error, ex.Message);
                this.ShowMessageKey(MessageButtonType.OK, "msg_title_save", "msg_save_error");
            }
        }


        private string GetTestInfoSEQ(string regNo, string serialNo)
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceTest.SearchTestInfoSEQ(regNo, serialNo);

            return dtResult.Rows[0]["SEQ"].ToString();
        }

        private void InsertTestInfo(string serialNo)
        {
            int number = 0;
            int totalNumber = 0;

            try
            {
                number = Convert.ToInt16(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.Number);
                totalNumber = Convert.ToInt16(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.TotalNumber);
            }
            catch { }

            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("reg_no", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo.REG_NO));
            paramInfo.Add(new SQLiteParamInfo("test_date_time", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.TestDateTime));
            paramInfo.Add(new SQLiteParamInfo("number", DbType.String, number.ToString()));
            paramInfo.Add(new SQLiteParamInfo("total_number", DbType.String, totalNumber.ToString()));            
            paramInfo.Add(new SQLiteParamInfo("cable_type", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.CABLE_TYPE));
            paramInfo.Add(new SQLiteParamInfo("cable_diameter", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.CableDiameter));
            paramInfo.Add(new SQLiteParamInfo("cable_major_axis", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.CableMajorAxis));
            paramInfo.Add(new SQLiteParamInfo("cable_minor_axis", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.CableMinorAxis));
            paramInfo.Add(new SQLiteParamInfo("test_pieces_count", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestProperties.TestPiecesCount));
            paramInfo.Add(new SQLiteParamInfo("serial_no", DbType.String, serialNo));

            ManagerQueryService.Current.QueryServiceTest.InsertTestInfo(paramInfo);
        }

        private void InsertTestData(string testInfoSEQ)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("test_info_seq", DbType.String, testInfoSEQ));
            paramInfo.Add(new SQLiteParamInfo("reg_no", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.RegistrationInfo.REG_NO));

            paramInfo.Add(new SQLiteParamInfo("start_chamber_temperature", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestSummary.StartChamberTemperature));
            paramInfo.Add(new SQLiteParamInfo("chamber_tc", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.SeriesChamberTC.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("fan_flowrate", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.SeriesFanFlowrate.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("transmission", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.SeriesTransmission.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.SeriesAbsorbance.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("max_absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.SeriesMaxAbsorbance.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("max_absorbance0", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.SeriesMaxAbsorbance0.GetValueString()));

            paramInfo.Add(new SQLiteParamInfo("maximum_absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.MaxAbsorbance));
            paramInfo.Add(new SQLiteParamInfo("minimum_transmission", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.MinTransmission));
            //paramInfo.Add(new SQLiteParamInfo("minimum_transmission_second", DbType.String, ConvertTimeStringToSecond(this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.MinTransmissionSecond)));
            paramInfo.Add(new SQLiteParamInfo("minimum_transmission_second", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.MinTransmissionSecond));

            //paramInfo.Add(new SQLiteParamInfo("flameout_second", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.FlameoutSecond));
            paramInfo.Add(new SQLiteParamInfo("flameout_second", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.FlameoutTime));
            paramInfo.Add(new SQLiteParamInfo("test_duration", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_Test.Standard.FinishTimeSecond));

            paramInfo.Add(new SQLiteParamInfo("summary_description", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetTest.Info_TestSummary.Description));

            ManagerQueryService.Current.QueryServiceTest.InsertTestData(paramInfo);
        }

        private int ConvertTimeStringToSecond(string timeString)
        {
            int rtnVal = 0;

            TimeSpan time;

            if (TimeSpan.TryParse(timeString, out time))
            {
                return (int)time.TotalSeconds;
            }            

            return rtnVal;
        }

        private void UpdateRegistStatus(string seq, string regNo)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("seq", DbType.String, seq));
            paramInfo.Add(new SQLiteParamInfo("regNo", DbType.String, regNo));
            paramInfo.Add(new SQLiteParamInfo("status", DbType.String, "T"));

            ManagerQueryService.Current.QueryServiceRegistration.UpdateTestStatus(paramInfo);
        }


        /// <summary>
        /// IEC 61034 기준에 따라 최소 투과율을 평가 (80mm 기준 정규화 포함)
        /// </summary>
        /// <param name="minTransmittance">기록된 최소 투과율 (0~1 범위)</param>
        /// <param name="cableDiameterMm">피시험 케이블의 외경 (mm)</param>
        /// <returns>정규화된 또는 원래의 최소 투과율</returns>
        public double EvaluateMinimumTransmittance(double minTransmittance, double cableDiameterMm)
        {
            const double ReferenceDiameter = 80.0;

            if (cableDiameterMm <= ReferenceDiameter)
            {
                // 외경이 80mm 이하 → 그대로 사용
                return minTransmittance;
            }
            else
            {
                // 외경이 80mm 초과 → 정규화 적용
                double normalized = minTransmittance * (cableDiameterMm / ReferenceDiameter);
                return normalized;
            }
        }

        bool IsPass(double evaluatedTransmittance, double threshold = 0.60)
        {
            return evaluatedTransmittance >= threshold;
        }
    }
}
