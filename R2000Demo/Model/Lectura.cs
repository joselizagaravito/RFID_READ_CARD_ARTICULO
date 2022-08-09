using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R2000Demo.Model
{

    //Actualizacion - Jose Liza: 20210208
    public class Lectura
    {
        public Lectura(int tag, string ePC, string tID, int invTimes, int rSSI, int aNTID, DateTime lastTime, DateTime first_Read_time_Cost, string color)
        {
            Tag = tag;
            EPC = ePC;
            TID = tID;
            InvTimes = invTimes;
            RSSI = rSSI;
            ANTID = aNTID;
            LastTime = lastTime;
            First_Read_time_Cost = first_Read_time_Cost;
            Color = color;
        }
        public Lectura()
        {

        }
        public int Tag { get; set; }
        public string EPC { get; set; }
        public string TID { get; set; }
        public int InvTimes { get; set; }
        public int RSSI { get; set; }
        public int ANTID { get; set; }
        public DateTime LastTime { get; set; }
        public DateTime First_Read_time_Cost { get; set; }
        public string Color { get; set; }

    }
}
