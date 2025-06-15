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

        private int _flameoutTime;
        public int FlameoutTime
        {
            get { return _flameoutTime; }
            set
            {
                if (this._flameoutTime != value)
                {
                    this._flameoutTime = value;
                    this.RaisePropertyChanged(nameof(FlameoutTime));
                }
            }
        }

        private string _currentStopTime;
        public string CurrentStopTime
        {
            get { return _currentStopTime; }
            set
            {
                if (this._currentStopTime != value)
                {
                    this._currentStopTime = value;

                    if(value.Equals("10 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 10;
                    }
                    else if (value.Equals("20 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 20;
                    }
                    else if (value.Equals("30 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 30;
                    }
                    else if (value.Equals("40 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 40;
                    }
                    else if (value.Equals("50 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 50;
                    }
                    else if (value.Equals("60 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 60;
                    }
                    else if (value.Equals("☞"))
                    {
                        this.Standard.MaxTestDurationMinute = 40;
                    }
                    else if (value.Equals("1 min"))
                    {
                        this.Standard.MaxTestDurationMinute = 1;
                    }

                    this.RaisePropertyChanged(nameof(CurrentStopTime));
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

                this.FlameoutTime = sourceValue.FlameoutTime;
                this.CurrentStopTime = sourceValue.CurrentStopTime;
            }
        }



        //###################################################################
        //  Public
        //###################################################################

        public void InitializeData(StandardType type)
        {
            this.FlameoutTime = 0;            

            switch (type)
            {
                case StandardType.IEC61034: this.Standard = new Standard.IEC61034("TEST"); break;
            }

            this.CurrentStopTime = IEC61034Const.DEFAULT_AUTO_STOP_TIME;

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
