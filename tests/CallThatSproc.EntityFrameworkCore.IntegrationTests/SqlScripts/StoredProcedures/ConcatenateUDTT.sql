create type TwoNVarchar as table
(
	Value1 nvarchar(max),
	Value2 nvarchar(max)
)

go;

create procedure dbo.ConcatenateUDTT
(
	@MyValues TwoNvarchar readonly,
	@Concatenated nvarchar(max) OUT
)
as
begin
	select @Concatenated = concat(Value1, Value2) from @MyValues 
end