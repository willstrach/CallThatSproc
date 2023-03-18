using System.Data;

namespace CallThatSproc;

public interface IStoredProcedureParameter
{
    string Name { get; }
    object? Value { get; set;  }
    DbType? DbType { get; }
    ParameterDirection Direction { get; }
    string TypeName { get; }
}
