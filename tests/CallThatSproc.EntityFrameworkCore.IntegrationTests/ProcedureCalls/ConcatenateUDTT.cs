using CallThatSproc.Attributes;
using System.Data;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

public class ConcatenateUDTT : StoredProcedureCall
{
    public override string Name => "ConcatenateUDTT";

    public ConcatenateUDTT(string value1, string value2)
    {
        var twoNVarchar = new TwoNVarchar() { Value1 = value1, Value2 = value2 };
        Parameters.Add(new StoredProcedureParameter("MyValues", twoNVarchar));
        Parameters.Add(new StoredProcedureParameter("Concatenated", "", direction: ParameterDirection.Output));
    }
}

[UDTName("TwoNVarchar")]
public class TwoNVarchar : UserDefinedTableType
{
    public string Value1 { get; set; } = string.Empty;
    public string Value2 { get; set; } = string.Empty;
}
