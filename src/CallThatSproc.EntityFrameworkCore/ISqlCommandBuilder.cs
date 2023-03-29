namespace CallThatSproc;

public interface ISqlCommandBuilder
{
    string BuildExecProcedureCommand(IStoredProcedureCall procedureCall);
}
