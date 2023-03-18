using CallThatSproc.Attributes;
using System.Data;

namespace CallThatSproc;

public abstract class UserDefinedTableType : IUserDefinedTableType
{
    public virtual string GetTypeName()
    {
        var nameAttribute = (UDTNameAttribute?)Attribute.GetCustomAttribute(this.GetType(), typeof(UDTNameAttribute));

        if (nameAttribute is null)
        {
            return this.GetType().Name;
        }

        return nameAttribute.Name;
    }

    public virtual DataTable ToDataTable()
    {
        var dataTable = new DataTable();
        var properties = this.GetType().GetProperties();
        
        if (properties.Length == 0)
        {
            return dataTable;
        }

        dataTable.Columns.AddRange(GetDataColumns());
        dataTable.Rows.Add(ToDataTableRow());
        return dataTable;
    }

    public virtual object?[] ToDataTableRow()
    {
        var properties = this.GetType().GetProperties();

        if (properties.Length == 0)
        {
            return Array.Empty<object>();
        }

        return properties.Select(property => property.GetValue(this)).ToArray();
    }

    public virtual DataColumn[] GetDataColumns()
    {
        var properties = this.GetType().GetProperties();

        if (properties.Length == 0)
        {
            return Array.Empty<DataColumn>();
        }

        return properties.Select(property =>
        {
            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            return new DataColumn(property.Name, propertyType);
        }).ToArray();
    }
}
