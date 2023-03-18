
# CallThatSproc
Create classes for calling stored procedures in MSSQL and Azure SQL that are easy to read and use.

## Installing CallThatSproc
> **⚠️ This package is not yet available on NuGet**

You can install using NuGet:
```dotnetcli
Install-Package CallThatSproc

<!-- If using with Entity Framework Core -->
Install-Package CallThatSproc.EntityFrameworkCore
```

or using the .NET CLI:
```dotnetcli
dotnet add package CallThatSproc

<!-- If using with Entity Framework Core -->
dotnet add package CallThatSproc.EntityFrameworkCore
```

or using Visual Studio's NuGet package manager.

## Getting started
The easiest way to get started is to [read the docs](./docs/Index.md).

## Configuring stored procedure calls
Setting up a stored procedure call is easy. Just inherit from the StoredProcedureCall abstract class, and set the name. If your schema isn't `dbo`, set the override the default value.

```csharp
public class MyStoredProcedureCall : StoredProcedureCall
{
    public override string Name => "MyStoredProcedure";
    public override string Schema => "SomethingOtherThanDbo";
}
```

If you need to add parameters, register them in the constructor.

```csharp
public class AnotherStoredProcedureCall : StoredProcedureCall
{
    public override string Name => "MyStoredProcedure";

    public AnotherStoredProcedureCall(string name, int count)
    {
        Parameters.Add(new StoredProcedureParameter("Name", name));
        Parameters.Add(new StoredProcedureParameter("Count", count));
    }
}
```

## Executing procedures with Entity Framework Core
Using the `CallThatSproc.EntityFrameworkCore` package, you can execute procedure calls against your `DbContext`.

```csharp
public class DatabaseContext : DbContext
{
    // ...

    public async Task ExecuteMyStoredProcedureCallAsync()
    {
        var procedureCall = new MyProcedureCall();
        var result = await Database.ExecuteStoredProcedureAsync(procedureCall);
    }

    private DbSet<Something> _somethings;

    public IEnumerable<Something> Task GetSomething()
    {
        var procedureCall = new ProcedureWithSelectCall();
        return await _somethings.GetFromStoredProcedureCallAsync(procedureCall);
    }
}
```

## Support for output parameters
If you need to get data back without a select, the package supports the use of output parameters.

```csharp
public class OutputStoredProcedureCall : StoredProcedureCall
{
    public override string Name => "MyStoredProcedure";

    public AnotherStoredProcedureCall(string name, int count)
    {
        Parameters.Add(new StoredProcedureParameter("Name", name, direction: ParameterDirection.Output));
    }
}
```

## Support for user-defined table types
If your stored procedure takes a user-defined table type as a parameter, define the type, and pass it in.

```csharp
[UDTName("MyUDTName")]
public class MyUDT : IUserDefinedTableType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class OutputStoredProcedureCall : StoredProcedureCall
{
    public override string Name => "MyStoredProcedure";

    public AnotherStoredProcedureCall(int id, string name)
    {
        var myUDT = new UDT() { Id: id, Name: name };
        Parameters.Add(new StoredProcedureParameter("Name", myUDT));
    }
}
```
