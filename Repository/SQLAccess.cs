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
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetMenySingelTabel<T>(string Selekt, Table table, IDictionary<string, string> Where)
        {
            throw new NotImplementedException();
        }

        public T GetSingel<T>(string Selekt, Table table, IDictionary<string, string> Where)
        {
            throw new NotImplementedException();
        }
    }
}
