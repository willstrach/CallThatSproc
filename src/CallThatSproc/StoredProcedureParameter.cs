using System.Data;
using System.Net.Http.Headers;

namespace CallThatSproc;

public class StoredProcedureParameter : IStoredProcedureParameter
{
    public StoredProcedureParameter(string name, IConvertible? value = default, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input)
    {
        Name = name;
        Value = value;
        DbType = dbType;
        Direction = direction;
    }

    public StoredProcedureParameter(string name, IUserDefinedTableType value)
    {
        Name = name;
        Value = value.ToDataTable();
    }

    public StoredProcedureParameter(string name, IEnumerable<IUserDefinedTableType> value)
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
    public DbType? DbType { get; }
    public ParameterDirection Direction { get; } = ParameterDirection.Input;
}