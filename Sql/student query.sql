create database WindowService
use WindowService

create table Student(
				Id int primary key identity,
				Name nvarchar(15),
				Address nvarchar(15),
				Phone nvarchar(15)
				)