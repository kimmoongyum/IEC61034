using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;
using System;

namespace FTSolutions.IEC61034.Common.Result
{
    public class TestProperty : BaseTestItem
    {
        public TestProperty()
        {

        }



        //###################################################################
        //  Property
        //###################################################################

        private string _testDateTime;
        public string TestDateTime
        {
            get { return _testDateTime; }
            set
            {
                if (this._testDateTime != value)
                {
                    this._testDateTime = value;
                    this.RaisePropertyChanged(nameof(TestDateTime));
                }
            }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                if (this._number != value)
                {
                    this._number = value;
                    this.RaisePropertyChanged(nameof(Number));
                }
            }
        }

        private string _totalNumber;
        public string TotalNumber
        {
            get { return _totalNumber; }
            set
            {
                if (this._totalNumber != value)
                {
                    this._totalNumber = value;
                    this.RaisePropertyChanged(nameof(TotalNumber));
                }
            }
        }


        private string _maxTestDuration;
        public string MaxTestDuration
        {
            get { return _maxTestDuration; }
            set
            {
                if (this._maxTestDuration != value)
                {
                    this._maxTestDuration = value;
                    this.RaisePropertyChanged(nameof(MaxTestDuration));
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

        private string _cableLength;
        public string CableLength
        {
            get { return _cableLength; }
            set
            {
                if (this._cableLength != value)
                {
                    this._cableLength = value;
                    this.RaisePropertyChanged(nameof(CableLength));
                }
            }
        }

        private string _cableDiameter;
        public string CableDiameter
        {
            get { return _cableDiameter; }
            set
            {
                if (this._cableDiameter != value)
                {
                    this._cableDiameter = value;
                    this.RaisePropertyChanged(nameof(CableDiameter));
                }
            }
        }

        private string _cableMajorAxis;
        public string CableMajorAxis
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

        private string _cableMinorAxis;
        public string CableMinorAxis
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

        private string _testPiecesCount;
        public string TestPiecesCount
        {
            get { return _testPiecesCount; }
            set
            {
                if (this._testPiecesCount != value)
                {
                    this._testPiecesCount = value;
                    this.RaisePropertyChanged(nameof(TestPiecesCount));
                }
            }
        }


        //###################################################################
        //  Override
        //###################################################################

        public override void Clear()
        {
            this.TestDateTime = String.Empty;

            this.Number = String.Empty;
            this.TotalNumber = String.Empty;

            this.CABLE_TYPE = string.Empty;
            this.CableLength = string.Empty;
            this.CableDiameter = string.Empty; 
            this.CableMajorAxis = string.Empty;
            this.CableMinorAxis = string.Empty;
            this.MaxTestDuration = string.Empty;
            this.TestPiecesCount = string.Empty;

        }

        public override bool IsValid()
        {
            if (this.Number == null || this.Number.Trim().Length < 1)
            {
                return false;
            }

            if (this.TotalNumber == null || this.TotalNumber.Trim().Length < 1)
            {
                return false;
            }

            return true;
        }

        public override void CopyValueFrom(BaseTestItem source)
        {
            TestProperty sourceValue = source as TestProperty;

            if (sourceValue != null)
            {
                this.TestDateTime = sourceValue.TestDateTime;

                this.Number = sourceValue.Number;
                this.TotalNumber = sourceValue.TotalNumber;

                this.CABLE_TYPE = sourceValue.CABLE_TYPE;
                this.CableLength = sourceValue.CableLength;
                this.CableDiameter = sourceValue.CableDiameter;
                this.CableMajorAxis = sourceValue.CableMajorAxis;
                this.CableMinorAxis = sourceValue.CableMinorAxis;

                this.MaxTestDuration = sourceValue.MaxTestDuration;
                this.TestPiecesCount = sourceValue.TestPiecesCount;

            }
        }


        //###################################################################
        //  Override
        //###################################################################

        public void SetNow()
        {
            TestDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        public void SetTestInfo(TypeRegistration regInfo)
        {
            this.CABLE_TYPE = regInfo.CABLE_TYPE;
            this.CableDiameter = regInfo.CABLE_DIAMETER;
            this.CableMajorAxis = regInfo.CABLE_MAJOR_AXIS;
            this.CableMinorAxis = regInfo.CABLE_MINOR_AXIS;

            this.TestPiecesCount = regInfo.TEST_PIECES_COUNT;
        }
    }
}
