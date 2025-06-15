using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.QueryService;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace FTSolutions.IEC61034.BizLogic.ViewModel.Utility
{
    public class vmPopup_New_QualificationRegistration : BaseIEC61034ViewModel
    {
        public vmPopup_New_QualificationRegistration()
        {
            this.RegistrationInfo = new TypeQualificationRegistration();

            this.IsToluene4 = false;
            this.IsToluene10 = false;

            this.AllowSave = false;

            this.QualificationInfoDataCollection = new QualificationInfoDataCollection();

            ChangeStandardCommand = new DelegateCommand((o) => ExecuteChangeStandardCommand(o));

            this.ExecuteChangeStandardCommand("1");
        }



        //###################################################################
        //  Command 
        //###################################################################

        public ICommand ChangeStandardCommand { get; private set; }

        //###################################################################
        //  Property
        //###################################################################

        private TypeQualificationRegistration _registrationInfo;
        public TypeQualificationRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                _registrationInfo = value;
                this.RaisePropertyChanged(nameof(RegistrationInfo));
            }
        }
        
        private QualificationInfoDataCollection _qualificationInfoDataCollection;
        public QualificationInfoDataCollection QualificationInfoDataCollection
        {
            get { return _qualificationInfoDataCollection; }
            set
            {
                _qualificationInfoDataCollection = value;
                this.RaisePropertyChanged(nameof(QualificationInfoDataCollection));
            }
        }


        #region Standard
        private bool _is61034;
        public bool Is61034
        {
            get { return _is61034; }
            set
            {
                if (this._is61034 != value)
                {
                    this._is61034 = value;
                    this.RaisePropertyChanged(nameof(Is61034));
                }
            }
        }
        #endregion       

        #region Toluene Radiobutton.
        private bool _isToluene4;
        public bool IsToluene4
        {
            get { return _isToluene4; }
            set
            {
                if (this._isToluene4 != value)
                {
                    this._isToluene4 = value;
                    this.RaisePropertyChanged(nameof(IsToluene4));
                }

                if (value)
                {
                    this.RegistrationInfo.TOLUENE_CONTENT = IEC61034Const.KEY_TOLUENE_4;
                }
            }
        }

        private bool _isToluene10;
        public bool IsToluene10
        {
            get { return _isToluene10; }
            set
            {
                if (this._isToluene10 != value)
                {
                    this._isToluene10 = value;
                    this.RaisePropertyChanged(nameof(IsToluene10));
                }

                if (value)
                {
                    this.RegistrationInfo.TOLUENE_CONTENT = IEC61034Const.KEY_TOLUENE_10;
                }
            }
        }
        #endregion

        #region Title
        private string _iec61034_Title;
        public string IEC61034_TITLE
        {
            get { return _iec61034_Title; }
            set
            {
                this._iec61034_Title = value;
                this.RaisePropertyChanged(nameof(IEC61034_TITLE));
            }
        }
        #endregion

        private bool _allowSave;
        public bool AllowSave
        {
            get { return _allowSave; }
            set
            {
                this._allowSave = value;
                this.RaisePropertyChanged(nameof(AllowSave));
            }
        }

        private double _tolueneContent;
        public double TolueneContent
        {
            get { return _tolueneContent; }
            set
            {
                if (this._tolueneContent != value)
                {
                    this._tolueneContent = value;
                    this.RaisePropertyChanged(nameof(TolueneContent));
                }
            }
        }
        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            this.IsPopupOwner = true;

            this.InitializeData();

            if(IEC61034Const.DEFAULT_TOLUENE_CONTENT == 4)
            {
                this.IsToluene4 = true;
                this.IsToluene10 = false;
            }
            else
            {
                this.IsToluene4 = false;
                this.IsToluene10 = true;
            }

            this.IEC61034_TITLE = this.SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_IEC61034);
        }

        public override void Dispose()
        {
            base.Dispose();
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        public void ExecuteChangeStandardCommand(object obj)
        {
            this.Is61034 = true;
            this.TolueneContent = IEC61034Const.DEFAULT_TOLUENE_CONTENT;
        }

    //###################################################################
    //  Public
    //###################################################################

        public void SetRegistrationInfo(TypeQualificationRegistration info)
        {
            this.RegistrationInfo.CopyValueFrom(info);

            this.Is61034 = true;

            if (this.RegistrationInfo.SEQ == null || this.RegistrationInfo.SEQ.Trim().Length < 1)
            {
                this.AllowSave = true;
            }
            else if (this.RegistrationInfo.STATUS.Equals(UILabelConst.CODE_R))
            {
                this.AllowSave = true;
            }
            else
            {
                this.AllowSave = false;
            }

            if(this.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_4)
            {
                this.IsToluene4 = true;
                this.IsToluene10 = false;
            }


            if (this.RegistrationInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_10)
            {
                this.IsToluene4 = false;
                this.IsToluene10 = true;
            }

            this.BindingQualificationInfoList();
        }

        public bool CheckValidation()
        {
            if (this.IsEmpty(this.RegistrationInfo.REG_NO) || this.IsEmpty(this.RegistrationInfo.REG_DATE))
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_input_missing");
                return false;
            }

            return true;
        }

        public void SaveQualificationRegstrationInfo()
        {
            string regNo = this.RegistrationInfo.REG_DATE + "_" + this.RegistrationInfo.REG_ORDER.PadLeft(3, '0');

            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("seq", DbType.String, this.RegistrationInfo.SEQ));
            paramInfo.Add(new SQLiteParamInfo("regNo", DbType.String, regNo));
            paramInfo.Add(new SQLiteParamInfo("regDate", DbType.String, this.RegistrationInfo.REG_DATE));
            paramInfo.Add(new SQLiteParamInfo("regOrder", DbType.String, this.RegistrationInfo.REG_ORDER));
            //paramInfo.Add(new SQLiteParamInfo("custNo", DbType.String, this.RegistrationInfo.CUST_NO));            

            paramInfo.Add(new SQLiteParamInfo("useSpecimenCondition", DbType.String, this.RegistrationInfo.USE_SPECIMEN_CONDITION ? "Y" : "N"));
            paramInfo.Add(new SQLiteParamInfo("conditionTemperature", DbType.String, this.RegistrationInfo.CONDITION_TEMPERATURE));
            paramInfo.Add(new SQLiteParamInfo("conditionHumidity", DbType.String, this.RegistrationInfo.CONDITION_HUMIDITY));

            //string standardType = this.RegistrationInfo.STANDARD_TYPE.ToString();
            //paramInfo.Add(new SQLiteParamInfo("standardType", DbType.String, standardType));

            paramInfo.Add(new SQLiteParamInfo("tolueneContent", DbType.String, this.RegistrationInfo.TOLUENE_CONTENT));

            if (this.RegistrationInfo.SEQ != null && this.RegistrationInfo.SEQ.Trim().Length > 0)
            {
                ManagerQueryService.Current.QueryServiceQualificationRegistration.UpdateRegistrationInfo(paramInfo);
            }
            else
            {
                ManagerQueryService.Current.QueryServiceQualificationRegistration.InsertRegistrationInfo(paramInfo);
            }
        }

        public void DeleteQualificationRegstrationInfo()
        {
            if (this.RegistrationInfo.SEQ != null && this.RegistrationInfo.SEQ.Trim().Length > 0)
            {
                SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
                paramInfo.Add(new SQLiteParamInfo("seq", DbType.String, this.RegistrationInfo.SEQ));

                ManagerQueryService.Current.QueryServiceQualificationRegistration.DeleteRegistrationInfo(paramInfo);
            }
        }

        public void DeleteQualificationInfo(string regNo, string seq)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_delete", "msg_delete_data");

            if (result)
            {
                ManagerQueryService.Current.QueryServiceQualification.DeleteQualificationInfo(regNo, seq);

                this.QualificationInfoDataCollection.Clear();
                this.BindingQualificationInfoList();
            }
        }


        public void DeleteQualificationInfo2(string regNo, string seq)
        {
            ManagerQueryService.Current.QueryServiceQualification.DeleteQualificationInfo(regNo, seq);

            this.QualificationInfoDataCollection.Clear();
            this.BindingQualificationInfoList();
        }




        //###################################################################
        //  Private
        //###################################################################

        private bool IsEmpty(string value)
        {
            if (value != null && value.Trim().Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void InitializeData()
        {
            this.AllowSave = true;

            if (this.RegistrationInfo.SEQ == null || this.RegistrationInfo.SEQ.Trim().Length < 1)
            {
                this.RegistrationInfo.REG_DATE = DateTime.Now.ToString("yyyyMMdd");
                int regOrder = ManagerQueryService.Current.QueryServiceQualificationRegistration.GetNextRegOrder(this.RegistrationInfo.REG_DATE);

                this.RegistrationInfo.REG_ORDER = regOrder.ToString();

                this.RegistrationInfo.REG_NO = this.RegistrationInfo.REG_DATE + "_" + this.RegistrationInfo.REG_ORDER.PadLeft(3, '0');
            }
        }


        private void BindingFlameApplicationTime(StandardType type, string minute)
        {           
        }

        private void BindingQualificationInfoList()
        {
            this.QualificationInfoDataCollection.GenerateQualificationInfoData(this.RegistrationInfo.REG_NO);
        }
    }
}
