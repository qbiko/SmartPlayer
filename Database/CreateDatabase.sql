use SmartPlayerDbTest
GO
Drop Table PulseSensor
Drop Table AccelerometerAndGyroscope
Drop Table PlayerInGame
Drop Table Player
Drop Table Game
Drop Table Club

create table Club(
	Id INTEGER IDENTITY(1,1) PRIMARY KEY,
	Name varchar(255),
	DateOfCreate Date
)

create table Player(
	Id INTEGER IDENTITY(1,1) PRIMARY KEY,
	FirstName varchar(255),
	LastName varchar(255),
	DateOfBirth Date,
	HeighOfUser int,
	WeightOfUser int,
	Fk_Club INTEGER FOREIGN KEY REFERENCES Club
)

create table Game(
	Id INTEGER IDENTITY(1,1) PRIMARY KEY,
	NameOfGame varchar(255),
	TimeOfStart smalldatetime,
	Fk_Club INTEGER FOREIGN KEY REFERENCES Club
)

create table PlayerInGame(
	Id INTEGER IDENTITY(1,1) PRIMARY KEY,
	Fk_Game INTEGER FOREIGN KEY REFERENCES Game,
	Fk_Player INTEGER FOREIGN KEY REFERENCES Player,
	Position varchar(255),
	Number INTEGER,
	Active BIT
)

create table PulseSensor(
	Value INTEGER,
	TimeOfOccur smalldatetime,
	Fk_PlayerInGame INTEGER FOREIGN KEY REFERENCES PlayerInGame
)

create table AccelerometerAndGyroscope(
	X INTEGER,
	Y INTEGER,
	TimeOfOccur smalldatetime,
	Fk_PlayerInGame INTEGER FOREIGN KEY REFERENCES PlayerInGame
)


