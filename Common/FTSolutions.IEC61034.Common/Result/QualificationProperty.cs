using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;
using System;

namespace FTSolutions.IEC61034.Common.Result
{
    public class QualificationProperty : BaseTestItem
    {
        public QualificationProperty()
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
            }
        }

        //###################################################################
        //  Override
        //###################################################################

        public override void Clear()
        {
            this.TestDateTime = String.Empty;

            this.IsToluene4 = false;
            this.IsToluene10 = false;
        }

        public override bool IsValid()
        {
            return true;
        }

        public override void CopyValueFrom(BaseTestItem source)
        {
            QualificationProperty sourceValue = source as QualificationProperty;

            if (sourceValue != null)
            {
                this.TestDateTime = sourceValue.TestDateTime;

                this.IsToluene4 = sourceValue.IsToluene4;
                this.IsToluene10 = sourceValue.IsToluene10;   
            }
        }


        //###################################################################
        //  Override
        //###################################################################

        public void SetNow()
        {
            TestDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        public void SetQualificationInfo(TypeQualificationRegistration regInfo)
        {
            if(regInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_4)
            {
                this.IsToluene4 = true;
                this.IsToluene10 = false;
            }
            else if (regInfo.TOLUENE_CONTENT == IEC61034Const.KEY_TOLUENE_10)
            {
                this.IsToluene4 = false;
                this.IsToluene10 = true;
            }
        }
    }
}
