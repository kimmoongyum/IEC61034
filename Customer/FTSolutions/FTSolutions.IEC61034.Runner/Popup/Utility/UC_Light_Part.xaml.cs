using System.Windows;
using System.Windows.Controls;

namespace FTSolutions.IEC61034.Runner.Popup.Utility
{
    /// <summary>
    /// Interaction logic for UC_Light_Part.xaml
    /// </summary>
    public partial class UC_Light_Part : UserControl
    {
        public UC_Light_Part()
        {
            InitializeComponent();
        }



        public static readonly DependencyProperty LightVisibilityProperty = DependencyProperty.Register("LightVisibility",
            typeof(Visibility), typeof(UC_Light_Part), new PropertyMetadata(Visibility.Collapsed));

        public Visibility LightVisibility
        {
            get { return (Visibility)GetValue(LightVisibilityProperty); }
            set { SetValue(LightVisibilityProperty, value); }
        }
    }
}
