using Repository.Enums;
using System.Collections.Generic;

namespace Repository
{
    public interface IModified
    {
        /// <summary>
        /// Add Entety in DB
        /// </summary>
        /// <returns>
        /// If It is ok (it chrack if not)
        /// </returns>
        /// <param name="entety"> the entety you want to add to db</param>
        /// <param name="table"> the table that you want to add in</param>
        /// <exception cref="System.Exception"></exception>
        bool Add<IdType>(dynamic entety, Table table, string pKName, List<string> ignore = null);
        bool Update<T>(dynamic entety, Table table, string pKName, dynamic pKvalue, List<string> ignore = null);
        bool Delete(Table table, string pKName, dynamic pKvalue);
    }
}
