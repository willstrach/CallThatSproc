namespace CallThatSproc
{
    public interface IStoredProcedureCall
    {
        string Schema { get; }
        string Name { get; }
        StoredProcedureParameters Parameters { get; }
    }
}