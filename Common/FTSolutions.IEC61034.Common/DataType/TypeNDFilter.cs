using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.QueryService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class TypeNDFilterCollection : BaseModel
    {
        public TypeNDFilterCollection()
        {
            this.LoadData();
        }


        private TypeNDFilter _filterNo1;
        public TypeNDFilter FilterNo1
        {
            get { return _filterNo1; }
            set
            {
                if (this._filterNo1 != value)
                {
                    this._filterNo1 = value;
                }

                this.RaisePropertyChanged(nameof(FilterNo1));
            }
        }

        private TypeNDFilter _filterNo2;
        public TypeNDFilter FilterNo2
        {
            get { return _filterNo2; }
            set
            {
                if (this._filterNo2 != value)
                {
                    this._filterNo2 = value;
                }

                this.RaisePropertyChanged(nameof(FilterNo2));
            }
        }

        private TypeNDFilter _filterNo3;
        public TypeNDFilter FilterNo3
        {
            get { return _filterNo3; }
            set
            {
                if (this._filterNo3 != value)
                {
                    this._filterNo3 = value;
                }

                this.RaisePropertyChanged(nameof(FilterNo3));
            }
        }

        private TypeNDFilter _filterNo4;
        public TypeNDFilter FilterNo4
        {
            get { return _filterNo4; }
            set
            {
                if (this._filterNo4 != value)
                {
                    this._filterNo4 = value;
                }

                this.RaisePropertyChanged(nameof(FilterNo4));
            }
        }

        private TypeNDFilter _filterNo5;
        public TypeNDFilter FilterNo5
        {
            get { return _filterNo5; }
            set
            {
                if (this._filterNo5 != value)
                {
                    this._filterNo5 = value;
                }

                this.RaisePropertyChanged(nameof(FilterNo5));
            }
        }

        private TypeNDFilter _filterNo6;
        public TypeNDFilter FilterNo6
        {
            get { return _filterNo6; }
            set
            {
                if (this._filterNo6 != value)
                {
                    this._filterNo6 = value;
                }

                this.RaisePropertyChanged(nameof(FilterNo6));
            }
        }


        public void LoadData()
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceSystem.SearchNDFilters();

            foreach (DataRow row in dtResult.Rows)
            {
                int index = Convert.ToInt16(row["FILTER_INDEX"].ToString());
                string no = row["FILTER_NO"].ToString();

                double calc = this.GetValue(row["CALC_TRANSMISSION"]).Value;
                double certi = this.GetValue(row["CERTI_TRANSMISSION"]).Value;
                double? measure = this.GetValue(row["MEASURE_TRANSMISSION"]);
                double? error = this.GetValue(row["ERROR_TRANSMISSION"]);

                switch (index)
                {
                    case 1: this.FilterNo1 = new TypeNDFilter(index, no, calc, certi, measure, error); break;
                    case 2: this.FilterNo2 = new TypeNDFilter(index, no, calc, certi, measure, error); break;
                    case 3: this.FilterNo3 = new TypeNDFilter(index, no, calc, certi, measure, error); break;
                    case 4: this.FilterNo4 = new TypeNDFilter(index, no, calc, certi, measure, error); break;
                    case 5: this.FilterNo5 = new TypeNDFilter(index, no, calc, certi, measure, error); break;
                    case 6: this.FilterNo6 = new TypeNDFilter(index, no, calc, certi, measure, error); break;
                }
            }
        }

        public List<PointF> GetMeasurePointList()
        {
            List<PointF> pointList = new List<PointF>();

            pointList.Add(new PointF(100, 100));
            if (this.FilterNo1.MEASURE_TRANSMISSION.HasValue) { pointList.Add(this.FilterNo1.GetMeasurePoint()); }
            if (this.FilterNo2.MEASURE_TRANSMISSION.HasValue) { pointList.Add(this.FilterNo2.GetMeasurePoint()); }
            if (this.FilterNo3.MEASURE_TRANSMISSION.HasValue) { pointList.Add(this.FilterNo3.GetMeasurePoint()); }
            if (this.FilterNo4.MEASURE_TRANSMISSION.HasValue) { pointList.Add(this.FilterNo4.GetMeasurePoint()); }
            if (this.FilterNo5.MEASURE_TRANSMISSION.HasValue) { pointList.Add(this.FilterNo5.GetMeasurePoint()); }
            if (this.FilterNo6.MEASURE_TRANSMISSION.HasValue) { pointList.Add(this.FilterNo6.GetMeasurePoint()); }
            pointList.Add(new PointF(0, 0));

            return pointList;
        }

        private double? GetValue(object data)
        {
            try
            {
                return Convert.ToDouble(data);
            }
            catch
            {
                return null;
            }
        }

    }


    public class TypeNDFilter : BaseModel
    {
        public TypeNDFilter(int index, string no, double calc, double certi, double? measure, double? error)
        {
            this.FILTER_INDEX = index;
            this.FILTER_NO = no;
            this.CALC_TRANSMISSION = calc;
            this.CERTI_TRANSMISSION = certi;
            this.MEASURE_TRANSMISSION = measure;
            this.ERROR_TRANSMISSION = error;
        }


        private int _filterIndex;
        public int FILTER_INDEX
        {
            get { return _filterIndex; }
            set
            {
                if (this._filterIndex != value)
                {
                    this._filterIndex = value;
                    this.RaisePropertyChanged(nameof(FILTER_INDEX));
                }
            }
        }

        private string _filterNo;
        public string FILTER_NO
        {
            get { return _filterNo; }
            set
            {
                if (this._filterNo != value)
                {
                    this._filterNo = value;
                    this.RaisePropertyChanged(nameof(FILTER_NO));
                }
            }
        }

        private double _calcTransmission;
        public double CALC_TRANSMISSION
        {
            get { return _calcTransmission; }
            set
            {
                if (this._calcTransmission != value)
                {
                    this._calcTransmission = value;
                    this.RaisePropertyChanged(nameof(CALC_TRANSMISSION));
                }
            }
        }

        private double _certiTransmission;
        public double CERTI_TRANSMISSION
        {
            get { return _certiTransmission; }
            set
            {
                if (this._certiTransmission != value)
                {
                    this._certiTransmission = value;
                    this.RaisePropertyChanged(nameof(CERTI_TRANSMISSION));
                }
            }
        }

        private double? _measureTransmission;
        public double? MEASURE_TRANSMISSION
        {
            get { return _measureTransmission; }
            set
            {
                if (this._measureTransmission != value)
                {
                    this._measureTransmission = value;
                    this.RaisePropertyChanged(nameof(MEASURE_TRANSMISSION));
                }
            }
        }

        private double? _errorTransmission;
        public double? ERROR_TRANSMISSION
        {
            get { return _errorTransmission; }
            set
            {
                if (this._errorTransmission != value)
                {
                    this._errorTransmission = value;
                    this.RaisePropertyChanged(nameof(ERROR_TRANSMISSION));
                }
            }
        }


        public void CalcErrors()
        {
            try
            {
                this.ERROR_TRANSMISSION = Math.Round(100 - (this.MEASURE_TRANSMISSION.Value / this.CERTI_TRANSMISSION) * 100, 3);
            }
            catch
            {
                this.ERROR_TRANSMISSION = null;
            }
        }

        public PointF GetCertifiedPoint()
        {
            float x = float.Parse(this.CERTI_TRANSMISSION.ToString());
            float y = float.Parse(this.CERTI_TRANSMISSION.ToString());

            return new PointF(x, y);
        }

        public PointF GetMeasurePoint()
        {
            float x = float.Parse(this.CERTI_TRANSMISSION.ToString());
            float y = float.Parse(this.MEASURE_TRANSMISSION.ToString());

            return new PointF(x, y);
        }

    }
}
