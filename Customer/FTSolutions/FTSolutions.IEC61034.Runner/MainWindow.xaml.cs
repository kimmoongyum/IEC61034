using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Behavior;
using FTSolutions.IEC61034.BizLogic.ViewModel;
using FTSolutions.IEC61034.Common;
using FTSolutions.IEC61034.Common.Setting;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FTSolutions.IEC61034.Runner
{
    public partial class MainWindow : BlackWindow
    {
        public MainWindow()
        {
            CheckDuplicationApp("IEC61034");

            InitializeComponent();

            SettingManager.ReadDefaultData();

            SessionManager.Current.AssemblyPath = Environment.CurrentDirectory + "\\" + this.GetType().Namespace + ".exe";
            SessionManager.Current.DefaultNamespace = this.GetType().Namespace;

            SessionManager.Current.CheckAuthentication();

            this.DataContext = new vmMainWindow();

            this.Loaded += MainWindow_Loaded;
            this.ContentRendered += MainWindow_ContentRendered;
            this.Closing += MainWindow_Closing;
            this.Closed += MainWindow_Closed;
            this.Activated += MainWindow_Activated;

            DeviceManager.Current.ShowMessageHandler += Current_ShowMessageHandler;
        }



        //###################################################################
        //  Property
        //###################################################################

        public vmMainWindow VIEW_MODEL { get { return (vmMainWindow)this.DataContext; } }

        public BaseView MAIN_UI { get; set; }




        //###################################################################
        //  EventHandler
        //###################################################################

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MAIN_UI = this.VIEW_MODEL.MainViewModelLoaded();

            if (this.MAIN_UI != null)
            {
                mainUIArea.Children.Add(this.MAIN_UI);
            }
        }

        private void MainWindow_ContentRendered(object sender, System.EventArgs e)
        {
            IMainUI mainUI = this.MAIN_UI as IMainUI;

            if (mainUI == null)
            {
                return;
            }

            IMainUIViewModel mainUIViewModel = this.MAIN_UI.DataContext as IMainUIViewModel;

            if (mainUIViewModel != null)
            {
                this.VIEW_MODEL.OwnerWindow = this;
                this.VIEW_MODEL.MainUI = mainUI;
                this.VIEW_MODEL.MainUIViewModel = mainUIViewModel;

                this.VIEW_MODEL.ShowMessageBox += VIEW_MODEL_ShowMessageBox;
                this.VIEW_MODEL.ShowMessageKeyBox += VIEW_MODEL_ShowMessageKeyBox;

                mainUIViewModel.Loaded(this);
            }
            else
            {
                mainUIArea.Children.Clear();
            }
        }

        private void menuListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(menuListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item == null)
            {
                menuListBox.SelectedIndex = -1;
            }
        }

        private void systemListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(systemListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item == null)
            {
                systemListBox.SelectedIndex = -1;
            }
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SessionManager.Current.IsRunning)
            {
                e.Cancel = true;

                this.ShowMessageKeyBox(MessageButtonType.OK, "msg_warning", "msg_quit_app");
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (this.MAIN_UI != null)
            {
                vmMainUI mainUIDataContext = this.MAIN_UI.DataContext as vmMainUI;

                if (mainUIDataContext != null)
                {
                    mainUIDataContext.CloseSoftware();

                    mainUIDataContext.Dispose();
                }

                this.MAIN_UI.DataContext = null;
                this.MAIN_UI.Dispose();
            }
        }


        private void MainWindow_Activated(object sender, EventArgs e)
        {

        }

        private void Current_ShowMessageHandler(string arg1, string arg2)
        {
            this.ShowMessageBox(MessageButtonType.OK, arg1, arg2);
        }

        private bool VIEW_MODEL_ShowMessageBox(MessageButtonType arg1, string arg2, string arg3)
        {
            return this.ShowMessageBox(arg1, arg2, arg3);
        }

        private bool VIEW_MODEL_ShowMessageKeyBox(MessageButtonType arg1, string arg2, string arg3)
        {
            return this.ShowMessageKeyBox(arg1, arg2, arg3);
        }

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();

            switch (SessionManager.Current.CurrentLanguage)
            {
                case "KOR":
                    dict.Source = new Uri("..\\Resources\\StringResources.kor.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
            }

            for (int i = this.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                if (this.Resources.MergedDictionaries[i].Source.OriginalString == dict.Source.OriginalString)
                {
                    this.Resources.MergedDictionaries.RemoveAt(i);
                }
            }

            for (int i = this.MAIN_UI.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                if (this.MAIN_UI.Resources.MergedDictionaries[i].Source.OriginalString == dict.Source.OriginalString)
                {
                    this.MAIN_UI.Resources.MergedDictionaries.RemoveAt(i);
                }
            }

            this.Resources.MergedDictionaries.Add(dict);
            this.MAIN_UI.Resources.MergedDictionaries.Add(dict);

            this.VIEW_MODEL.ChangedMultiLanguage();
        }
    }
}
