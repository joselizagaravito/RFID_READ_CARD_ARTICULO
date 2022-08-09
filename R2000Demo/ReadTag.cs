using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R2000Demo
{
    public class ReadTag
    {
        public int Id { get; set; }

        public int TAG { get; set; }
        public string EPC { get; set; }

        public string TID { get; set; }

        public int InvTimes { get; set; }
        public int RSSI { get; set; }
        public int AntID { get; set; }
        public DateTime LastTime { get; set; }
        public DateTime FirstReadTime { get; set; }
        public string Color { get; set; }
        public string ModuloId { get; set; }
        public string  ModuloRol { get; set; }
        public ReadTag(int id, string epc, string tid, int invtimes, int rssi, int antid, DateTime lasttime, DateTime firstreadtime, string color, string moduloid, string modulorol)
        {
            Id = id;
            EPC = epc;
            TID = tid;
            InvTimes = invtimes;
            rssi = RSSI;
            AntID = antid;
            LastTime = lasttime;
            FirstReadTime = firstreadtime;
            Color = color;
            ModuloId = moduloid;
            ModuloRol = modulorol;
        }

    }
}
