using System.Data;

namespace CallThatSproc;

public interface IStoredProcedureParameter
{
    string Name { get; }
    object? Value { get; }
    DbType? DbType { get; }
    ParameterDirection Direction { get; }
}
