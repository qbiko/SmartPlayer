use SmartPlayerDbTest
Go
SET IDENTITY_INSERT Club ON
insert into Club("Id", "Name", "DateOfCreate") values (1,'Informatyka ETI', GETDATE());
SET IDENTITY_INSERT Club OFF

SET IDENTITY_INSERT Game ON
insert into Game("Id", "NameOfGame", "TimeOfStart", "Fk_Club") values (1,'Epleczki - Sisteme', GETDATE(), 1);
SET IDENTITY_INSERT Game OFF

SET IDENTITY_INSERT Player ON
insert into Player("Id", "FirstName", "LastName", "DateOfBirth", "HeighOfUser", "WeightOfUser", "Fk_Club") values (1,'Sebastian', 'Sarnecki', GETDATE(), 184, 83, 1);
SET IDENTITY_INSERT Player OFF

SET IDENTITY_INSERT PlayerInGame ON
insert into PlayerInGame("Id", "Fk_Game", "Fk_Player", "Position", "Number", "Active") values (1, 1, 1, 'Striker', 11, 1);
SET IDENTITY_INSERT PlayerInGame OFF

insert into PulseSensor("Value", "TimeOfOccur", "Fk_PlayerInGame") values (120, GETDATE(), 1);

insert into AccelerometerAndGyroscope("X", "Y", "TimeOfOccur", "Fk_PlayerInGame") values (20,20, GETDATE(), 1);