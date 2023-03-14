using System.Data;

namespace CallThatSproc;

public class UserDefinedTableTypeParameter : IStoredProcedureParameter
{
    public UserDefinedTableTypeParameter(string name, IUserDefinedTableType value)
    {
        Name = name;
        Value = value.ToDataTable();
    }

    public UserDefinedTableTypeParameter(string name, IEnumerable<IUserDefinedTableType> value)
    {
        Name = name;

        var dataTable = new DataTable();
        dataTable.Columns.AddRange(value.First().GetDataColumns());
        
        foreach (var tableType in value)
        {
            dataTable.Rows.Add(tableType.ToDataTableRow());
        }

        Value = dataTable;
    }

    public string Name { get; }
    public object? Value { get; }
    public DbType? DbType => throw new NotImplementedException();
    public ParameterDirection Direction { get;  } = ParameterDirection.Input;
}
