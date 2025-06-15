using System.Windows;

namespace FTSolutions.IEC61034.Common.DataType
{
    public class IEC61034Const
    {
        public static readonly string INI_CONFIG_SECTION = "Config";

        public const string SYSTEM_DATABASE_NAME = "ftsolutions_system.db";

        public const string MENU_CALIBRATION_PROPERTY_KEY = "Popup.Popup_CalibrationProperties";

        public const string MENU_CONTROL_PANEL = "Control Panel";
        public const string MENU_CONTROL_PANEL_KOR = "제어판";
        public const string MENU_CONTROL_PANEL_KEY = "Popup.Popup_ControlPanel";

        public const string MENU_REGISTER_MANAGER = "Test - Registration";
        public const string MENU_REGISTER_MANAGER_KOR = "시험 - 접수";
        public const string MENU_REGISTER_MANAGER_KEY = "Popup.Popup_Registration";

        public const string MENU_QUALIFICATION_REGISTER_MANAGER = "Qualification fire sources";
        public const string MENU_QUALIFICATION_REGISTER_MANAGER_KOR = "화원 검증";
        public const string MENU_QUALIFICATION_REGISTER_MANAGER_KEY = "Popup.Popup_QualificationRegistration";

        public const string MENU_CONFIG_SETTING = "System - Config Setting";
        public const string MENU_CONFIG_SETTING_KOR = "시스템 - 설정 정보";
        public const string MENU_CONFIG_SETTING_KEY = "Popup.Popup_ConfigSetting";

        public const string MENU_CHANNEL_SETTING = "System - Channel Setting";
        public const string MENU_CHANNEL_SETTING_KOR = "시스템 - 채널 정보";
        public const string MENU_CHANNEL_SETTING_KEY = "Popup.Popup_ChannelSetting";

        public const string MENU_TEST_PROPERTY_KEY = "Popup.Popup_TestProperties";
        public const string MENU_QUALIFICATION_PROPERTY_KEY = "Popup.Popup_QualificationProperties";
        public const string MENU_TEST_KEY = "Popup.Popup_Test";
        public const string MENU_QUALIFICATION_KEY = "Popup.Popup_Qualification";
        public const string MENU_TEST_SUMMARY_KEY = "Popup.Popup_TestSummary";
        public const string MENU_QUALIFICATION_SUMMARY_KEY = "Popup.Popup_QualificationSummary";

        public const string MENU_BLANK_TEST = "Calibration - Blank Test";
        public const string MENU_BLANK_TEST_KOR = "작동교정 - 공시험";
        public const string MENU_BLANK_TEST_KEY = "Popup.Popup_BlankTest";

        public const string MENU_LIGHT_CALIBRATION = "Calibration - Light";
        public const string MENU_LIGHT_CALIBRATION_KOR = "작동교정 - 광원";
        public const string MENU_LIGHT_CALIBRATION_KEY = "Popup.Popup_Light";

        public const string MENU_FILTER_CALIBRATION = "Calibration - ND Filter";
        public const string MENU_FILTER_CALIBRATION_KOR = "작동교정 - ND 필터";
        public const string MENU_FILTER_CALIBRATION_KEY = "Popup.Popup_NDFilter";

        public const string MENU_SMOKE_VENTILATION = "Smoke Ventilation";
        public const string MENU_SMOKE_VENTILATION_KOR = "연기 배출";
        public const string MENU_SMOKE_VENTILATION_KEY = "Popup.Popup_SmokeVentilation";

        public static string KEY_LANGUAGE = "LANGUAGE";
        public static string KEY_LABORATORY = "LABORATORY";
        public static string KEY_LAB_ADDRESS = "LAB_ADDRESS";
        public static string KEY_REPORT_PATH = "REPORT_PATH";
        public static string KEY_OPERATOR = "OPERATOR";
        public static string KEY_MANAGER = "MANAGER";
        public static string KEY_EMPTY_LOGO = "EMPTY_LOGO";
        public static string KEY_LOGO_NAME = "LOGO_NAME";

