using eccFramework.SharedLib.Core.Base;

namespace FTSolutions.IEC61034.Common.Result
{
    public class CurrentMeasurement : BaseTestItem
    {
        public CurrentMeasurement()
        {
            this.Clear();
        }



        //###################################################################
        //  Property
        //###################################################################

        private double _absorbance;
        public double Absorbance
        {
            get { return _absorbance; }
            set
            {
                if (this._absorbance != value)
                {
                    this._absorbance = value;
                    this.RaisePropertyChanged(nameof(Absorbance));
                }
            }
        }

        private double _transmission;
        public double Transmission
        {
            get { return _transmission; }
            set
            {
                if (this._transmission != value)
                {
                    this._transmission = value;
                    this.RaisePropertyChanged(nameof(Transmission));
                }
            }
        }

        private double _chamberTemperature;
        public double ChamberTemperature
        {
            get { return _chamberTemperature; }
            set
            {
                if (this._chamberTemperature != value)
                {
                    this._chamberTemperature = value;
                    this.RaisePropertyChanged(nameof(ChamberTemperature));
                }
            }
        }

        private double _fanFlowrate;
        public double FanFlowrate
        {
            get { return _fanFlowrate; }
            set
            {
                if (this._fanFlowrate != value)
                {
                    this._fanFlowrate = value;
                    this.RaisePropertyChanged(nameof(FanFlowrate));
                }
            }
        }



        //###################################################################
        //  Override
        //###################################################################

        public override void Clear()
        {
            this.Absorbance = 0;
            this.Transmission = 0;
            this.ChamberTemperature = 0;
            this.FanFlowrate = 0;
        }

        public override bool IsValid()
        {
            return true;
        }



        //###################################################################
        //  Public
        //###################################################################

    }
}
