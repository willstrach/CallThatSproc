create procedure dbo.SelectWithParameters
(
	@numberOfLegs int
)
as
begin
	select * from dbo.Animals
	where NumberOfLegs = @numberOfLegs;
end