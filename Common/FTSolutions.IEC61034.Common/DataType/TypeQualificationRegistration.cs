using eccFramework.SharedLib.Core.Base;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class TypeQualificationRegistrationCollection : ObservableCollection<TypeQualificationRegistration>
    {
        public TypeQualificationRegistrationCollection()
        { }


        public bool AddNew()
        {
            this.Add(new TypeQualificationRegistration());

            return true;
        }

        public bool AddNew(TypeQualificationRegistration regInfo)
        {
            if (regInfo.SEQ != null)
            {
                if (this.Where(t => t.SEQ == regInfo.SEQ).Count() > 0)
                {
                    return false;
                }
            }

            this.Add(regInfo);

            return true;
        }
    }
    

    public class TypeQualificationRegistration : BaseModel
    {
        public TypeQualificationRegistration()
        {
        }

        public TypeQualificationRegistration(DataRow row) : this()
        {
            this.SEQ = row["SEQ"].ToString();
            this.REG_NO = row["REG_NO"].ToString();
            this.REG_DATE = row["REG_DATE"].ToString();
            this.REG_ORDER = row["REG_ORDER"].ToString();       

            this.USE_SPECIMEN_CONDITION = "Y".Equals(row["USE_SPECIMEN_CONDITION"].ToString().ToUpper());
            this.CONDITION_TEMPERATURE = row["CONDITION_TEMPERATURE"].ToString();
            this.CONDITION_HUMIDITY = row["CONDITION_HUMIDITY"].ToString();

            this.TEST_DATE_TIME = row["TEST_DATE_TIME"].ToString(); 
            this.TOLUENE_CONTENT = row["TOLUENE_CONTENT"].ToString();

            this.STATUS = row["STATUS"].ToString();
            this.UPDATE_DT = row["UPDATE_DT"].ToString();
        }


        public string SEQ { get; set; }

        private string _regNo;
        public string REG_NO
        {
            get { return _regNo; }
            set
            {
                if (this._regNo != value)
                {
                    this._regNo = value;
                    this.RaisePropertyChanged(nameof(REG_NO));
                }
            }
        }

        public string REG_DATE { get; set; }
        public string REG_ORDER { get; set; }

               
        private bool _useSpecimenCondition;
        public bool USE_SPECIMEN_CONDITION
        {
            get { return _useSpecimenCondition; }
            set
            {
                if (this._useSpecimenCondition != value)
                {
                    this._useSpecimenCondition = value;
                    this.RaisePropertyChanged(nameof(USE_SPECIMEN_CONDITION));
                }
            }
        }

        private string _conditionTemperature;
        public string CONDITION_TEMPERATURE
        {
            get { return _conditionTemperature; }
            set
            {
                if (this._conditionTemperature != value)
                {
                    this._conditionTemperature = value;
                    this.RaisePropertyChanged(nameof(CONDITION_TEMPERATURE));
                }
            }
        }

        private string _conditionHumidity;
        public string CONDITION_HUMIDITY
        {
            get { return _conditionHumidity; }
            set
            {
                if (this._conditionHumidity != value)
                {
                    this._conditionHumidity = value;
                    this.RaisePropertyChanged(nameof(CONDITION_HUMIDITY));
                }
            }
        }

        /*
        private StandardType _standardType;
        public StandardType STANDARD_TYPE
        {
            get { return _standardType; }
            set
            {
                if (this._standardType != value)
                {
                    this._standardType = value;
                }

                this.RaisePropertyChanged(nameof(STANDARD_TYPE));
            }
        }

        private string _standardDesc;
        public string STANDARD_DESC
        {
            get
            {
                return SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_IEC61034);
            }
            set
            {
                if (this._standardDesc != value)
                {
                    this._standardDesc = value;
                    this.RaisePropertyChanged(nameof(STANDARD_DESC));
                }
            }
        }
        */


        private string _testDateTime;
        public string TEST_DATE_TIME
        {
            get { return _testDateTime; }
            set
            {
                if (this._testDateTime != value)
                {
                    this._testDateTime = value;
                    this.RaisePropertyChanged(nameof(TEST_DATE_TIME));
                }
            }
        }
        private string _tolueneContent;
        public string TOLUENE_CONTENT
        {
            get { return _tolueneContent; }
            set
            {
                if (this._tolueneContent != value)
                {
                    this._tolueneContent = value;
                    this.RaisePropertyChanged(nameof(TOLUENE_CONTENT));
                }
            }
        }

        private string _status;
        public string STATUS
        {
            get { return _status; }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.RaisePropertyChanged(nameof(STATUS));
                }
            }
        }

        public string UPDATE_DT { get; set; }


        public void CopyValueFrom(TypeQualificationRegistration source)
        {
            if (source != null)
            {
                this.SEQ = source.SEQ;
                this.REG_NO = source.REG_NO;
                this.REG_DATE = source.REG_DATE;
                this.REG_ORDER = source.REG_ORDER;            

                this.USE_SPECIMEN_CONDITION = source.USE_SPECIMEN_CONDITION;
                this.CONDITION_TEMPERATURE = source.CONDITION_TEMPERATURE;
                this.CONDITION_HUMIDITY = source.CONDITION_HUMIDITY;

                //this.STANDARD_TYPE = source.STANDARD_TYPE;
                //this.STANDARD_DESC = source.STANDARD_DESC;
                
                this.TOLUENE_CONTENT = source.TOLUENE_CONTENT;            

                this.STATUS = source.STATUS;
                this.UPDATE_DT = source.UPDATE_DT;
            }
        }

        public void Clear()
        {
            this.SEQ = string.Empty;
            this.REG_NO = string.Empty;
            this.REG_DATE = string.Empty;
            this.REG_ORDER = string.Empty;

            this.USE_SPECIMEN_CONDITION = false;
            this.CONDITION_TEMPERATURE = string.Empty;
            this.CONDITION_HUMIDITY = string.Empty;

            //this.STANDARD_TYPE = StandardType.NA;
            //this.STANDARD_DESC = string.Empty;

            this.TOLUENE_CONTENT = string.Empty;

            this.STATUS = string.Empty;
            this.UPDATE_DT = string.Empty;
        }
    }
}
