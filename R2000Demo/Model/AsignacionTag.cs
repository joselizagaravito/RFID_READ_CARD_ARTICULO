using System;

namespace R2000Demo.Model
{
    public class AsignacionTag
    {
        public int UsuarioId { get; set; }
        public String Epc { get; set; }
        public String Tipo { get; set; }
        public String Color { get; set; }
        public int Modulo { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Idlectura { get; set; }

        public AsignacionTag()
        {
        }

        public AsignacionTag(int usuarioId, string epc, string tipo,string color,int modulo, DateTime fechaAsignacion, DateTime fechaSalida, int idlectura)
        {
            UsuarioId = usuarioId;
            Epc = epc;
            Tipo = tipo;
            Color = color;
            Modulo = modulo;
            FechaAsignacion = fechaAsignacion;
            FechaSalida = fechaSalida;
            Idlectura = idlectura;
        }
    }
}
