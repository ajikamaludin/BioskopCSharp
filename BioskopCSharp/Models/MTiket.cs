﻿namespace BioskopCSharp.Models
{
    public class MTiket
    {
        public int IdTiket { get; set; }
        public MJadwal Jadwal { get; set; }
        public string Kursi { get; set; }
        public string TglTiket { get; set; }

        public MTiket()
        {
            Jadwal = new MJadwal();
        }
    }
}
