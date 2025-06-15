using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.GlobalType.Core;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.DataType.SessionType;
using FTSolutions.IEC61034.Common.Result;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace FTSolutions.IEC61034.Common
{
    public class SessionManager : BaseSessionManager
    {
        #region Singleton
        private static SessionManager _instance;
        private static object m_lock = new object();


        public static SessionManager Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (m_lock)
                    {
                        _instance = new SessionManager();
                    }
                }

                return _instance;
            }
        }
        #endregion


        public SessionManager()
        {
            this.IsRunning = false;
            this.TargetExhaustFlowCMM = 10;

            this.IEC61034_DataSetTest = new DataSetTest();
            this.IEC61034_DataSetQualification = new DataSetQualification();

            this.LightPolynomialInfo = new TypePolynomial();

            this.NDFilterCollection = new TypeNDFilterCollection();
        }



        //###################################################################
        //  Property
        //###################################################################

        private DataSetTest _IEC61034_DataSetTest;
        public DataSetTest IEC61034_DataSetTest
        {
            get { return _IEC61034_DataSetTest; }
            set
            {
                _IEC61034_DataSetTest = value;
                this.RaisePropertyChanged(nameof(IEC61034_DataSetTest));
            }
        }
        
        private DataSetQualification _IEC61034_DataSetQualification;
        public DataSetQualification IEC61034_DataSetQualification
        {
            get { return _IEC61034_DataSetQualification; }
            set
            {
                _IEC61034_DataSetQualification = value;
                this.RaisePropertyChanged(nameof(IEC61034_DataSetQualification));
            }
        }

        private TypeConfigSettingCollection _configSetting;
        public TypeConfigSettingCollection ConfigSetting
        {
            get { return _configSetting; }
            private set
            {
                _configSetting = value;
                this.RaisePropertyChanged(nameof(ConfigSetting));
            }
        }

        private TypeChannelSettingCollection _channelSetting;
        public TypeChannelSettingCollection ChannelSetting
        {
            get { return _channelSetting; }
            private set
            {
                _channelSetting = value;
                this.RaisePropertyChanged(nameof(ChannelSetting));
            }
        }

        private bool _isLightOn;
        public bool IsLightOn
        {
            get { return _isLightOn; }
            set
            {
                if (this._isLightOn != value)
                {
                    this._isLightOn = value;
                }

                if(value)
                {
                    Console.WriteLine("Light on");
                }
                else
                {
                    Console.WriteLine("Light off");
                }

                this.RaisePropertyChanged(nameof(IsLightOn));
                this.RaisePropertyChanged(nameof(IsLightOff));
            }
        }
        public bool IsLightOff { get { return !this.IsLightOn; } }

        private bool _isIgnitorOn;
        public bool IsIgnitorOn
        {
            get { return _isIgnitorOn; }
            set
            {
                if (this._isIgnitorOn != value)
                {
                    this._isIgnitorOn = value;
                }

                this.RaisePropertyChanged(nameof(IsIgnitorOn));
                this.RaisePropertyChanged(nameof(IsIgnitorOff));
            }
        }
        public bool IsIgnitorOff { get { return !this.IsIgnitorOn; } }

        private bool _isFanOn;
        public bool IsFanOn
        {
            get { return _isFanOn; }
            set
            {
                if (this._isFanOn != value)
                {
                    this._isFanOn = value;
                }

                if(value)
                {
                    DeviceManager.Current.AnalogOutput.WriteInverter(this.TargetExhaustFlowCMM);
                }

                this.RaisePropertyChanged(nameof(IsFanOn));
                this.RaisePropertyChanged(nameof(IsFanOff));
            }
        }
        public bool IsFanOff { get { return !this.IsFanOn; } }

       
        private bool _isLampOn;
        public bool IsLampOn
        {
            get { return _isLampOn; }
            set
            {
                if (this._isLampOn != value)
                {
                    this._isLampOn = value;
                }

                this.RaisePropertyChanged(nameof(IsLampOn));
                this.RaisePropertyChanged(nameof(IsLampOff));
            }
        }
        public bool IsLampOff { get { return !this.IsLampOn; } }

        private int _targetExhaustFlowCMM;
        public int TargetExhaustFlowCMM
        {
            get { return _targetExhaustFlowCMM; }
            set
            {
                if (this._targetExhaustFlowCMM != value)
                {
                    this._targetExhaustFlowCMM = value;
                }

                this.RaisePropertyChanged(nameof(TargetExhaustFlowCMM));
            }
        }

        private bool _isPowerOn;
        public bool IsPowerOn
        {
            get { return _isPowerOn; }
            set
            {
                if (this._isPowerOn != value)
                {
                    this._isPowerOn = value;
                }

                this.RaisePropertyChanged(nameof(IsPowerOn));
                this.RaisePropertyChanged(nameof(IsPowerOff));
            }
        }
        public bool IsPowerOff { get { return !this.IsPowerOn; } }

        private TypeNDFilterCollection _nDFilterCollection;
        public TypeNDFilterCollection NDFilterCollection 
        {
            get { return _nDFilterCollection; }
            set
            {
                _nDFilterCollection = value;
                this.RaisePropertyChanged(nameof(TypeNDFilterCollection));
            }
        }

        private TypePolynomial _lightPolynomialInfo;
        public TypePolynomial LightPolynomialInfo
        {
            get { return _lightPolynomialInfo; }
            set
            {
                if (this._lightPolynomialInfo != value)
                {
                    this._lightPolynomialInfo = value;
                    this.RaisePropertyChanged(nameof(LightPolynomialInfo));
                }
            }
        }
        //###################################################################
        //  Public
        //###################################################################

        public void Initialize()
        {
            this.NDFilterCollection.LoadData();  // safe call after full SessionManager setup
        }

        //public string ConnectionStringSystemDB()
        //{
        //    string dbFullName = Environment.CurrentDirectory + IniConfig.DatabasePath + IEC61034Const.SYSTEM_DATABASE_NAME;
        //    return String.Format("Data Source={0};Version=3;", dbFullName);
        //}

        public void BindingConfigSetting(TypeConfigSettingCollection dataSource)
        {
            this.ConfigSetting?.Clear();
            this.ConfigSetting = dataSource;
        }

        public void BindingChannelSetting(TypeChannelSettingCollection dataSource)
        {
            this.ChannelSetting?.Clear();
            this.ChannelSetting = dataSource;
        }

        public void BindingLogoImage()
        {
            string isEmptyLogo = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_EMPTY_LOGO);
            string logoName = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_LOGO_NAME);

            if (SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_EMPTY_LOGO).Equals("Y"))
            {
                SessionManager.Current.LogoBackground = Brushes.Transparent;
                SessionManager.Current.ClientLogoPath = "";
            }
            else
            {
                SessionManager.Current.LogoBackground = Brushes.White;

                try
                {
                    if (logoName != null && logoName.Trim().Length > 0)
                    {
                        FileInfo file = new FileInfo(logoName);

                        if (file.Exists)
                        {
                            SessionManager.Current.ClientLogoPath = logoName;
                            return;
                        }
                    }

                    SessionManager.Current.ClientLogoPath = IEC61034Const.DEFAULT_LOGO_FILE;
                }
                catch
                {
                    SessionManager.Current.LogoBackground = Brushes.Transparent;
                    SessionManager.Current.ClientLogoPath = "";
                }
            }
        }

        public void SetLightPolynomialInfo(TypePolynomial polynomial)
        {
            this.LightPolynomialInfo = polynomial;
        }

        //###################################################################
        //  Private
        //###################################################################


    }
}
