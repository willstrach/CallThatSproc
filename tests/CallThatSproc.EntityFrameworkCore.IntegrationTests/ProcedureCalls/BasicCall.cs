namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class BasicCall : StoredProcedureCall
{
    public override string Name => "BasicProcedure";
    public override string Schema => "dbo";
}
