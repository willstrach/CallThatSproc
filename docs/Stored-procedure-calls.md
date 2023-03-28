> [← Go back](./Index.md)

# Stored procedure calls
Stored procedure calls can be defined by inheriting from the abstract `StoredProcedureCall` class.

Take, for example, the below stored procedure.

```sql
create procedure dbo.AddNewAnimal
(
    @AnimalName nvarchar(max),
    @NumberOfLegs int
)
as
begin
    -- The procedure implementation
end
```

We can represent a call to this procedure using the below class. The name is set in the `Name` property, and the parameters are added as part of the constructor.
```csharp
public class AddNewAnimalCall : StoredProcedureCall
{
    public override string Name => "AddNewAnimal";

    public AddNewAnimalCall(string name, int numberOfLegs)
    {
        Parameters.Add(new StoredProcedureParameter("AnimalName", name));
        Parameters.Add(new StoredProcedureParameter("@NumberOfLegs", numberOfLegs));
    }
}
```
**What if my procedure has no parameters?** No parameters means no constructor is necessary.

## Custom schemas
If your stored procedure is not in the `dbo` schema, you can set the schema to use by overriding the `Schema` property.
```csharp
public class AddNewAnimalCall : StoredProcedureCall
{
    public override string Name => "AddNewAnimal";
    public override string Schema => "notDbo";
    // ...
}
```
## Parameter SQL types
By default, the parameter type is derived using the type of the value you pass in. If you need to specifiy the type of the parameter directly, use the `dbType` parameter when creating a new `StoredProcedureParameter`.

```csharp
public class AddNewAnimalCall : StoredProcedureCall
{
    public override string Name => "AddNewAnimal";

    public AddNewAnimalCall(string name, int numberOfLegs)
    {
        Parameters.Add(new StoredProcedureParameter("AnimalName", name, dbType = DbType.AnsiString));
        Parameters.Add(new StoredProcedureParameter("@NumberOfLegs", numberOfLegs));
    }
}
```

## Output parameters
Retrieving information from a procedure can be achieved using output parameters.

Take, for example, the below stored procedure.

```sql
create procedure dbo.SumValues
(
	@Number1 int,
	@Number2 int,
	@Sum int out
)
as
begin
	set @Sum = @Number1 + @Number2;
end

```
We can represent a call to this procedure using the below class. Output parameters are set using the `direction` paramter when creating your `StoredProcedureParameter`.

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
Access to these paramters following execution is handled by the [Entity Framwork Core extensions](./Using-with-Entity-Framework-Core.md);