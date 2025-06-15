using eccFramework.SharedLib.GlobalType.Core;
using eccFramework.SharedLib.GlobalType.SysType;
using eccFramework.SharedLib.Utility.Services;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Device;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Threading;
using System.Windows.Media;

namespace FTSolutions.IEC61034.Common
{
    public enum DOCommandType
    {
        DO_LIGHT_ON,
        DO_LIGHT_OFF,
        DO_IGNITOR_ON,
        DO_IGNITOR_OFF,
        DO_FAN_ON,
        DO_FAN_OFF,        
        OPEN_OUT_DAMPER,
        CLOSE_OUT_DAMPER,
        OPEN_IN_DAMPER,
        CLOSE_IN_DAMPER,
        DO_LAMP_ON,
        DO_LAMP_OFF,
        DO_CLEAR_ON,
        DO_CLEAR_OFF,
        DO_DARK_ON,
        DO_DARK_OFF,
        DO_NDFILTER01_ON,
        DO_NDFILTER01_OFF,
        DO_NDFILTER02_ON,
        DO_NDFILTER02_OFF,
        DO_NDFILTER03_ON,
        DO_NDFILTER03_OFF,
        DO_NDFILTER05_ON,
        DO_NDFILTER05_OFF,
        DO_NDFILTER07_ON,
        DO_NDFILTER07_OFF,
        DO_NDFILTER10_ON,
        DO_NDFILTER10_OFF,
    }

    public class DeviceManager : BaseNotifyProperty
    {
        #region Singleton.
        private static DeviceManager _instance;
        private static object m_lock = new object();

        public static DeviceManager Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (m_lock)
                    {
                        _instance = new DeviceManager();
                    }
                }

                return _instance;
            }
        }
        #endregion


        public event Action<string, string> ShowMessageHandler;

        public DeviceManager()
        {
            this.AnalogInput = new DeviceAnalogInput();
            this.AnalogOutput = new DeviceAnalogOutput();
            this.DigitalInput = new DeviceDigitalInput(IEC61034Const.DEVICE_DIGITAL_INPUT_INTERVAL);
            this.DigitalOutput = new DeviceDigitalOutput();

            this.AnalogInput.CheckDAQService += (status, err) => { StatusAnalog = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
            this.AnalogOutput.CheckDAQService += (status, err) => { StatusAnalog = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
            this.DigitalInput.CheckDAQService += (status, err) => { StatusDigital = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
            this.DigitalOutput.CheckDAQService += (status, err) => { StatusDigital = status ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH; };
        }




        //###################################################################
        //  Property
        //###################################################################

        public DeviceAnalogInput AnalogInput { get; set; }

        public DeviceAnalogOutput AnalogOutput { get; set; }

        public DeviceDigitalInput DigitalInput { get; set; }

        public DeviceDigitalOutput DigitalOutput { get; set; }


        public SolidColorBrush StatusAnalog { get; set; }

        public SolidColorBrush StatusDigital { get; set; }



        //###################################################################
        //  EventHandler
        //###################################################################



        //###################################################################
        //  Public
        //###################################################################

        public void Run()
        {
            try
            {
                this.AnalogInput.Run();
                this.DigitalInput.Run();
            }
            catch (Exception ex)
            {
                if (this.ShowMessageHandler != null)
                {
                    this.ShowMessageHandler("Device Checking", ex.Message);
                }
            }
        }

        public void Stop()
        {
            try
            {
                this.AnalogInput.Stop();
                this.DigitalInput.Stop();
            }
            catch (Exception ex)
            {
                if (this.ShowMessageHandler != null)
                {
                    this.ShowMessageHandler("Device Checking", ex.Message);
                }
            }
        }


        public void ExecuteRunDOCommand(string command)
        {
            DOCommandType type = (DOCommandType)Enum.Parse(typeof(DOCommandType), command);

            this.ExecuteRunDOCommand(type);
        }

        public void ExecuteRunDOCommand(DOCommandType type)
        {
            if (this.DigitalOutput != null)
            {
                switch (type)
                {
                    case DOCommandType.DO_LIGHT_ON: this.DigitalOutput.WriteDO(DbChannel.DO_LIGHT, true); SessionManager.Current.IsLightOn = true; break;
                    case DOCommandType.DO_LIGHT_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_LIGHT, false); SessionManager.Current.IsLightOn = false; break;
                    case DOCommandType.DO_IGNITOR_ON: this.DigitalOutput.WriteDO(DbChannel.DO_IGNITOR, true); SessionManager.Current.IsIgnitorOn = true; break;
                    case DOCommandType.DO_IGNITOR_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_IGNITOR, false); SessionManager.Current.IsIgnitorOn = false; break;
                    case DOCommandType.DO_FAN_ON: this.DigitalOutput.WriteDO(DbChannel.DO_FAN, true); SessionManager.Current.IsFanOn = true; break;
                    case DOCommandType.DO_FAN_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_FAN, false); SessionManager.Current.IsFanOn = false; break;
                    case DOCommandType.DO_CLEAR_ON: this.DigitalOutput.WriteDO(DbChannel.DO_FILTER_CLEAR, true);
#if IS_LOCAL
                        this.DigitalInput.IsClearFilterOn = true;
#endif
                        break;
                    case DOCommandType.DO_CLEAR_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_FILTER_CLEAR, false);
#if IS_LOCAL
                        this.DigitalInput.IsClearFilterOn = false;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER01_ON: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_01, true);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter01On = true;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER01_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_01, false);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter01On = false;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER02_ON: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_02, true);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter02On = true;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER02_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_02, false);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter02On = false;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER03_ON: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_03, true);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter03On = true;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER03_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_03, false);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter03On = false;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER05_ON: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_05, true);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter05On = true;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER05_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_05, false);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter05On = false;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER07_ON: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_07, true);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter07On = true;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER07_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_07, false);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter07On = false;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER10_ON: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_10, true);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter10On = true;
