
IF OBJECT_ID(N'dbo.tblApprovalList', N'U') IS NULL
CREATE TABLE tblApprovalList (
    ApprovalID INT PRIMARY KEY,
    Programmer NVARCHAR(255),
    Engineer NVARCHAR(255),
    Quality NVARCHAR(255)
)

IF OBJECT_ID(N'dbo.tblPermission', N'U') IS NULL
CREATE TABLE tblPermission (
    PermissionID INT PRIMARY KEY,
    DevelopmentApproval BIT,
    ProductionApproval BIT,
    ObsoleteApproval BIT,
    SendToCNC BIT,
    WriteToVault BIT
)

IF OBJECT_ID(N'dbo.tblUserGroup', N'U') IS NULL
CREATE TABLE tblUserGroup (
    UserGroupID INT PRIMARY KEY,
    UserGroupName NVARCHAR(255),
    PermissionID INT FOREIGN KEY REFERENCES tblPermission(PermissionID)
)

IF OBJECT_ID(N'dbo.tblUsers', N'U') IS NULL
CREATE TABLE tblUsers (
    UserID INT PRIMARY KEY,
    UserName NVARCHAR(255),
    UserEmail NVARCHAR(255),
    UserGroupID INT FOREIGN KEY REFERENCES tblUserGroup(UserGroupID)
)

IF OBJECT_ID(N'dbo.tblHead', N'U') IS NULL
CREATE TABLE tblHead (
    HeadID INT PRIMARY KEY,
    HeadName NVARCHAR(255),
    Destination NVARCHAR(255)
)

IF OBJECT_ID(N'dbo.tblMachines', N'U') IS NULL
CREATE TABLE tblMachines (
    MachineID INT PRIMARY KEY,
    MachineName NVARCHAR(255),
    NumberOfHeads INT,
    HeadID INT FOREIGN KEY REFERENCES tblHead(HeadID),
    Details NVARCHAR(MAX)
)


IF OBJECT_ID(N'dbo.tblMachineTypes', N'U') IS NULL
CREATE TABLE tblMachineTypes (
    MachineTypeID INT PRIMARY KEY,
    MachineTypeName NVARCHAR(255)
)


IF OBJECT_ID(N'dbo.tblMachineTypeMembers', N'U') IS NULL
CREATE TABLE tblMachineTypeMembers (
    MachineTypeMemberID INT PRIMARY KEY,
    MachineTypeID INT FOREIGN KEY REFERENCES tblMachineTypes(MachineTypeID),
    MachineID INT FOREIGN KEY REFERENCES tblMachines(MachineID)
)


IF OBJECT_ID(N'dbo.tblPart', N'U') IS NULL
CREATE TABLE tblPart (
    PartID INT PRIMARY KEY,
    PartName NVARCHAR(255),
    ApprovalID INT FOREIGN KEY REFERENCES tblApprovalList(ApprovalID),
    Description NVARCHAR(255),
    ITARRestricted BIT,
    Revision NVARCHAR(50)
)

IF OBJECT_ID(N'dbo.tblOperation', N'U') IS NULL
CREATE TABLE tblOperation (
    OperationID INT PRIMARY KEY,
    PartID INT FOREIGN KEY REFERENCES tblPart(PartID),
    OperationName NVARCHAR(255)
    --OperationMachineTypeID INT FOREIGN KEY REFERENCES tblOperationMachineTypes(OperationMachineTypeID)
)

IF OBJECT_ID(N'dbo.tblOperationMachineTypes', N'U') IS NULL
CREATE TABLE tblOperationMachineTypes (
    OperationMachineTypeID INT PRIMARY KEY,
    MachineTypeID INT FOREIGN KEY REFERENCES tblMachineTypes(MachineTypeID),
    OperationID INT FOREIGN KEY REFERENCES tblOperation(OperationID)
)

IF OBJECT_ID(N'dbo.tblStatus', N'U') IS NULL
CREATE TABLE tblStatus (
    StatusID INT PRIMARY KEY,
    Development BIT,
    Production BIT,
    Obsolete BIT
)

IF OBJECT_ID(N'dbo.tblRevision', N'U') IS NULL
CREATE TABLE tblRevision (
    RevisionID INT PRIMARY KEY,
    RevisionNumber NVARCHAR(50),
    CreatedByID INT FOREIGN KEY REFERENCES tblUsers(UserID),
    CreationDate DATE,
    RevisedByID INT FOREIGN KEY REFERENCES tblUsers(UserID),
    RevisionDate DATE,
    RevisionComment NVARCHAR(MAX),
    DatabasePath NVARCHAR(MAX),
    StatusID INT FOREIGN KEY REFERENCES tblStatus(StatusID)
)

IF OBJECT_ID(N'dbo.tblCNCProgram', N'U') IS NULL
CREATE TABLE tblCNCProgram (
    ProgramID INT PRIMARY KEY,
    PartID INT FOREIGN KEY REFERENCES tblPart(PartID),
    OperationID INT FOREIGN KEY REFERENCES tblOperation(OperationID),
    MachineTypeID INT FOREIGN KEY REFERENCES tblMachineTypes(MachineTypeID),
    HeadID INT FOREIGN KEY REFERENCES tblHead(HeadID), -- To Confrim
    OrderType NVARCHAR(50),
    ApprovalRequirements NVARCHAR(MAX), -- To Confrim
    ProgramName NVARCHAR(255),
    RevisionID INT FOREIGN KEY REFERENCES tblRevision(RevisionID)
)

IF OBJECT_ID(N'dbo.tblSendHistory', N'U') IS NULL
CREATE TABLE tblSendHistory (
    SendID INT PRIMARY KEY,
    ProgramID INT FOREIGN KEY REFERENCES tblCNCProgram(ProgramID),
    RevisionID INT FOREIGN KEY REFERENCES tblRevision(RevisionID),
    SentByID INT FOREIGN KEY REFERENCES tblUsers(UserID),
    MachineSentTo NVARCHAR(255),
    HeadSentTo NVARCHAR(255),
    SendDate DATE
);