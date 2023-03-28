namespace CallThatSproc;

public abstract class StoredProcedureCall : IStoredProcedureCall
{
    public virtual string Schema { get; } = "dbo";
    public abstract string Name { get; }
    public virtual StoredProcedureParameters Parameters { get; } = new();
}
