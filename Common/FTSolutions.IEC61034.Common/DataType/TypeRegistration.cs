using eccFramework.SharedLib.Core.Base;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class TypeRegistrationCollection : ObservableCollection<TypeRegistration>
    {
        public TypeRegistrationCollection()
        { }


        public bool AddNew()
        {
            this.Add(new TypeRegistration());

            return true;
        }

        public bool AddNew(TypeRegistration regInfo)
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
    

    public class TypeRegistration : BaseModel
    {
        public TypeRegistration()
        {
        }

        public TypeRegistration(DataRow row) : this()
        {
            this.SEQ = row["SEQ"].ToString();
            this.REG_NO = row["REG_NO"].ToString();
            this.REG_DATE = row["REG_DATE"].ToString();
            this.REG_ORDER = row["REG_ORDER"].ToString();
            this.CUST_NO = row["CUST_NO"].ToString();

            this.SPONSOR_NAME = row["SPONSOR_NAME"].ToString();
            this.SPONSOR_ADDR = row["SPONSOR_ADDR"].ToString();
            this.SUPPLIER_NAME = row["SUPPLIER_NAME"].ToString();
            this.SUPPLIER_ADDR = row["SUPPLIER_ADDR"].ToString();
            this.PRODUCT_NAME = row["PRODUCT_NAME"].ToString();
            this.PRODUCT_DESC = row["PRODUCT_DESC"].ToString();
            this.OPERATOR_NAME = row["OPERATOR_NAME"].ToString();
            this.MANAGER_NAME = row["MANAGER_NAME"].ToString();

            this.USE_SPECIMEN_CONDITION = "Y".Equals(row["USE_SPECIMEN_CONDITION"].ToString().ToUpper());
            this.CONDITION_TEMPERATURE = row["CONDITION_TEMPERATURE"].ToString();
            this.CONDITION_HUMIDITY = row["CONDITION_HUMIDITY"].ToString();

            this.CABLE_TYPE = row["CABLE_TYPE"].ToString();
            this.CABLE_DESC = row["CABLE_DESC"].ToString();
            this.CABLE_DIAMETER = row["CABLE_DIAMETER"].ToString();
            this.CABLE_MAJOR_AXIS = row["CABLE_MAJOR_AXIS"].ToString();
            this.CABLE_MINOR_AXIS = row["CABLE_MINOR_AXIS"].ToString();
            this.TEST_PIECES_COUNT = row["TEST_PIECES_COUNT"].ToString();

            this.STATUS = row["STATUS"].ToString();
            this.NUMBER = row["NUMBER"].ToString();
            this.TOTAL_NUMBER = row["TOTAL_NUMBER"].ToString();
            this.TEST_COUNT = row["TEST_COUNT"].ToString();
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

        private string _custNo;
        public string CUST_NO
        {
            get { return _custNo; }
            set
            {
                if (this._custNo != value)
                {
                    this._custNo = value;
                }

                this.RaisePropertyChanged(nameof(CUST_NO));
            }
        }


        private string _sponsorName;
        public string SPONSOR_NAME
        {
            get { return _sponsorName; }
            set
            {
                if (this._sponsorName != value)
                {
                    this._sponsorName = value;
                }

                this.RaisePropertyChanged(nameof(SPONSOR_NAME));
            }
        }

        private string _sponsorAddr;
        public string SPONSOR_ADDR
        {
            get { return _sponsorAddr; }
            set
            {
                if (this._sponsorAddr != value)
                {
                    this._sponsorAddr = value;
                    this.RaisePropertyChanged(nameof(SPONSOR_ADDR));
                }
            }
        }

        private string _supplierName;
        public string SUPPLIER_NAME
        {
            get { return _supplierName; }
            set
            {
                if (this._supplierName != value)
                {
                    this._supplierName = value;
                    this.RaisePropertyChanged(nameof(SUPPLIER_NAME));
                }
            }
        }

        private string _supplierAddr;
        public string SUPPLIER_ADDR
        {
            get { return _supplierAddr; }
            set
            {
                if (this._supplierAddr != value)
                {
                    this._supplierAddr = value;
                    this.RaisePropertyChanged(nameof(SUPPLIER_ADDR));
                }
            }
        }

        private string _productName;
        public string PRODUCT_NAME
        {
            get { return _productName; }
            set
            {
                if (this._productName != value)
                {
                    this._productName = value;
                }

                this.RaisePropertyChanged(nameof(PRODUCT_NAME));
            }
        }

        private string _productDesc;
        public string PRODUCT_DESC
        {
            get { return _productDesc; }
            set
            {
                if (this._productDesc != value)
                {
                    this._productDesc = value;
                    this.RaisePropertyChanged(nameof(PRODUCT_DESC));
                }
            }
        }


        private string _operatorName;
        public string OPERATOR_NAME
        {
            get { return _operatorName; }
            set
            {
                if (this._operatorName != value)
                {
                    this._operatorName = value;
                    this.RaisePropertyChanged(nameof(OPERATOR_NAME));
                }
            }
        }

        private string _managerName;
        public string MANAGER_NAME
        {
            get { return _managerName; }
            set
            {
                if (this._managerName != value)
                {
                    this._managerName = value;
                    this.RaisePropertyChanged(nameof(MANAGER_NAME));
                }
            }
        }
        
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

        private string _cableType;
        public string CABLE_TYPE
        {
            get { return _cableType; }
            set
            {
                if (this._cableType != value)
                {
                    this._cableType = value;
                    this.RaisePropertyChanged(nameof(CABLE_TYPE));
                }
            }
        }

        private string _cableDesc;
        public string CABLE_DESC
        {
            get { return _cableDesc; }
            set
            {
                if (this._cableDesc != value)
                {
                    this._cableDesc = value;
                    this.RaisePropertyChanged(nameof(CABLE_DESC));
                }
            }
        }

        private string _cableDiameter;
        public string CABLE_DIAMETER
        {
            get { return _cableDiameter; }
            set
            {
                if (this._cableDiameter != value)
                {
                    this._cableDiameter = value;
                    this.RaisePropertyChanged(nameof(CABLE_DIAMETER));
                }
            }
        }

        private string _cableMajorAxis;
        public string CABLE_MAJOR_AXIS
        {
            get { return _cableMajorAxis; }
            set
            {
                if (this._cableMajorAxis != value)
                {
                    this._cableMajorAxis = value;
                    this.RaisePropertyChanged(nameof(CABLE_MAJOR_AXIS));
                }
            }
        }

        private string _cableMinorAxis;
        public string CABLE_MINOR_AXIS
        {
            get { return _cableMinorAxis; }
            set
            {
                if (this._cableMinorAxis != value)
                {
                    this._cableMinorAxis = value;
                    this.RaisePropertyChanged(nameof(CABLE_MINOR_AXIS));
                }
            }
        }

        private string _testPiecesCount;
        public string TEST_PIECES_COUNT
        {
            get { return _testPiecesCount; }
            set
            {
                if (this._testPiecesCount != value)
                {
                    this._testPiecesCount = value;
                    this.RaisePropertyChanged(nameof(TEST_PIECES_COUNT));
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

        private string _number;
        public string NUMBER
        {
            get { return _number; }
            set
            {
                if (this._number != value)
                {
                    this._number = value;
                    this.RaisePropertyChanged(nameof(NUMBER));
                }
            }
        }

        private string _totalNumber;
        public string TOTAL_NUMBER
        {
            get { return _totalNumber; }
            set
            {
                if (this._totalNumber != value)
                {
                    this._totalNumber = value;
                    this.RaisePropertyChanged(nameof(TOTAL_NUMBER));
                }
            }
        }

        private string _testCounter;
        public string TEST_COUNT
        {
            get { return _testCounter; }
            set
            {
                if (this._testCounter != value)
                {
                    this._testCounter = value;
                    this.RaisePropertyChanged(nameof(TEST_COUNT));
                }
            }
        }

        public string UPDATE_DT { get; set; }


        public void CopyValueFrom(TypeRegistration source)
        {
            if (source != null)
            {
                this.SEQ = source.SEQ;
                this.REG_NO = source.REG_NO;
                this.REG_DATE = source.REG_DATE;
                this.REG_ORDER = source.REG_ORDER;
                this.CUST_NO = source.CUST_NO;
                this.SPONSOR_NAME = source.SPONSOR_NAME;
                this.SPONSOR_ADDR = source.SPONSOR_ADDR;
                this.SUPPLIER_NAME = source.SUPPLIER_NAME;
                this.SUPPLIER_ADDR = source.SUPPLIER_ADDR;
                this.PRODUCT_NAME = source.PRODUCT_NAME;
                this.PRODUCT_DESC = source.PRODUCT_DESC;
                this.OPERATOR_NAME = source.OPERATOR_NAME;
                this.MANAGER_NAME = source.MANAGER_NAME;

                this.USE_SPECIMEN_CONDITION = source.USE_SPECIMEN_CONDITION;
                this.CONDITION_TEMPERATURE = source.CONDITION_TEMPERATURE;
                this.CONDITION_HUMIDITY = source.CONDITION_HUMIDITY;

                this.CABLE_TYPE = source.CABLE_TYPE;
                this.CABLE_DESC = source.CABLE_DESC;
                this.CABLE_DIAMETER = source.CABLE_DIAMETER;

                if (this.CABLE_TYPE.Equals("FLAT", StringComparison.OrdinalIgnoreCase))
                {
                    this.CABLE_MAJOR_AXIS = source.CABLE_MAJOR_AXIS;
                    this.CABLE_MINOR_AXIS = source.CABLE_MINOR_AXIS;
                }
                else
                {
                    this.CABLE_MAJOR_AXIS = string.Empty;
                    this.CABLE_MINOR_AXIS = string.Empty;
                }

                this.TEST_PIECES_COUNT = source.TEST_PIECES_COUNT;           

                this.STATUS = source.STATUS;
                this.NUMBER = source.NUMBER;
                this.TOTAL_NUMBER = source.TOTAL_NUMBER;
                this.TEST_COUNT = source.TEST_COUNT;
                this.UPDATE_DT = source.UPDATE_DT;
            }
        }

        public void Clear()
        {
            this.SEQ = string.Empty;
            this.REG_NO = string.Empty;
            this.REG_DATE = string.Empty;
            this.REG_ORDER = string.Empty;
            this.CUST_NO = string.Empty;
            this.SPONSOR_NAME = string.Empty;
            this.SPONSOR_ADDR = string.Empty;
            this.SUPPLIER_NAME = string.Empty;
            this.SUPPLIER_ADDR = string.Empty;
            this.PRODUCT_NAME = string.Empty;
            this.PRODUCT_DESC = string.Empty;
            this.OPERATOR_NAME = string.Empty;
            this.MANAGER_NAME = string.Empty;

            this.USE_SPECIMEN_CONDITION = false;
            this.CONDITION_TEMPERATURE = string.Empty;
            this.CONDITION_HUMIDITY = string.Empty;

            this.CABLE_TYPE = string.Empty;
            this.CABLE_DESC = string.Empty;
            this.CABLE_DIAMETER = string.Empty;
            this.CABLE_MAJOR_AXIS = string.Empty;
            this.CABLE_MINOR_AXIS = string.Empty;
            this.TEST_PIECES_COUNT = string.Empty;

            this.STATUS = string.Empty;
            this.NUMBER = string.Empty;
            this.TOTAL_NUMBER = string.Empty;
            this.TEST_COUNT = string.Empty;
            this.UPDATE_DT = string.Empty;
        }
    }
}
