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
