using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DentistryManager.Models;
using System.Data.SqlClient;
using System.Data;
using DentistryManager.Common;
namespace DentistryManager.Repository
{
    public class PatientsRepository : IRepository<Patients>
    {
        public Patients GetById(string id)
        {
            Patients obj = new Patients();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GePatientByid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    obj.id = SafeConvert.ToString(reader["id"]);
                    obj.name = SafeConvert.ToString(reader["name"]);
                    obj.birthday = SafeConvert.ToDateTime(SafeConvert.ToString(reader["birthday"]));
                    obj.address = SafeConvert.ToString(reader["address"]);
                    obj.image = SafeConvert.ToString(reader["image"]);
                    obj.gender = SafeConvert.ToBoolean(reader["gender"]);
                    obj.telephone = SafeConvert.ToString(reader["telephone"]);
                    obj.age = SafeConvert.ToInt(reader["age"]);
                    obj.email = SafeConvert.ToString(reader["email"]);
                    obj.metadata = SafeConvert.ToString(reader["metadata"]);
                    obj.status = SafeConvert.ToString(reader["status"]);
                    obj.statusinaday = SafeConvert.ToString(reader["statusinaday"]);
                    obj.active = SafeConvert.ToBoolean(reader["active"]);
                };
                con.Close();
                return obj;
            }

        }

        public IEnumerable<Patients> List()
        {
            List<Patients> lstPatients = new List<Patients>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllPatients", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Patients obj = new Patients();
                        obj.id = SafeConvert.ToString(reader["id"]);
                        obj.name = SafeConvert.ToString(reader["name"]);
                        obj.birthday = SafeConvert.ToDateTime(SafeConvert.ToString(reader["birthday"]));
                        obj.address = SafeConvert.ToString(reader["address"]);
                        obj.image = SafeConvert.ToString(reader["image"]);
                        obj.gender = SafeConvert.ToBoolean(reader["gender"]);
                        obj.telephone = SafeConvert.ToString(reader["telephone"]);
                        obj.age = SafeConvert.ToInt(reader["age"]);
                        obj.email = SafeConvert.ToString(reader["email"]);
                        obj.metadata = SafeConvert.ToString(reader["metadata"]);
                        obj.status = SafeConvert.ToString(reader["status"]);
                        obj.statusinaday = SafeConvert.ToString(reader["statusinaday"]);
                        obj.active = SafeConvert.ToBoolean(reader["active"]);
                        lstPatients.Add(obj);
                    }
                    return lstPatients.AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return null;
            }
        }

        public IEnumerable<Patients> List(Func<Patients, bool> predicate)
        {
            List<Patients> lstPatients = new List<Patients>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllPatients", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Patients obj = new Patients();
                        obj.id = SafeConvert.ToString(reader["id"]);
                        obj.name = SafeConvert.ToString(reader["name"]);
                        obj.birthday = SafeConvert.ToDateTime(SafeConvert.ToString(reader["birthday"]));
                        obj.address = SafeConvert.ToString(reader["address"]);
                        obj.image = SafeConvert.ToString(reader["image"]);
                        obj.gender = SafeConvert.ToBoolean(reader["gender"]);
                        obj.telephone = SafeConvert.ToString(reader["telephone"]);
                        obj.age = SafeConvert.ToInt(reader["age"]);
                        obj.email = SafeConvert.ToString(reader["email"]);
                        obj.metadata = SafeConvert.ToString(reader["metadata"]);
                        obj.status = SafeConvert.ToString(reader["status"]);
                        obj.statusinaday = SafeConvert.ToString(reader["statusinaday"]);
                        obj.active = SafeConvert.ToBoolean(reader["active"]);
                        lstPatients.Add(obj);
                    }
                    return lstPatients.AsEnumerable().Where(predicate);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return null;
            }
        }

        public int Add(Patients entity)
        {
            try
            {
                int i;
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd_seq = new SqlCommand("GetNextPatientsSeq", con);
                    string seq = SafeConvert.ToString(cmd_seq.ExecuteScalar());
                    string id = "BN" + DateTime.Now.ToString("ddMMyyyy") + seq;

                    SqlCommand cmd = new SqlCommand(Const.FSP_PATIENT_INSERT, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", entity.name);

                    cmd.Parameters.AddWithValue("@birthday", entity.birthday);

                    if (entity.address.IsNotNull())
                        cmd.Parameters.AddWithValue("@address", entity.address);
                    else
                        cmd.Parameters.AddWithValue("@address", DBNull.Value);

                    if (entity.image.IsNotNull())
                        cmd.Parameters.AddWithValue("@image", entity.image);
                    else
                        cmd.Parameters.AddWithValue("@image", DBNull.Value);

                    cmd.Parameters.AddWithValue("@gender", entity.gender);

                    if (entity.telephone.IsNotNull())
                        cmd.Parameters.AddWithValue("@telephone", entity.telephone);
                    else
                        cmd.Parameters.AddWithValue("@telephone", DBNull.Value);

                    cmd.Parameters.AddWithValue("@age", entity.age);

                    if (entity.email.IsNotNull())
                        cmd.Parameters.AddWithValue("@email", entity.email);
                    else
                        cmd.Parameters.AddWithValue("@email", DBNull.Value);

                    if (entity.metadata.IsNotNull())
                        cmd.Parameters.AddWithValue("@metadata", entity.metadata);
                    else
                        cmd.Parameters.AddWithValue("@metadata", DBNull.Value);

                    if (entity.status.IsNotNull())
                        cmd.Parameters.AddWithValue("@status", entity.status);
                    else
                        cmd.Parameters.AddWithValue("@status", DBNull.Value);

                    if (entity.statusinaday.IsNotNull())
                        cmd.Parameters.AddWithValue("@statusinaday", entity.statusinaday);
                    else
                        cmd.Parameters.AddWithValue("@statusinaday", DBNull.Value);

                    cmd.Parameters.AddWithValue("@active", entity.active);

                    i = cmd.ExecuteNonQuery();
                }
                return i;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return -1;
            }
        }

        public int Delete(Patients entity)
        {
            int i = SqlHelper.ExecuteNonQuery(Const.Connectring, Const.FSP_PATIENT_DELETE, entity.id);
            return i;
        }

        public int Edit(Patients entity)
        {
            int i = SqlHelper.ExecuteNonQuery(Const.Connectring, Const.FSP_PATIENT_DELETE, entity);
            return i;
        }
    }
}