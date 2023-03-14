namespace CallThatSproc.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class UDTNameAttribute: Attribute
{
    public UDTNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
