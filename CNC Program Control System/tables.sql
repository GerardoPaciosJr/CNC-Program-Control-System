
CREATE TABLE Permission (
    PermissionID INT PRIMARY KEY,
    DevelopmentApproval BIT,
    ProductionApproval BIT,
    ObsoleteApproval BIT,
    SendToCNC BIT,
    WriteToVault BIT
);

CREATE TABLE UserGroup (
    UserGroupID INT PRIMARY KEY,
    UserGroupName NVARCHAR(255),
    PermissionID INT FOREIGN KEY REFERENCES Permission(PermissionID)
);

CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    UserName NVARCHAR(255),
    UserEmail NVARCHAR(255),
    UserGroupID INT FOREIGN KEY REFERENCES UserGroup(UserGroupID)
);

CREATE TABLE Head (
    HeadID INT PRIMARY KEY,
    HeadName NVARCHAR(255),
    Destination NVARCHAR(255)
);


CREATE TABLE Machines (
    MachineID INT PRIMARY KEY,
    MachineName NVARCHAR(255),
    HeadID INT FOREIGN KEY REFERENCES Head(HeadID),
    Details NVARCHAR(MAX)
);


CREATE TABLE MachineTypes (
    MachineTypeID INT PRIMARY KEY,
    MachineTypeName NVARCHAR(255),
    MachineID INT FOREIGN KEY REFERENCES Machines(MachineID)  -- To Confrim
);


CREATE TABLE Operation (
    OperationID INT PRIMARY KEY,
    OperationNumber NVARCHAR(255),
    MachineTypeID INT FOREIGN KEY REFERENCES MachineTypes(MachineTypeID)
);


CREATE TABLE Part (
    PartID INT PRIMARY KEY,
    PartName NVARCHAR(255),
    OperationID INT FOREIGN KEY REFERENCES Operation(OperationID),
    --ApprovalListID INT FOREIGN KEY REFERENCES ApprovalList(ApprovalListID),
    ITARRestricted BIT,
    Revision NVARCHAR(50)
);


CREATE TABLE Status (
    StatusID INT PRIMARY KEY,
    Development BIT,
    Production BIT,
    Obsolete BIT
);


CREATE TABLE Revision (
    RevisionID INT PRIMARY KEY,
    RevisionNumber NVARCHAR(50),
    CreatedByID INT FOREIGN KEY REFERENCES Users(UserID),
    CreationDate DATE,
    RevisedByID INT FOREIGN KEY REFERENCES Users(UserID),
    RevisionDate DATE,
    Comment NVARCHAR(MAX),
    DatabasePath NVARCHAR(MAX),
    StatusID INT FOREIGN KEY REFERENCES Status(StatusID)
);


CREATE TABLE CNCProgram (
    ProgramID INT PRIMARY KEY,
    PartID INT FOREIGN KEY REFERENCES Part(PartID),
    OperationID INT FOREIGN KEY REFERENCES Operation(OperationID),
    MachineTypeID INT FOREIGN KEY REFERENCES MachineTypes(MachineTypeID),
    HeadID INT FOREIGN KEY REFERENCES Head(HeadID), -- To Confrim
    OrderType NVARCHAR(50),
    ApprovalRequirements NVARCHAR(MAX),
    ProgramName NVARCHAR(255),
    RevisionID INT FOREIGN KEY REFERENCES Revision(RevisionID)
);


CREATE TABLE SendHistory (
    SendID INT PRIMARY KEY,
    ProgramID INT FOREIGN KEY REFERENCES CNCProgram(ProgramID),
    RevisionID INT FOREIGN KEY REFERENCES Revision(RevisionID),
    SentByID INT FOREIGN KEY REFERENCES Users(UserID),
    MachineSentTo NVARCHAR(255),
    HeadSentTo NVARCHAR(255),
    SendDate DATE
);