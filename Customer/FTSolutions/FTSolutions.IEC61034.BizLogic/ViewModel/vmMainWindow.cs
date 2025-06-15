using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Behavior;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.SysType;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.QueryService;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmMainWindow : BaseViewModel
    {
        public vmMainWindow()
        {
            WizardClick = new DelegateCommand((o) => ExecuteWizardClick(o));
            MenuIconClick = new DelegateCommand((o) => ExecuteMenuIconClick(o));
            SystemIconClick = new DelegateCommand((o) => ExecuteSystemIconClick(o));
            PowerIconClick = new DelegateCommand((o) => ExecutePowerIconClick(o));
            MenuSelect = new DelegateCommand((o) => ExecuteMenuSelect(o));

            CaptureClick = new DelegateCommand((o) => ExecuteCaptureClick(o));

            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.FromMilliseconds(IEC61034Const.TIMER_INTERVAL);
            dTimer.Tick += (s, e) =>
            {
                StatusDatabase = ManagerQueryService.Current.StatusDatabase;
                StatusDeviceAnalog = DeviceManager.Current.StatusAnalog;
                StatusDeviceDigital = DeviceManager.Current.StatusDigital;

                CurrentDateTime = DateTime.Now;
            };

            dTimer.Start();
        }





        //###################################################################
        //  Initialize
        //###################################################################

        public BaseView MainViewModelLoaded()
        {
            return this.LoadMainUI();
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand WizardClick { get; private set; }

        public ICommand MenuIconClick { get; private set; }

        public ICommand SystemIconClick { get; private set; }

        public ICommand PowerIconClick { get; private set; }

        public ICommand MenuSelect { get; private set; }


        public ICommand CaptureClick { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        public Window OwnerWindow { get; set; }

        public IMainUI MainUI { get; set; }
        public IMainUIViewModel MainUIViewModel { get; set; }

        public SessionManager SESSION_MANAGER { get { return SessionManager.Current; } }


        private DateTime _currentDateTime;
        public DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                if (this._currentDateTime != value)
                {
                    this._currentDateTime = value;
                    this.RaisePropertyChanged(nameof(CurrentDateTime));
                }
            }
        }

        private Visibility _menuPanelVisibility;
        public Visibility MenuPanelVisibility
        {
            get { return _menuPanelVisibility; }
            set
            {
                if (this._menuPanelVisibility != value)
                {
                    this._menuPanelVisibility = value;
                    this.RaisePropertyChanged(nameof(MenuPanelVisibility));
                }
            }
        }

        private Visibility _systemPanelVisibility;
        public Visibility SystemPanelVisibility
        {
            get { return _systemPanelVisibility; }
            set
            {
                if (this._systemPanelVisibility != value)
                {
                    this._systemPanelVisibility = value;
                    this.RaisePropertyChanged(nameof(SystemPanelVisibility));
                }
            }
        }

        private bool _menuIsChecked;
        public bool MenuIsChecked
        {
            get { return _menuIsChecked; }
            set
            {
                if (this._menuIsChecked != value)
                {
                    this._menuIsChecked = value;
                    this.RaisePropertyChanged(nameof(MenuIsChecked));
                }
            }
        }

        private bool _systemIsChecked;
        public bool SystemIsChecked
        {
            get { return _systemIsChecked; }
            set
            {
                if (this._systemIsChecked != value)
                {
                    this._systemIsChecked = value;
                    this.RaisePropertyChanged(nameof(SystemIsChecked));
                }
            }
        }


        private MenuInfoCollection _systemMenuInfo;
        public MenuInfoCollection SystemMenuInfo
        {
            get { return _systemMenuInfo; }
            set
            {
                if (this._systemMenuInfo != value)
                {
                    this._systemMenuInfo = value;
                    this.RaisePropertyChanged(nameof(SystemMenuInfo));
                }
            }
        }

        private MenuInfoCollection _mainMenuInfo;
        public MenuInfoCollection MainMenuInfo
        {
            get { return _mainMenuInfo; }
            set
            {
                if (this._mainMenuInfo != value)
                {
                    this._mainMenuInfo = value;
                    this.RaisePropertyChanged(nameof(MainMenuInfo));
                }
            }
        }


        private SolidColorBrush _statusDatabase;
        public SolidColorBrush StatusDatabase
        {
            get { return _statusDatabase; }
            set
            {
                if (this._statusDatabase != value)
                {
                    this._statusDatabase = value;
                    this.RaisePropertyChanged(nameof(StatusDatabase));
                }
            }
        }

        private SolidColorBrush _statusDeviceAnalog;
        public SolidColorBrush StatusDeviceAnalog
        {
            get { return _statusDeviceAnalog; }
            set
            {
                if (this._statusDeviceAnalog != value)
                {
                    this._statusDeviceAnalog = value;
                    this.RaisePropertyChanged(nameof(StatusDeviceAnalog));
                }
            }
        }

        private SolidColorBrush _statusDeviceDigital;
        public SolidColorBrush StatusDeviceDigital
        {
            get { return _statusDeviceDigital; }
            set
            {
                if (this._statusDeviceDigital != value)
                {
                    this._statusDeviceDigital = value;
                    this.RaisePropertyChanged(nameof(StatusDeviceDigital));
                }
            }
        }

        private bool _wizardClickState;
        public bool WizardClickState
        {
            get { return _wizardClickState; }
            set
            {
                if (this._wizardClickState != value)
                {
                    this._wizardClickState = value;
                    this.RaisePropertyChanged(nameof(WizardClickState));
                }
            }
        }




        //###################################################################
        //  Override
        //###################################################################




        //###################################################################
        //  EventHandler
        //###################################################################

        private void ExecuteWizardClick(object obj)
        {
            MenuIsChecked = true;
            SystemIsChecked = false;

            this.ChangePanelVisibility(true);
        }

        private void ExecuteMenuIconClick(object obj)
        {
            if ((bool)obj)
            {
                SystemIsChecked = false;

                this.ChangePanelVisibility(true);
            }
        }

        private void ExecuteSystemIconClick(object obj)
        {
            if ((bool)obj)
            {
                MenuIsChecked = false;

                this.ChangePanelVisibility(false);
            }
        }

        private void ExecutePowerIconClick(object obj)
        {
            if (SessionManager.Current.IsRunning)
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_quit_app");
                return;
            }

            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_quit_app", "msg_quit_app2");

            if (result && this.CloseWindow != null)
            {
                this.CloseWindow();
            }
        }

        private void ExecuteMenuSelect(object obj)
        {
            if (SessionManager.Current.IsRunning)
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_warning", "msg_run_menu");
                return;
            }

            MenuInfo menuInfo = obj as MenuInfo;

            if (menuInfo != null)
            {
                this.WizardClickState = false;
                this.MainUIViewModel.RunMenu(this.OwnerWindow, menuInfo);
            }
        }

        private void ExecuteCaptureClick(object obj)
        {
            int screenLeft = (int)SystemParameters.VirtualScreenLeft;
            int screenTop = (int)SystemParameters.VirtualScreenTop;
            int pScreenWidth = (int)SystemParameters.PrimaryScreenWidth;
            int pScreenHeight = (int)SystemParameters.PrimaryScreenHeight;
            int vScreenWidth = (int)SystemParameters.VirtualScreenWidth;
            int vScreenHeight = (int)SystemParameters.VirtualScreenHeight;

            this.CreateImage(screenLeft, screenTop, pScreenWidth, pScreenHeight);
            this.CreateImage(pScreenWidth, screenTop, vScreenWidth - pScreenWidth, vScreenHeight);
        }



        //###################################################################
        //  Private
        //###################################################################

        private BaseView LoadMainUI()
        {
            BaseView mainUI = PopupHelper.GetViewInstance(SessionManager.Current.AssemblyPath, SessionManager.Current.DefaultNamespace, "MainUI") as BaseView;

            if (mainUI is IMainUI)
            {
                SystemMenuInfo = ((IMainUI)mainUI).GetSystemMenuInfo();
                MainMenuInfo = ((IMainUI)mainUI).GetMainMenuInfo();
            }

            return mainUI as BaseView;
        }

        private void ChangePanelVisibility(bool isMenuPanelVisible)
        {
            this.MenuPanelVisibility = isMenuPanelVisible ? Visibility.Visible : Visibility.Collapsed;
            this.SystemPanelVisibility = !isMenuPanelVisible ? Visibility.Visible : Visibility.Collapsed;
        }


        private void CreateImage(int screenLeft, int screenTop, int screenWidth, int screenHeight)
        {
            if (screenWidth < 1 || screenHeight < 1)
            {
                return;
            }

            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    string dir = SessionManager.Current.ConfigSetting.GetValue(IEC61034Const.KEY_REPORT_PATH);

                    DirectoryInfo diSave = new DirectoryInfo(dir);
                    if (!diSave.Exists)
                    {
                        diSave.Create();
                    }

                    string fileName = String.Format("{0}\\Capture_Furnace_{1}.png", dir, DateTime.Now.ToString("yyyyMMdd_HHmmssff"));

                    this.OwnerWindow.Opacity = .0;
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                    bmp.Save(fileName);
                    this.OwnerWindow.Opacity = 1;
                }
            }
        }



        //###################################################################
        //  Public
        //###################################################################

        public void ChangedMultiLanguage()
        {
            bool isLanguageEng = this.SESSION_MANAGER.CurrentLanguage.Equals("KOR") ? false : true;

            this.SystemMenuInfo.ChangeMultiLanguage(isLanguageEng);
            this.MainMenuInfo.ChangeMultiLanguage(isLanguageEng);
        }
    }
}
