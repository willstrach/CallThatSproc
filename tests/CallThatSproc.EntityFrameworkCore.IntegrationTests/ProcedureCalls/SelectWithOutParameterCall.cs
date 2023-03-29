namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class SelectWithOutParameterCall : StoredProcedureCall
{
    public override string Name => "SelectWithOutParameter";

    public SelectWithOutParameterCall()
    {
        Parameters.Add("RandomNumber", 0, true);
    }
}
