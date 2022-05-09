use MovieCatalog;

create table director(
	name nvarchar(450) not null unique,
	directorID int not null identity primary key,
);

create table movie(
	title nvarchar(450) not null unique,
	rating tinyint not null,
	dateAdded date not null,
	directorID int foreign key references director(directorID),
	movieID int not null identity primary key,
);

GO