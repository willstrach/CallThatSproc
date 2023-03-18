using Microsoft.Data.SqlClient;

namespace CallThatSproc.EntityFrameworkCore;

public class StoredProcedureCallResult : IStoredProcedureCallResult
{
    internal StoredProcedureCallResult() { }

    public int RowsAffected { get; set; }
    public IStoredProcedureParameter[] OutParameters { get; set; } = Array.Empty<IStoredProcedureParameter>();
}
