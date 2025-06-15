using eccFramework.SharedLib.Core.Attributes;
using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Behavior;
using eccFramework.SharedLib.GlobalType.SysType;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;

namespace FTSolutions.IEC61034.Runner
{
    [TargetViewModel(typeof(vmMainUI))]
    public partial class MainUI : BaseView, IMainUI
    {
        public MainUI()
        {
            InitializeComponent();
        }



        //###################################################################
        //  IMainUI
        //###################################################################

        public MenuInfoCollection GetSystemMenuInfo()
        {
            MenuInfoCollection menuInfoCollection = new MenuInfoCollection();

            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_CONFIG_SETTING, IEC61034Const.MENU_CONFIG_SETTING_KOR, "ic_menu_system.png", "", ""));
            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_CHANNEL_SETTING, IEC61034Const.MENU_CHANNEL_SETTING_KOR, "ic_menu_system.png", "", ""));

            return menuInfoCollection;
        }

        public MenuInfoCollection GetMainMenuInfo()
        {
            MenuInfoCollection menuInfoCollection = new MenuInfoCollection();

            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_REGISTER_MANAGER, IEC61034Const.MENU_REGISTER_MANAGER_KOR, "ic_menu_registration.png", "", ""));
            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_QUALIFICATION_REGISTER_MANAGER, IEC61034Const.MENU_QUALIFICATION_REGISTER_MANAGER_KOR, "ic_menu_registration.png", "", ""));
            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_BLANK_TEST, IEC61034Const.MENU_BLANK_TEST_KOR, "ic_menu_operation.png", "", ""));
            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_LIGHT_CALIBRATION, IEC61034Const.MENU_LIGHT_CALIBRATION_KOR, "ic_menu_operation.png", "", ""));
            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_FILTER_CALIBRATION, IEC61034Const.MENU_FILTER_CALIBRATION_KOR, "ic_menu_operation.png", "", ""));
            menuInfoCollection.Add(new MenuInfo(IEC61034Const.MENU_CONTROL_PANEL, IEC61034Const.MENU_CONTROL_PANEL_KOR, "ic_menu_control_panel.png", "", ""));

            return menuInfoCollection;
        }
    }
}
