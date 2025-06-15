using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Behavior;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.SysType;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Result;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Windows;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmMainUI : BaseIEC61034ViewModel, IMainUIViewModel
    {
        private DispatcherTimer _timerCheckState;

        public vmMainUI()
        {
            this.CurrentItem_Measurement = new CurrentMeasurement();

            SessionManager.Current.LayoutOpacity = IEC61034Const.OPACITY_LAYOUT_ACTIVE;
        }



        //###################################################################
        //  Command
        //###################################################################




        //###################################################################
        //  Property
        //###################################################################

        private CurrentMeasurement _currentItem_Measurement;
        public CurrentMeasurement CurrentItem_Measurement
        {
            get { return _currentItem_Measurement; }
            set
            {
                if (this._currentItem_Measurement != value)
                {
                    this._currentItem_Measurement = value;
                    this.RaisePropertyChanged(nameof(CurrentItem_Measurement));
                }
            }
        }



        //###################################################################
        //  Override
        //###################################################################

        public override void Dispose()
        {
            base.Dispose();

            if (this._timerCheckState != null)
            {
                this._timerCheckState.Stop();
                this._timerCheckState = null;
            }
        }



        //###################################################################
        //  IMainUIViewModel
        //###################################################################

        public void RunMenu(Window owner, MenuInfo menuInfo)
        {
            BasePopup pop = null;
            string nextMenu = "";

            switch (menuInfo.MENU_NAME)
            {
                case IEC61034Const.MENU_CONFIG_SETTING:
                    nextMenu = IEC61034Const.MENU_CONFIG_SETTING_KEY;
                    break;
                case IEC61034Const.MENU_CHANNEL_SETTING:
                    nextMenu = IEC61034Const.MENU_CHANNEL_SETTING_KEY;
                    break;
                case IEC61034Const.MENU_REGISTER_MANAGER:
                    nextMenu = IEC61034Const.MENU_REGISTER_MANAGER_KEY;
                    break;
                case IEC61034Const.MENU_CONTROL_PANEL:
                    nextMenu = IEC61034Const.MENU_CONTROL_PANEL_KEY;
                    break;
                case IEC61034Const.MENU_BLANK_TEST:
                    nextMenu = IEC61034Const.MENU_BLANK_TEST_KEY;
                    break;
                case IEC61034Const.MENU_LIGHT_CALIBRATION:
                    nextMenu = IEC61034Const.MENU_LIGHT_CALIBRATION_KEY;
                    break;
                case IEC61034Const.MENU_FILTER_CALIBRATION:
                    nextMenu = IEC61034Const.MENU_FILTER_CALIBRATION_KEY;
                    break;
                case IEC61034Const.MENU_QUALIFICATION_REGISTER_MANAGER:
                    nextMenu = IEC61034Const.MENU_QUALIFICATION_REGISTER_MANAGER_KEY;
                    break;

            }

            if (nextMenu != null && nextMenu.Trim().Length > 1)
            {
                pop = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, nextMenu);
            }

            if (pop != null)
            {
                pop.Owner = owner;
                pop.ShowInTaskbar = false;
                pop.SendSignHandler += Pop_SendSignHandler;
                
                pop.DataContextChanged += (s, o) =>
                {
                    BaseIEC61034ViewModel vm = pop.DataContext as BaseIEC61034ViewModel;
                    vm.CallerMenu = MenuKind.MAIN;
                };

                pop.ShowDialog();
            }
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        public void Loaded(Window owner)
        {
            this.DEVICE_MANAGER.Run();

            this.Initialize();

            this.Owner = owner;
        }

        private void Pop_SendSignHandler(string menu, string key, object data)
        {
            switch (menu)
            {
                case IEC61034Const.MENU_REGISTER_MANAGER:
                    this.MenuHandleOnRegistration(key, data);
                    break;
                case IEC61034Const.MENU_QUALIFICATION_REGISTER_MANAGER:
                    this.MenuHandleOnQualificationRegistration(key, data);
                    break;
            }
        }

        private void MenuHandleOnRegistration(string key, object data)
        {
            if (key.ToUpper().Equals("TEST") && data is TypeRegistration)
            {
                TypeRegistration regInfo = data as TypeRegistration;

                if (regInfo != null)
                {
                    string targetMenu = IEC61034Const.MENU_TEST_PROPERTY_KEY;
                    BasePopup pop = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);

                    if (pop != null)
                    {
                        pop.Owner = this.Owner;
                        pop.ShowInTaskbar = false;
                        pop.DataContextChanged += (s, o) => 
                        {
                            vmPopup_TestProperties vm = pop.DataContext as vmPopup_TestProperties;
                            vm.RegistrationInfo = regInfo;
                            vm.CallerMenu = MenuKind.REGISTRATION;
                        };

                        pop.ShowDialog();
                    }
                }
            }
        }

        private void MenuHandleOnQualificationRegistration(string key, object data)
        {
            if (key.ToUpper().Equals("QUALIFICATION") && data is TypeQualificationRegistration)
            {
                TypeQualificationRegistration regInfo = data as TypeQualificationRegistration;

                if (regInfo != null)
                {
                    string targetMenu = IEC61034Const.MENU_QUALIFICATION_PROPERTY_KEY;
                    BasePopup pop = PopupHelper.GetPopupInstance(SESSION_MANAGER.AssemblyPath, SESSION_MANAGER.DefaultNamespace, targetMenu);

                    if (pop != null)
                    {
                        pop.Owner = this.Owner;
                        pop.ShowInTaskbar = false;
                        pop.DataContextChanged += (s, o) =>
                        {
                            vmPopup_QualificationProperties vm = pop.DataContext as vmPopup_QualificationProperties;
                            vm.RegistrationInfo = regInfo;
                            vm.CallerMenu = MenuKind.QUALIFICATION_REGISTRATION;
                        };

                        pop.ShowDialog();
                    }
                }
            }
        }
        //###################################################################
        //  Private
        //###################################################################

        private void Initialize()
        {
            SESSION_MANAGER.CurrentLanguage = SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_LANGUAGE);

            this.DEVICE_MANAGER.TurnDeviceOff();

            this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.DO_LIGHT_ON);
            //this.DEVICE_MANAGER.ExecuteRunDOCommand(DOCommandType.DO_CLEAR_ON);
            this.DEVICE_MANAGER.ExecuteNDFilterCommand("CLEAR");

            this._timerCheckState = new DispatcherTimer();
            this._timerCheckState.Interval = TimeSpan.FromMilliseconds(IEC61034Const.MEASURING_INTERVAL);
            this._timerCheckState.Tick += (s, e) =>
            {
                this.CheckCurrentState();
            };

            this._timerCheckState.Start();
        }

        private void Clear()
        {
        }

        private void CheckCurrentState()
        {
            this.CurrentItem_Measurement.Transmission = this.DEVICE_MANAGER.AnalogInput.Transmission;
            this.CurrentItem_Measurement.Absorbance = this.DEVICE_MANAGER.AnalogInput.Absorbance;
            this.CurrentItem_Measurement.ChamberTemperature = this.DEVICE_MANAGER.AnalogInput.ChamberTemperature;
            this.CurrentItem_Measurement.FanFlowrate = this.DEVICE_MANAGER.AnalogInput.FanFlowrate;
        }


        //###################################################################
        //  Public
        //###################################################################

        public void CloseSoftware()
        {
            this.DEVICE_MANAGER.TurnDeviceOff();
        }
    }
}