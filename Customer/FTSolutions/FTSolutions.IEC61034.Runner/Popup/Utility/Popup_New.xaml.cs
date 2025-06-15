using FTSolutions.IEC61034.Common.Base;
using System;

namespace FTSolutions.IEC61034.Runner.Popup.Utility
{
    public partial class Popup_New : BaseIEC61034Popup
    {
        public event Action<bool, string> EndEventHandler;

        public Popup_New()
        {
            InitializeComponent();

            this.ShowCaptureButton = false;
            this.ShowSettingButton = false;
            this.ShowDamperButton = false;
        }



        //###################################################################
        //  Property
        //###################################################################





        //###################################################################
        //  EventHandler
        //###################################################################

        private void Apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EndEventHandler != null)
            {
                this.EndEventHandler(true, this.txtInputValue.Text.Trim());
            }

            this.Close();
        }

        private void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EndEventHandler != null)
            {
                this.EndEventHandler(false, null);
            }

            this.Close();
        }
    }
}
