using eccFramework.SharedLib.Core.Base;
using System;

namespace FTSolutions.IEC61034.Common.DataType.SessionType
{
    public class TypePolynomial : BaseModel
    {
        public TypePolynomial()
        {
            this.Coefficient4 = 0;
            this.Coefficient3 = 0;
            this.Coefficient2 = 0;
            this.Coefficient1 = 1;

            this.YIntercept = 0;
        }


        private int _polynomial;
        public int Polynomial
        {
            get { return _polynomial; }
            set
            {
                if (this._polynomial != value)
                {
                    this._polynomial = value;
                    this.RaisePropertyChanged(nameof(Polynomial));
                }
            }
        }

        private double _coefficient4;
        public double Coefficient4
        {
            get { return _coefficient4; }
            set
            {
                if (this._coefficient4 != value)
                {
                    this._coefficient4 = value;
                    this.RaisePropertyChanged(nameof(Coefficient4));
                }
            }
        }

        private double _coefficient3;
        public double Coefficient3
        {
            get { return _coefficient3; }
            set
            {
                if (this._coefficient3 != value)
                {
                    this._coefficient3 = value;
                    this.RaisePropertyChanged(nameof(Coefficient3));
                }
            }
        }

        private double _coefficient2;
        public double Coefficient2
        {
            get { return _coefficient2; }
            set
            {
                if (this._coefficient2 != value)
                {
                    this._coefficient2 = value;
                    this.RaisePropertyChanged(nameof(Coefficient2));
                }
            }
        }

        private double _coefficient1;
        public double Coefficient1
        {
            get { return _coefficient1; }
            set
            {
                if (this._coefficient1 != value)
                {
                    this._coefficient1 = value;
                    this.RaisePropertyChanged(nameof(Coefficient1));
                }
            }
        }

        private double _yIntercept;
        public double YIntercept
        {
            get { return _yIntercept; }
            set
            {
                if (this._yIntercept != value)
                {
                    this._yIntercept = value;
                    this.RaisePropertyChanged(nameof(YIntercept));
                }
            }
        }


        //public void SetData(TypeCalibrationValueCollection infoCollection)
        //{
        //    this.Polynomial = infoCollection.GetIntegerValue(NDFilterKey.ND_POLYNOMIAL.ToString());
        //    this.Coefficient1 = infoCollection.GetDoubleValue(NDFilterKey.ND_1ST_TERM.ToString());
        //    this.Coefficient2 = infoCollection.GetDoubleValue(NDFilterKey.ND_2ND_TERM.ToString());
        //    this.Coefficient3 = infoCollection.GetDoubleValue(NDFilterKey.ND_3RD_TERM.ToString());
        //    this.Coefficient4 = infoCollection.GetDoubleValue(NDFilterKey.ND_4TH_TERM.ToString());
        //    this.YIntercept = infoCollection.GetDoubleValue(NDFilterKey.ND_INTERCEPT.ToString());
        //}

        public double CorrectedTransmission(double transmission)
        {
            return Math.Round(this.Coefficient4 * Math.Pow(transmission, 4) + this.Coefficient3 * Math.Pow(transmission, 3) +
                this.Coefficient2 * Math.Pow(transmission, 2) + this.Coefficient1 * transmission + this.YIntercept, 2, MidpointRounding.AwayFromZero);
        }
    }
}
