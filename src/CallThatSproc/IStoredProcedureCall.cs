namespace CallThatSproc
{
    public interface IStoredProcedureCall
    {
        string Schema { get; }
        string Name { get; }
        List<IStoredProcedureParameter> Parameters { get; set; }
    }
}