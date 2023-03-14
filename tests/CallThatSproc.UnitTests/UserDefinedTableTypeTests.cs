using CallThatSproc.Attributes;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace CallThatSproc.UnitTests;

public class UserDefinedTableTypeTests
{
    [Theory]
    [MemberData(nameof(TestDataGenerator.GetNameTestUDTs), MemberType = typeof(TestDataGenerator))]
    public void GetTypeName_WithMemberData_ReturnCorrectName(IUserDefinedTableType userDefinedTableType, string expectedName)
    {
        // Act
        var actualName = userDefinedTableType.GetTypeName();

        // Assert
        Assert.Equal(expectedName, actualName);
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetColumnCountUDTs), MemberType = typeof(TestDataGenerator))]
    public void GetDataColumns_WithMemberData_ReturnCorrectNumberOfColumns(IUserDefinedTableType userDefinedTableType, int expectedNumberOfColumns)
    {
        // Act
        var dataColumns = userDefinedTableType.GetDataColumns();

        // Assert
        Assert.Equal(expectedNumberOfColumns, dataColumns.Length);
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetColumnNameUDTs), MemberType = typeof(TestDataGenerator))]
    public void GetDataColumns_WithMemberData_ReturnColumnsWithCorrectNames(IUserDefinedTableType userDefinedTableType, IEnumerable<string> expectedNames)
    {
        // Act
        var dataColumns = userDefinedTableType.GetDataColumns();

        // Assert
        Assert.All(dataColumns, dataColumn => expectedNames.Contains(dataColumn.ColumnName));
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetColumnNameUDTs), MemberType = typeof(TestDataGenerator))]
    public void GetDataColumns_WithMemberData_ReturnColumnsInCorrectOrder(IUserDefinedTableType userDefinedTableType, IEnumerable<string> expectedOrder)
    {
        // Act
        var dataColumns = userDefinedTableType.GetDataColumns();

        // Assert
        Assert.Equal(expectedOrder.ToArray(), dataColumns.Select(column => column.ColumnName).ToArray());
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetColumnTypeUDTs), MemberType = typeof(TestDataGenerator))]
    public void GetDataColumns_WithMemberData_ReturnColumnsOfTheCorrectType(IUserDefinedTableType userDefinedTableType, Dictionary<string, Type> expectedTypes)
    {
        // Act
        var dataColumns = userDefinedTableType.GetDataColumns();
        
        // Assert
        foreach (var (columnName, expectedType) in expectedTypes)
        {
            Assert.Equal(expectedType, dataColumns.First(column => column.ColumnName.Equals(columnName)).DataType);
        }
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetRowValueUDTs), MemberType = typeof(TestDataGenerator))]
    public void ToDataTableRow_WithMemberData_ReturnExpectedArray(IUserDefinedTableType userDefinedTableType, object[] expectedValue)
    {
        // Act
        var dataRow = userDefinedTableType.ToDataTableRow();

        // Assert
        Assert.Equal(dataRow, expectedValue);
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetUDTs), MemberType = typeof(TestDataGenerator))]
    public void ToDataTable_WithMemberData_MatchOtherMethods(IUserDefinedTableType userDefinedTableType)
    {
        // Arrange
        var dataColumnNames = userDefinedTableType.GetDataColumns().Select(column => column.ColumnName).ToArray();
        var dataRow = userDefinedTableType.ToDataTableRow();

        // Act
        var dataTable = userDefinedTableType.ToDataTable();

        // Assert
        Assert.Equal(dataColumnNames, dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray());
        Assert.Equal(dataTable.Rows[0].ItemArray, dataRow);
    }
}

public static class TestDataGenerator
{
    private class UDTNoProperties : UserDefinedTableType { }

    [UDTName("ArbitraryName")]
    private class UDTNoPropertiesWithNameAttribute : UserDefinedTableType { }

    private class UDTOneIntProperty : UserDefinedTableType
    {
        public int PropertyOne { get; set; }
    }

    private class UDTTwoIntProperties : UserDefinedTableType
    {
        public int Property1 { get; set; }
        public int Property2 { get; set; }
    }

    private class UDTMixedProperties : UserDefinedTableType
    {
        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; } = string.Empty;
        public bool BoolProperty { get; set; }
        public char CharProperty { get; set; }
    }

    public static IEnumerable<object[]> GetNameTestUDTs()
    {
        yield return new object[] { new UDTNoProperties(), "UDTNoProperties" };
        yield return new object[] { new UDTNoPropertiesWithNameAttribute(), "ArbitraryName" };
    }

    public static IEnumerable<object[]> GetColumnCountUDTs()
    {
        yield return new object[] { new UDTNoProperties(), 0 };
        yield return new object[] { new UDTOneIntProperty(), 1 };
        yield return new object[] { new UDTTwoIntProperties(), 2 };
    }

    public static IEnumerable<object[]> GetColumnNameUDTs()
    {
        yield return new object[] { new UDTOneIntProperty(), new List<string> { "PropertyOne" } };
        yield return new object[] { new UDTTwoIntProperties(), new List<string> { "Property1", "Property2" } };
        yield return new object[] { new UDTMixedProperties(), new List<string> { "IntegerProperty", "StringProperty", "BoolProperty", "CharProperty" } };
    }

    public static IEnumerable<object[]> GetColumnTypeUDTs()
    {
        yield return new object[] { new UDTOneIntProperty(), new Dictionary<string, Type>() { { "PropertyOne", typeof(int) } } };
        yield return new object[] { new UDTTwoIntProperties(), new Dictionary<string, Type>() { { "Property1", typeof(int) }, { "Property2", typeof(int) } } };
        yield return new object[] { new UDTMixedProperties(), new Dictionary<string, Type>() { { "IntegerProperty", typeof(int) }, { "StringProperty", typeof(string) }, { "BoolProperty", typeof(bool) }, { "CharProperty", typeof(char) } } };
    }

    public static IEnumerable<object[]> GetRowValueUDTs()
    {
        yield return new object[] { new UDTNoProperties(), Array.Empty<object>() };
        yield return new object[] { new UDTOneIntProperty() { PropertyOne = 86 }, new object[] { 86 } };
        yield return new object[] { new UDTTwoIntProperties() { Property1 = 86, Property2 = 45 }, new object[] { 86, 45 } };
        yield return new object[] { new UDTMixedProperties() { IntegerProperty = -47, StringProperty = "arbitrary string", BoolProperty = false, CharProperty = 'a' }, new object[] { -47, "arbitrary string", false, 'a' } };
    }

    public static IEnumerable<object[]> GetUDTs()
    {
        yield return new object[] { new UDTOneIntProperty() };
        yield return new object[] { new UDTTwoIntProperties() };
        yield return new object[] { new UDTMixedProperties() };
    }
}
