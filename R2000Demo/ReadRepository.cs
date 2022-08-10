using R2000Demo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace R2000Demo
{
    public class ReadRepository
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();
            con = new SqlConnection(constr);

        }
        //To Add ReadTag details    
        public AsignacionTag AddReadTag(ReadTag obj)
        {
            AsignacionTag result = new AsignacionTag();
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("AddNewReadTag", con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@TAG", obj.TAG);
                com.Parameters.AddWithValue("@EPC", obj.EPC);
                com.Parameters.AddWithValue("@TID", obj.TID);
                com.Parameters.AddWithValue("@InvTimes", obj.InvTimes);
                com.Parameters.AddWithValue("@RSSI", obj.RSSI);
                com.Parameters.AddWithValue("@AntID", obj.AntID);
                com.Parameters.AddWithValue("@LastTime", obj.LastTime);
                com.Parameters.AddWithValue("@FirstReadTime", obj.FirstReadTime);
                com.Parameters.AddWithValue("@Color", obj.Color);
                com.Parameters.AddWithValue("@ModuloId", obj.ModuloId);
                com.Parameters.AddWithValue("@ModuloRol", obj.ModuloRol);
                
                try
                {
                    con.Open();
                    SqlDataReader dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        result.Idlectura = dr.GetInt32(0);
                        result.Epc = dr.GetString(1);
                    }
                    con.Close();

                    com = new SqlCommand("usp_BuscarTag", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TAG", result.Epc);
                    con.Open();
                    SqlDataReader dr2 = com.ExecuteReader();
                    if (dr.Read())
                    {
                        result.UsuarioId = dr.GetInt32(0);
                        result.Tipo = dr.GetString(2);
                        result.FechaAsignacion = dr.GetDateTime(3);
                        result.FechaSalida = dr.GetDateTime(4);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Hay un error en la Base de Datos" + ex.Message);
                }
                return result;
            }
        }

        //AddIncidenciaReadTag
        public int AddIncidenciaReadTag(ReadTag obj)
        {
            int result = 0;
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("AddNewIncidenciaReadTag", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@TAG", obj.TAG);
                com.Parameters.AddWithValue("@EPC", obj.EPC);
                com.Parameters.AddWithValue("@TID", obj.TID);
                com.Parameters.AddWithValue("@InvTimes", obj.InvTimes);
                com.Parameters.AddWithValue("@RSSI", obj.RSSI);
                com.Parameters.AddWithValue("@AntID", obj.AntID);
                com.Parameters.AddWithValue("@LastTime", obj.LastTime);
                com.Parameters.AddWithValue("@FirstReadTime", obj.FirstReadTime);
                com.Parameters.AddWithValue("@Color", obj.Color);
                com.Parameters.AddWithValue("@ModuloId", obj.ModuloId);
                com.Parameters.AddWithValue("@ModuloRol", obj.ModuloRol);
                com.Parameters.AddWithValue("@FechaHoraLocal", DateTime.Now);
                try
                {
                    con.Open();
                    SqlDataReader dr = com.ExecuteReader();
                    if (dr.Read())
                        result = dr.GetInt32(0);
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Hay un error en la Base de Datos" + ex.Message);
                }
                return result;
            }
        }
        //Verificar lectura en Caja
        public bool GetReadInBox(int id)
        {
            bool Result = false;
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("GetReadInBox", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@Id", id));
                try
                {
                    //string s;
                    con.Open();
                    SqlDataReader dr = com.ExecuteReader();
                    if (dr.Read())
                        Result = dr.GetString(0) == "True";
                    dr.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Hay un error en la Base de Datos" + ex.Message);
                }
            }
            return Result;
        }
        public bool GetReadInBox(string epc)
        {
            bool Result = false;
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("GetEPCReadInBox", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@epc", epc));
                try
                {
                    //string s;
                    con.Open();
                    SqlDataReader dr = com.ExecuteReader();
                    if (dr.Read())
                        Result = dr.GetString(0) == "True";
                    dr.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Hay un error en la Base de Datos" + ex.Message);
                }
            }
            return Result;
        }
        //To view ReadTag details with generic list     
        public List<ReadTag> GetAllReadTags()
        {
            List<ReadTag> Result = new List<ReadTag>();
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("GetReadTags", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr;
                DataTable dt = new DataTable();
                try
                {
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        Result.Add(
                            item: new ReadTag
                            (
                                id: dr.GetInt32(0),
                                epc: dr.GetString(1),
                                tid: dr.GetString(2),
                                invtimes: dr.GetInt32(3),
                                rssi: dr.GetInt32(4),
                                antid: dr.GetInt32(5),
                                lasttime: dr.GetDateTime(6),
                                firstreadtime: dr.GetDateTime(7),
                                color: dr.GetString(8),
                                moduloid: dr.GetString(9),
                                modulorol: dr.GetString(10)
                            )
                            );
                    }
                    dr.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Hay un error en la Base de Datos" + ex.Message);
                }
            }
            return Result;
        }

    }
}
