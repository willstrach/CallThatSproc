create procedure dbo.BasicWithParameters
(
	@IntParameter int,
	@NVarcharParameter nvarchar(max)
)
as
begin
	declare @a int;
	set @a = 99;
end
