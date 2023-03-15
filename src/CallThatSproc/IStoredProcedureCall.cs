namespace CallThatSproc
{
    public interface IStoredProcedureCall
    {
        string Schema { get; set; }
        string Name { get; set; }
        List<IStoredProcedureParameter> Parameters { get; set; }
    }
}