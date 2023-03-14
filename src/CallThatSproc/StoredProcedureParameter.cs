using System.Data;

namespace CallThatSproc;

public class StoredProcedureParameter<TValue> : IStoredProcedureParameter where TValue : IConvertible
{
    public StoredProcedureParameter(string name, TValue? value = default, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input)
    {
        Name = name;
        Value = value;
        DbType = dbType;
        Direction = direction;
    }

    public string Name { get; }
    public object? Value { get; }
    public DbType? DbType { get; }
    public ParameterDirection Direction { get; } = ParameterDirection.Input;
}
