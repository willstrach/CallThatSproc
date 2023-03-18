using Microsoft.Data.SqlClient;

namespace CallThatSproc.EntityFrameworkCore;

public interface IStoredProcedureCallResult
{
    IStoredProcedureParameter[] OutParameters { get; set; }
    int RowsAffected { get; set; }
}