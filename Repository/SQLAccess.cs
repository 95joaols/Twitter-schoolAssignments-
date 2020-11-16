using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repository.Enums;

namespace Repository
{
    internal class SQLAccess : IFullControl
    {
        private readonly string connectionString;
        public SQLAccess(string ConnectionStringName)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            connectionString = config.GetConnectionString(ConnectionStringName);
        }

        public bool Add<IdType>(dynamic entety, Table table, string pKName, List<string> ignore = null)
        {
            string prop = "";
            string value = "";
            foreach (PropertyInfo e in entety.GetType().GetProperties())
            {


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

        public IEnumerable<T> GetMenyEntitys<T>(int top, string Select, Table table, IDictionary<string, string> Where = null)
        {
            if (string.IsNullOrWhiteSpace(Select))
            {
                throw new ArgumentException($"'{nameof(Select)}' cannot be null or whitespace", nameof(Select));
            }

            IEnumerable<T> entety;
            String sSelect = "Select ";
            if (top > 0)
            {
                sSelect += $"TOP({top}) ";
            }
            sSelect += Select;

            string sWhere = "";
            if (Where != null)
            {
                foreach (KeyValuePair<string, string> kvp in Where)
                {
                    if (sWhere == "")
                    {
                        sWhere += $"where {kvp.Key} = '{kvp.Value.Replace("'", "").Replace("\"", "")}'";
                    }
                    else
                    {
                        sWhere += $", {kvp.Key} = '{kvp.Value.Replace("'", "").Replace("\"", "")}'";
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //get data from sql
                entety = connection.Query<T>($"{sSelect} FROM {CodeTools.GetEnumDescription(table)} {sWhere}");
            }
            return entety;
        }

        public T GetSingel<T>(string Select, Table table, IDictionary<string, string> Where = null)
        {
            if (string.IsNullOrWhiteSpace(Select))
            {
                throw new ArgumentException($"'{nameof(Select)}' cannot be null or whitespace", nameof(Select));
            }

            T entety;
            string sWhere = "";
            foreach (KeyValuePair<string, string> kvp in Where)
            {
                if (sWhere == "")
                {
                    sWhere += $"where {kvp.Key} = '{kvp.Value.Replace("'", "").Replace("\"", "")}'";
                }
                else
                {
                    sWhere += $", {kvp.Key} = '{kvp.Value.Replace("'", "").Replace("\"", "")}'";
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //get data from sql
                entety = connection.Query<T>($"SELECT TOP(1) {Select} FROM {CodeTools.GetEnumDescription(table)} {sWhere}").FirstOrDefault();
            }
            return entety;
        }

        public bool Update<T>(dynamic entety, Table table, string pKName, dynamic pKvalue, List<string> ignore = null)
        {
            string SET = "";
            foreach (PropertyInfo e in entety.GetType().GetProperties())
            {
                if (e.Name != pKName && !ignore.Contains(e.Name))
                {
                    if (SET != "")
                    {
                        SET += ",";
                    }

                    // object o = e.GetValue(entety);
                    // if (IsNumericType(o))
                    // {
                    //     SET += e.Name + "=" + o;
                    // }
                    // else
                    // {
                    SET += e.Name + $"='{e.GetValue(entety)}'";
                    // }
                }
            }

            string sql = $"UPDATE {CodeTools.GetEnumDescription(table)} SET {SET}" +
                $" WHERE {pKName}={pKvalue};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Query(sql);
            }

            return true;
        }
        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
