using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace R2000Demo.Repository
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
        public void AddReadTag(ReadTag obj)
        {
            string constr = ConfigurationManager.ConnectionStrings["cnn"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("AddNewReadTag", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EPC", obj.EPC);
                com.Parameters.AddWithValue("@TID", obj.TID);
                com.Parameters.AddWithValue("@InvTimes", obj.InvTimes);
                com.Parameters.AddWithValue("@RSSI", obj.RSSI);
                com.Parameters.AddWithValue("@AntID", obj.AntID);
                com.Parameters.AddWithValue("@LastTime", obj.LastTime);
                com.Parameters.AddWithValue("@FirstReadTime", obj.FirstReadTime);
                //com.Parameters.AddWithValue("@Color", obj.Color);

                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Hay un error en la Base de Datos" + ex.Message);
                }
            }
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
                            new ReadTag(
                                 dr.GetInt32(0),
                               dr.GetString(1),
                                dr.GetString(2),
                                 dr.GetInt32(3),
                              dr.GetInt32(4),
                               dr.GetInt32(5),
                              dr.GetDateTime(6),
                            dr.GetDateTime(7))//,
                             //dr.GetString(8))
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
