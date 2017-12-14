using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BioskopCSharp.Models;
using BioskopCSharp.SetupRDBMS;
using BioskopCSharp.Views.RuangView;

namespace BioskopCSharp.Controllers
{
    class cRuang
    {
        //Class
        private RuangView _view;
        private Command _sql;

        private static cRuang _ctrl;

        //Variable
        private string[] _column;

        private static DataTable _table;

        //Fungsi
        public string Code { get; set; }

        public static cRuang GetInstance
        {
            get
            {
                if (_ctrl == null) _ctrl = new cRuang();
                return _ctrl;
            }
        }


    }
}
