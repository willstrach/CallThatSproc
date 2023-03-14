using System.Data;

namespace CallThatSproc;

public interface IUserDefinedTableType
{
    DataColumn[] GetDataColumns();
    string GetTypeName();
    DataTable ToDataTable();
    object?[] ToDataTableRow();
}
