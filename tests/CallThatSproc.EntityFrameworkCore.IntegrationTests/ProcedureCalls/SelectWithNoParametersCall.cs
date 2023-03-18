namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class SelectWithNoParametersCall : StoredProcedureCall
{
    public override string Name => "SelectWithNoParameters";
}
