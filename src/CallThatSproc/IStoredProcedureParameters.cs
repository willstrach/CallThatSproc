using Microsoft.Data.SqlClient;

namespace CallThatSproc;

public interface IStoredProcedureParameters : IEnumerable<SqlParameter>
{
    void Add(SqlParameter parameter);
    void Add<TValue>(string name, TValue value, bool output = false);
    object GetValueOf(string name);
}
