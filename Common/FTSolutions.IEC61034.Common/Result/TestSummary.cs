using eccFramework.SharedLib.Core.Base;
using System;
using System.Windows.Media;

namespace FTSolutions.IEC61034.Common.Result
{
    public class TestSummary : BaseTestItem
    {
        public TestSummary()
        {

        }



        //###################################################################
        //  Property
        //###################################################################     

        private string _flameoutTime;
        public string FlameoutTime
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

        private double _startChamberTemperature;
        public double StartChamberTemperature
        {
            get { return _startChamberTemperature; }
            set
            {
                if (this._startChamberTemperature != value)
                {
                    this._startChamberTemperature = value;
                    this.RaisePropertyChanged(nameof(StartChamberTemperature));
                }
            }
        }

        private double _maxAbsorbance;
        public double MaxAbsorbance
        {
            get { return _maxAbsorbance; }
            set
            {
                if (this._maxAbsorbance != value)
                {
                    this._maxAbsorbance = value;
                    this.RaisePropertyChanged(nameof(MaxAbsorbance));
                }
            }
        }

        private double _minTransmission;
        public double MinTransmission
        {
            get { return _minTransmission; }
            set
            {
                if (this._minTransmission != value)
                {
                    this._minTransmission = value;
                    this.RaisePropertyChanged(nameof(MinTransmission));
                }
            }
        }

        private string _minTransmissionSecond;
        public string MinTransmissionSecond
        {
            get { return _minTransmissionSecond; }
            set
            {
                if (this._minTransmissionSecond != value)
                {
                    this._minTransmissionSecond = value;
                    this.RaisePropertyChanged(nameof(MinTransmissionSecond));
                }
            }
        }

        private SolidColorBrush _evaluationColor;
        public SolidColorBrush EvaluationColor
        {
            get { return _evaluationColor; }
            set
            {
                if (this._evaluationColor != value)
                {
                    this._evaluationColor = value;
                    this.RaisePropertyChanged(nameof(EvaluationColor));
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (this._description != value)
                {
                    this._description = value;
                    this.RaisePropertyChanged(nameof(Description));
                }
            }
        }



        //###################################################################
        //  Override
        //###################################################################

        public override void Clear()
        {
            this.Description = String.Empty;
            this.FlameoutTime = String.Empty;
            
            this.MaxAbsorbance = 0;
            this.MinTransmission = 0;
            this.StartChamberTemperature = 0;

            TimeSpan totalSeconds = TimeSpan.FromSeconds(0);
            this.MinTransmissionSecond = totalSeconds.ToString(@"hh\:mm\:ss");    
        }

        public override bool IsValid()
        {
            return true;
        }

        public override void CopyValueFrom(BaseTestItem source)
        {
            TestSummary sourceValue = source as TestSummary;

            if (sourceValue != null)
            {
                this.Description = sourceValue.Description;
                this.FlameoutTime = sourceValue.FlameoutTime;

                this.MaxAbsorbance = sourceValue.MaxAbsorbance;
                this.MinTransmission = sourceValue.MinTransmission;
                this.MinTransmissionSecond = sourceValue.MinTransmissionSecond;

                this.StartChamberTemperature = sourceValue.StartChamberTemperature;      
            }
        }
    }
}
