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
        TypeName = "";
    }

    public StoredProcedureParameter(string name, IUserDefinedTableType value)
    {
        Name = name;
        Value = value.ToDataTable();
        TypeName = value.GetTypeName();
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

        TypeName = value.First().GetTypeName();

        Value = dataTable;
    }

    public string Name { get; }
    public object? Value { get; set; }
    public DbType? DbType { get; }
    public ParameterDirection Direction { get; } = ParameterDirection.Input;

    public string TypeName { get; }
}