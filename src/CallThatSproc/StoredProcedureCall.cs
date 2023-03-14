namespace CallThatSproc;

public abstract class StoredProcedureCall : IStoredProcedureCall
{
    public virtual string Schema { get; set; } = "dbo";
    public abstract string Name { get; set; }
    public virtual List<IStoredProcedureParameter> Parameters { get; set; } = new();
}
