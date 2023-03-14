using System.Data;

namespace CallThatSproc;

public class UserDefinedTableTypeParameter : IStoredProcedureParameter
{
    public UserDefinedTableTypeParameter(string name, IUserDefinedTableType value)
    {
        Name = name;
        Value = value;
    }

    public UserDefinedTableTypeParameter(string name, IEnumerable<IUserDefinedTableType> value)
    {
        Name = name;
        Value = value.ToArray();
    }

    public string Name { get; }
    public object? Value { get; }
    public DbType? DbType => throw new NotImplementedException();
    public ParameterDirection Direction { get;  } = ParameterDirection.Input;
}
