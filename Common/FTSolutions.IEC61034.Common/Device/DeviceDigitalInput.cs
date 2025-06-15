using eccFramework.SharedLib.Core.Device.DAQ;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.Setting;
using NationalInstruments.DAQmx;
using System;

namespace FTSolutions.IEC61034.Common.Device
{
    public class DeviceDigitalInput : BaseIEC61034Device
    {
        public event Action<string, string> ShowMessageHandler;

        NIDigitalInput _daq = null;


        public DeviceDigitalInput(int interval)
        {
            this.Interval = interval;
        }



        //###################################################################
        //  Property
        //###################################################################

        public double Interval { get; set; }

        private bool _isClearFilterOn;
        public bool IsClearFilterOn
        {
            get { return _isClearFilterOn; }
            set
            {
                if (this._isClearFilterOn != value)
                {
                    this._isClearFilterOn = value;
                    this.StatusLightClear = value;
                }

                this.RaisePropertyChanged(nameof(IsClearFilterOn));
                this.RaisePropertyChanged(nameof(IsClearFilterOff));
            }
        }
        public bool IsClearFilterOff { get { return !this.IsClearFilterOn; } }

        private bool _isDarkFilterOn;
        public bool IsDarkFilterOn
        {
            get { return _isDarkFilterOn; }
            set
            {
                if (this._isDarkFilterOn != value)
                {
                    this._isDarkFilterOn = value;
                    this.StatusLightDark = value;
                }

                if(value)
                {
                    Console.WriteLine(string.Format("{0} : {1} : {2} : {3} : {4} : {5} : {6} : {7}", this.IsClearFilterOn, this.IsNDFilter01On, this.IsNDFilter02On, this.IsNDFilter03On, this.IsNDFilter05On, this.IsNDFilter07On, this.IsNDFilter10On, this.IsDarkFilterOn));
                }

                this.RaisePropertyChanged(nameof(IsDarkFilterOn));
                this.RaisePropertyChanged(nameof(IsDarkFilterOff));
            }
        }
        public bool IsDarkFilterOff { get { return !this.IsDarkFilterOn; } }


        private bool _isNDFilter01On;
        public bool IsNDFilter01On
        {
            get { return _isNDFilter01On; }
            set
            {
                if (this._isNDFilter01On != value)
                {
                    this._isNDFilter01On = value;
                    this.StatusLightFilter1 = value;
                }

                this.RaisePropertyChanged(nameof(IsNDFilter01On));
                this.RaisePropertyChanged(nameof(IsNDFilter01Off));
            }
        }
        public bool IsNDFilter01Off { get { return !this.IsNDFilter01On; } }

        private bool _isNDFilter02On;
        public bool IsNDFilter02On
        {
            get { return _isNDFilter02On; }
            set
            {
                if (this._isNDFilter02On != value)
                {
                    this._isNDFilter02On = value;
                    this.StatusLightFilter2 = value;
                }

                this.RaisePropertyChanged(nameof(IsNDFilter02On));
                this.RaisePropertyChanged(nameof(IsNDFilter02Off));
            }
        }
        public bool IsNDFilter02Off { get { return !this.IsNDFilter02On; } }

        private bool _isNDFilter03On;
        public bool IsNDFilter03On
        {
            get { return _isNDFilter03On; }
            set
            {
                if (this._isNDFilter03On != value)
                {
                    this._isNDFilter03On = value;
                    this.StatusLightFilter3 = value;
                }

                this.RaisePropertyChanged(nameof(IsNDFilter03On));
                this.RaisePropertyChanged(nameof(IsNDFilter03Off));
            }
        }
        public bool IsNDFilter03Off { get { return !this.IsNDFilter03On; } }

        private bool _isNDFilter05On;
        public bool IsNDFilter05On
        {
            get { return _isNDFilter05On; }
            set
            {
                if (this._isNDFilter05On != value)
                {
                    this._isNDFilter05On = value;
                    this.StatusLightFilter4 = value;
                }

                this.RaisePropertyChanged(nameof(IsNDFilter05On));
                this.RaisePropertyChanged(nameof(IsNDFilter05Off));
            }
        }
        public bool IsNDFilter05Off { get { return !this.IsNDFilter05On; } }

        private bool _isNDFilter07On;
        public bool IsNDFilter07On
        {
            get { return _isNDFilter07On; }
            set
            {
                if (this._isNDFilter07On != value)
                {
                    this._isNDFilter07On = value;
                    this.StatusLightFilter5 = value;
                }

                this.RaisePropertyChanged(nameof(IsNDFilter07On));
                this.RaisePropertyChanged(nameof(IsNDFilter07Off));
            }
        }
        public bool IsNDFilter07Off { get { return !this.IsNDFilter07On; } }

        private bool _isNDFilter10On;
        public bool IsNDFilter10On
        {
            get { return _isNDFilter10On; }
            set
            {
                if (this._isNDFilter10On != value)
                {
                    this._isNDFilter10On = value;
                    this.StatusLightFilter6 = value;
                }

                this.RaisePropertyChanged(nameof(IsNDFilter10On));
                this.RaisePropertyChanged(nameof(IsNDFilter10Off));
            }
        }
        public bool IsNDFilter10Off { get { return !this.IsNDFilter10On; } }


        private bool _isOutDamperOpen;
        public bool IsOutDamperOpen
        {
            get { return _isOutDamperOpen; }
            set
            {
                if (this._isOutDamperOpen != value)
                {
                    this._isOutDamperOpen = value;
                }

                this.RaisePropertyChanged(nameof(IsOutDamperOpen));
                this.RaisePropertyChanged(nameof(IsOutDamperClose));
            }
        }
        public bool IsOutDamperClose { get { return !this.IsOutDamperOpen; } }


