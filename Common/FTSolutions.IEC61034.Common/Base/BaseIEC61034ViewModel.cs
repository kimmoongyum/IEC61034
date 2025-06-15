using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Utility.Helper;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace FTSolutions.IEC61034.Common.Base
{
    public class BaseIEC61034ViewModel : BaseViewModel
    {
        public BaseIEC61034ViewModel()
        {
            IsValidState = false;

            BackClick = new DelegateCommand((o) => ExecuteBackClick(o));
            NextClick = new DelegateCommand((o) =>
            {
                if (this.IsValid())
                {
                    ExecuteNextClick(o);
                }
                else
                {
                    AlertInvalid();
                }
            });
            FilterClick = new DelegateCommand((o) =>
            {
                if (this.IsValid())
                {
                    ExecuteFilterClick(o);
                }
                else
                {
                    AlertInvalidLightCalibration();
                }
            });
            CancelClick = new DelegateCommand((o) =>
            {
                if (ExecuteCancelClick(o) && !IsPopupOwner)
                {
                    SessionManager.Current.LayoutOpacity = IEC61034Const.OPACITY_LAYOUT_ACTIVE;
                }
            });
            CloseClick = new DelegateCommand((o) =>
            {
                if (ExecuteCloseClick(o) && !IsPopupOwner)
                {
                    SessionManager.Current.LayoutOpacity = IEC61034Const.OPACITY_LAYOUT_ACTIVE;
                }
            });
            SaveClick = new DelegateCommand((o) =>
            {
                if (ExecuteSaveClick(o) && !IsPopupOwner)
                {
                    SessionManager.Current.LayoutOpacity = IEC61034Const.OPACITY_LAYOUT_ACTIVE;
                }
            });

            RunDOCommand = new DelegateCommand((o) => ExecuteRunDOCommand(o));      
        }



        //###################################################################
        //  Property
        //###################################################################

        public SessionManager SESSION_MANAGER { get { return SessionManager.Current; } }

        public DeviceManager DEVICE_MANAGER { get { return DeviceManager.Current; } }

        private MenuKind _callerMenu;
        public MenuKind CallerMenu
        {
            get { return _callerMenu; }
            set
            {
                _callerMenu = value;
                this.RaisePropertyChanged(nameof(CallerMenu));
            }
        }

        public bool IsValidState { get; set; }

        public Visibility BackVisibility { get; set; }

        public Visibility NextVisibility { get; set; }

        public Visibility CancelVisibility { get; set; }

        public Visibility CloseVisibility { get; set; }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand BackClick { get; protected set; }

        public ICommand NextClick { get; protected set; }

        public ICommand FilterClick { get; protected set; }

        public ICommand CancelClick { get; protected set; }

        public ICommand CloseClick { get; protected set; }

        public ICommand SaveClick { get; protected set; }


        public ICommand RunDOCommand { get; protected set; }


        //###################################################################
        //  Override
        //###################################################################

        public override void ExecuteCapture(Window window)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    string path = SESSION_MANAGER.ConfigSetting.GetValue(IEC61034Const.KEY_REPORT_PATH);

                    DirectoryInfo diSave = new DirectoryInfo(path);
                    if (!diSave.Exists)
                    {
                        diSave.Create();
                    }

                    string fileName = String.Format("{0}\\Capture_IEC61034_{1}.png", path, DateTime.Now.ToString("yyyyMMdd_HHmmssff"));

                    if (this.Owner != null)
                    {
                        this.Owner.Opacity = .0;
                    }

                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save(fileName);

                    if (this.Owner != null)
                    {
                        this.Owner.Opacity = 1;
                    }
                }
            }
        }

        public override void VMLoaded()
        {
            BackVisibility = this.CallerMenu == MenuKind.MAIN ? Visibility.Collapsed : Visibility.Visible;
            NextVisibility = this.CallerMenu == MenuKind.MAIN ? Visibility.Collapsed : Visibility.Visible;
            CancelVisibility = this.CallerMenu == MenuKind.MAIN ? Visibility.Collapsed : Visibility.Visible;
            CloseVisibility = this.CallerMenu == MenuKind.MAIN ? Visibility.Visible : Visibility.Collapsed;
        }



        //###################################################################
        //  Virtual
        //###################################################################

        protected virtual void ExecuteBackClick(object obj)
        {
            this.IsPopupOwner = true;
        }

        protected virtual void ExecuteNextClick(object obj)
        {
            this.IsPopupOwner = true;
        }

        protected virtual void ExecuteFilterClick(object obj)
        {
            this.IsPopupOwner = true;
        }

        protected virtual bool ExecuteCancelClick(object obj)
        {
            return true;
        }

        protected virtual bool ExecuteCloseClick(object obj)
        {
            return true;
        }

        protected virtual bool ExecuteSaveClick(object obj)
        {
            return true;
        }

        public virtual void SetFooterButtonVisibleState(bool backVisible, bool nextVisible, bool cancelVisible)
        {
            BackVisibility = backVisible ? Visibility.Visible : Visibility.Collapsed;
            NextVisibility = nextVisible ? Visibility.Visible : Visibility.Collapsed;
            CancelVisibility = cancelVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public virtual void SetFooterButtonVisibleState(bool backVisible, bool nextVisible, bool cancelVisible, bool closeVisible)
        {
            this.SetFooterButtonVisibleState(backVisible, nextVisible, cancelVisible);

            CloseVisibility = closeVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public virtual bool IsValid()
        {
            return IsValidState;
        }

        public virtual void AlertInvalid()
        {
            this.ShowMessageKey(MessageButtonType.OK, "msg_title_check_data", "msg_invalid_item");
        }


        public virtual void AlertInvalidLightCalibration()
        {
            this.ShowMessageKey(MessageButtonType.OK, "msg_title_check_data", "msg_invalid_light_calibration");
        }

        //###################################################################
        //  EventHandler
        //###################################################################

        public void ExecuteRunDOCommand(object obj)
        {
            if (obj != null)
            {
                DOCommandType type = (DOCommandType)Enum.Parse(typeof(DOCommandType), obj.ToString());

                this.DEVICE_MANAGER.ExecuteRunDOCommand(type);
            }
        }

        //###################################################################
        //  Public
        //###################################################################

        public void WriteRealtime(string msg)
        {
            if (IniConfig.IsAllowRealtimeLog)
            {
                LogIt.WriteRealtime(msg);
            }
        }

        public void WriteEvent(LogType type, string msg)
        {
            if (IniConfig.IsAllowEventLog)
            {
                LogIt.WriteEvent(type, msg);
            }
        }
    }
}
