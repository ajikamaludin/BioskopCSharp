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
using System.Windows.Shapes;

namespace BioskopCSharp.Views.CetakView
{
    /// <summary>
    /// Interaction logic for CetakViewAct.xaml
    /// </summary>
    public partial class CetakViewAct : Window
    {
        public CetakViewAct()
        {
            InitializeComponent();
        }

        private void BtnCetak_Click(object sender, RoutedEventArgs e)
        {
            string dateStart = DateStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            Console.WriteLine(dateStart);
        }
    }
}
