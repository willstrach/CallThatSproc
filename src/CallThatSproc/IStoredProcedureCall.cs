namespace CallThatSproc
{
    public interface IStoredProcedureCall
    {
        string Schema { get; set; }
        string Name { get; set; }
    }
}