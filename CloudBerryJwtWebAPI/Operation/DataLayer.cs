using CloudBerryJwtWebAPI.Model;
using Pomelo.Data.MySql;
using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
namespace CloudBerryJwtWebAPI.Operation
{
    public partial class DataLayer : IDataLayer
    {
        private IConfiguration _configuration;
        public DataLayer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Connection
        {
            get
            {

                return _configuration["ConnectionStrings:DefualtConnection"];
            }
        }

        public List<AdminModel> AdminList()
        {
            List<AdminModel> allAdmin = new List<AdminModel>();

            using (MySqlConnection connMysql = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmdMysql = connMysql.CreateCommand())
                {
                    cmdMysql.CommandText = AdminListQuery();
                    cmdMysql.CommandType = System.Data.CommandType.Text;

                    cmdMysql.Connection = connMysql;
                    connMysql.Open();

                    using (MySqlDataReader reader = cmdMysql.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allAdmin.Add(new AdminModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password"))

                            });
                        }
                    }
                }
                connMysql.Close();
            }

            return allAdmin;
        }

        public KeyValuePair<bool, AdminModel> CheckAdmin(AdminModel requestModel)
        {
            AdminModel model = null;
            using (MySqlConnection connMysql = new MySqlConnection(Connection))
            {
                using (MySqlCommand cmdMysql = connMysql.CreateCommand())
                {
                    cmdMysql.CommandText = AdminCheckQuery(requestModel);
                    cmdMysql.CommandType = System.Data.CommandType.Text;

                    cmdMysql.Connection = connMysql;
                    connMysql.Open();

                    using MySqlDataReader reader = cmdMysql.ExecuteReader();
                    while (reader.Read())
                    {
                        model = new AdminModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = reader.GetString(reader.GetOrdinal("Password"))

                        };

                    }
                }
                connMysql.Close();
            }

            return new KeyValuePair<bool, AdminModel>(model != null, model);
        }

        public KeyValuePair<bool, string> CreateUser(UserModel users)
        {
            try
            {
                using (MySqlConnection connMysql = new MySqlConnection(Connection))
                {
                    using (MySqlCommand cmdMysql = connMysql.CreateCommand())
                    {
                        var querytext = UserCreateQuery(users);
                        cmdMysql.CommandText = querytext;
                        cmdMysql.CommandType = System.Data.CommandType.Text;

                        cmdMysql.Connection = connMysql;
                        connMysql.Open();

                        var result = cmdMysql.ExecuteNonQuery();
                    }
                    connMysql.Close();
                }

                return new KeyValuePair<bool, string>(true, "İşlem Başarılı");
            }
            catch (Exception ex)
            {

                return new KeyValuePair<bool, string>(false, ex.Message);
            }
        }

        public KeyValuePair<bool, string> UpdateUser(UserModel users)
        {
            try
            {
                using (MySqlConnection connMysql = new MySqlConnection(Connection))
                {
                    using (MySqlCommand cmdMysql = connMysql.CreateCommand())
                    {
                        cmdMysql.CommandText = UserUpdateQuery(users);
                        cmdMysql.CommandType = System.Data.CommandType.Text;

                        cmdMysql.Connection = connMysql;
                        connMysql.Open();

                        var result = cmdMysql.ExecuteNonQuery();
                    }
                    connMysql.Close();
                }
                return new KeyValuePair<bool, string>(true, "İşlem Başarılı");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }

        }

        public KeyValuePair<bool, string> DeleteUser(int id)
        {
            try
            {
                using (MySqlConnection connMysql = new MySqlConnection(Connection))
                {
                    using (MySqlCommand cmdMysql = connMysql.CreateCommand())
                    {
                        cmdMysql.CommandText = UserDeleteQuery(id);
                        cmdMysql.CommandType = System.Data.CommandType.Text;

                        cmdMysql.Connection = connMysql;
                        connMysql.Open();

                        var result = cmdMysql.ExecuteNonQuery();
                    }
                    connMysql.Close();
                }
                return new KeyValuePair<bool, string>(true, "İşlem Başarılı");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }

        }

    }
}
