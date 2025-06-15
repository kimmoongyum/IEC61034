using eccFramework.SharedLib.GlobalType.Core;
using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSolutions.IEC61034.Common.Setting
{
    public class ZeroSpanMethod : BaseNotifyProperty
    {
        private double _gradient;
        public double Gradient
        {
            get { return _gradient; }
            set
            {
                if (this._gradient != value)
                {
                    this._gradient = value;
                    this.RaisePropertyChanged(nameof(Gradient));
                }
            }
        }

        private double _intercept;
        public double Intercept
        {
            get { return _intercept; }
            set
            {
                if (this._intercept != value)
                {
                    this._intercept = value;
                    this.RaisePropertyChanged(nameof(Intercept));
                }
            }
        }

        public ZeroSpanMethod()
        {

        }

        public void SetZero(double voltage, double gradient, double intercept)
        {
            double gradientValue = 0;
            double interceptValue = 0;

            double gradientVoltage = 0;
            double interceptVoltage = 0;

            double zeroValue = 0.0;
            double zeroVoltage = voltage;

            double spanValue = IEC61034Const.DEFAULT_PHOTODIODE_MAX_VALUE;
            double spanVoltage = IEC61034Const.DEFAULT_PHOTODIODE_MAX_VOLTAGE;

            gradientVoltage = (spanValue - zeroValue) / (spanVoltage - zeroVoltage);
            interceptVoltage = zeroValue - gradientVoltage * zeroVoltage;

            //gradientVoltage = Math.Round(gradientVoltage, 2, MidpointRounding.AwayFromZero);
            //interceptVoltage = Math.Round(interceptVoltage, 2, MidpointRounding.AwayFromZero);

            gradientValue = 1.0 / gradientVoltage;
            interceptValue = -(interceptVoltage / gradientVoltage);

            //gradientValue = Math.Round(gradientValue, 2, MidpointRounding.AwayFromZero);
            //interceptValue = Math.Round(interceptValue, 2, MidpointRounding.AwayFromZero);

            this.Gradient = gradientVoltage;
            this.Intercept = interceptVoltage;

            //SettingManager.WriteChannelMinVoltageInfoInDatabase("AI_LIGHT_PHOTODIODE", string.Format("{0}", zeroVoltage), string.Format("{0}", gradientVoltage), string.Format("{0}", interceptVoltage), string.Format("{0}", gradientValue), string.Format("{0}", interceptValue));
            //SettingManager.ReadChannelDatabase();
        }

        public void SetSpan(double voltage, double gradient, double intercept)
        {
            double gradientValue = 0;
            double interceptValue = 0;

            double gradientVoltage = 0;
            double interceptVoltage = 0;

            double zeroValue = DbChannel.AI_LIGHT_PHOTODIODE.MinValue;
            double zeroVoltage = DbChannel.AI_LIGHT_PHOTODIODE.MinVoltage;

            double spanValue = DbChannel.AI_LIGHT_PHOTODIODE.MaxValue;
            double spanVoltage = voltage;

            gradientVoltage = (spanValue - zeroValue) / (spanVoltage - zeroVoltage);
            interceptVoltage = zeroValue - gradientVoltage * zeroVoltage;

            //gradientVoltage = Math.Round(gradientVoltage, 2, MidpointRounding.AwayFromZero);
            //interceptVoltage = Math.Round(interceptVoltage, 2, MidpointRounding.AwayFromZero);

            gradientValue = 1.0 / gradientVoltage;
            interceptValue = -(interceptVoltage / gradientVoltage);

            //gradientValue = Math.Round(gradientValue, 2, MidpointRounding.AwayFromZero);
            //interceptValue = Math.Round(interceptValue, 2, MidpointRounding.AwayFromZero);

            this.Gradient = gradientVoltage;
            this.Intercept = interceptVoltage;
            //SettingManager.WriteChannelMaxVoltageInfoInDatabase("AI_LIGHT_PHOTODIODE", string.Format("{0}", spanVoltage), string.Format("{0}", gradientVoltage), string.Format("{0}", interceptVoltage), string.Format("{0}", gradientValue), string.Format("{0}", interceptValue));
            //SettingManager.ReadChannelDatabase();
        }
    }
}
