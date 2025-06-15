using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class TypeConfigSettingCollection : ObservableCollection<TypeConfigSetting>
    {
        public string GetValue(string key)
        {
            return this.Where(t => t.KEY == key).FirstOrDefault().VALUE;
        }

        public double GetDoubleValue(string key)
        {
            return Convert.ToDouble(GetValue(key));
        }
    }


    public class TypeConfigSetting
    {
        public TypeConfigSetting(string key, string value, string desc)
        {
            this.KEY = key;
            this.VALUE = value;
            this.DESC = desc;
        }

        public string KEY { get; set; }

        public string VALUE { get; set; }

        public string DESC { get; set; }
    }
}
