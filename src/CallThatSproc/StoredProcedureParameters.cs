using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CallThatSproc.UnitTests")]
namespace CallThatSproc;

public class StoredProcedureParameters : IStoredProcedureParameters
{
    private List<SqlParameter> _parameters = new();

    public IEnumerator<SqlParameter> GetEnumerator() => _parameters.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(SqlParameter parameter) => _parameters.Add(parameter);

    public void Add<TValue>(string name, TValue value, bool output = false)
    {
        var normalisedName = Normalise(name);

        if (value is string || value is decimal || typeof(TValue).IsPrimitive)
        {
            _parameters.Add(new SqlParameter(normalisedName, value)
            {
                Direction = output ? ParameterDirection.Output : ParameterDirection.Input,
                Size = -1,
            });
            return;
        }

        if (output) throw new NotSupportedException("Output table types are not supported");

        var (typeName, dataTable) = ConvertToDataTableWithName(value);
        _parameters.Add(new SqlParameter(normalisedName, dataTable)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = typeName,
            Size = -1,
        });
    }

    public object GetValueOf(string name)
    {
        var normalisedName = Normalise(name);
        return _parameters.First(parameter => parameter.ParameterName.ToUpper() == normalisedName.ToUpper()).Value;
    }

    private static string Normalise(string name) => name.StartsWith("@") ? name : $"@{name}";

    internal static (string, DataTable) ConvertToDataTableWithName<TValue>(TValue value)
    {
        var tableTypeAttribute = (TableTypeAttribute?)Attribute.GetCustomAttribute(typeof(TValue), typeof(TableTypeAttribute));
        var columnOrderAttribute = (ColumnOrderAttribute?)Attribute.GetCustomAttribute(typeof(TValue), typeof(ColumnOrderAttribute));

        var dataTable = new DataTable();
        var valueType = typeof(TValue);
        var properties = valueType.GetProperties();

        var columnNames = columnOrderAttribute is null ? properties.Select(property => property.Name)
            : columnOrderAttribute.Order;
        var typeName = tableTypeAttribute is null ? $"dbo.{valueType.Name}" : $"{tableTypeAttribute.Schema}.{tableTypeAttribute.Name}";

        var dataRow = dataTable.NewRow();

        foreach (var columnName in  columnNames)
        {
            var relevantProperty = properties.First(property => property.Name == columnName);
            var propertyType = Nullable.GetUnderlyingType(relevantProperty.PropertyType) ?? relevantProperty.PropertyType;
            var propertyValue = relevantProperty.GetValue(value);
            dataTable.Columns.Add(columnName, propertyType);
            dataRow.SetField(columnName, propertyValue);
        }

        dataTable.Rows.Add(dataRow);

        return (typeName, dataTable);
    }
}