        private bool _isInDamperOpen;
        public bool IsInDamperOpen
        {
            get { return _isInDamperOpen; }
            set
            {
                if (this._isInDamperOpen != value)
                {
                    this._isInDamperOpen = value;
                }

                this.RaisePropertyChanged(nameof(IsInDamperOpen));
                this.RaisePropertyChanged(nameof(IsInDamperClose));
            }
        }
        public bool IsInDamperClose { get { return !this.IsInDamperOpen; } }


        private bool _statusLightClear;
        public bool StatusLightClear
        {
            get { return _statusLightClear; }
            set
            {
                if (this._statusLightClear != value)
                {
                    this._statusLightClear = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightClear));
            }
        }

        private bool _statusLightDark;
        public bool StatusLightDark
        {
            get { return _statusLightDark; }
            set
            {
                if (this._statusLightDark != value)
                {
                    this._statusLightDark = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightDark));
            }
        }

        private bool _statusLightFilter1;
        public bool StatusLightFilter1
        {
            get { return _statusLightFilter1; }
            set
            {
                if (this._statusLightFilter1 != value)
                {
                    this._statusLightFilter1 = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightFilter1));
            }
        }

        private bool _statusLightFilter2;
        public bool StatusLightFilter2
        {
            get { return _statusLightFilter2; }
            set
            {
                if (this._statusLightFilter2 != value)
                {
                    this._statusLightFilter2 = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightFilter2));
            }
        }

        private bool _statusLightFilter3;
        public bool StatusLightFilter3
        {
            get { return _statusLightFilter3; }
            set
            {
                if (this._statusLightFilter3 != value)
                {
                    this._statusLightFilter3 = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightFilter3));
            }
        }

        private bool _statusLightFilter4;
        public bool StatusLightFilter4
        {
            get { return _statusLightFilter4; }
            set
            {
                if (this._statusLightFilter4 != value)
                {
                    this._statusLightFilter4 = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightFilter4));
            }
        }

        private bool _statusLightFilter5;
        public bool StatusLightFilter5
        {
            get { return _statusLightFilter5; }
            set
            {
                if (this._statusLightFilter5 != value)
                {
                    this._statusLightFilter5 = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightFilter5));
            }
        }

        private bool _statusLightFilter6;
        public bool StatusLightFilter6
        {
            get { return _statusLightFilter6; }
            set
            {
                if (this._statusLightFilter6 != value)
                {
                    this._statusLightFilter6 = value;
                }

                this.RaisePropertyChanged(nameof(StatusLightFilter6));
            }
        }
        //###################################################################
        //  Override
        //###################################################################

        public override void Run()
        {
            try
            {
                if (_daq == null)
                {
                    _daq = new NIDigitalInput(AnalysisData);

                    _daq.AddChannel(DbChannel.DI_FILTER_CLEAR.Address, DbChannel.DI_FILTER_CLEAR.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_ND_FILTER_01.Address, DbChannel.DI_ND_FILTER_01.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_ND_FILTER_02.Address, DbChannel.DI_ND_FILTER_02.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_ND_FILTER_03.Address, DbChannel.DI_ND_FILTER_03.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_ND_FILTER_05.Address, DbChannel.DI_ND_FILTER_05.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_ND_FILTER_07.Address, DbChannel.DI_ND_FILTER_07.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_ND_FILTER_10.Address, DbChannel.DI_ND_FILTER_10.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_FILTER_DARK.Address, DbChannel.DI_FILTER_DARK.Channel, ChannelLineGrouping.OneChannelForEachLine);

                    _daq.AddChannel(DbChannel.DI_OUT_DAMPER.Address, DbChannel.DI_OUT_DAMPER.Channel, ChannelLineGrouping.OneChannelForEachLine);
                    _daq.AddChannel(DbChannel.DI_IN_DAMPER.Address, DbChannel.DI_IN_DAMPER.Channel, ChannelLineGrouping.OneChannelForEachLine);
                }

                _daq.Interval = this.Interval;

                _daq.ReadDigitalInputContinuous();

                if (this.CheckDAQService != null)
                {
                    this.CheckDAQService(true, null);
                }
            }
            catch (Exception ex)
            {
                if (this.CheckDAQService != null)
                {
                    this.CheckDAQService(false, ex);
                }
            }
        }

        public override void Stop()
        {
            _daq.StopDigitalInputContinuous();
        }

        public override void Clear()
        {

        }




        //###################################################################
        //  Public
        //###################################################################

        public void AnalysisData(bool[] values)
        {
#if !IS_LOCAL
            try
            {
                this.IsClearFilterOn = values[0];
                this.IsNDFilter01On = values[1];
                this.IsNDFilter02On = values[2];
                this.IsNDFilter03On = values[3];
                this.IsNDFilter05On = values[4];
                this.IsNDFilter07On = values[5];
                this.IsNDFilter10On = values[6];
                this.IsDarkFilterOn = values[7];
                this.IsOutDamperOpen = values[8];
                this.IsInDamperOpen = values[9];
            }
            catch { }

            this.WriteDevice($"[DigitalIn]Clear:{IsClearFilterOn},NDFilter(0.1):{IsNDFilter01On},NDFilter(0.2):{IsNDFilter02On},NDFilter(0.3):{IsNDFilter03On},NDFilter(0.5):{IsNDFilter05On},NDFilter(0.7):{IsNDFilter07On},NDFilter(1.0):{IsNDFilter10On},NDFilter(Dark):{IsDarkFilterOn},OUTDamper:{IsOutDamperOpen},INDamper:{IsInDamperOpen}");
#endif
        }
    }
}
