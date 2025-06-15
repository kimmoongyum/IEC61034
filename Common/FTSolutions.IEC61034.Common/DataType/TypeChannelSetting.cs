using eccFramework.SharedLib.GlobalType.Protocol;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class TypeChannelSettingCollection : ObservableCollection<ChannelInfo>
    {
        public void LoadData(DataTable dtData)
        {
            this.Clear();

            foreach (DataRow row in dtData.Rows)
            {
                string channel = row["CHANNEL"].ToString();
                string address = row["ADDRESS"].ToString();
                string desc = row["DESC"].ToString();

                double minVoltage = Convert.ToDouble(row["MIN_VOLTAGE"]);
                double maxVoltage = Convert.ToDouble(row["MAX_VOLTAGE"]);
                double gradientVoltage = Convert.ToDouble(row["GRADIENT_VOLTAGE"]);
                double interceptVoltage = Convert.ToDouble(row["INTERCEPT_VOLTAGE"]);
                double minValue = Convert.ToDouble(row["MIN_VALUE"]);
                double maxValue = Convert.ToDouble(row["MAX_VALUE"]);
                double gradientValue = Convert.ToDouble(row["GRADIENT_VALUE"]);
                double interceptValue = Convert.ToDouble(row["INTERCEPT_VALUE"]);

                this.Add(new ChannelInfo(channel, address, desc, minVoltage, maxVoltage, gradientVoltage, interceptVoltage,
                    minValue, maxValue, gradientValue, interceptValue));
            }
        }


        public TypeChannelSettingCollection Copy()
        {
            TypeChannelSettingCollection copy = new TypeChannelSettingCollection();

            foreach (var item in this)
            {
                copy.Add(new ChannelInfo(item.Channel, item.Address, item.Description, item.MinVoltage, item.MaxVoltage, item.GradientVoltage, item.InterceptVoltage,
                    item.MinValue, item.MaxValue, item.GradientValue, item.InterceptValue));
            }

            return copy;
        }

        public bool AddNew(string channelID)
        {
            if (this.Where(t => t.Channel == channelID).Count() > 0)
            {
                return false;
            }

            this.Add(new ChannelInfo(channelID, "", "", 0, 0, 0, 0, 0, 0, 1, 0));

            return true;
        }
    }
}
