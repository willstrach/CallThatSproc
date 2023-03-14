namespace CallThatSproc;

public abstract class StoredProcedureCall : IStoredProcedureCall
{
    public string Schema { get; set; } = "dbo";
    public abstract string Name { get; set; }
    public List<IStoredProcedureParameter> Parameters { get; set; } = new();
}
