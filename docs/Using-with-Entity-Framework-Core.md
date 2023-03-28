> [← Go back](./Index.md)

# Entity Framework Core
Using the `CallThatSproc.EntityFrameworkCore` package, you can execute procedure calls against your `DbContext`.

## Executing procedures
Stored procedure calls can be executed easily by using one of the `DatabaseFacade` extension methods:`ExecuteStoredProcedure` and `ExecuteStoredProcedureAsync`.

```csharp
public class DatabaseContext : DbContext
{
    // ...

    public async Task ExecuteMyStoredProcedureCallAsync()
    {
        var procedureCall = new MyProcedureCall();
        var result = await Database.ExecuteStoredProcedureAsync(procedureCall);
    }
}
```

Both methods return an `IStoredProcedureCallResult`, which can then be used to get the values of any out parameters.

```csharp
public interface IStoredProcedureCallResult
{
    IStoredProcedureParameter[] OutParameters { get; set; }
    int RowsAffected { get; set; }
}
```

## Reading from procedures
If you need to read from a stored procedure call, you can use one of the `DbSet` extension methods:
`GetFromStoredProcedureCall` and `GetFromStoredProcedureCallAsync`.

```csharp
public class DatabaseContext : DbContext
{
    // ...

    private DbSet<Something> _somethings;

    public IEnumerable<Something> Task GetSomething()
    {
        var procedureCall = new ProcedureWithSelectCall();
        return await _somethings.GetFromStoredProcedureCallAsync(procedureCall);
    }
}
```