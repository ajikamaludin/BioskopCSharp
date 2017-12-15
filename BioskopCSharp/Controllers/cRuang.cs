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
    class CRuang
    {
        //Class
        private RuangView _view;
        private Command _sql;

        private static CRuang _ctrl;

        //Variable
        private string[] _column;

        private static DataTable _table;

        //Fungsi
        public string Code { get; set; }

        public static CRuang GetInstance
        {
            get
            {
                if (_ctrl == null) _ctrl = new CRuang();
                return _ctrl;
            }
        }


    }
}
