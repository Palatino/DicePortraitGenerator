using System;
using System.Collections.Generic;
using System.IO;
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

namespace DicePictureGeneratorUI
{
    /// <summary>
    /// Interaction logic for DicePortraitGenerator.xaml
    /// </summary>
    public partial class DicePortraitGeneratorView : UserControl
    {

        public DicePortraitGeneratorView()
        {
            InitializeComponent();
            this.DataContext = new DicePortraitGeneratorViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }
    }
}
