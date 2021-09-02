# Project
Card Project
- Install SqlServer https://download.microsoft.com/download/7/f/8/7f8a9c43-8c8a-4f7c-9f92-83c18d96b681/SQL2019-SSEI-Expr.exe
- Script to Create DB :  CREATE DATABASE db_corelogin;
- Script for Creating Tables for storing Data :
-------------------------------------------------------------------------
USE [db_corelogin]
GO

/****** Object:  Table [dbo].[Login]    Script Date: 02/09/2021 22:06:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Login](
	[id] [INT] IDENTITY(1,1) NOT NULL,
	[username] [VARCHAR](50) NOT NULL,
	[password] [VARCHAR](50) NOT NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


-----------------------------------------------------------------

USE [db_corelogin]
GO

/****** Object:  Table [dbo].[t_ToDoList]    Script Date: 02/09/2021 22:06:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_ToDoList](
	[IdTask] [INT] NOT NULL,
	[TaskDescription] [NVARCHAR](MAX) NOT NULL,
	[Finished] [BIT] NULL,
	[InProgress] [BIT] NULL,
	[InsertDate] [DATETIME] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[t_ToDoList] ADD  CONSTRAINT [DF_t_ToDoList_Finished]  DEFAULT ((0)) FOR [Finished]
GO

ALTER TABLE [dbo].[t_ToDoList] ADD  CONSTRAINT [DF_t_ToDoList_InProgress]  DEFAULT ((0)) FOR [InProgress]
GO


-------------------------------------------------------------------
Stored Procedure for user Log in  :

USE [db_corelogin]
GO

/****** Object:  StoredProcedure [dbo].[LoginByUsernamePassword]    Script Date: 02/09/2021 22:07:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:  <Author,,Vaska Stambolliu>
-- Create date: <Create Date,,>
-- Description: <Description,,You are Allow to Distribute this Code>
-- =============================================
CREATE PROCEDURE [dbo].[LoginByUsernamePassword] 
 @username VARCHAR(50),
 @password VARCHAR(50)
AS
BEGIN
 SELECT id, username, password
 FROM Login
 WHERE username = @username
 AND password = @password
END

GO


-------------------------------------------------
Insert for user Created in table  Login
INSERT INTO dbo.Login
(
    username,
    password
)
VALUES
(   'admin', -- username - varchar(50)
    'Desktop.12'  -- password - varchar(50)
 ) 
 ----------------------------------------------------
