namespace CallThatSproc;

[AttributeUsage(AttributeTargets.Class)]
public class TableTypeAttribute : Attribute
{
    public TableTypeAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public string Schema { get; set; } = "dbo";
}
