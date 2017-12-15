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
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.UserView
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        //Class Deklarasi
        private CUser _ctrl;

        public UserView()
        {
            InitializeComponent();
        }
        //Window Event
        private void FrmUser_Loaded(object sender, RoutedEventArgs e)
        {
            TableDecor();
            _ctrl = CUser.GetInstance;
            TableDecor();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Code = string.Empty;
            _ctrl.Index("Action");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TableDecor()
        {
            if (TblDataUser.Columns.Count > 0)
            {
                TblDataUser.Columns[0].Visibility = Visibility.Collapsed;
                TblDataUser.Columns[1].Visibility = Visibility.Hidden;
                TblDataUser.Columns[0].Visibility = Visibility.Hidden;
                TblDataUser.Columns[2].Visibility = Visibility.Hidden;
                TblDataUser.Columns[3].Visibility = Visibility.Hidden;

                TblDataUser.Columns[1].Width = DataGridLength.Auto;
                TblDataUser.Columns[2].Width = DataGridLength.Auto;
                TblDataUser.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
