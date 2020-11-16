using System.Collections.Generic;
using Repository.Enums;

namespace Repository
{
    public interface ISeekable
    {
        T GetSingel<T>(string Select, Table table, IDictionary<string, string> Where = null);
        IEnumerable<T> GetMenyEntitys<T>(int top, string Select, Table table, IDictionary<string, string> Where = null, string orderColumBy = "");
    }
}
