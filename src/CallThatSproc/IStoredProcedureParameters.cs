using Microsoft.Data.SqlClient;

namespace CallThatSproc;

public interface IStoredProcedureParameters : IEnumerable<SqlParameter>
{
    void Add(SqlParameter parameter);
    void Add<TValue>(string name, TValue value, bool output = false) where TValue : struct;
    void Add(string name, string value, bool output = false);
    void Add(string name, ITableType value);
    object GetValueOf(string name);
}
