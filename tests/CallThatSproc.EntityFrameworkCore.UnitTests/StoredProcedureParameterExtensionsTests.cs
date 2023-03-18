using System.Data;

namespace CallThatSproc.EntityFrameworkCore.UnitTests;

public class StoredProcedureParameterExtensionsTests
{
    [Fact]
    public void ToSqlString_WithoutAtPrefix_ShouldReturnValidString()
    {
        //  Arrange
        var parameterName = "nameWithoutPrefix";
        var parameter = new StoredProcedureParameter(parameterName, "some arbitrary value");
        var expectedString = "@nameWithoutPrefix=@nameWithoutPrefix";

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.Equal(expectedString, sqlString);
    }

    [Fact]
    public void ToSqlString_WithAtPrefix_ShouldReturnValidString()
    {
        //  Arrange
        var parameterName = "@nameWithPrefix";
        var parameter = new StoredProcedureParameter(parameterName, "some arbitrary value");
        var expectedString = "@nameWithPrefix=@nameWithPrefix";

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.Equal(expectedString, sqlString);
    }

    [Fact]
    public void ToSqlString_InputParameter_ShouldNotHaveOutSuffix()
    {
        // Arrange
        var parameter = new StoredProcedureParameter("arbitraryName", "some arbitrary value", direction: System.Data.ParameterDirection.Input);

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.DoesNotContain(" OUT", sqlString);
    }

    [Fact]
    public void ToSqlString_OutputParameter_ShouldHaveOutSuffix()
    {
        // Arrange
        var parameter = new StoredProcedureParameter("arbitraryName", "some arbitrary value", direction: System.Data.ParameterDirection.Output);

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.EndsWith(" OUT", sqlString);
    }

    [Fact]
    public void ToSqlString_InOutParameter_ShouldHaveOutSuffix()
    {
        // Arrange
        var parameter = new StoredProcedureParameter("arbitraryName", "some arbitrary value", direction: System.Data.ParameterDirection.InputOutput);

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.EndsWith(" OUT", sqlString);
    }

    [Fact]
    public void ToSqlParameter_WithoutAtPrefix_ShouldAppendAtToName()
    {
        //  Arrange
        var parameterName = "nameWithoutPrefix";
        var parameter = new StoredProcedureParameter(parameterName, "some arbitrary value");
        var expectedName = $"@{parameterName}";

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.Equal(expectedName, sqlParameter.ParameterName);
    }

    [Fact]
    public void ToSqlParameter_WithAtPrefix_ShouldNotAppendAtToName()
    {
        //  Arrange
        var expectedName = "@nameWithPrefix";
        var parameter = new StoredProcedureParameter(expectedName, "some arbitrary value");

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.Equal(expectedName, sqlParameter.ParameterName);
    }

