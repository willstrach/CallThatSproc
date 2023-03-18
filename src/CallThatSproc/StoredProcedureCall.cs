namespace CallThatSproc;

public abstract class StoredProcedureCall : IStoredProcedureCall
{
    public virtual string Schema { get; } = "dbo";
    public abstract string Name { get; }
    public virtual List<IStoredProcedureParameter> Parameters { get; set; } = new();
}
