using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Repository
{
    internal class SQLAccess : IFullControl
    {
        private string connectionString;
        public SQLAccess(string ConnectionStringName)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            connectionString = config.GetConnectionString(ConnectionStringName);
        }

        public bool Add<IdType>(dynamic entety, Table table, string pKName)
        {
            string prop = "";
            string value = "";
            foreach (PropertyInfo e in entety.GetType().GetProperties())
            {
                if (e.Name != pKName)
                {
                    if (prop != "")
                    {
                        prop += ",";
                        value += ",";
                    }

                    prop += e.Name;
                    value += "@" + e.Name;

                }
            }

            string sql = $"INSERT INTO {table} ({prop})" +
                "OUTPUT Inserted.ID" +
                $" VALUES({value});";
            IdType id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                id = connection.QuerySingle<IdType>(sql, (object)entety);
            }
            PropertyInfo propertyInfo = entety.GetType().GetProperty(pKName);
            propertyInfo.SetValue(entety, Convert.ChangeType((IdType)id, propertyInfo.PropertyType), null);

            return true;
        }

        public IEnumerable<T> GetMenySingelTabel<T>(string Selekt, Table table, IDictionary<string, string> Where)
        {
            throw new NotImplementedException();
        }

        public T GetSingel<T>(string Selekt, Table table, IDictionary<string, string> Where)
        {
            T entety;
            string sWhere = "where ";
            foreach (KeyValuePair<string,string> kvp in Where)
            {
                if(sWhere == "where ")
                {
                    sWhere += $"{kvp.Key} = '{kvp.Value.Replace("'","").Replace("\"","")}'";
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //get data from sql
                entety = connection.Query<T>($"SELECT * FROM {table} {sWhere}").FirstOrDefault();
            }
            return entety;
        }
    }
}
