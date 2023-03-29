create procedure dbo.SelectWithOutParameter
(
	@RandomNumber int out
)
as
begin
	set @RandomNumber = rand(100) * 100;

	select * from dbo.Animals;
end