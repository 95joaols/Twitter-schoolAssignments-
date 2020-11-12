using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;

namespace Repository
{
    internal class SQLAccess : IFullControl
    {
        private string connectionString;
        public SQLAccess(string connectionString)
        {
            this.connectionString = connectionString;
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
