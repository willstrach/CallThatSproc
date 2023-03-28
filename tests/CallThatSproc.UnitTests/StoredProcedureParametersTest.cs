using Microsoft.Data.SqlClient;
using System.Data;

namespace CallThatSproc.UnitTests;

public class StoredProcedureParametersTest
{
    [Fact]
    public void AddSqlParameter_WithArbitraryParameter_ShouldAddTheSqlParameter()
    {
        // Arrange
        var parameters = new StoredProcedureParameters();
        var sqlParameter = new SqlParameter("theName", "THE VALUE")
        {
            DbType = DbType.AnsiString,
        };

        // Act
        parameters.Add(sqlParameter);

        // Assert
        Assert.Contains(sqlParameter, parameters);
    }

    [Theory]
    [InlineData("@arbitraryName", "Arbitrary Value")]
    [InlineData("@another", "With a different value")]
    public void AddString_WithArbitraryString_ShouldAddParameterWithCorrectName(string name, string value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => parameter.ParameterName == name);
    }

    [Theory]
    [InlineData("@arbitraryName", "Arbitrary Value")]
    [InlineData("@another", "With a different value")]
    public void AddString_WithArbitraryString_ShouldAddParameterWithCorrectValue(string name, string value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => (string)parameter.Value == value);
    }

    [Fact]
    public void AddString_AsOutputParameter_ShouldAddOutputParamter()
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add("name", "value", true);

        // Assert
        Assert.Equal(ParameterDirection.Output, parameters.First(parameter => parameter.ParameterName == "@name").Direction);
    }

    [Theory]
    [InlineData("@arbitraryName", 19)]
    [InlineData("@another", 76)]
    public void AddInt_WithArbitraryInt_ShouldAddParameterWithCorrectName(string name, int value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => parameter.ParameterName == name);
    }


    [Theory]
    [InlineData("@arbitraryName", 19)]
    [InlineData("@another", 76)]
    public void AddInt_WithArbitraryInt_ShouldAddParameterWithCorrectValue(string name, int value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => (int)parameter.Value == value);
    }

    [Fact]
    public void AddInt_AsOutputParameter_ShouldAddOutputParamter()
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add("name", 86, true);

        // Assert
        Assert.Equal(ParameterDirection.Output, parameters.First(parameter => parameter.ParameterName == "@name").Direction);
    }

    [Theory]
    [InlineData("@arbitraryName", true)]
    [InlineData("@another", false)]
    public void AddBool_WithArbitraryBool_ShouldAddParameterWithCorrectName(string name, bool value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => parameter.ParameterName == name);
    }

    [Theory]
    [InlineData("@arbitraryName", true)]
    [InlineData("@another", false)]
    public void AddBool_WithArbitraryBool_ShouldAddParameterWithCorrectValue(string name, bool value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => (bool)parameter.Value == value);
    }

    [Fact]
    public void AddBool_AsOutputParameter_ShouldAddOutputParamter()
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add("name", false, true);

        // Assert
        Assert.Equal(ParameterDirection.Output, parameters.First(parameter => parameter.ParameterName == "@name").Direction);
    }

    [Theory]
    [InlineData("@arbitraryName", 'c')]
    [InlineData("@another", 'l')]
    public void AddChar_WithArbitraryChar_ShouldAddParameterWithCorrectName(string name, char value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => parameter.ParameterName == name);
    }

    [Theory]
    [InlineData("@arbitraryName", 'c')]
    [InlineData("@another", 'l')]
    public void AddChar_WithArbitraryChar_ShouldAddParameterWithCorrectValue(string name, char value)
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add(name, value);

        // Assert
        Assert.Contains(parameters, parameter => (char)parameter.Value == value);
    }

    [Fact]
    public void AddChar_AsOutputParameter_ShouldAddOutputParamter()
    {
        // Arrange
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add("name", 'a', true);

        // Assert
        Assert.Equal(ParameterDirection.Output, parameters.First(parameter => parameter.ParameterName == "@name").Direction);
    }

    class ArbitraryObject
    {
        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; } = string.Empty;
    }

    [Fact]
    public void ConvertToDataTableWithName_WithArbitraryObject_ShouldHaveCorrectColumnNames()
    {
        // Arrange
        var testObject = new ArbitraryObject();
        var columnNames = new string[] { "IntegerProperty", "StringProperty" };

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);

        // Assert
        Assert.All(dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName), columnName => columnName.Contains(columnName));
    }

    [Fact]
    public void ConvertToDataTableWithName_WithArbitraryObject_ShouldHaveCorrectValues()
    {
        // Arrange
        var testObject = new ArbitraryObject() { IntegerProperty = 87, StringProperty = "A string value"};
        var expectedValues = new object[] { 87, "A string value" };

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);
        var row = dataTable.Rows[0].ItemArray;

        // Assert
        Assert.Equal(expectedValues, row);
    }

    [TableType("ThisIsTheTypeName")] class ObjectWithName { }
    [TableType("ThisIsTheTypeName", Schema = "theSchema")] class ObjectWithNameAndSchema { }

    [Fact]
    public void ConvertToDataTableWithName_WithUnnamedObject_ShouldHaveCorrectTypeName()
    {
        // Arrange
        var testObject = new ArbitraryObject();
        var expectedName = "dbo.ArbitraryObject";

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);

        // Assert
        Assert.Equal(expectedName, typeName);
    }

    [Fact]
    public void ConvertToDataTableWithName_WithNamedObject_ShouldHaveCorrectTypeName()
    {
        // Arrange
        var testObject = new ObjectWithName();
        var expectedName = "dbo.ThisIsTheTypeName";

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);

        // Assert
        Assert.Equal(expectedName, typeName);
    }

    [Fact]
    public void ConvertToDataTableWithName_WithNamedObjectAndSchema_ShouldHaveCorrectTypeName()
    {
        // Arrange
        var testObject = new ObjectWithNameAndSchema();
        var expectedName = "theSchema.ThisIsTheTypeName";

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);

        // Assert
        Assert.Equal(expectedName, typeName);
    }

    [ColumnOrder("StringProperty, BoolProperty,IntegerProperty")]
    class ObjectWithOrder
    {
        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; } = string.Empty;
        public bool BoolProperty { get; set; }
    }

    [Fact]
    public void ConvertToDataTableWithName_WithoutColumnOrder_ShouldHaveCorrectOrder()
    {
        // Arrange
        var testObject = new ArbitraryObject();
        var columnNames = new string[] { "IntegerProperty", "StringProperty" };

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);

        // Assert
        Assert.Equal(columnNames, dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName));
    }

    [Fact]
    public void ConvertToDataTableWithName_WithColumnOrder_ShouldHaveCorrectOrder()
    {
        // Arrange
        var testObject = new ObjectWithOrder();
        var columnNames = new string[] { "StringProperty", "BoolProperty", "IntegerProperty" };

        // Act
        var (typeName, dataTable) = StoredProcedureParameters.ConvertToDataTableWithName(testObject);

        // Assert
        Assert.Equal(columnNames, dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName));
    }

    [TableType("typeName", Schema = "schema")]
    [ColumnOrder("StringProperty, BoolProperty, IntegerProperty")]
    class TestObject
    {
        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; } = string.Empty;
        public bool BoolProperty { get; set; }
    }

    [Fact]
    public void AddClass_WithArbitraryObject_ShouldAddParameterWithStructuredType()
    {
        // Arrange
        var testObject = new TestObject();
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add("name", testObject);
        var addedParameter = parameters.First(parameter => parameter.ParameterName == "@name");

        // Assert
        Assert.Equal(SqlDbType.Structured, addedParameter.SqlDbType);
    }

    [Fact]
    public void AddClass_WithArbitraryObject_ShouldAddParameterWithDataTableValue()
    {
        // Arrange
        var testObject = new TestObject();
        var parameters = new StoredProcedureParameters();

        // Act
        parameters.Add("name", testObject);
        var addedParameter = parameters.First(parameter => parameter.ParameterName == "@name");

        // Assert
        Assert.IsType<DataTable>(addedParameter.Value);
        Assert.NotEmpty(((DataTable)addedParameter.Value).Rows);
        Assert.NotEmpty(((DataTable)addedParameter.Value).Columns.Cast<DataColumn>());
    }
}
