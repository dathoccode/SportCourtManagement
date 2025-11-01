use QuanLySanTheThao
create table tCourt(
	CourtID nvarchar(10),
	CourtName nvarchar(10),
	CourtAddress nvarchar(50),
	Contact nvarchar(11),
	OpenTime time,
	CloseTime time,
	Img nvarchar(50),
	Rating float,
	primary key(CourtID)
)

create table tSlot(
	SlotID nvarchar(10),
	CourtID nvarchar(10),
	SlotType nvarchar(100),
	primary key(SlotID, CourtID),
	foreign key(CourtID) references tCourt(CourtID)
)

create table tPrice(
	CourtID nvarchar(10),
	SlotID nvarchar(10),
	StartTime time,
	EndTime time,
	UnitPrice money,
	primary key(CourtID, SlotID),
	foreign key(CourtID) references tCourt(CourtID)
)

create table tRole(
	RoleID int,
	RoleName nvarchar(20),
	primary key(RoleID)
)

create table tAccount(
	AccountID NVARCHAR(10) PRIMARY KEY,
	RoleID INT,
	AccName NVARCHAR(50),
	AccPassword NVARCHAR(100),
	AccImg NVARCHAR(255),
	Email NVARCHAR(100) UNIQUE,      
	Phone NVARCHAR(15),             
	FOREIGN KEY(RoleID) REFERENCES tRole(RoleID)
)

create table tStatus(
	StatusID int not null,
	StatusName nvarchar(30),
	primary key(StatusID)
)

create table tBooking(
	BookingID nvarchar(10),
	AccountID nvarchar(10),
	BookingDate datetime,
	Sale float null,
	StatusID int,
	Price money,
	primary key(BookingID),
	foreign key(AccountID) references tAccount(AccountID),
	foreign key(StatusID) references tStatus(StatusID)
)

create table tBookingDetail(
	DetailID nvarchar(10),
	BookingID nvarchar(10),
	CourtID nvarchar(10),
	SlotID nvarchar(10),
	StartTime time,
	EndTime time,
	primary key(DetailID),
	foreign key(BookingID) references tBooking(BookingID),
	foreign key(CourtID) references tCourt(CourtID)
)

create table tFavoriteCourt(
	CourtID nvarchar(10),
	AccountID nvarchar(10),
	primary key(CourtID, AccountID),
	foreign key(CourtID) references tCourt(CourtID),
	foreign key(AccountID) references tAccount(AccountID)
)

