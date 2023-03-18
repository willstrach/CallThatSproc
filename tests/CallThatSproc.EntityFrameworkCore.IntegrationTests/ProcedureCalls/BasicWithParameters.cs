namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class BasicWithParameters : StoredProcedureCall
{
    public override string Name => "BasicWithParameters";
    public override string Schema => "dbo";

    public BasicWithParameters(int intParameter, string stringParameter)
    {
        Parameters.Add(new StoredProcedureParameter("IntParameter", intParameter));
        Parameters.Add(new StoredProcedureParameter("@NVarcharParameter", stringParameter));
    }
}
