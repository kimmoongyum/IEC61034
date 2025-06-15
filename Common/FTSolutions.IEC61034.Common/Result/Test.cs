using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Standard;
using System;

namespace FTSolutions.IEC61034.Common.Result
{
    public class Test : BaseTestItem
    {
        public event Action StopAction;


        public Test()
        {

        }



        //###################################################################
        //  Property
        //###################################################################

        private baseStandard _standard;
        public baseStandard Standard
        {
            get { return _standard; }
            set
            {
                if (this._standard != value)
                {
                    this._standard = value;
                    this.RaisePropertyChanged(nameof(Standard));
                }
            }
        }



        //###################################################################
        //  Override
        //###################################################################

        public override void Clear()
        {
            this.Standard?.Clear();
        }

        public override bool IsValid()
        {
            return true;
        }

        public override void CopyValueFrom(BaseTestItem source)
        {
            Test sourceValue = source as Test;

            if (sourceValue != null)
            {
                this.Standard = sourceValue.Standard;
            }
        }



        //###################################################################
        //  Public
        //###################################################################

        public void InitializeData(StandardType type)
        {
            switch (type)
            {
                case StandardType.IEC61034: this.Standard = new Standard.IEC61034("TEST"); break;
            }

            if (this.Standard != null)
            {
                this.Standard.StopAction += () =>
                {
                    if (this.StopAction != null)
                    {
                        this.StopAction();
                    }
                };
            }
        }

        public void StartTest()
        {
            this.Standard.Start();
        }

        public void StopTest()
        {
            this.Standard.Stop();
        }



        //###################################################################
        //  Private
        //###################################################################

    }
}
