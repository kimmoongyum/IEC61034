namespace FTSolutions.IEC61034.Common.DataType
{
    public enum StandardType
    {
        NA,
        IEC61034,
        CUSTOM
    }

    public enum MenuKind
    {
        NA,
        MAIN,
        CALIBRATION_PROPERTY,
        TEST_PROPERTY,        
        CONTROL_PANEL,
        REGISTRATION,
        BLANK_TEST,
        CALIB_LIGHT,
        CALIB_FILTER,
        TEST,
        TEST_SUMMARY,
        QUALIFICATION_REGISTRATION,
        QUALIFICATION_BLANK_TEST,
        QUALIFICATION_CALIB_LIGHT,
        QUALIFICATION_CALIB_FILTER,
        QUALIFICATION,
        QUALIFICATION_SUMMARY
    }

    public enum CableKind
    {
        NA,
        ROUND,
        FLAT
    }

    public enum NDFilterKey
    {
        ND_POLYNOMIAL,
        ND_1ST_TERM,
        ND_2ND_TERM,
        ND_3RD_TERM,
        ND_4TH_TERM,
        ND_INTERCEPT
    }

}
