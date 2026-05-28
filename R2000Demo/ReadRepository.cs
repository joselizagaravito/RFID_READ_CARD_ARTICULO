using R2000Demo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace R2000Demo
{
    /// <summary>
    /// Repositorio de acceso a datos para ReadTag y AsignacionTag.
    /// Todas las conexiones se abren y cierran dentro de bloques using.
    /// Sprint 7 / T-C#: agrega soporte offline — GetPendientesHttp y MarcarEnviadoHttp.
    /// </summary>
    public class ReadRepository
    {
        private static readonly string _connStr =
            ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        // ── Lectura ─────────────────────────────────────────────────────────

        public List<ReadTag> GetAllReadTags()
        {
            var result = new List<ReadTag>();
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand("usp_GetReadTags", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    using (var dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                            result.Add(MapReadTag(dr));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("GetAllReadTags: " + ex.Message, ex);
                }
            }
            return result;
        }

        // ── Escritura ────────────────────────────────────────────────────────

        /// <summary>
        /// Inserta un ReadTag en la BD local y retorna su AsignacionTag asociada.
        /// El SP AddNewReadTag debe retornar (Id INT, EPC VARCHAR).
        /// La columna EnviadoHttp se inicializa en 0 por DEFAULT en la tabla.
        /// </summary>
        public AsignacionTag AddReadTag(ReadTag obj)
        {
            var result = new AsignacionTag();
            using (var con = new SqlConnection(_connStr))
            {
                try
                {
                    // 1. Insertar y obtener Id + Epc del registro nuevo
                    con.Open();
                    using (var com = new SqlCommand("AddNewReadTag", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        AgregarParametrosReadTag(com, obj);

                        using (var dr = com.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                result.Idlectura = Convert.ToInt32(dr.GetValue(0));
                                result.Epc = dr.IsDBNull(1) ? string.Empty : dr.GetString(1);
                            }
                        }
                    }

                    // 2. Buscar AsignacionTag del EPC recién insertado
                    using (var com2 = new SqlCommand("usp_BuscarTag", con))
                    {
                        com2.CommandType = CommandType.StoredProcedure;
                        com2.Parameters.AddWithValue("@EPC", result.Epc);

                        using (var dr2 = com2.ExecuteReader())
                        {
                            if (dr2.Read())
                            {
                                result.UsuarioId       = dr2.GetInt32(0);
                                result.Tipo            = dr2.GetString(2);
                                result.Color           = dr2.GetString(3);
                                result.Modulo          = dr2.GetInt32(4);
                                result.FechaAsignacion = dr2.GetDateTime(5);
                                result.FechaSalida     = dr2.GetDateTime(6);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("AddReadTag: " + ex.Message, ex);
                }
            }
            return result;
        }

        public int AddIncidenciaReadTag(ReadTag obj)
        {
            int result = 0;
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand("AddNewIncidenciaReadTag", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                AgregarParametrosReadTag(com, obj);
                com.Parameters.AddWithValue("@FechaHoraLocal", DateTime.Now);
                try
                {
                    con.Open();
                    using (var dr = com.ExecuteReader())
                    {
                        if (dr.Read()) result = dr.GetInt32(0);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("AddIncidenciaReadTag: " + ex.Message, ex);
                }
            }
            return result;
        }

        // ── Consultas ────────────────────────────────────────────────────────

        public bool GetReadInBox(int id)
        {
            return EjecutarGetReadInBox("GetReadInBox", "@Id", id);
        }

        public bool GetReadInBox(string epc)
        {
            return EjecutarGetReadInBox("GetEPCReadInBox", "@epc", epc);
        }

        public List<AsignacionTag> GetTagsAsignados(int usuarioId)
        {
            var result = new List<AsignacionTag>();
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand("usp_TagDelUsuario", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UsuarioId", usuarioId);
                try
                {
                    con.Open();
                    using (var dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                            result.Add(MapAsignacionTag(dr));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("GetTagsAsignados: " + ex.Message, ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Determina el tipo de tag según esta prioridad:
        /// 1. Si EPC está en pallet_tags → PALLET
        /// 2. Si TID contiene el keyword configurado → PALLET
        /// 3. En cualquier otro caso → LPN
        /// </summary>
        public string ObtenerTipoTag(string epc, string tid = "")
        {
            using (var con = new SqlConnection(_connStr))
            {
                try
                {
                    con.Open();

                    // 1. Verificar tabla pallet_tags
                    System.Diagnostics.Debug.WriteLine($"[TIPO] EPC recibido: '{epc}' | sin guiones: '{epc.Replace("-", "")}'");
                    using (var com = new SqlCommand(
                        "SELECT COUNT(1) FROM pallet_tags WHERE EPC = @EPC", con))
                    {
                        com.Parameters.AddWithValue("@EPC", epc.Replace("-", ""));
                        int count = Convert.ToInt32(com.ExecuteScalar());
                        if (count > 0) return "PALLET";
                    }

                    // 2. Verificar TID con keyword de configuración
                    string keyword = ConfigurationManager.AppSettings["PalletTidKeyword"] ?? "PALLET";
                    if (!string.IsNullOrEmpty(tid) &&
                        tid.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "PALLET";
                    }

                    // 3. Default
                    return "LPN";
                }
                catch (Exception ex)
                {
                    throw new Exception("ObtenerTipoTag: " + ex.Message, ex);
                }
            }
        }
        // ── Actualizaciones ─────────────────────────────────────────────────

        public void UpdateReadTag(string epc, string ant, string color)
        {
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand("UpdateReadTagColor", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EPC",   epc);
                com.Parameters.AddWithValue("@Color", color);
                com.Parameters.AddWithValue("@AntId", ant);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("UpdateReadTag: " + ex.Message, ex);
                }
            }
        }

        public void UpdateAsignacionTag(string epc, DateTime fechaSalida, string color, int modulo)
        {
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand("UpdateAsignacionTagColor", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EPC",        epc);
                com.Parameters.AddWithValue("@FechaSalida", fechaSalida);
                com.Parameters.AddWithValue("@Color",      color);
                com.Parameters.AddWithValue("@Modulo",     modulo);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("UpdateAsignacionTag: " + ex.Message, ex);
                }
            }
        }

        // ── Sprint 7 / T-C# — Offline Sync ──────────────────────────────────

        /// <summary>
        /// Retorna los ReadTag pendientes de envío HTTP (EnviadoHttp = 0).
        /// Ordena por FirstReadTime ASC para respetar el orden de captura.
        /// Requiere columna: ALTER TABLE ReadTag ADD EnviadoHttp BIT NOT NULL DEFAULT 0
        /// </summary>
        public List<ReadTag> GetPendientesHttp()
        {
            var result = new List<ReadTag>();
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand(
    "SELECT Idlectura, TAG, EPC, TID, RSSI, AntID, " +
    "FirstReadTime, LastTime, Color, ModuloId, ModuloRol " +
    "FROM read_tags WHERE EnviadoHttp = 0 " +
    "ORDER BY CreatedAt ASC", con))
            {
                try
                {
                    con.Open();
                    using (var dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tag = new ReadTag(
                                tag: dr.IsDBNull(1) ? "" : dr.GetString(1),
                                epc: dr.IsDBNull(2) ? "" : dr.GetString(2),
                                tid: dr.IsDBNull(3) ? "" : dr.GetString(3),
                                invtimes: 0,
                                rssi: dr.IsDBNull(4) ? 0 : dr.GetInt32(4),
                                antid: dr.IsDBNull(5) ? 0 : dr.GetInt32(5),
                                lasttime: dr.IsDBNull(7) ? DateTime.Now : dr.GetDateTime(7),
                                firstreadtime: dr.IsDBNull(6) ? DateTime.Now : dr.GetDateTime(6),
                                color: dr.IsDBNull(8) ? "" : dr.GetString(8),
                                moduloid: dr.IsDBNull(9) ? 0 : (int.TryParse(dr.GetString(9), out int mid) ? mid : 0),
                                modulorol: dr.IsDBNull(10) ? "" : dr.GetString(10));
                            tag.Id = dr.GetInt32(0);
                            result.Add(tag);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("GetPendientesHttp: " + ex.Message, ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Marca un ReadTag como enviado al servidor HTTP.
        /// Solo actualiza el flag EnviadoHttp — no modifica ningún otro dato.
        /// </summary>
        public void MarcarEnviadoHttp(int id)
        {
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand(
                "UPDATE read_tags SET EnviadoHttp = 1 WHERE Idlectura = @Id", con))
            {
                com.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("MarcarEnviadoHttp id=" + id + ": " + ex.Message, ex);
                }
            }
        }

        // ── Helpers privados ─────────────────────────────────────────────────

        private static void AgregarParametrosReadTag(SqlCommand com, ReadTag obj)
        {
            com.Parameters.AddWithValue("@TAG",           obj.TAG);
            com.Parameters.AddWithValue("@EPC",           obj.EPC);
            com.Parameters.AddWithValue("@TID",           obj.TID);
            com.Parameters.AddWithValue("@InvTimes",      obj.InvTimes);
            com.Parameters.AddWithValue("@RSSI",          obj.RSSI);
            com.Parameters.AddWithValue("@AntID",         obj.AntID);
            com.Parameters.AddWithValue("@LastTime",      obj.LastTime);
            com.Parameters.AddWithValue("@FirstReadTime", obj.FirstReadTime);
            com.Parameters.AddWithValue("@Color",         obj.Color);
            com.Parameters.AddWithValue("@ModuloId",      obj.ModuloId);
            com.Parameters.AddWithValue("@ModuloRol",     obj.ModuloRol);
        }

        /// <summary>
        /// Mapea una fila del DataReader a ReadTag.
        /// columnOffset=0: SELECT sin Id al frente (usp_GetReadTags).
        /// columnOffset=1: SELECT con Id en columna 0 (GetPendientesHttp).
        /// </summary>
        private static ReadTag MapReadTag(SqlDataReader dr, int columnOffset = 0)
        {
            int o = columnOffset;
            var tag = new ReadTag(
                tag:           dr.GetString(o),
                epc:           dr.GetString(o + 1),
                tid:           dr.GetString(o + 2),
                invtimes:      dr.GetInt32(o + 3),
                rssi:          dr.GetInt32(o + 4),
                antid:         dr.GetInt32(o + 5),
                lasttime:      dr.GetDateTime(o + 6),
                firstreadtime: dr.GetDateTime(o + 7),
                color:         dr.GetString(o + 8),
                moduloid:      dr.GetInt32(o + 9),
                modulorol:     dr.GetString(o + 10));
            return tag;
        }

        private static AsignacionTag MapAsignacionTag(SqlDataReader dr)
        {
            return new AsignacionTag(
                usuarioId:      dr.GetInt32(0),
                epc:            dr.GetString(1),
                tipo:           dr.GetString(2),
                color:          dr.GetString(3),
                modulo:         dr.GetInt32(4),
                fechaAsignacion: dr.GetDateTime(5),
                fechaSalida:    dr.GetDateTime(6),
                idlectura:      0);
        }

        private bool EjecutarGetReadInBox(string spName, string paramName, object value)
        {
            bool result = false;
            using (var con = new SqlConnection(_connStr))
            using (var com = new SqlCommand(spName, con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue(paramName, value);
                try
                {
                    con.Open();
                    using (var dr = com.ExecuteReader())
                    {
                        if (dr.Read()) result = dr.GetString(0) == "True";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(spName + ": " + ex.Message, ex);
                }
            }
            return result;
        }
    }
}
