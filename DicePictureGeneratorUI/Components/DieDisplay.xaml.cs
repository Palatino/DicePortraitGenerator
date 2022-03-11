using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DicePictureGeneratorUI.Components
{
    /// <summary>
    /// Interaction logic for DieDisplay.xaml
    /// </summary>
    public partial class DieDisplay : UserControl
    {
        public DieDisplay()
        {
            InitializeComponent();
        }

        public int DieValue
        {
            get { return (int)GetValue(DieValueProperty); }
            set { SetValue(DieValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DieValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DieValueProperty =
            DependencyProperty.Register("DieValue", typeof(int), typeof(DieDisplay));
        public string DieColor
        {
            get { return (string)GetValue(DieColorProperty); }
            set { SetValue(DieColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DieColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DieColorProperty =
            DependencyProperty.Register("DieColor", typeof(string), typeof(DieDisplay));


    }
}
