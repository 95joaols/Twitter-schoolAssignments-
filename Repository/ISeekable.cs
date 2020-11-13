using System.Collections.Generic;
using Repository.Enums;

namespace Repository
{
    public interface ISeekable
    {
        T GetSingel<T>(string Selekt, Table table, IDictionary<string, string> Where);
        IEnumerable<T> GetMenyEntitysSingelTabel<T>(int top, string Selekt, Table table, IDictionary<string, string> Where = null);
    }
}
