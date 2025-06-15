using eccFramework.SharedLib.Core.Attributes;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;

namespace FTSolutions.IEC61034.Runner.Popup
{                           
    [TargetViewModel(typeof(vmPopup_SmokeVentilation))]
    public partial class Popup_SmokeVentilation : BaseIEC61034Popup
    {
        public Popup_SmokeVentilation()
        {
            InitializeComponent();
        }
        /*
        private void chk1000_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender != null)
            {
                vmPopup_CalibrationProperties vm = this.DataContext as vmPopup_CalibrationProperties;

                if (vm != null)
                {
                    if (sender == chk1000 && chk1000.IsChecked == true)
                    {
                        vm.CalibrationItem_Properties.FlameTemperatureMin = IEC61034Const.CALIBRATION_TEMPERATURE_61034_1000_MIN.ToString();
                        vm.CalibrationItem_Properties.FlameTemperatureMax = IEC61034Const.CALIBRATION_TEMPERATURE_61034_1000_MAX.ToString();
                        vm.CalibrationItem_Properties.FlameTemperatureDiff = IEC61034Const.CALIBRATION_TEMPERATURE_61034_1000_DIFF.ToString();
                        vm.CalibrationItem_Properties.UseFlameTemperature1000 = "Y";
                    }
                }
            }
        }

        private void chk1000_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender != null)
            {
                vmPopup_CalibrationProperties vm = this.DataContext as vmPopup_CalibrationProperties;

                if (vm != null)
                {
                    if (sender == chk1000 && chk1000.IsChecked == false)
                    {
                        vm.CalibrationItem_Properties.FlameTemperatureMin = IEC61034Const.CALIBRATION_TEMPERATURE_61034_MIN.ToString();
                        vm.CalibrationItem_Properties.FlameTemperatureMax = IEC61034Const.CALIBRATION_TEMPERATURE_61034_MAX.ToString();
                        vm.CalibrationItem_Properties.FlameTemperatureDiff = IEC61034Const.CALIBRATION_TEMPERATURE_61034_DIFF.ToString();
                        vm.CalibrationItem_Properties.UseFlameTemperature1000 = string.Empty;
                    }
                }
            }
        }
        */
    }
}
