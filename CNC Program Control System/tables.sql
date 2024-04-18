
CREATE TABLE tblPermission (
    PermissionID INT PRIMARY KEY,
    DevelopmentApproval BIT,
    ProductionApproval BIT,
    ObsoleteApproval BIT,
    SendToCNC BIT,
    WriteToVault BIT
);

CREATE TABLE tblUserGroup (
    UserGroupID INT PRIMARY KEY,
    UserGroupName NVARCHAR(255),
    PermissionID INT FOREIGN KEY REFERENCES tblPermission(PermissionID)
);

CREATE TABLE tblUsers (
    UserID INT PRIMARY KEY,
    UserName NVARCHAR(255),
    UserEmail NVARCHAR(255),
    UserGroupID INT FOREIGN KEY REFERENCES tblUserGroup(UserGroupID)
);

CREATE TABLE tblHead (
    HeadID INT PRIMARY KEY,
    HeadName NVARCHAR(255),
    Destination NVARCHAR(255)
);


CREATE TABLE tblMachines (
    MachineID INT PRIMARY KEY,
    MachineName NVARCHAR(255),
    HeadID INT FOREIGN KEY REFERENCES tblHead(HeadID),
    Details NVARCHAR(MAX)
);


CREATE TABLE tblMachineTypes (
    MachineTypeID INT PRIMARY KEY,
    MachineTypeName NVARCHAR(255),
    MachineID INT FOREIGN KEY REFERENCES tblMachines(MachineID)  -- To Confrim
);


CREATE TABLE tblOperation (
    OperationID INT PRIMARY KEY,
    OperationNumber NVARCHAR(255),
    MachineTypeID INT FOREIGN KEY REFERENCES tblMachineTypes(MachineTypeID)
);


CREATE TABLE tblPart (
    PartID INT PRIMARY KEY,
    PartName NVARCHAR(255),
    OperationID INT FOREIGN KEY REFERENCES tblOperation(OperationID),
    --ApprovalListID INT FOREIGN KEY REFERENCES ApprovalList(ApprovalListID),
    ITARRestricted BIT,
    Revision NVARCHAR(50)
);


CREATE TABLE tblStatus (
    StatusID INT PRIMARY KEY,
    Development BIT,
    Production BIT,
    Obsolete BIT
);


CREATE TABLE tblRevision (
    RevisionID INT PRIMARY KEY,
    RevisionNumber NVARCHAR(50),
    CreatedByID INT FOREIGN KEY REFERENCES tblUsers(UserID),
    CreationDate DATE,
    RevisedByID INT FOREIGN KEY REFERENCES tblUsers(UserID),
    RevisionDate DATE,
    Comment NVARCHAR(MAX),
    DatabasePath NVARCHAR(MAX),
    StatusID INT FOREIGN KEY REFERENCES tblStatus(StatusID)
);


CREATE TABLE tblCNCProgram (
    ProgramID INT PRIMARY KEY,
    PartID INT FOREIGN KEY REFERENCES tblPart(PartID),
    OperationID INT FOREIGN KEY REFERENCES tblOperation(OperationID),
    MachineTypeID INT FOREIGN KEY REFERENCES tblMachineTypes(MachineTypeID),
    HeadID INT FOREIGN KEY REFERENCES tblHead(HeadID), -- To Confrim
    OrderType NVARCHAR(50),
    ApprovalRequirements NVARCHAR(MAX),
    ProgramName NVARCHAR(255),
    RevisionID INT FOREIGN KEY REFERENCES tblRevision(RevisionID)
);


CREATE TABLE tblSendHistory (
    SendID INT PRIMARY KEY,
    ProgramID INT FOREIGN KEY REFERENCES tblCNCProgram(ProgramID),
    RevisionID INT FOREIGN KEY REFERENCES tblRevision(RevisionID),
    SentByID INT FOREIGN KEY REFERENCES tblUsers(UserID),
    MachineSentTo NVARCHAR(255),
    HeadSentTo NVARCHAR(255),
    SendDate DATE
);