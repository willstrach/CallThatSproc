namespace CallThatSproc;

[AttributeUsage(AttributeTargets.Class)]
public class TypeNameAttribute : Attribute
{
    public TypeNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public string Schema { get; set; } = "dbo";
}
