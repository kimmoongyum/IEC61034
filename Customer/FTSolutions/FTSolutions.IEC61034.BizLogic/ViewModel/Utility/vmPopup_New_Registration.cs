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
    public class vmPopup_New_Registration : BaseIEC61034ViewModel
    {
        public vmPopup_New_Registration()
        {
            this.RegistrationInfo = new TypeRegistration();

            this.AllowSave = false;

            this.TestInfoDataCollection = new TestInfoDataCollection();

            ChangeStandardCommand = new DelegateCommand((o) => ExecuteChangeStandardCommand(o));
            CalculateCableCountCommand = new DelegateCommand((o) => ExecuteCalculateCableCountCommand(o));

            this.FlatCableDiameter = string.Empty;
            this.RoundCableDiameter = string.Empty;
            
            this.ExecuteChangeStandardCommand("1");
        }



        //###################################################################
        //  Command 
        //###################################################################

        public ICommand ChangeStandardCommand { get; private set; }
        public ICommand CalculateCableCountCommand { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        private TypeRegistration _registrationInfo;
        public TypeRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                _registrationInfo = value;
                this.RaisePropertyChanged(nameof(RegistrationInfo));
            }
        }
        
        private TestInfoDataCollection _testInfoDataCollection;
        public TestInfoDataCollection TestInfoDataCollection
        {
            get { return _testInfoDataCollection; }
            set
            {
                _testInfoDataCollection = value;
                this.RaisePropertyChanged(nameof(TestInfoDataCollection));
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

        #region Cable Radiobutton.
        // flat 케이블 일때, 단축이 2mm 보다 작거나, 주축/단축 비가 16을 초과할 경우, 케이블 수를 직접 입력하게 해야 한다.
        private bool _isCustom;
        public bool IsCustom
        {
            get { return _isCustom; }
            set
            {
                if (this._isCustom != value)
                {
                    this._isCustom = value;
                    this.RaisePropertyChanged(nameof(IsCustom));
                }
            }
        }

        private bool _isCableRound;
        public bool IsCableRound
        {
            get { return _isCableRound; }
            set
            {
                if (this._isCableRound != value)
                {
                    this._isCableRound = value;
                    this.RaisePropertyChanged(nameof(IsCableRound));
                }

                if(value)
                {
                    this.FlatCableDiameter = string.Empty;
                    this.RoundCableDiameter = string.Empty;
                    this.RegistrationInfo.CABLE_TYPE = "ROUND";
                    this.RegistrationInfo.CABLE_DIAMETER = string.Empty;
                    this.RegistrationInfo.CABLE_MINOR_AXIS = string.Empty;
                    this.RegistrationInfo.CABLE_MAJOR_AXIS = string.Empty;
                    this.RegistrationInfo.TEST_PIECES_COUNT = string.Empty;
                }
            }
        }

        private bool _isCableFlat;
        public bool IsCableFlat
        {
            get { return _isCableFlat; }
            set
            {
                if (this._isCableFlat != value)
                {
                    this._isCableFlat = value;
                    this.RaisePropertyChanged(nameof(IsCableFlat));
                }

                if (value)
                {
                    this.FlatCableDiameter = string.Empty;
                    this.RoundCableDiameter = string.Empty;
                    this.RegistrationInfo.CABLE_TYPE = "FLAT";
                    this.RegistrationInfo.CABLE_DIAMETER = string.Empty;
                    this.RegistrationInfo.CABLE_MINOR_AXIS = string.Empty;
                    this.RegistrationInfo.CABLE_MAJOR_AXIS = string.Empty;
                    this.RegistrationInfo.TEST_PIECES_COUNT = string.Empty;
                }
            }
        }

        private string _roundCableDiameter;
        public string RoundCableDiameter
        {
            get { return _roundCableDiameter; }
            set
            {
                this._roundCableDiameter = value;
                this.RaisePropertyChanged(nameof(RoundCableDiameter));
            }
        }

        private string _flatCableDiameter;
        public string FlatCableDiameter
        {
            get { return _flatCableDiameter; }
            set
            {
                this._flatCableDiameter = value;
                this.RaisePropertyChanged(nameof(FlatCableDiameter));
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

        private bool _cableDimeter;
        public bool CableDimeter
        {
            get { return _cableDimeter; }
            set
            {
                if (this._cableDimeter != value)
                {
                    this._cableDimeter = value;
                    this.RaisePropertyChanged(nameof(CableDimeter));
                }
            }
        }

        private bool _cableMinorAxis;
        public bool CableMinorAxis
        {
            get { return _cableMinorAxis; }
            set
            {
                if (this._cableMinorAxis != value)
                {
                    this._cableMinorAxis = value;
                    this.RaisePropertyChanged(nameof(CableMinorAxis));
                }
            }
        }


        private bool _cableMajorAxis;
        public bool CableMajorAxis
        {
            get { return _cableMajorAxis; }
            set
            {
                if (this._cableMajorAxis != value)
                {
                    this._cableMajorAxis = value;
                    this.RaisePropertyChanged(nameof(CableMajorAxis));
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

            this.RegistrationInfo.OPERATOR_NAME = SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_OPERATOR);
            this.RegistrationInfo.MANAGER_NAME = SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_MANAGER);

            this.IEC61034_TITLE = this.SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_IEC61034);

            this.FlatCableDiameter = string.Empty;
            this.RoundCableDiameter = string.Empty;
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
            this.IsCustom = false;
            this.IsCableFlat = false;
            this.IsCableRound = false;

            this.FlatCableDiameter = string.Empty;
            this.RoundCableDiameter = string.Empty;

            switch (obj.ToString().ToUpper())
            {
                case "1":
                    this.Is61034 = true;

                    this.IsCableFlat = false;
                    this.IsCableRound = true;

                    break;
            }
        }

        public void ExecuteCalculateCableCountCommand(object obj)
        {
            if(this.IsCableRound)
            {
                this.RegistrationInfo.CABLE_DIAMETER = this.RoundCableDiameter;
                this.RegistrationInfo.TEST_PIECES_COUNT = string.Format("{0}", CalculateSpecimenCount(Convert.ToDouble(this.RegistrationInfo.CABLE_DIAMETER)));
            }
            else
            {
                double diameter = CalculateFlatDiameter(Convert.ToDouble(this.RegistrationInfo.CABLE_MAJOR_AXIS), Convert.ToDouble(this.RegistrationInfo.CABLE_MINOR_AXIS));
                if (diameter < 0)
                {
                    this.IsCustom = true;
                    this.FlatCableDiameter = string.Empty;
                    this.RegistrationInfo.CABLE_DIAMETER = this.FlatCableDiameter;
                }
                else
                {
                    this.IsCustom = false;
                    //this.RegistrationInfo.TEST_PIECES_COUNT = string.Format("{0}", CalculateSpecimenCount(Convert.ToDouble(this.RegistrationInfo.CABLE_MAJOR_AXIS), Convert.ToDouble(this.RegistrationInfo.CABLE_MINOR_AXIS)));
                    this.FlatCableDiameter = diameter.ToString();
                    this.RegistrationInfo.CABLE_DIAMETER = this.FlatCableDiameter;
                    this.RegistrationInfo.TEST_PIECES_COUNT = string.Format("{0}", CalculateNonCircularSpecimenCount(diameter));
                }
            }
        }

        private int CalculateSpecimenCount(double diameter)
        {
            if (diameter <= 0)
                return -1;
            //throw new ArgumentException("지름은 0 보다 커야 합니다.");

            if (diameter > 40)
                return 1;
            else if (diameter > 20)
                return 2;
            else if (diameter > 10)
                return 3;
            else if (diameter > 5)
                return (int)Math.Floor(45.0 / diameter); // N1
            else if (diameter >= 1)
                return (int)Math.Floor(45.0 / (3.0 * diameter)); // N2
            else
                return -1;
                //throw new ArgumentException("지름은 1mm 이상이어야 합니다.");
        }

        public double CalculateFlatDiameter(double majorAxis, double minorAxis)
        {
            if (majorAxis <= 0 || minorAxis <= 0)
                return -1;
            //throw new ArgumentException("주축과 단축은 0보다 커야 합니다.");

            if (minorAxis < 2.0)
                return -1;
                //throw new ArgumentException("단축이 2.0mm 미만인 경우는 이 계산에 해당되지 않습니다.");

            double ratio = majorAxis / minorAxis;
            double diameter = -1;

            if (ratio <= 3)
            {
                diameter = minorAxis;
            }
            else if (ratio <= 16)
            {
                double perimeter = ApproximatePerimeter(majorAxis, minorAxis);
                diameter = perimeter / (2 * 3.14); // 문서 기준
            }
            else
            {
                return -1;
                //throw new ArgumentException("주축:단축 비가 16을 초과합니다.");
            }

            return Math.Round(diameter, 1, MidpointRounding.AwayFromZero);
            //return CalculateNonCircularSpecimenCount(diameter); // 비원형 계산 함수 호출
        }

        private int CalculateNonCircularSpecimenCount(double diameter)
        {
            /*
            if (diameter > 40)
                return 1;
            else if (diameter > 20)
                return 2;
            else if (diameter > 10)
                return 3;
            else if (diameter > 5)
                return (int)Math.Floor(45.0 / diameter); // N1
            else if (diameter >= 1)
                return (int)Math.Floor(45.0 / (3.0 * diameter)); // N2
            else
                return -1;
            */

            if (diameter > 40)
                return 1;
            else if (diameter > 20)
                return 2;
            else if (diameter > 10)
                return 3;
            else if (diameter >= 1)
                return (int)Math.Floor(45.0 / diameter); // 비원형은 항상 N1 적용
            else
                return -1;
                //throw new ArgumentException("지름 D는 1mm 이상이어야 합니다.");
        }

        // 타원의 근사 둘레 계산 (Ramanujan 공식)
        private double ApproximatePerimeter(double a, double b)
        {
            return Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

    //###################################################################
    //  Public
    //###################################################################

        public void SetRegistrationInfo(TypeRegistration info)
        {
            /*
            this.RegistrationInfo.CopyValueFrom(info);

            this.Is61034 = true;
            this.IsCustom = false;
            this.IsCableFlat = false;
            this.IsCableRound = false;

            if(this.RegistrationInfo.CABLE_TYPE.Equals("ROUND"))
            {
                this.IsCableFlat = false;
                this.IsCableRound = true;
            }

            if (this.RegistrationInfo.CABLE_TYPE.Equals("FLAT"))
            {
                this.IsCableFlat = true;
                this.IsCableRound = false;
            }

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
            */

            this.Is61034 = true;

            if (info.CABLE_TYPE.Equals("ROUND"))
            {
                this.IsCableFlat = false;
                this.IsCableRound = true;
                this.RoundCableDiameter = info.CABLE_DIAMETER;
            }

            if (info.CABLE_TYPE.Equals("FLAT"))
            {
                this.IsCableFlat = true;
                this.IsCableRound = false;
                this.FlatCableDiameter = info.CABLE_DIAMETER;
            }

            if (info.SEQ == null || info.SEQ.Trim().Length < 1)
            {
                this.AllowSave = true;
            }
            else if (info.STATUS.Equals(UILabelConst.CODE_R))
            {
                this.AllowSave = true;
            }
            else
            {
                this.AllowSave = false;
            }

            this.RegistrationInfo.CopyValueFrom(info);
            this.BindingTestInfoList();
        }

        public bool CheckValidation()
        {
            if (this.IsEmpty(this.RegistrationInfo.REG_NO) || this.IsEmpty(this.RegistrationInfo.REG_DATE) ||
                this.IsEmpty(this.RegistrationInfo.SPONSOR_NAME))
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_input_missing");
                return false;
            }

            return true;
        }

        public void SaveRegstrationInfo()
        {
            if (this.RegistrationInfo.CABLE_TYPE.Equals("ROUND", StringComparison.OrdinalIgnoreCase))
            {
                this.RegistrationInfo.CABLE_DIAMETER = this.RoundCableDiameter;
            }
            else
            {
                this.RegistrationInfo.CABLE_DIAMETER = this.FlatCableDiameter;
            }
                
            string regNo = this.RegistrationInfo.REG_DATE + "_" + this.RegistrationInfo.REG_ORDER.PadLeft(3, '0');

            SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
            paramInfo.Add(new SQLiteParamInfo("seq", DbType.String, this.RegistrationInfo.SEQ));
            paramInfo.Add(new SQLiteParamInfo("regNo", DbType.String, regNo));
            paramInfo.Add(new SQLiteParamInfo("regDate", DbType.String, this.RegistrationInfo.REG_DATE));
            paramInfo.Add(new SQLiteParamInfo("regOrder", DbType.String, this.RegistrationInfo.REG_ORDER));
            paramInfo.Add(new SQLiteParamInfo("custNo", DbType.String, this.RegistrationInfo.CUST_NO));
            paramInfo.Add(new SQLiteParamInfo("sponsorName", DbType.String, this.RegistrationInfo.SPONSOR_NAME));
            paramInfo.Add(new SQLiteParamInfo("sponsorAddr", DbType.String, this.RegistrationInfo.SPONSOR_ADDR));
            paramInfo.Add(new SQLiteParamInfo("supplierName", DbType.String, this.RegistrationInfo.SUPPLIER_NAME));
            paramInfo.Add(new SQLiteParamInfo("supplierAddr", DbType.String, this.RegistrationInfo.SUPPLIER_ADDR));
            paramInfo.Add(new SQLiteParamInfo("productName", DbType.String, this.RegistrationInfo.PRODUCT_NAME));
            paramInfo.Add(new SQLiteParamInfo("productDesc", DbType.String, this.RegistrationInfo.PRODUCT_DESC));
            paramInfo.Add(new SQLiteParamInfo("operatorName", DbType.String, this.RegistrationInfo.OPERATOR_NAME));
            paramInfo.Add(new SQLiteParamInfo("managerName", DbType.String, this.RegistrationInfo.MANAGER_NAME));

            paramInfo.Add(new SQLiteParamInfo("useSpecimenCondition", DbType.String, this.RegistrationInfo.USE_SPECIMEN_CONDITION ? "Y" : "N"));
            paramInfo.Add(new SQLiteParamInfo("conditionTemperature", DbType.String, this.RegistrationInfo.CONDITION_TEMPERATURE));
            paramInfo.Add(new SQLiteParamInfo("conditionHumidity", DbType.String, this.RegistrationInfo.CONDITION_HUMIDITY));

            paramInfo.Add(new SQLiteParamInfo("cableType", DbType.String, this.RegistrationInfo.CABLE_TYPE));

            string cableDesc = string.Empty;
            if(this.RegistrationInfo.CABLE_TYPE.Equals("ROUND", StringComparison.OrdinalIgnoreCase))
            {
                cableDesc = string.Format("Round ({0}mm)", this.RegistrationInfo.CABLE_DIAMETER);
            }
            else
            {
                cableDesc = string.Format("Flat ({0}/{1} -> {2}mm)", this.RegistrationInfo.CABLE_MINOR_AXIS, this.RegistrationInfo.CABLE_MAJOR_AXIS, this.RegistrationInfo.CABLE_DIAMETER);
            }

            paramInfo.Add(new SQLiteParamInfo("cableDesc", DbType.String, cableDesc));
            paramInfo.Add(new SQLiteParamInfo("cableDiameter", DbType.String, this.RegistrationInfo.CABLE_DIAMETER));
            paramInfo.Add(new SQLiteParamInfo("cableMajorAxis", DbType.String, this.RegistrationInfo.CABLE_MAJOR_AXIS));
            paramInfo.Add(new SQLiteParamInfo("cableMinorAxis", DbType.String, this.RegistrationInfo.CABLE_MINOR_AXIS));

            paramInfo.Add(new SQLiteParamInfo("testPiecesCount", DbType.String, this.RegistrationInfo.TEST_PIECES_COUNT));

            if (this.RegistrationInfo.SEQ != null && this.RegistrationInfo.SEQ.Trim().Length > 0)
            {
                ManagerQueryService.Current.QueryServiceRegistration.UpdateRegistrationInfo(paramInfo);
            }
            else
            {
                ManagerQueryService.Current.QueryServiceRegistration.InsertRegistrationInfo(paramInfo);
            }
        }

        public void DeleteRegstrationInfo()
        {
            if (this.RegistrationInfo.SEQ != null && this.RegistrationInfo.SEQ.Trim().Length > 0)
            {
                SQLiteParamInfoCollection paramInfo = new SQLiteParamInfoCollection();
                paramInfo.Add(new SQLiteParamInfo("seq", DbType.String, this.RegistrationInfo.SEQ));

                ManagerQueryService.Current.QueryServiceRegistration.DeleteRegistrationInfo(paramInfo);
            }
        }

        public void DeleteTestInfo(string regNo, string seq)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_delete", "msg_delete_data");

            if (result)
            {
                ManagerQueryService.Current.QueryServiceTest.DeleteTestInfo(regNo, seq);

                this.TestInfoDataCollection.Clear();
                this.BindingTestInfoList();
            }
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
                int regOrder = ManagerQueryService.Current.QueryServiceRegistration.GetNextRegOrder(this.RegistrationInfo.REG_DATE);

                this.RegistrationInfo.REG_ORDER = regOrder.ToString();

                this.RegistrationInfo.REG_NO = this.RegistrationInfo.REG_DATE + "_" + this.RegistrationInfo.REG_ORDER.PadLeft(3, '0');
            }
        }


        private void BindingFlameApplicationTime(StandardType type, string minute)
        {           
        }

        private void BindingTestInfoList()
        {
            this.TestInfoDataCollection.GenerateTestInfoData(this.RegistrationInfo.REG_NO);
        }
    }
}
