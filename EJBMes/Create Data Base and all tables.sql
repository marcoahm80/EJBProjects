USE [master]
GO

/****** Object:  Database [EJBProductionReport]    Script Date: 12/12/2023 3:25:17 PM ******/
CREATE DATABASE [EJBProductionReport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EJBProductionReport', FILENAME = N'G:\data\EJBProductionReport.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EJBProductionReport_log', FILENAME = N'H:\log\EJBProductionReport_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EJBProductionReport].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [EJBProductionReport] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [EJBProductionReport] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [EJBProductionReport] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [EJBProductionReport] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [EJBProductionReport] SET ARITHABORT OFF 
GO

ALTER DATABASE [EJBProductionReport] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [EJBProductionReport] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [EJBProductionReport] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [EJBProductionReport] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [EJBProductionReport] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [EJBProductionReport] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [EJBProductionReport] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [EJBProductionReport] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [EJBProductionReport] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [EJBProductionReport] SET  DISABLE_BROKER 
GO

ALTER DATABASE [EJBProductionReport] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [EJBProductionReport] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [EJBProductionReport] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [EJBProductionReport] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [EJBProductionReport] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [EJBProductionReport] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [EJBProductionReport] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [EJBProductionReport] SET RECOVERY FULL 
GO

ALTER DATABASE [EJBProductionReport] SET  MULTI_USER 
GO

ALTER DATABASE [EJBProductionReport] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [EJBProductionReport] SET DB_CHAINING OFF 
GO

ALTER DATABASE [EJBProductionReport] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [EJBProductionReport] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [EJBProductionReport] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [EJBProductionReport] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [EJBProductionReport] SET QUERY_STORE = ON
GO

ALTER DATABASE [EJBProductionReport] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [EJBProductionReport] SET  READ_WRITE 
GO

USE [EJBProductionReport]
GO

/****** Object:  Table [dbo].[UserMES]    Script Date: 12/12/2023 3:26:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserMES](
	[UserId] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[EmployeeID] [nvarchar](10) NOT NULL,
	[Company] [nvarchar](8) NOT NULL,
	[Site] [nvarchar](8) NOT NULL,
 CONSTRAINT [PK_UserMES] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



USE [EJBProductionReport]
GO

/****** Object:  Table [dbo].[ProdReport]    Script Date: 12/12/2023 3:25:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProdReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNum] [nvarchar](10) NOT NULL,
	[JobNum] [nvarchar](50) NOT NULL,
	[AssemblyNum] [int] NOT NULL,
	[OpSeq] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[ResourceGroup] [nvarchar](10) NOT NULL,
	[ResourceId] [nvarchar](10) NOT NULL,
	[ReferenceNotes] [varchar](5000) NULL,
	[LaborQty] [decimal](18, 2) NOT NULL,
	[ActiveLabor] [bit] NOT NULL,
	[Procesed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProdReport] ADD  DEFAULT ((0)) FOR [AssemblyNum]
GO

ALTER TABLE [dbo].[ProdReport] ADD  DEFAULT ((0)) FOR [OpSeq]
GO

ALTER TABLE [dbo].[ProdReport] ADD  DEFAULT ('') FOR [ReferenceNotes]
GO

ALTER TABLE [dbo].[ProdReport] ADD  DEFAULT ((0)) FOR [ActiveLabor]
GO

ALTER TABLE [dbo].[ProdReport] ADD  DEFAULT ((0)) FOR [Procesed]
GO

USE [EJBProductionReport]
GO

/****** Object:  Table [dbo].[ScrapReport]    Script Date: 12/12/2023 3:26:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScrapReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNum] [nvarchar](10) NOT NULL,
	[JobNum] [nvarchar](50) NOT NULL,
	[AssemblyNum] [int] NOT NULL,
	[OpSeq] [int] NOT NULL,
	[ScrapDate] [datetime] NOT NULL,
	[ResourceGroup] [nvarchar](10) NOT NULL,
	[ResourceId] [nvarchar](10) NOT NULL,
	[ReferenceNotes] [varchar](5000) NULL,
	[ScrapQty] [decimal](18, 2) NOT NULL,
	[ReasonCode] [nvarchar](10) NOT NULL,
	[WhseCode] [nvarchar](10) NOT NULL,
	[BinNum] [nvarchar](10) NOT NULL,
	[Procesed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScrapReport] ADD  DEFAULT ((0)) FOR [AssemblyNum]
GO

ALTER TABLE [dbo].[ScrapReport] ADD  DEFAULT ((0)) FOR [OpSeq]
GO

ALTER TABLE [dbo].[ScrapReport] ADD  DEFAULT ('') FOR [ReferenceNotes]
GO

ALTER TABLE [dbo].[ScrapReport] ADD  DEFAULT ((0)) FOR [Procesed]
GO

USE [EJBProductionReport]
GO

/****** Object:  Table [dbo].[DowntimeReport]    Script Date: 12/12/2023 4:50:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DowntimeReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNum] [nvarchar](10) NOT NULL,
	[JobNum] [nvarchar](50) NOT NULL,
	[AssemblyNum] [int] NOT NULL,
	[OpSeq] [int] NOT NULL,
	[DownTimeStartDate] [datetime] NOT NULL,
	[DownTimeEndDate] [datetime] NOT NULL,
	[ResourceGroup] [nvarchar](10) NOT NULL,
	[ResourceId] [nvarchar](10) NOT NULL,
	[ReferenceNotes] [varchar](5000) NULL,
	[ReasonCode] [nvarchar](10) NOT NULL,
	[Procesed] [bit] NOT NULL,
	[ActiveDowntime] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DowntimeReport] ADD  DEFAULT ((0)) FOR [AssemblyNum]
GO

ALTER TABLE [dbo].[DowntimeReport] ADD  DEFAULT ((0)) FOR [OpSeq]
GO

ALTER TABLE [dbo].[DowntimeReport] ADD  DEFAULT ('') FOR [ReferenceNotes]
GO

ALTER TABLE [dbo].[DowntimeReport] ADD  DEFAULT ((0)) FOR [Procesed]
GO

ALTER TABLE [dbo].[DowntimeReport] ADD  DEFAULT ((0)) FOR [ActiveDowntime]
GO





