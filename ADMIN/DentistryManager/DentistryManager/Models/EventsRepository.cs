using DentistryManager.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DentistryManager.Common;
using System.Linq.Expressions;

namespace DentistryManager.Models
{
    public class EventsRepository : IRepository<Events>
    {

        public Events GetById(string id)
        {
            Events obj = new Events();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GeProductByCategoryId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    obj.id = reader["id"].ToString();
                    obj.title = reader["title"].ToString();
                    obj.start = reader["start"].ToString();
                    obj.end = reader["end"].ToString();
                    obj.url = reader["url"].ToString();
                    obj.allDay = Boolean.Parse(reader["Value"].ToString());
                };
                con.Close();
                return obj;
            }
        }

        public IEnumerable<Events> List()
        {
            List<Events> lstEvents = new List<Events>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllEvents", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Events e = new Events();
                        e.id = reader["id"].ToString();
                        e.title = reader["title"].ToString();
                        e.start = reader["start"].ToString();
                        e.end = reader["end"].ToString();
                        e.url = reader["url"].ToString();
                        if (reader["allDay"] != DBNull.Value)
                        {
                            e.allDay = (bool)reader["allDay"];
                        }
                        lstEvents.Add(e);
                    }
                    return lstEvents.AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public IEnumerable<Events> List(Func<Events, bool> predicate)
        {
            List<Events> lstEvents = new List<Events>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllProduct", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lstEvents.Add(new Events()
                        {
                            id = reader["id"].ToString(),
                            title = reader["title"].ToString(),
                            start = reader["start"].ToString(),
                            end = reader["end"].ToString(),
                            url = reader["url"].ToString(),
                            allDay = Boolean.Parse(reader["Value"].ToString())
                        });
                    }
                    return lstEvents.Where(predicate);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public int Add(Events entity)
        {
            int res = SqlHelper.ExecuteNonQuery(Const.Connectring, "InsertEvents", entity);
            return res;
            throw new NotImplementedException();
        }

        public int Delete(Events entity)
        {
            throw new NotImplementedException();
        }

        public int Edit(Events entity)
        {

            throw new NotImplementedException();
        }
    }
}