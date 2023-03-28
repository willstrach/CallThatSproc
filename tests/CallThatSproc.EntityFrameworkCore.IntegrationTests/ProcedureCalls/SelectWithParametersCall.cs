namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class SelectWithParametersCall : StoredProcedureCall
{
    public override string Name => "SelectWithParameters";

    public SelectWithParametersCall(int numberOfLegs)
    {
        Parameters.Add("numberOfLegs", numberOfLegs);
    }
}
