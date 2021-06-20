CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `Bank` (
    `Code` varchar(4) NOT NULL,
    `Name` text NOT NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Code`)
);

CREATE TABLE `Customer` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `Remark` text NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `District` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `UMK` int NOT NULL,
    `Remark` text NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `EmploymentStatus` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `Remark` text NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `FamilyStatus` (
    `Code` varchar(2) NOT NULL,
    `Name` text NOT NULL,
    `PTKP` int NOT NULL,
    `Remark` text NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Code`)
);

CREATE TABLE `Position` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `Remark` text NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Role` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `Remark` text NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Location` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `DistrictId` int NOT NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Location_District_DistrictId` FOREIGN KEY (`DistrictId`) REFERENCES `District` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Employee` (
    `NIK` int NOT NULL AUTO_INCREMENT,
    `Name` text NOT NULL,
    `Sex` varchar(1) NULL,
    `BirthPlace` text NOT NULL,
    `BirthDate` datetime NOT NULL,
    `Religion` text NOT NULL,
    `Address` text NOT NULL,
    `PhoneNumber` text NOT NULL,
    `KTP` varchar(16) NOT NULL,
    `KK` varchar(16) NOT NULL,
    `Email` text NOT NULL,
    `Image` text NOT NULL,
    `DriverLicense` text NULL,
    `DriverLicenseExpire` datetime NOT NULL,
    `FamilyStatusCode` varchar(2) NOT NULL,
    `BpjsNumber` varchar(14) NOT NULL,
    `BpjsRemark` text NULL,
    `JamsostekNumber` varchar(12) NOT NULL,
    `JamsostekRemark` text NULL,
    `NPWP` text NULL,
    `JoinCompanyDate` datetime NOT NULL,
    `StartContract` datetime NOT NULL,
    `EndContract` datetime NOT NULL,
    `BankCode` varchar(4) NOT NULL,
    `AccountNumber` text NOT NULL,
    `AccountName` text NOT NULL,
    `EmploymentStatusId` int NOT NULL,
    `HasUniform` bit NOT NULL,
    `HasIdCard` bit NOT NULL,
    `HasTraining` bit NOT NULL,
    `PositionId` int NOT NULL,
    `LocationId` int NOT NULL,
    `CustomerId` int NOT NULL,
    `JoinCustomerDate` datetime NOT NULL,
    `RoleId` int NOT NULL,
    `IsExist` bit NOT NULL,
    `CreateBy` int NOT NULL,
    `ModifyBy` int NULL,
    `CreateDateUtc` datetime NOT NULL,
    `ModifyTimeUtc` datetime NOT NULL,
    PRIMARY KEY (`NIK`),
    CONSTRAINT `FK_Employee_Bank_BankCode` FOREIGN KEY (`BankCode`) REFERENCES `Bank` (`Code`) ON DELETE CASCADE,
    CONSTRAINT `FK_Employee_Customer_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `Customer` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Employee_EmploymentStatus_EmploymentStatusId` FOREIGN KEY (`EmploymentStatusId`) REFERENCES `EmploymentStatus` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Employee_FamilyStatus_FamilyStatusCode` FOREIGN KEY (`FamilyStatusCode`) REFERENCES `FamilyStatus` (`Code`) ON DELETE CASCADE,
    CONSTRAINT `FK_Employee_Location_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Location` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Employee_Position_PositionId` FOREIGN KEY (`PositionId`) REFERENCES `Position` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Employee_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_Employee_BankCode` ON `Employee` (`BankCode`);

CREATE INDEX `IX_Employee_CustomerId` ON `Employee` (`CustomerId`);

CREATE INDEX `IX_Employee_EmploymentStatusId` ON `Employee` (`EmploymentStatusId`);

CREATE INDEX `IX_Employee_FamilyStatusCode` ON `Employee` (`FamilyStatusCode`);

CREATE INDEX `IX_Employee_LocationId` ON `Employee` (`LocationId`);

CREATE INDEX `IX_Employee_PositionId` ON `Employee` (`PositionId`);

CREATE INDEX `IX_Employee_RoleId` ON `Employee` (`RoleId`);

CREATE INDEX `IX_Location_DistrictId` ON `Location` (`DistrictId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210620045918_Initial', '5.0.7');

COMMIT;

