using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;
using System;
using System.Windows;

namespace FTSolutions.IEC61034.Common.Base
{
    public class BaseIEC61034Popup : BasePopup
    {
        public BaseIEC61034Popup()
        {
            this.ShowCaptureButton = true;
            this.ShowDamperButton = false;
            this.ShowSettingButton = false;

            this.IgnoreOpacity = false;

            this.Loaded += BaseIEC61034Popup_Loaded;
            this.Activated += BaseIEC61034Popup_Activated;
            this.Deactivated += BaseIEC61034Popup_Deactivated;
            this.Closed += BaseIEC61034Popup_Closed;
        }

        private void BaseIEC61034Popup_Closed(object sender, EventArgs e)
        {
            BaseIEC61034ViewModel vm = this.DataContext as BaseIEC61034ViewModel;

            if (vm != null && !vm.IsPopupOwner)
            {
                SessionManager.Current.LayoutOpacity = IEC61034Const.OPACITY_LAYOUT_ACTIVE;
            }
        }

        private void BaseIEC61034Popup_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLanguageDictionary();
        }

        private void BaseIEC61034Popup_Deactivated(object sender, System.EventArgs e)
        {

        }

        private void BaseIEC61034Popup_Activated(object sender, System.EventArgs e)
        {
            if (!this.IgnoreOpacity)
            {             
                SessionManager.Current.LayoutOpacity = IEC61034Const.OPACITY_LAYOUT_DEACTIVE;
            }
        }




        //###################################################################
        //  Property
        //###################################################################

        public bool IgnoreOpacity { get; set; }




        //###################################################################
        //  Private
        //###################################################################

        private void SetLanguageDictionary()
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

            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}