        public const string KEY_IEC61034 = "IEC61034_TITLE";
        public const string KEY_FAN_SPEED = "IEC61034_FAN_SPEED";
        public const string KEY_TEMPERATURE_MIN = "IEC61034_TEMPERATURE_MIN";
        public const string KEY_TEMPERATURE_MAX = "IEC61034_TEMPERATURE_MAX";
        public const string KEY_MAX_TEST_DURATION = "IEC61034_MAX_TEST_DURATION";
        public const string KEY_CHAMBER_VOLUME = "IEC61034_CHAMBER_VOLUME";
        public const string KEY_LIGHT_PATH = "IEC61034_LIGHT_PATH";
        public const string KEY_TOLUENE_4_ABSORBANCE_MIN = "IEC61034_TOLUENE_4_ABSORBANCE_MIN";
        public const string KEY_TOLUENE_4_ABSORBANCE_MAX = "IEC61034_TOLUENE_4_ABSORBANCE_MAX";
        public const string KEY_TOLUENE_10_ABSORBANCE_MIN = "IEC61034_TOLUENE_10_ABSORBANCE_MIN";
        public const string KEY_TOLUENE_10_ABSORBANCE_MAX = "IEC61034_TOLUENE_10_ABSORBANCE_MAX";

        public const string DEFAULT_STANDARD_TYPE = "IEC61034";        

        public static string DEFAULT_LOGO_FILE = "pack://application:,,,/FTSolutions.IEC61034.Runner;component/Images/logo.png";

        public const string SIGN_KEY_QUALIFICATION = "Qualification";
        public const string SIGN_KEY_TEST = "TEST";
        public const string SIGN_KEY_REPORT = "REPORT";

        public const int SAMPLING_RATE = 1000;
        public const int SAMPLE_PER_CHANNEL = 1000;
        public const int BUFFER_SIZE = 10000;

        public const int TIMER_INTERVAL = 500;
        public const int MEASURING_INTERVAL = 1000;

        public const int DEVICE_ANALOG_INPUT_INTERVAL = 1000;
        public const int DEVICE_DIGITAL_INPUT_INTERVAL = 1000;

        public const double OPACITY_LAYOUT_ACTIVE = 1;
        public const double OPACITY_LAYOUT_DEACTIVE = 0.15;
        public const double OPACITY_POPUP_DEACTIVE = 0.7;

        public const int CAPACITY_DUMMY = 2;
        public const int CAPACITY_60 = 61;
        public const int CAPACITY_300 = 301;
        public const int CAPACITY_600 = 601;

        public const int START_REG_YEAR = 2024;

        public const int UPPER_REMOVE_PERCENTAGE = 10;
        public const int LOWER_REMOVE_PERCENTAGE = 10;

        public const int DEFAULT_MAX_TEST_DURATION = 40;

        public const int TEMPERATURE_STABILIZATION_DATA_COUNT = 30;         // 1분간 챔버 온도가 안정화 되었는지 검사
        public const int TRANSMISSION_STABILIZATION_DATA_COUNT = 1 + 5;         // 1분간 투과율이 안정화 되었는지 검사

        public const int MIN_CHAMBER_TEMPERATURE = 20;
        public const int MAX_CHAMBER_TEMPERATURE = 30;

        public const double DEFAULT_PHOTODIODE_MAX_VALUE = 100;
        public const double DEFAULT_PHOTODIODE_MAX_VOLTAGE = 5;

        public const double TRANSMISSION_STABILITY_TOLERANCE = 0.05;    // 안정화 허용 오차 (예: 투과율 변화가 5% 이내일 때 안정화로 판단)
        public const int FLAMEOUT_STABILIZATION_DURATION = 1;           // 불꽃이 꺼지고 나서 5분동안에 빛의 투과 감소 체크

        public const double DEFAULT_TOLUENE_CONTENT = 4;    // 4 or 10 %

        public const int DEFAULT_CHAMBER_VOLUME = 27; // 입방체의 용적(m³) - 3000±30
        public const int DEFAULT_LIGHT_PATH = 3; // 단위(m)

        public const double DEFAULT_TOLUENE4_MIN_VALUE = 0.18;
        public const double DEFAULT_TOLUENE4_MAX_VALUE = 0.26;

        public const double DEFAULT_TOLUENE10_MIN_VALUE = 0.80;
        public const double DEFAULT_TOLUENE10_MAX_VALUE = 1.20;

        public const int EXHAUST_FLOW_TARGET = 25;
        public const int EXHAUST_FLOW_VALID_RANGE = 5;
        public const int EXHAUST_FLOW_DRIFT_VALID_RANGE = 3;

        public const string KEY_TOLUENE_4 = "4%";
        public const string KEY_TOLUENE_10 = "10%";

        public const string QUALIFICATION_STANDARD_TYPE = "Qualification fire Sources";

        public const string DEFAULT_AUTO_STOP_TIME = "40 min";
        public const string DEFAULT_MANUAL_STOP_TIME = "60 min";

    }
}
