> [← Go back](./Index.md)

# User-defined table types
If your stored procedure takes a user-defined table type as a parameter, define the type, and pass it in.

Take, for example, the below type.
```sql
create type TwoNVarchar as table
(
	Value1 nvarchar(max),
	Value2 nvarchar(max)
)
```

We can represent this type using a class which inherits from `UserDefinedTableType`.

```csharp
[UDTName("TwoNVarchar")]
public class TwoNVarchar : UserDefinedTableType
{
    public string Value1 { get; set; } = string.Empty;
    public string Value2 { get; set; } = string.Empty;
}

```

Notice the attribute `UDTName`. This attribute allows us to define the type name of the type. If this is not provided, the name of the class will be used.

## Using user-defined table types as parameters
UDTTs can be passed in when creating a `StoredProcedureParameter`, similarly to the way simple paramters are created. Because the type is known, and UDTT paramters cannot be output parameters, values for `dbType` and `direction` cannot be specified.

```csharp
public class ConcatenateUDTT : StoredProcedureCall
{
    public override string Name => "ConcatenateUDTT";

    public ConcatenateUDTT(string value1, string value2)
    {
        var twoNVarchar = new TwoNVarchar() { Value1 = value1, Value2 = value2 };
        Parameters.Add(new StoredProcedureParameter("MyValues", twoNVarchar));
    }
}
```