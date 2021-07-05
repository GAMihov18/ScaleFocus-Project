USE [master]
GO

/****** Object:  Database [ProjectDB]    Script Date: 7/5/2021 10:18:04 PM ******/
CREATE DATABASE [ProjectDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProjectDB', FILENAME = N'C:\Users\georg\ProjectDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ProjectDB_log', FILENAME = N'C:\Users\georg\ProjectDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProjectDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ProjectDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ProjectDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ProjectDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ProjectDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ProjectDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [ProjectDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ProjectDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ProjectDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ProjectDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ProjectDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ProjectDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ProjectDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ProjectDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ProjectDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ProjectDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ProjectDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ProjectDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ProjectDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ProjectDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ProjectDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ProjectDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ProjectDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ProjectDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [ProjectDB] SET  MULTI_USER 
GO

ALTER DATABASE [ProjectDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ProjectDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ProjectDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [ProjectDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [ProjectDB] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [ProjectDB] SET QUERY_STORE = OFF
GO

USE [ProjectDB]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE [ProjectDB] SET  READ_WRITE 
GO


USE [ProjectDB]
GO

CREATE TABLE [dbo].[Projects](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatorId] [int] NULL,
	[LastChangeDate] [datetime] NOT NULL,
	[LastChangeUserId] [int] NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_LastChangeDate]  DEFAULT (getdate()) FOR [LastChangeDate]
GO

ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_CreatorId_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_CreatorId_Users]
GO

ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_LastChangeUserId_Users] FOREIGN KEY([LastChangeUserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_LastChangeUserId_Users]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tasks](
	[Id] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[AssignedUserId] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatorId] [int] NULL,
	[LastChangeDate] [datetime] NOT NULL,
	[LastChangeUserId] [int] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_LastChangeDate]  DEFAULT (getdate()) FOR [LastChangeDate]
GO

ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_AssignedUserId_Tasks] FOREIGN KEY([AssignedUserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_AssignedUserId_Tasks]
GO

ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_CreatorId_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_CreatorId_Users]
GO

ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_LastChangeuserId_Users] FOREIGN KEY([LastChangeUserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_LastChangeuserId_Users]
GO

CREATE TABLE [dbo].[Teams](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatorId] [int] NULL,
	[LastChangeDate] [datetime] NOT NULL,
	[LastChangeUserId] [int] NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Teams] ADD  CONSTRAINT [DF_Teams_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[Teams] ADD  CONSTRAINT [DF_Teams_LastChangeDate]  DEFAULT (getdate()) FOR [LastChangeDate]
GO

ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_Teams_CreatorId_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_Teams_CreatorId_Users]
GO

ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_Teams_LastChangeUserId_Users] FOREIGN KEY([LastChangeUserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_Teams_LastChangeUserId_Users]
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Roles] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatorId] [int] NULL,
	[LastChangeDate] [datetime] NOT NULL,
	[LastChangeUserId] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_LastChangeDate]  DEFAULT (getdate()) FOR [LastChangeDate]
GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_User_LastChangeUserId] FOREIGN KEY([LastChangeUserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_User_LastChangeUserId]
GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_CreatorId] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_CreatorId]
GO

CREATE TABLE [dbo].[WorkLogs](
	[Id] [int] NOT NULL,
	[TaskId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[TimeSpent] [int] NOT NULL,
	[DateSpent] [datetime] NOT NULL,
 CONSTRAINT [PK_WorkLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WorkLogs] ADD  CONSTRAINT [DF_WorkLogs_DateSpent]  DEFAULT (getdate()) FOR [DateSpent]
GO

ALTER TABLE [dbo].[WorkLogs]  WITH CHECK ADD  CONSTRAINT [FK_WorkLogs_TaskId_Tasks] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Tasks] ([Id])
GO

ALTER TABLE [dbo].[WorkLogs] CHECK CONSTRAINT [FK_WorkLogs_TaskId_Tasks]
GO

ALTER TABLE [dbo].[WorkLogs]  WITH CHECK ADD  CONSTRAINT [FK_WorkLogs_UserId_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[WorkLogs] CHECK CONSTRAINT [FK_WorkLogs_UserId_Users]
GO



