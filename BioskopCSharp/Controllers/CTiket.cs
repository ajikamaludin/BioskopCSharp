using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BioskopCSharp.SetupDBS;
using BioskopCSharp.Models;
using BioskopCSharp.Views.TiketView;

namespace BioskopCSharp.Controllers
{
    class CTiket
    {
        //Class
        private TiketView _view;
        private Command_SQLite _sql;

        private static CTiket _ctrl;

        //Variabel
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CTiket()
        {
            _sql = new Command_SQLite();
            _view = new TiketView();
        }

        public static CTiket GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CTiket();
                }
                return _ctrl;
            }
        }
        public void Index()
        {
            _view.Show();
            //_table = Read();
        }
    }
}
