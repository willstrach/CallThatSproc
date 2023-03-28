namespace CallThatSproc;

[AttributeUsage(AttributeTargets.Class)]
public class ColumnOrderAttribute : Attribute
{
    public ColumnOrderAttribute(string order)
    {
        Order = order.Split(',', StringSplitOptions.TrimEntries);
    }

    public string[] Order { get; }
}