    [Fact]
    public void ToSqlParameter_WithIntValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = 47;
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.IsType<int>(sqlParameter.Value);
        Assert.Equal(expectedValue, sqlParameter.Value);
    }

    [Fact]
    public void ToSqlParameter_WithStringValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = "some string";
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.IsType<string>(sqlParameter.Value);
        Assert.Equal(expectedValue, sqlParameter.Value);
    }

    [Fact]
    public void ToSqlParameter_WithBooleanValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = true;
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.IsType<bool>(sqlParameter.Value);
        Assert.Equal(expectedValue, sqlParameter.Value);
    }

    [Fact]
    public void ToSqlParameter_WithCharValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = 'a';
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.IsType<char>(sqlParameter.Value);
        Assert.Equal(expectedValue, sqlParameter.Value);
    }

    [Fact]
    public void ToSqlParameter_WithUDTValue_ShouldHaveDataTableValue()
    {
        // Arrange
        var expectedValue = new UDTMixedProperties();
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.IsType<DataTable>(sqlParameter.Value);
    }

    [Fact]
    public void ToSqlParameter_WithListOfUDTValue_ShouldHaveDataTableValue()
    {
        // Arrange
        var expectedValue = new List<UserDefinedTableType>()
        {
            new UDTMixedProperties(),
            new UDTMixedProperties(),
            new UDTMixedProperties(),
            new UDTMixedProperties(),
        };
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.IsType<DataTable>(sqlParameter.Value);
    }
    [Fact]
    public void ToSqlParameter_WithUDTValue_ShouldHaveStructuredSqlDbType()
    {
        // Arrange
        var value = new UDTMixedProperties();
        var parameter = new StoredProcedureParameter("arbitraryName", value);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.Equal(SqlDbType.Structured, sqlParameter.SqlDbType);
    }

    [Fact]
    public void ToSqlParameter_WithListOfUDTValue_ShouldHaveStructuredSqlDbType()
    {
        // Arrange
        var value = new List<UserDefinedTableType>()
        {
            new UDTMixedProperties(),
            new UDTMixedProperties(),
            new UDTMixedProperties(),
            new UDTMixedProperties(),
        };
        var parameter = new StoredProcedureParameter("arbitraryName", value);

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.Equal(SqlDbType.Structured, sqlParameter.SqlDbType);
    }

    [Fact]
    public void ToSqlParameter_WithUDTValue_ShouldHaveCorrectDatatable()
    {
        // Arrange
        var value = new UDTWithFakeDataTable();
        var parameter = new StoredProcedureParameter("arbitraryName", value);
        var expectedDataTable = value.ToDataTable();
        var expectedColumns = expectedDataTable.Columns.Cast<DataColumn>();
        var expectedRows = expectedDataTable.Rows.Cast<DataRow>();

        // Act
        var sqlParameter = parameter.ToSqlParameter();

        // Assert
        Assert.All(((DataTable)sqlParameter.Value).Columns.Cast<DataColumn>(),
            column => expectedColumns.Any(expectedColumn =>  column.ColumnName == expectedColumn.ColumnName && column.DataType == expectedColumn.DataType));

        Assert.All(((DataTable)sqlParameter.Value).Rows.Cast<DataRow>(),
            row => expectedRows.Any(expectedRow => row.ItemArray == expectedRow.ItemArray));
    }

    //[Fact]
    //public void ToSqlParameter_WithListOfUDTValue_ShouldHaveCorrectDatatable()
    //{
    //    // Arrange
    //    var value = new List<UserDefinedTableType>()
    //    {
    //        new UDTWithFakeDataTable(),
    //        new AnotherUDTWithFakeDataTable(),
    //    };
    //    var parameter = new StoredProcedureParameter("arbitraryName", value);
    //    var expectedDataTable = value[0].ToDataTable();
    //    expectedDataTable.Merge(value[1].ToDataTable());
    //    var expectedColumns = expectedDataTable.Columns.Cast<DataColumn>();
    //    var expectedRows = expectedDataTable.Rows.Cast<DataRow>();

    //    // Act
    //    var sqlParameter = parameter.ToSqlParameter();

    //    // Assert
    //    Assert.All(((DataTable)sqlParameter.Value).Columns.Cast<DataColumn>(),
    //        column => expectedColumns.Any(expectedColumn => column.ColumnName == expectedColumn.ColumnName && column.DataType == expectedColumn.DataType));

    //    Assert.All(((DataTable)sqlParameter.Value).Rows.Cast<DataRow>(),
    //        row => expectedRows.Any(expectedRow => row.ItemArray == expectedRow.ItemArray));
    //}

}

public class UDTMixedProperties : UserDefinedTableType
{
    public int IntegerProperty { get; set; }
    public string StringProperty { get; set; } = string.Empty;
    public bool BoolProperty { get; set; }
    public char CharProperty { get; set; }
}

public class UDTWithFakeDataTable : UserDefinedTableType
{
    public UDTWithFakeDataTable()
    {
        var dataTable = new DataTable();
        var dataColumns = new DataColumn[] { new DataColumn("Column1", typeof(string)), new DataColumn("Column2", typeof(int)) };
        var dataRow = new object[] { "Some value", 0 };
        dataTable.Columns.AddRange(dataColumns);
        dataTable.Rows.Add(dataRow);
        _dataTable = dataTable;
    }
    private DataTable _dataTable;

    public override DataTable ToDataTable() => _dataTable;
    public override DataColumn[] GetDataColumns() => _dataTable.Columns.Cast<DataColumn>().ToArray();
    public override object?[] ToDataTableRow() => _dataTable.Rows.Cast<DataRow>().First().ItemArray;
}

public class AnotherUDTWithFakeDataTable : UserDefinedTableType
{
    public AnotherUDTWithFakeDataTable()
    {
        var dataTable = new DataTable();
        var dataColumns = new DataColumn[] { new DataColumn("Column1", typeof(string)), new DataColumn("Column2", typeof(int)) };
        var dataRow = new object[] { "Another value", 84 };
        dataTable.Columns.AddRange(dataColumns);
        dataTable.Rows.Add(dataRow);
        _dataTable = dataTable;
    }
    private DataTable _dataTable;

    public override DataTable ToDataTable() => _dataTable;
    public override DataColumn[] GetDataColumns() => _dataTable.Columns.Cast<DataColumn>().ToArray();
    public override object?[] ToDataTableRow() => _dataTable.Rows.Cast<DataRow>().First().ItemArray;
}
