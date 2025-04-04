CREATE TABLE "Users" (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    RegistrationDate DATETIME NOT NULL,
    NotificationsEnabled BIT NOT NULL
);

