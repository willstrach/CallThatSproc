using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests;

public class DatabaseFixture : IDisposable
{
    public string ConnectionString => $"{_baseConnectionString}Database=CallThatSproc_EntityFrameworkTesting";
    public SqlConnection DatabaseConnection { get; private set; }
    public DatabaseContext DatabaseContext { get; private set; }

    public DatabaseFixture()
    {
        CreateDatabase();
        DatabaseConnection = new SqlConnection(ConnectionString);
        ExecuteSetupScripts();
        DatabaseContext = new(ConnectionString);
    }

    private string _baseConnectionString => "Server=localhost;Trusted_Connection=True;Trust Server Certificate=True;";
    private string _masterConnectionString => $"{_baseConnectionString}Database=master";

    public void Dispose()
    {
        DropDatabase();
    }

    private void CreateDatabase()
    {
        var masterDatabaseConnection = new SqlConnection(_masterConnectionString);
        if (masterDatabaseConnection.State == ConnectionState.Closed) masterDatabaseConnection.Open();

        var command = GetSqlCommandFromFile("CreateTestDatabase.sql", masterDatabaseConnection);
        command.ExecuteNonQuery();

        if (masterDatabaseConnection.State == ConnectionState.Open) masterDatabaseConnection.Close();
    }

    private void DropDatabase()
    {
        var masterDatabaseConnection = new SqlConnection(_masterConnectionString);
        if (masterDatabaseConnection.State == ConnectionState.Closed) masterDatabaseConnection.Open();

        var command = GetSqlCommandFromFile("DropTestDatabase.sql", masterDatabaseConnection);
        command.ExecuteNonQuery();

        if (masterDatabaseConnection.State == ConnectionState.Open) masterDatabaseConnection.Close();
    }

    private void ExecuteSetupScripts()
    {
        if (DatabaseConnection.State == ConnectionState.Closed) DatabaseConnection.Open();
        
        foreach (var procedureCreationCommand in GetProcedureCreationCommands())
        {
            procedureCreationCommand.ExecuteNonQuery();
        }

        foreach (var tableCreationCommand in GetTableCreationCommands())
        {
            tableCreationCommand.ExecuteNonQuery();
        }

        if (DatabaseConnection.State == ConnectionState.Open) DatabaseConnection.Close();
    }

    private string GetSqlScriptsDirectory()
    {
        var binDirectory = Directory.GetCurrentDirectory();
        var projectDirectory = binDirectory.Split("\\bin\\")[0];
        return $"{projectDirectory}\\SqlScripts";
    }

    private SqlCommand[] GetProcedureCreationCommands()
    {
        var sqlScriptsDirectory = GetSqlScriptsDirectory();
        var storedProceduresDirectory = $"{sqlScriptsDirectory}\\StoredProcedures";
        var fileNames = Directory.GetFiles(storedProceduresDirectory) ?? Array.Empty<string>();
        var scripts = fileNames.Select(fileName => File.ReadAllText(fileName));

        var goVariations = new string[] { "go;", "GO;", "go", "GO" };

        var scriptsWithSplitTransactions = scripts.Aggregate(new List<string>(), (scriptsList, script) =>
        {
            scriptsList.AddRange(script.Split(goVariations, StringSplitOptions.RemoveEmptyEntries));
            return scriptsList;
        });
        var commands = scriptsWithSplitTransactions.Select(script => new SqlCommand(script, DatabaseConnection)).ToArray();
        return commands;
    }

    private SqlCommand[] GetTableCreationCommands()
    {
        var sqlScriptsDirectory = GetSqlScriptsDirectory();
        var storedProceduresDirectory = $"{sqlScriptsDirectory}\\Tables";
        var fileNames = Directory.GetFiles(storedProceduresDirectory) ?? Array.Empty<string>();
        var scripts = fileNames.Select(fileName => File.ReadAllText(fileName));
        var commands = scripts.Select(script => new SqlCommand(script, DatabaseConnection)).ToArray();
        return commands;
    }

    private SqlCommand GetSqlCommandFromFile(string fileName, SqlConnection databaseConnection)
    {
        var sqlScriptsDirectory = GetSqlScriptsDirectory();
        var scriptPath = $"{sqlScriptsDirectory}\\{fileName}";
        var script = File.ReadAllText(scriptPath);
        return new SqlCommand(script, databaseConnection);
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }