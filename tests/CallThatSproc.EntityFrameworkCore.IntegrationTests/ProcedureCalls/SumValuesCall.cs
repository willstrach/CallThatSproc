using System.Data;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class SumValuesCall : StoredProcedureCall
{
    public override string Name => "SumValues";
    public override string Schema => base.Schema;

    public SumValuesCall(int number1, int number2)
    {
        Parameters.Add("Number1", number1);
        Parameters.Add("Number2", number2);
        Parameters.Add("Sum", 0, true);
    }
}
