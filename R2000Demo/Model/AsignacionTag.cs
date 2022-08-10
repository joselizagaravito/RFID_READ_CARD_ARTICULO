using System;

namespace R2000Demo.Model
{
    public class AsignacionTag
    {
        public int UsuarioId { get; set; }
        public String Epc { get; set; }
        public String Tipo { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Idlectura { get; set; }
    }
}
