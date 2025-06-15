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
    public class vmPopup_QualificationSummary : BaseIEC61034ViewModel
    {
        public vmPopup_QualificationSummary()
        {
            this.TestItem_QualificationSummary = new QualificationSummary();

            SaveNextCommand = new DelegateCommand((o) => ExecuteSaveNextCommand(o));
        }




        //###################################################################
        //  Command
        //###################################################################

        public ICommand SaveNextCommand { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        private QualificationSummary _testItem_QualificationSummary;
        public QualificationSummary TestItem_QualificationSummary
        {
            get { return _testItem_QualificationSummary; }
            set
            {
                if (this._testItem_QualificationSummary != value)
                {
                    this._testItem_QualificationSummary = value;
                    this.RaisePropertyChanged(nameof(TestItem_QualificationSummary));
                }
            }
        }
        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            this.TestItem_QualificationSummary.IsToluene4 = false;
            this.TestItem_QualificationSummary.IsToluene10 = false;

            Qualification qualificationInfo = this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification;
            if (qualificationInfo != null) {
                //TimeSpan totalSeconds = TimeSpan.FromSeconds(qualificationInfo.Standard.FlameoutSecond);
                TimeSpan totalSeconds = TimeSpan.FromSeconds(qualificationInfo.FlameoutTime);
                this.TestItem_QualificationSummary.FlameoutTime = totalSeconds.ToString(@"hh\:mm\:ss");
                this.TestItem_QualificationSummary.MaxAbsorbance = qualificationInfo.Standard.MaxAbsorbance;
                this.TestItem_QualificationSummary.MinTransmission = qualificationInfo.Standard.MinTransmission;
                this.TestItem_QualificationSummary.CorrectedAbsorbance = qualificationInfo.Standard.CorrectedAbsorbance;
                this.TestItem_QualificationSummary.StartChamberTemperature = qualificationInfo.Standard.StartChamberTemperature;

                totalSeconds = TimeSpan.FromSeconds(qualificationInfo.Standard.MinTransmissionSecond);
                this.TestItem_QualificationSummary.MinTransmissionSecond = totalSeconds.ToString(@"hh\:mm\:ss");
            }

            QualificationProperty qualificationProperty = this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties;
            if (qualificationProperty != null) {
                if(qualificationProperty.IsToluene4)
                {
                    this.TestItem_QualificationSummary.IsToluene4 = true;
                }
                else if(qualificationProperty.IsToluene10)
                {
                    this.TestItem_QualificationSummary.IsToluene10 = true;
                }

                if(qualificationInfo.Standard.TargetAbsorbanceMin >= this.TestItem_QualificationSummary.CorrectedAbsorbance && this.TestItem_QualificationSummary.CorrectedAbsorbance <= qualificationInfo.Standard.TargetAbsorbanceMax)
                {
                    this.TestItem_QualificationSummary.Evaluation = "PASS";
                    this.TestItem_QualificationSummary.EvaluationColor = GlobalConst.VALID_BLUSH;
                }
                else
                {
                    this.TestItem_QualificationSummary.Evaluation = "FAIL";
                    this.TestItem_QualificationSummary.EvaluationColor = GlobalConst.INVALID_BLUSH;
                }
            }
        }

        protected override bool ExecuteCancelClick(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_popup", "msg_quit_menu");

            if (result)
            {
                this.SESSION_MANAGER.IEC61034_DataSetQualification.ClearAll();

                this.CloseWindow();
            }

            return result;
        }

        public override bool IsValid()
        {
            return this.TestItem_QualificationSummary.IsValid();
        }



        //###################################################################
        //  EventHandler
        //###################################################################
        
        public void ExecuteSaveNextCommand(object obj)
        {
            this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationSummary.CopyValueFrom(this.TestItem_QualificationSummary);

            this.DatabaseUpdate();

            this.SESSION_MANAGER.IEC61034_DataSetQualification.ClearAll();

            string targetMenu = IEC61034Const.MENU_SMOKE_VENTILATION_KEY;
            BlackPopup popup = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);
            popup.Owner = this.Owner;

            popup.DataContextChanged += (s, o) =>
            {
                BaseIEC61034ViewModel vm = popup.DataContext as BaseIEC61034ViewModel;
                vm.CallerMenu = MenuKind.QUALIFICATION_SUMMARY;
            };

            this.SESSION_MANAGER.IEC61034_DataSetQualification.ContinueTest();

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
                this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationSummary.CopyValueFrom(this.TestItem_QualificationSummary);

                string serialNo = Guid.NewGuid().ToString();

                this.InsertQualificationInfo(serialNo);

                string testInfoSEQ = this.GetVerificaionInfoSEQ(this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo.REG_NO, serialNo);
                this.InsertQualificationData(testInfoSEQ);

                this.UpdateRegistStatus(this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo.SEQ, this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo.REG_NO);

                this.ShowMessageKey(MessageButtonType.OK, "msg_title_save", "msg_save_complete");
            }
            catch (Exception ex)
            {
                this.WriteEvent(LogType.Error, ex.Message);
                this.ShowMessageKey(MessageButtonType.OK, "msg_title_save", "msg_save_error");
            }
        }


        private string GetVerificaionInfoSEQ(string regNo, string serialNo)
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceQualification.SearchQualificationInfoSEQ(regNo, serialNo);

            return dtResult.Rows[0]["SEQ"].ToString();
        }

        private void InsertQualificationInfo(string serialNo)
        {
            //int number = 0;
            //int totalNumber = 0;

            try
            {
                //number = Convert.ToInt16(this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties.Number);
                //totalNumber = Convert.ToInt16(this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties.TotalNumber);
            }
            catch { }

            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("reg_no", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo.REG_NO));
            paramInfo.Add(new SQLiteParamInfo("test_date_time", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties.TestDateTime));
            //paramInfo.Add(new SQLiteParamInfo("number", DbType.String, number.ToString()));
            //paramInfo.Add(new SQLiteParamInfo("total_number", DbType.String, totalNumber.ToString()));
            if (this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties.IsToluene4)
            {
                paramInfo.Add(new SQLiteParamInfo("toluene_content", DbType.String, IEC61034Const.KEY_TOLUENE_4));                
            }
            else if (this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationProperties.IsToluene10)
            {
                paramInfo.Add(new SQLiteParamInfo("toluene_content", DbType.String, IEC61034Const.KEY_TOLUENE_10));
            }

            paramInfo.Add(new SQLiteParamInfo("target_absorbance_min", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.TargetAbsorbanceMin));
            paramInfo.Add(new SQLiteParamInfo("target_absorbance_max", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.TargetAbsorbanceMax));

            paramInfo.Add(new SQLiteParamInfo("serial_no", DbType.String, serialNo));

            ManagerQueryService.Current.QueryServiceQualification.InsertQualificationInfo(paramInfo);
        }

        private void InsertQualificationData(string testInfoSEQ)
        {
            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("qualification_info_seq", DbType.String, testInfoSEQ));
            paramInfo.Add(new SQLiteParamInfo("reg_no", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.RegistrationInfo.REG_NO));

            paramInfo.Add(new SQLiteParamInfo("start_chamber_temperature", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationSummary.StartChamberTemperature));
            paramInfo.Add(new SQLiteParamInfo("chamber_tc", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.SeriesChamberTC.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("fan_flowrate", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.SeriesFanFlowrate.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("transmission", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.SeriesTransmission.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.SeriesAbsorbance.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("max_absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.SeriesMaxAbsorbance.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("max_absorbance0", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.SeriesMaxAbsorbance0.GetValueString()));
            paramInfo.Add(new SQLiteParamInfo("corrected_absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.CorrectedAbsorbance));

            paramInfo.Add(new SQLiteParamInfo("maximum_absorbance", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.MaxAbsorbance));
            paramInfo.Add(new SQLiteParamInfo("minimum_transmission", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.MinTransmission));
            //paramInfo.Add(new SQLiteParamInfo("minimum_transmission_second", DbType.String, ConvertTimeStringToSecond(this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.MinTransmissionSecond)));
            paramInfo.Add(new SQLiteParamInfo("minimum_transmission_second", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.MinTransmissionSecond));

            //paramInfo.Add(new SQLiteParamInfo("flameout_second", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.FlameoutSecond));
            paramInfo.Add(new SQLiteParamInfo("flameout_second", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.FlameoutTime));
            paramInfo.Add(new SQLiteParamInfo("test_duration", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_Qualification.Standard.FinishTimeSecond));

            paramInfo.Add(new SQLiteParamInfo("summary_description", DbType.String, this.SESSION_MANAGER.IEC61034_DataSetQualification.Info_QualificationSummary.Description));

            ManagerQueryService.Current.QueryServiceQualification.InsertQualificationData(paramInfo);
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

            ManagerQueryService.Current.QueryServiceQualificationRegistration.UpdateQualificationStatus(paramInfo);
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
