using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Standard;
using System;

namespace FTSolutions.IEC61034.Common.Result
{
    public class Qualification : BaseTestItem
    {
        public event Action StopAction;


        public Qualification()
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
            Qualification sourceValue = source as Qualification;

            if (sourceValue != null)
            {
                this.Standard = sourceValue.Standard;
            }
        }



        //###################################################################
        //  Public
        //###################################################################

        public void InitializeData()
        {
            this.Standard = new Standard.IEC61034("QUALIFICATION");

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

        public void StartQualification()
        {
            this.Standard.Start();
        }

        public void StopQualification()
        {
            this.Standard.Stop();
        }



        //###################################################################
        //  Private
        //###################################################################

    }
}