#endif
                        break;
                    case DOCommandType.DO_NDFILTER10_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_ND_FILTER_10, false);
#if IS_LOCAL
                        this.DigitalInput.IsNDFilter10On = false;
#endif
                        break;
                    case DOCommandType.DO_DARK_ON: this.DigitalOutput.WriteDO(DbChannel.DO_FILTER_DARK, true); 
#if IS_LOCAL
                        this.DigitalInput.IsDarkFilterOn = true;
#endif
                        break;
                    case DOCommandType.DO_DARK_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_FILTER_DARK, false);
#if IS_LOCAL
                        this.DigitalInput.IsDarkFilterOn = false;
#endif
                        break;
                    case DOCommandType.OPEN_OUT_DAMPER: this.DigitalOutput.WriteDO(DbChannel.DO_OUT_DAMPER, true);
#if IS_LOCAL
                        this.DigitalInput.IsOutDamperOpen = true;
#endif
                        break;
                    case DOCommandType.CLOSE_OUT_DAMPER: this.DigitalOutput.WriteDO(DbChannel.DO_OUT_DAMPER, false);
#if IS_LOCAL
                        this.DigitalInput.IsOutDamperOpen = false;
#endif
                        break;
                    case DOCommandType.OPEN_IN_DAMPER: this.DigitalOutput.WriteDO(DbChannel.DO_IN_DAMPER, true);
#if IS_LOCAL
                        this.DigitalInput.IsInDamperOpen = true;
#endif
                        break;
                    case DOCommandType.CLOSE_IN_DAMPER: this.DigitalOutput.WriteDO(DbChannel.DO_IN_DAMPER, false);
#if IS_LOCAL
                        this.DigitalInput.IsInDamperOpen = false;
#endif
                        break;
                    case DOCommandType.DO_LAMP_ON: this.DigitalOutput.WriteDO(DbChannel.DO_LAMP, true); SessionManager.Current.IsLampOn = true; break;
                    case DOCommandType.DO_LAMP_OFF: this.DigitalOutput.WriteDO(DbChannel.DO_LAMP, false); SessionManager.Current.IsLampOn = false; break;
                }
                Console.WriteLine(string.Format("{0}", type.ToString()));
            }
        }


        public void TurnDeviceOff()
        {
            this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);

            this.ExecuteRunDOCommand(DOCommandType.DO_LIGHT_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_IGNITOR_OFF);
            this.ExecuteRunDOCommand(DOCommandType.DO_FAN_OFF);
            this.ExecuteRunDOCommand(DOCommandType.CLOSE_OUT_DAMPER);
            this.ExecuteRunDOCommand(DOCommandType.CLOSE_IN_DAMPER);    
            this.ExecuteRunDOCommand(DOCommandType.DO_LAMP_OFF);
        }

        public void StartDevice(double inverter)
        {
            ThreadWorker.DoWork(() =>
            {
                DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_LIGHT_ON);

                Thread.Sleep(100);
                //DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_ON);
                DeviceManager.Current.ExecuteNDFilterCommand("CLEAR");

                // Control Inverter
                Thread.Sleep(100);
                DeviceManager.Current.AnalogOutput.WriteInverter(inverter);

                Thread.Sleep(200);
                DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_FAN_ON);                

                Thread.Sleep(100);
                DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_IGNITOR_ON);

                Thread.Sleep(IniConfig.ChamberTCSecondValue * 1000);
                DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_IGNITOR_OFF);
            });
        }

        public void StopDevice()
        {
            ThreadWorker.DoWork(() =>
            {
                DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_IGNITOR_OFF);

                Thread.Sleep(100);
                DeviceManager.Current.AnalogOutput.WriteInverter(0);

                Thread.Sleep(200);
                DeviceManager.Current.ExecuteRunDOCommand(DOCommandType.DO_FAN_OFF);
            });
        }

        public void ExecuteNDFilterCommand(object obj)
        {
            switch (obj.ToString().ToUpper())
            {
                case "CLEAR":
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_ON);
                    break;
                case "DARK":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_ON);                    
                    break;
                case "#1":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_ON);
                    break;
                case "#2":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_ON);
                    break;
                case "#3":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_ON);
                    break;
                case "#4":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_ON);
                    break;
                case "#5":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_ON);
                    break;
                case "#6":
                    this.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER01_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER02_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER03_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER05_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER07_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_DARK_OFF);
                    Thread.Sleep(100);
                    this.ExecuteRunDOCommand(DOCommandType.DO_NDFILTER10_ON);
                    break;
            }
        }

        //public void SetLightSpan(DbChannel channel , double voltage)
        //{

        //}

        //public void SetLightZero(DbChannel channel, double voltage)
        //{

        //}
    }
}