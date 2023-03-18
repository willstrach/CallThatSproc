create table dbo.Animals
(
	ID int not null primary key identity(1,1),
	AnimalName nvarchar(max) not null,
	NumberOfLegs int not null
)

insert into dbo.Animals (AnimalName, NumberOfLegs)
values
	('Dog', 4),
	('Cat', 4),
	('Kangaroo', 2),
	('Human', 2)
