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

        private CRuang()
        {
            _sql = new Command();
            _view = new RuangView();
        }

        public static CRuang GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CRuang();
                }
                return _ctrl;
            }
        }

        private MRuang Entity(dynamic result)
        {
            var entity = new MRuang()
            {
                Id = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Nama = result[_column[1]].ToString() as string,
            };
            return entity;
        }


        public void Index()
        {
            Dispose();
            _view.Show();
            _table = Read();
        }

        public void Dispose()
        {
            _ctrl = null;
            _table = null;
        }

        public DataTable Read()
        {
            List<MRuang> list = null;
            _column = new[] { "id_ruang", "nama_ruang"};
            _sql.Query = "SELECT * FROM ruang";
            list = _sql.ExecuteQuery(Entity);
        
            var table = new DataTable();
            var header = new string[] { "NAMA"};
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.Nama as string;
                        table.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return table;
        }
    }
}
