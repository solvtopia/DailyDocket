USE [wdivzylm_utilitywizards]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC sys.sp_fulltext_database @action = 'enable'
end
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET ARITHABORT OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET  DISABLE_BROKER 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
--ALTER DATABASE [wdivzylm_utilitywizards] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET READ_COMMITTED_SNAPSHOT OFF 
GO
--ALTER DATABASE [wdivzylm_utilitywizards] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET  MULTI_USER 
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET PAGE_VERIFY CHECKSUM  
GO
--ALTER DATABASE [wdivzylm_utilitywizards] SET DB_CHAINING OFF 
GO
USE [wdivzylm_utilitywizards]
GO
/****** Object:  UserDefinedFunction [wdivzylm_utilitywizards].[GetElement]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 7/22/2016
-- Description:	Get element from xml
-- =============================================
CREATE FUNCTION [wdivzylm_utilitywizards].[GetElement] 
(
	-- Add the parameters for the function here
	@xml xml,
	@root nvarchar(255),
	@field nvarchar(255)

)
RETURNS xml
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result xml,
			@x xml

	--SET @sql = 'SELECT xmlData LIKE ''%' + @CustAcctNum + '%'''

	-- Add the T-SQL statements to compute the return value here
	SELECT @x = @xml.query('/*[local-name()=sql:variable("@root")]/*[local-name()=sql:variable("@field")]');

	SELECT @Result = @x --@x.value('(/*[local-name()=sql:variable("@field")]/text())[1]', 'varchar(255)')

	-- Return the result of the function
	RETURN @Result

END

GO
/****** Object:  UserDefinedFunction [wdivzylm_utilitywizards].[GetElementValue]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 7/20/2016
-- Description:	Get value of xml element
-- =============================================
CREATE FUNCTION [wdivzylm_utilitywizards].[GetElementValue] 
(
	-- Add the parameters for the function here
	@xml xml,
	@root nvarchar(255),
	@field nvarchar(255)

)
RETURNS nvarchar(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(255) = '',
			@x xml

	--SET @sql = 'SELECT xmlData LIKE ''%' + @CustAcctNum + '%'''

	-- Add the T-SQL statements to compute the return value here
	SELECT @x = @xml.query('/*[local-name()=sql:variable("@root")]/*[local-name()=sql:variable("@field")]');

	SELECT @Result = @x.value('(/*[local-name()=sql:variable("@field")]/text())[1]', 'varchar(255)')

	-- Return the result of the function
	RETURN @Result

END

GO
/****** Object:  UserDefinedFunction [wdivzylm_utilitywizards].[GetModulesXml]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 1/27/2016
-- Description:	Build the xModules column for clients
-- =============================================
CREATE FUNCTION [wdivzylm_utilitywizards].[GetModulesXml] 
(
	-- Add the parameters for the function here
	@clientID int
)
RETURNS nvarchar(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(max),
			@tmpElement nvarchar(max)

	-- Add the T-SQL statements to compute the return value here
	SELECT @tmpElement = (SELECT
		(SELECT [xmlData] FROM [Modules] WHERE [Active] = 1 AND [xClientID] = @clientID FOR XML PATH('USELESS1'), TYPE) AS 'USELESS2'
	FOR XML PATH(''), ROOT('ROOT'))

	-- remove the root elements created above and fix up the format for the project to use
	SET @tmpElement	= REPLACE(@tmpElement, '<ROOT>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '</ROOT>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '<USELESS1>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '</USELESS1>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '<USELESS2>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '</USELESS2>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '<xmlData>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '</xmlData>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '<Data>', '<Module>')
	SET @tmpElement	= REPLACE(@tmpElement, '</Data>', '</Module>')

	SELECT @Result = @tmpElement

	-- Return the result of the function
	RETURN @Result

END

GO
/****** Object:  UserDefinedFunction [wdivzylm_utilitywizards].[GetElementValue2]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 8/10/2016
-- Description:	Return value of an XmlElement
-- =============================================
CREATE FUNCTION [wdivzylm_utilitywizards].[GetElementValue2] 
(
	-- Add the parameters for the function here
	@xml xml,
	@root nvarchar(255),
	@field nvarchar(255)

)
RETURNS nvarchar(max)
WITH SCHEMABINDING
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(255) = '',
			@x xml

	--SET @sql = 'SELECT xmlData LIKE ''%' + @CustAcctNum + '%'''

	-- Add the T-SQL statements to compute the return value here
	SELECT @x = @xml.query('/*[local-name()=sql:variable("@root")]/*[local-name()=sql:variable("@field")]');

	SELECT @Result = @x.value('(/*[local-name()=sql:variable("@field")]/text())[1]', 'varchar(255)')

	-- Return the result of the function
	RETURN @Result

END

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Clients]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Clients](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
	[xName]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemClient','Name')) PERSISTED,
	[xModules]  AS ([wdivzylm_utilitywizards].[GetModulesXml]([ID])),
	[xActive]  AS (isnull(CONVERT([bit],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemClient','Active')),(0))) PERSISTED NOT NULL,
	[xApproved]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemClient','Approved')) PERSISTED,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [wdivzylm_utilitywizards].[Sys_History]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Sys_History](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemText] [nvarchar](max) NULL,
	[dtInserted] [datetime] NULL,
	[insertedBy] [int] NULL,
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Users]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
	[Active] [bit] NULL,
	[xClientID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','ClientID')),(0))) PERSISTED NOT NULL,
	[xName]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','Name')) PERSISTED,
	[xEmail]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','Email')) PERSISTED,
	[xMobileUsername]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','MobileUsername')) PERSISTED,
	[xPassword]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','Password')) PERSISTED,
	[xPermissions]  AS (isnull([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','Permissions'),'User')) PERSISTED NOT NULL,
	[xActive]  AS (isnull(CONVERT([bit],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','Active')),[Active])) PERSISTED,
	[xApiEnabled]  AS (isnull(CONVERT([bit],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','ApiEnabled')),(0))) PERSISTED NOT NULL,
	[xWebEnabled]  AS (isnull(CONVERT([bit],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','WebEnabled')),(1))) PERSISTED NOT NULL,
	[xMobileDeviceId]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','MobileDeviceId')) PERSISTED,
	[xOneSignalUserId]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','OneSignalUserID')) PERSISTED,
	[xOneSignalPushToken]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','OneSignalPushToken')) PERSISTED,
	[xMobileDeviceType]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','MobileDeviceType')) PERSISTED,
	[xSupervisorID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemUser','SupervisorID')),(0))) PERSISTED NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [wdivzylm_utilitywizards].[vwHistory]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [wdivzylm_utilitywizards].[vwHistory]
AS
SELECT        wdivzylm_utilitywizards.Sys_History.ID, wdivzylm_utilitywizards.Sys_History.ItemText, wdivzylm_utilitywizards.Sys_History.dtInserted, wdivzylm_utilitywizards.Users.ID AS UserID, wdivzylm_utilitywizards.Users.xName AS UserName, wdivzylm_utilitywizards.Clients.ID AS ClientID, wdivzylm_utilitywizards.Users.xSupervisorID
FROM            wdivzylm_utilitywizards.Sys_History INNER JOIN
                         wdivzylm_utilitywizards.Users ON wdivzylm_utilitywizards.Sys_History.insertedBy = wdivzylm_utilitywizards.Users.ID INNER JOIN
                         wdivzylm_utilitywizards.Clients ON wdivzylm_utilitywizards.Users.xClientID = wdivzylm_utilitywizards.Clients.ID

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Modules]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Modules](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
	[Active] [bit] NULL,
	[xClientID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','ClientID')),(0))) PERSISTED NOT NULL,
	[xName]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','Name')) PERSISTED,
	[xDescription]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','Description')) PERSISTED,
	[xIcon]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','Icon')) PERSISTED,
	[xType]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','Type')) PERSISTED,
	[xFolderID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','FolderID')),(0))) PERSISTED NOT NULL,
	[xSupervisorID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemModule','SupervisorID')),(0))) PERSISTED NOT NULL,
 CONSTRAINT [PK_Modules] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [wdivzylm_utilitywizards].[Customers]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Customers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[Active] [bit] NULL,
	[xAccount]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','Account')) PERSISTED,
	[xFullName]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','FullName')) PERSISTED,
	[xLocationID]  AS (left([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','Account'),(11))) PERSISTED,
	[xClientID]  AS (CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ClientID'))) PERSISTED,
 CONSTRAINT [PK_CustomersNew] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [wdivzylm_utilitywizards].[GetLocationID]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 11/8/2016
-- Description:	Get Location ID
-- =============================================
CREATE FUNCTION [wdivzylm_utilitywizards].[GetLocationID] 
(
	-- Add the parameters for the function here
	@xml xml
)
RETURNS nvarchar(255)
WITH SCHEMABINDING
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(255),
			@a nvarchar(50),
			@l nvarchar(50)

	-- Add the T-SQL statements to compute the return value here
	SELECT @a = wdivzylm_utilitywizards.GetElementValue2(@xml,'Data','Account');
	SELECT @l = wdivzylm_utilitywizards.GetElementValue2(@xml,'Data','Location');

	IF CHARINDEX('-',@l,0) = 0
	BEGIN
		-- invalid location id, use the first 11 characters of the account number
		SET @Result = LEFT(@a, 3) + '-' + @l
	END
	ELSE
	BEGIN
		SET @Result = LEFT(@l, 11)
	END

	-- Return the result of the function
	RETURN @Result

END

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Locations]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Locations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[Active] [bit] NULL,
	[xAccount]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','Account')) PERSISTED,
	[xServiceAddress]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ServiceAddress')) PERSISTED,
	[xCity]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','City')) PERSISTED,
	[xState]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','State')) PERSISTED,
	[xZipCode]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ZipCode')) PERSISTED,
	[xLocationID]  AS ([wdivzylm_utilitywizards].[GetLocationID]([xmlData])) PERSISTED,
	[xClientID]  AS (CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ClientID'))) PERSISTED,
	[xSearchAddress]  AS (((((([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ServiceAddress')+' ')+[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','City'))+' ')+[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','State'))+' ')+[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ZipCode')) PERSISTED,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [wdivzylm_utilitywizards].[ModuleData]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[ModuleData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
	[Active] [bit] NULL,
	[xStatus]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','Status')),(0))) PERSISTED NOT NULL,
	[xClientID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ClientID')),(0))) PERSISTED NOT NULL,
	[xModuleID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ModuleID')),(0))) PERSISTED NOT NULL,
	[xUserEmail]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','UserEmail')) PERSISTED,
	[xCustAcctNum]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','CustAcctNum')) PERSISTED,
	[xCustServiceAddress]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','CustomerServiceAddress')) PERSISTED,
	[xViewed]  AS (isnull(CONVERT([bit],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','ViewedOnMobile')),(0))) PERSISTED NOT NULL,
	[xTechnicianID]  AS (isnull([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','Technician'),(0))) PERSISTED,
	[xPriority]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','Priority')),(0))) PERSISTED NOT NULL,
	[xLocationNum]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'Data','LocationNum')) PERSISTED,
 CONSTRAINT [PK_ModuleData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [wdivzylm_utilitywizards].[vwModuleData]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [wdivzylm_utilitywizards].[vwModuleData]
AS
SELECT        wdivzylm_utilitywizards.ModuleData.ID, wdivzylm_utilitywizards.ModuleData.xmlData, wdivzylm_utilitywizards.ModuleData.dtInserted, wdivzylm_utilitywizards.ModuleData.dtUpdated, wdivzylm_utilitywizards.ModuleData.insertedBy, wdivzylm_utilitywizards.ModuleData.updatedBy, wdivzylm_utilitywizards.ModuleData.Active, wdivzylm_utilitywizards.ModuleData.xStatus, 
                         wdivzylm_utilitywizards.ModuleData.xClientID, wdivzylm_utilitywizards.ModuleData.xModuleID, wdivzylm_utilitywizards.ModuleData.xUserEmail, wdivzylm_utilitywizards.ModuleData.xCustAcctNum, wdivzylm_utilitywizards.Locations.xServiceAddress AS xCustServiceAddress, wdivzylm_utilitywizards.ModuleData.xViewed, 
                         wdivzylm_utilitywizards.ModuleData.xTechnicianID, wdivzylm_utilitywizards.ModuleData.xPriority, wdivzylm_utilitywizards.Locations.xLocationID AS xLocationNum
FROM            wdivzylm_utilitywizards.Locations LEFT OUTER JOIN
                         wdivzylm_utilitywizards.Customers ON wdivzylm_utilitywizards.Locations.xLocationID = wdivzylm_utilitywizards.Customers.xLocationID RIGHT OUTER JOIN
                         wdivzylm_utilitywizards.ModuleData ON wdivzylm_utilitywizards.Customers.xAccount = wdivzylm_utilitywizards.ModuleData.xCustAcctNum

GO
/****** Object:  View [wdivzylm_utilitywizards].[vwMobileWorkOrders]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [wdivzylm_utilitywizards].[vwMobileWorkOrders]
AS
SELECT        wdivzylm_utilitywizards.vwModuleData.ID, wdivzylm_utilitywizards.vwModuleData.xViewed, wdivzylm_utilitywizards.vwModuleData.xStatus, wdivzylm_utilitywizards.vwModuleData.xPriority, wdivzylm_utilitywizards.vwModuleData.xModuleID, wdivzylm_utilitywizards.Modules.xFolderID, 
                         CASE WHEN Modules.xFolderID = 0 THEN Modules.xName ELSE Modules_1.xName + ' > ' + Modules.xName END AS ModuleName, 
                         CASE WHEN Modules.xFolderID = 0 THEN Modules.xIcon ELSE Modules_1.xIcon END AS ModuleIcon, wdivzylm_utilitywizards.vwModuleData.xCustServiceAddress AS CustomerAddress, CASE WHEN Users_2.xName IS NULL 
                         THEN CASE WHEN Users_1.xName IS NULL THEN Users.ID ELSE Users_1.ID END ELSE Users_2.ID END AS AssignedToID, CASE WHEN Users_2.xName IS NULL THEN CASE WHEN Users_1.xName IS NULL 
                         THEN Users.xName + ' (Supervisor)' ELSE Users_1.xName + ' (Supervisor)' END ELSE Users_2.xName + ' (Technician)' END AS AssignedToName, CASE WHEN Users_2.xName IS NULL 
                         THEN CASE WHEN Users_1.xName IS NULL THEN Users.xMobileDeviceId ELSE Users_1.xMobileDeviceId END ELSE Users_2.xMobileDeviceId END AS AssignedToDeviceID, 
                         CASE WHEN Users_1.xName IS NULL THEN Users.ID ELSE Users_1.ID END AS SupervisorID, wdivzylm_utilitywizards.vwModuleData.xTechnicianID AS TechnicianID, Users_2.xName AS TechnicianName, 
                         wdivzylm_utilitywizards.Modules.xSupervisorID AS ModuleSupervisorID, Users_1.xName AS ModuleSupervisorName, Modules_1.xSupervisorID AS FolderSupervisorID, wdivzylm_utilitywizards.Users.xName AS FolderSupervisorName
FROM            wdivzylm_utilitywizards.Users RIGHT OUTER JOIN
                         wdivzylm_utilitywizards.Modules AS Modules_1 INNER JOIN
                         wdivzylm_utilitywizards.Modules ON Modules_1.ID = wdivzylm_utilitywizards.Modules.xFolderID ON wdivzylm_utilitywizards.Users.ID = Modules_1.xSupervisorID LEFT OUTER JOIN
                         wdivzylm_utilitywizards.Users AS Users_1 ON wdivzylm_utilitywizards.Modules.xSupervisorID = Users_1.ID RIGHT OUTER JOIN
                         wdivzylm_utilitywizards.Users AS Users_2 RIGHT OUTER JOIN
                         wdivzylm_utilitywizards.vwModuleData ON Users_2.ID = wdivzylm_utilitywizards.vwModuleData.xTechnicianID ON wdivzylm_utilitywizards.Modules.ID = wdivzylm_utilitywizards.vwModuleData.xModuleID
WHERE        (wdivzylm_utilitywizards.Modules.Active = 1) AND (wdivzylm_utilitywizards.vwModuleData.Active = 1) AND (wdivzylm_utilitywizards.vwModuleData.xStatus = 0)

GO
/****** Object:  View [wdivzylm_utilitywizards].[vwUserInfo]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [wdivzylm_utilitywizards].[vwUserInfo]
AS
SELECT        wdivzylm_utilitywizards.Users.ID, wdivzylm_utilitywizards.Users.xName, wdivzylm_utilitywizards.Users.xEmail, wdivzylm_utilitywizards.Users.xMobileUsername, wdivzylm_utilitywizards.Users.xPassword, wdivzylm_utilitywizards.Users.xPermissions, wdivzylm_utilitywizards.Users.xClientID, wdivzylm_utilitywizards.Users.xActive, wdivzylm_utilitywizards.Users.xMobileDeviceId, 
                         wdivzylm_utilitywizards.Users.xApiEnabled, wdivzylm_utilitywizards.Users.xWebEnabled, wdivzylm_utilitywizards.Users.xOneSignalUserId, wdivzylm_utilitywizards.Users.xMobileDeviceType, wdivzylm_utilitywizards.Users.xSupervisorID, wdivzylm_utilitywizards.Users.xmlData AS UserXmlData, 
                         wdivzylm_utilitywizards.Clients.xName AS xClientName, wdivzylm_utilitywizards.Clients.xModules AS xClientModules, wdivzylm_utilitywizards.Clients.xActive AS xClientActive, wdivzylm_utilitywizards.Clients.xApproved AS xClientApproved, wdivzylm_utilitywizards.Clients.xmlData AS ClientXmlData
FROM            wdivzylm_utilitywizards.Users INNER JOIN
                         wdivzylm_utilitywizards.Clients ON wdivzylm_utilitywizards.Users.xClientID = wdivzylm_utilitywizards.Clients.ID

GO
/****** Object:  View [wdivzylm_utilitywizards].[vwCustomerSearch]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [wdivzylm_utilitywizards].[vwCustomerSearch]
AS
SELECT        wdivzylm_utilitywizards.Customers.ID AS CustomerID, wdivzylm_utilitywizards.Customers.dtInserted, wdivzylm_utilitywizards.Customers.dtUpdated, wdivzylm_utilitywizards.Customers.xmlData, wdivzylm_utilitywizards.Customers.Active, wdivzylm_utilitywizards.Customers.xClientID, wdivzylm_utilitywizards.Customers.xAccount AS Account, 
                         wdivzylm_utilitywizards.Customers.xFullName AS FullName, wdivzylm_utilitywizards.Locations.ID AS LocationID, wdivzylm_utilitywizards.Locations.xLocationID, wdivzylm_utilitywizards.Locations.xServiceAddress AS ServiceAddress, wdivzylm_utilitywizards.Locations.xCity AS City, 
                         wdivzylm_utilitywizards.Locations.xState AS State, wdivzylm_utilitywizards.Locations.xZipCode AS ZipCode, wdivzylm_utilitywizards.Locations.xSearchAddress AS SearchAddress
FROM            wdivzylm_utilitywizards.Customers FULL OUTER JOIN
                         wdivzylm_utilitywizards.Locations ON wdivzylm_utilitywizards.Customers.xLocationID = wdivzylm_utilitywizards.Locations.xLocationID
WHERE        (wdivzylm_utilitywizards.Customers.Active = 1)

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Questions]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Questions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
	[Active] [bit] NULL,
	[xModuleID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemQuestion','ModuleID')),(0))) PERSISTED NOT NULL,
	[xDataFieldName]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemQuestion','DataFieldName')) PERSISTED,
	[xQuestion]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemQuestion','Question')) PERSISTED,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [wdivzylm_utilitywizards].[vwQuestions]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [wdivzylm_utilitywizards].[vwQuestions]
AS
SELECT        wdivzylm_utilitywizards.Questions.ID, wdivzylm_utilitywizards.Questions.xmlData, wdivzylm_utilitywizards.Questions.dtUpdated, wdivzylm_utilitywizards.Questions.Active, wdivzylm_utilitywizards.Questions.xModuleID, wdivzylm_utilitywizards.Questions.xQuestion, wdivzylm_utilitywizards.Modules.xName AS ModuleName, 
                         wdivzylm_utilitywizards.Modules.xType AS ModuleType
FROM            wdivzylm_utilitywizards.Questions INNER JOIN
                         wdivzylm_utilitywizards.Modules ON wdivzylm_utilitywizards.Questions.xModuleID = wdivzylm_utilitywizards.Modules.ID

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Reports]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Reports](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
	[Active] [bit] NULL,
	[xClientID]  AS (isnull(CONVERT([int],[wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemReport','ClientID')),(0))) PERSISTED NOT NULL,
	[xName]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemReport','Name')) PERSISTED,
	[xDescription]  AS ([wdivzylm_utilitywizards].[GetElementValue2]([xmlData],'SystemReport','Description')) PERSISTED,
	[xFields]  AS ([wdivzylm_utilitywizards].[GetElement]([xmlData],'SystemReport','Fields')),
 CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [wdivzylm_utilitywizards].[GetXmlValue]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		James Davis
-- Create date: 5/2/2013
-- Description:	Return a value from a specified xml field
-- =============================================
CREATE FUNCTION [wdivzylm_utilitywizards].[GetXmlValue] 
(
	-- Add the parameters for the function here
	@xml xml,
	@field as nvarchar(255)
)
RETURNS nvarchar(MAX)
WITH SCHEMABINDING
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result nvarchar(MAX),
			@tmpElement nvarchar(MAX)

	-- Add the T-SQL statements to compute the return value here
	SET @tmpElement = CAST(@xml.query('//*[local-name()=sql:variable("@field")]') as nvarchar(MAX))
	
	-- If we are looking at a LOB then check the old and new xml fields
	IF @field = 'LOB'
	BEGIN
		SET @tmpElement = CAST(ISNULL(@xml.value('(/Data/LOB/text())[1]', 'varchar(255)'), @xml.value('(/Data/LoB/text())[1]', 'varchar(255)')) as nvarchar(MAX))
	END
	
	-- if we are looking at the LOBID then check the old and new xml fields
	IF @field = 'LOBID'
	BEGIN
		SET @tmpElement = CAST(ISNULL(@xml.value('(/Data/LOBID/text())[1]', 'varchar(255)'), @xml.value('(/Data/LoBID/text())[1]', 'varchar(255)')) as nvarchar(MAX))
	END

	-- remove the element open and close tags
	SET @tmpElement	= REPLACE(@tmpElement, @field, '')
	SET @tmpElement	= REPLACE(@tmpElement, '<>', '')
	SET @tmpElement	= REPLACE(@tmpElement, '</>', '')
	
	SELECT @Result = @tmpElement

	-- Return the result of the function
	RETURN @Result

END


GO
/****** Object:  Table [wdivzylm_utilitywizards].[Sys_ErrorLog]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Sys_ErrorLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NULL,
	[xmlData] [xml] NULL,
	[dtInserted] [datetime] NULL,
	[dtUpdated] [datetime] NULL,
	[insertedBy] [int] NULL,
	[updatedBy] [int] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Sys_MaintenanceMode]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Sys_MaintenanceMode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Web] [bit] NULL,
	[Android] [bit] NULL,
	[iOS] [bit] NULL,
	[Notes] [nvarchar](max) NULL,
	[dtStart] [datetime] NULL,
	[dtEnd] [datetime] NULL,
 CONSTRAINT [PK_MaintenanceMode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [wdivzylm_utilitywizards].[Sys_News]    Script Date: 12/15/2016 10:25:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [wdivzylm_utilitywizards].[Sys_News](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[htmlData] [nvarchar](max) NULL,
	[dtInserted] [datetime] NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [XML_IX_Clients]    Script Date: 12/15/2016 10:25:39 AM ******/
CREATE PRIMARY XML INDEX [XML_IX_Clients] ON [wdivzylm_utilitywizards].[Clients]
(
	[xmlData]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [XML_IX_Customers]    Script Date: 12/15/2016 10:25:39 AM ******/
CREATE PRIMARY XML INDEX [XML_IX_Customers] ON [wdivzylm_utilitywizards].[Customers]
(
	[xmlData]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procApproveClient]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 2/7/2016
-- Description:	Approve client record
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procApproveClient] 
	-- Add the parameters for the stored procedure here
	@clientID int = 0,
	@value nvarchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [Clients] SET xmlData.modify('replace value of (/Data/Approved/text())[1] with sql:variable("@value")') WHERE [ID] = @clientID;
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procClientDBStats]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 7/25/2016
-- Description:	Client Database Stats
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procClientDBStats] 
	-- Add the parameters for the stored procedure here
	@clientID int = 0

AS
BEGIN
	DECLARE @ClientName nvarchar(255),
			@Modules int,
			@WorkOrders int,
			@Reports int,
			@Users int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @ClientName = [xName] FROM [Clients] WHERE [ID] = @clientID;
	SELECT @Modules = COUNT([ID]) FROM [Modules] WHERE [xClientID] = @clientID;
	SELECT @WorkOrders = COUNT([ID]) FROM [ModuleData] WHERE [xClientID] = @clientID;
	SELECT @Reports = COUNT([ID]) FROM [Reports] WHERE [xClientID] = @clientID;
	SELECT @Users = COUNT([ID]) FROM [Users] WHERE [xClientID] = @clientID;

    -- Insert statements for procedure here
	SELECT @ClientName AS [Client], @Modules AS [Modules], @WorkOrders AS [Workorders], @Reports AS [Reports], @Users AS [Users];
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procCloseWorkOrder_M]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 5/9/2016
-- Description:	Update Technician Comments and Status for Work Orders from Mobile
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procCloseWorkOrder_M] 
	-- Add the parameters for the stored procedure here
	@deviceId nvarchar(50),
	@id int,
	@status int = 0, 
	@comments nvarchar(max) = ''
AS
BEGIN
    -- Insert statements for procedure here
	DECLARE @userId int,
			@statusExists int,
			@commentsExist int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT @userId = [ID] FROM [Users] WHERE [xMobileDeviceId] LIKE @deviceId;
	SELECT @statusExists = xmlData.exist('//Status') FROM [ModuleData] WHERE [ID] = @id;
	SELECT @commentsExist = xmlData.exist('//TechnicianComments') FROM [ModuleData] WHERE [ID] = @id;

	IF @statusExists = 0
	BEGIN
		UPDATE [ModuleData] SET xmlData.modify('insert <Status>0</Status> into (/Data)[1]'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;
	END
	UPDATE [ModuleData] SET xmlData.modify('replace value of (/Data/Status/text())[1] with sql:variable("@status")'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;

	IF @commentsExist = 1
	BEGIN
		UPDATE [ModuleData] SET xmlData.modify('delete (/Data/TechnicianComments)[1]'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;
	END
	UPDATE [ModuleData] SET xmlData.modify('insert <TechnicianComments>{sql:variable("@comments")}</TechnicianComments> into (/Data)[1]'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;

	UPDATE [ModuleData] SET xmlData.modify('delete (/Data/ViewedOnMobile)[1]'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;
	UPDATE [ModuleData] SET xmlData.modify('insert <ViewedOnMobile>1</ViewedOnMobile> into (/Data)[1]'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;

	--UPDATE [ModuleData] SET xmlData.modify('replace value of (/Data/TechnicianComments/text())[1] with sql:variable("@comments")'), dtUpdated = GETDATE(), updatedBy = @userId WHERE [ID] = @id;

	--SELECT @userId AS [UserID], @id AS [ID], @status AS [Status], @comments AS [Comments], @statusExists AS [StatusExists], @commentsExist AS [CommentsExist]
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procCopyClientModule]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 2/9/2016
-- Description:	Copy a module to a new client
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procCopyClientModule] 
	-- Add the parameters for the stored procedure here
	@moduleID int = 0, 
	@newClientID int = 0,
	@folderID int
AS
BEGIN
	DECLARE @newModuleIdentity int,
			@minQuestionID int,
			@maxQuestionID int,
			@Iterator int,
			@newQuestionIdentity int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT [Modules] SELECT [xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active] FROM [Modules] WHERE [ID] = @moduleID;
	SET @newModuleIdentity = SCOPE_IDENTITY()

	UPDATE [Modules] SET xmlData.modify('replace value of (/Data/ID/text())[1] with sql:variable("@newModuleIdentity")') WHERE [ID] = @newModuleIdentity;
	UPDATE [Modules] SET xmlData.modify('replace value of (/Data/ClientID/text())[1] with sql:variable("@newClientID")') WHERE [ID] = @newModuleIdentity;
	UPDATE [Modules] SET xmlData.modify('replace value of (/Data/FolderID/text())[1] with sql:variable("@folderID")') WHERE [ID] = @newModuleIdentity;

	SELECT @minQuestionID = MIN([ID]), @maxQuestionID = MAX([ID]) FROM [Questions] WHERE [xModuleID] = @moduleID;

	SET @Iterator = @minQuestionID

	WHILE (@Iterator <= @maxQuestionID)
	BEGIN
		INSERT [Questions] SELECT [xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active] FROM [Questions] WHERE [ID] = @Iterator;
		SET @newQuestionIdentity = SCOPE_IDENTITY()

		UPDATE [Questions] SET xmlData.modify('replace value of (/Data/ID/text())[1] with sql:variable("@newQuestionIdentity")') WHERE [ID] = @newQuestionIdentity;
		UPDATE [Questions] SET xmlData.modify('replace value of (/Data/ModuleID/text())[1] with sql:variable("@newModuleIdentity")') WHERE [ID] = @newQuestionIdentity;
		UPDATE [Questions] SET xmlData.modify('replace value of (/Data/ClientID/text())[1] with sql:variable("@newClientID")') WHERE [ID] = @newQuestionIdentity;

		SET @Iterator = @Iterator + 1
	END 	
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procCopyQuestion]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 2/9/2016
-- Description:	Copy a question from one module to another
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procCopyQuestion] 
	-- Add the parameters for the stored procedure here
	@questionID int = 0, 
	@newModuleID int = 0
AS
BEGIN
	DECLARE @newIdentity int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT [Questions] SELECT [xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active] FROM [Questions] WHERE [ID] = @questionID;
	SET @newIdentity = SCOPE_IDENTITY()

	UPDATE [Questions] SET xmlData.modify('replace value of (/Data/ModuleID/text())[1] with sql:variable("@newModuleID")') WHERE [ID] = @newIdentity;
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procCustomerSearch]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 2/6/2016
-- Description:	Search Customer Account Records
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procCustomerSearch] 
	-- Add the parameters for the stored procedure here
	@CustAcctNum nvarchar(20) = '', 
	@ServiceAddress nvarchar(255) = ''
AS
BEGIN
	DECLARE @sql nvarchar(max) = ''

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @CustAcctNum <> ''
	BEGIN
		SET @sql = '[Account] LIKE ''%' + @CustAcctNum + '%'''
	END

	IF @ServiceAddress <> ''
	BEGIN
		IF @sql = ''
		BEGIN
			SET @sql = '[ServiceAddress] LIKE ''%' + @ServiceAddress + '%'''
		END
		ELSE
		BEGIN
			SET @sql = @sql + ' OR [ServiceAddress] LIKE ''%' + @ServiceAddress + '%'''
		END
	END

	IF @sql = ''
	BEGIN
		SET @sql = 'SELECT [Account] AS [Account ID], [xLocationID] AS [Location ID], [FullName] AS [Full Name], [ServiceAddress] AS [Service Address], [City], [State], [ZipCode] AS [Zip Code] FROM [vwCustomerSearch] ORDER BY [Account]'
	END
	ELSE
	BEGIN
		SET @sql = 'SELECT [Account] AS [Account ID], [xLocationID] AS [Location ID], [FullName] AS [Full Name], [ServiceAddress] AS [Service Address], [City], [State], [ZipCode] AS [Zip Code] FROM [vwCustomerSearch] WHERE ' + @sql + ' ORDER BY [xLocationID]'
	END

	EXECUTE sp_executesql @sql
	--SELECT @CustAcctNum AS CustAcctNum, @ServiceAddress AS ServiceAddress, @sql AS generatedSQL
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procCustomerSearch_new]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 7/28/2016
-- Description:	Customer Search
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procCustomerSearch_new] 
	-- Add the parameters for the stored procedure here
	@account nvarchar(255) = '', 
	@serviceAddress nvarchar(255) = ''
AS
BEGIN
	DECLARE @acctCount int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @acctCount = COUNT([Account]) FROM [vwCustomerSearch] WHERE [Account] LIKE @account;
	IF @acctCount > 0
	BEGIN
		SELECT * FROM [vwCustomerSearch] WHERE [Account] LIKE @account;
	END
	ELSE
	BEGIN
		SELECT * FROM [vwCustomerSearch] WHERE [ServiceAddress] LIKE @serviceAddress;
	END
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procMarkAsViewed_M]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 5/9/2016
-- Description:	Updated the Viewed Flag for Work Orders from Mobile
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procMarkAsViewed_M] 
	-- Add the parameters for the stored procedure here
	@deviceId nvarchar(50) = ''
AS
BEGIN
    -- Insert statements for procedure here
	DECLARE @userId int,
			@viewedExists int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT @userId = [ID] FROM [Users] WHERE [xMobileDeviceId] LIKE @deviceId;

	UPDATE [ModuleData] SET xmlData.modify('delete (/Data/ViewedOnMobile)[1]') WHERE [xTechnicianID] = @userId;
	UPDATE [ModuleData] SET xmlData.modify('insert <ViewedOnMobile>1</ViewedOnMobile> into (/Data)[1]') WHERE [xTechnicianID] = @userId;

	--SELECT @viewedExists = xmlData.exist('//ViewedOnMobile') FROM [ModuleData] WHERE [xViewed] = 0;

	--IF @viewedExists = 0
	--BEGIN
	--	UPDATE [ModuleData] SET xmlData.modify('insert <ViewedOnMobile>0</ViewedOnMobile> into (/Data)[1]') WHERE [xViewed] = 0;
	--END

	--UPDATE [ModuleData] SET xmlData.modify('replace value of (/Data/ViewedOnMobile/text())[1] with 1'), [dtUpdated] = GETDATE(), [updatedBy] = @userId WHERE [xTechnicianID] = @userId;
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procMoveModule]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 1/28/2016
-- Description:	Change folder for one or all modules in a folder
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procMoveModule] 
	-- Add the parameters for the stored procedure here
	@moduleID int = 0, 
	@oldFolderID int = 0,
	@newFolderID int = 0,
	@moveAll bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @moveAll = 1
	BEGIN
		UPDATE [Modules] SET xmlData.modify('replace value of (/Data/FolderID/text())[1] with sql:variable("@newFolderID")') WHERE [xFolderID] = @oldFolderID;
	END
	ELSE
	BEGIN
		UPDATE [Modules] SET xmlData.modify('replace value of (/Data/FolderID/text())[1] with sql:variable("@newFolderID")') WHERE [ID] = @moduleID;
	END
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procQuestionsAnswers]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 2/21/2016
-- Description:	Get the questions and answers for a record
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procQuestionsAnswers] 
	-- Add the parameters for the stored procedure here
	@id int = 0,
	@section nvarchar(255)
AS
BEGIN
	DECLARE @xml xml,
			@totalQuestions int,
			@Iterator int

	DECLARE @elementsTable TABLE([name] varchar(255) NOT NULL);
	DECLARE @resultsTable TABLE([Question] varchar(255) NOT NULL, [Answer] varchar(max) NOT NULL);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @xml = [xmlData] FROM [ModuleData] WHERE [ID] = @id;

	WITH Xml_CTE AS
	(
		SELECT
			CAST('/' + node.value('fn:local-name(.)',
				'varchar(100)') AS varchar(100)) AS name,
			node.query('*') AS children
		FROM @xml.nodes('/*') AS roots(node)

		UNION ALL

		SELECT
			CAST(x.name + '/' + 
				node.value('fn:local-name(.)', 'varchar(100)') AS varchar(100)),
			node.query('*') AS children
		FROM Xml_CTE x
		CROSS APPLY x.children.nodes('*') AS child(node)
	)
	INSERT INTO @elementsTable SELECT DISTINCT [name] FROM Xml_CTE OPTION (MAXRECURSION 1000);

	IF @section = ''
	BEGIN
		SET @section = '%'
	END

	SELECT @totalQuestions = COUNT(*) FROM @elementsTable WHERE [name] LIKE '/Data/' + @section + '%';

	SET @Iterator = 0

	WHILE (@Iterator <= @totalQuestions)
	BEGIN
		DECLARE @Question nvarchar(255),
				@Answer nvarchar(max)

		--SELECT @Question = ''

		--INSERT INTO @

		SET @Iterator = @Iterator + 1
	END
	--SELECT
	--	Tbl.Col.value('IdInvernadero[1]', 'smallint'),  
	--	Tbl.Col.value('IdProducto[1]', 'smallint'),  
	--	Tbl.Col.value('IdCaracteristica1[1]', 'smallint'),
	--	Tbl.Col.value('IdCaracteristica2[1]', 'smallint'),
	--	Tbl.Col.value('Cantidad[1]', 'int'),
	--	Tbl.Col.value('Folio[1]', 'varchar(7)')
	--FROM   @xml.nodes('//row') Tbl(Col)  

	SELECT * FROM @resultsTable;
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procRefreshXmlIDs]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		James Davis
-- Create date: 2/1/2016
-- Description:	Update ID's in the Xml
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procRefreshXmlIDs] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [Clients] SET xmlData.modify('delete (/SystemClient/ID)[1]');
	UPDATE [Clients] SET xmlData.modify('insert <ID>1</ID> into (/SystemClient)[1]');
	UPDATE [Clients] SET xmlData.modify('replace value of (/SystemClient/ID/text())[1] with sql:column("Clients.ID")');

	UPDATE [ModuleData] SET xmlData.modify('delete (/Data/ID)[1]');
	UPDATE [ModuleData] SET xmlData.modify('insert <ID>1</ID> into (/Data)[1]');
	UPDATE [ModuleData] SET xmlData.modify('replace value of (/Data/ID/text())[1] with sql:column("ModuleData.ID")');

	UPDATE [Modules] SET xmlData.modify('delete (/SystemModule/ID)[1]');
	UPDATE [Modules] SET xmlData.modify('insert <ID>1</ID> into (/SystemModule)[1]');
	UPDATE [Modules] SET xmlData.modify('replace value of (/SystemModule/ID/text())[1] with sql:column("Modules.ID")');

	UPDATE [Questions] SET xmlData.modify('delete (/SystemQuestion/ID)[1]');
	UPDATE [Questions] SET xmlData.modify('insert <ID>1</ID> into (/SystemQuestion)[1]');
	UPDATE [Questions] SET xmlData.modify('replace value of (/SystemQuestion/ID/text())[1] with sql:column("Questions.ID")');

	UPDATE [Reports] SET xmlData.modify('delete (/SystemReport/ID)[1]');
	UPDATE [Reports] SET xmlData.modify('insert <ID>1</ID> into (/SystemReport)[1]');
	UPDATE [Reports] SET xmlData.modify('replace value of (/SystemReport/ID/text())[1] with sql:column("Reports.ID")');

	UPDATE [Users] SET xmlData.modify('delete (/SystemUser/ID)[1]');
	UPDATE [Users] SET xmlData.modify('insert <ID>1</ID> into (/SystemUser)[1]');
	UPDATE [Users] SET xmlData.modify('replace value of (/SystemUser/ID/text())[1] with sql:column("Users.ID")');

END


GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procReportGrid]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 6/12/2016
-- Description:	Procedure for creating the report grid
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procReportGrid] 
	-- Add the parameters for the stored procedure here
	@fields nvarchar(max) = '',
	@moduleID int = 0
AS
BEGIN
	DECLARE @sql nvarchar(max)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--xmlData.value('(/Data/something/text())[1]', 'varchar(255)') AS Field1

	SET @sql = 'SELECT ' + @fields + ' FROM [ModuleData] WHERE [Active] = 1 AND [xModuleID] = ' + CAST(@moduleID AS nvarchar(10))

	EXECUTE sp_executesql @sql
	--SELECT @sql AS [generatedSQL], @fields AS [Fields], @moduleID AS [ModuleID]
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procSearchGrid]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 1/31/2016
-- Description:	Results for the Search Grid
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procSearchGrid] 
	-- Add the parameters for the stored procedure here
	@fields nvarchar(max) = '',
	@clientID nvarchar(5) = '0',
	@moduleID nvarchar(5) = '0',
	@CustAcctNum nvarchar(20) = '',
	@id nvarchar(10) = '0'
AS
BEGIN
	DECLARE @sql nvarchar(max)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--xmlData.value('(/Data/something/text())[1]', 'varchar(255)') AS Field1

	SET @sql = 'SELECT [ID], [xCustAcctNum] AS [Customer Account], CASE WHEN [xPriority] = 0 THEN ''Normal'' ELSE ''Emergency'' END AS [Priority], [xStatus] AS [Status]'
	IF @fields <> ''
	BEGIN
		SET @sql = @sql + ', ' + @fields
	END

	IF @id = 0
	BEGIN
		IF @CustAcctNum = ''
		BEGIN
			SET @sql = @sql + ', [dtUpdated] AS [Updated], [xUserEmail] AS [Updated By] FROM [ModuleData] WHERE [Active] = 1 AND [xClientID] = ' + @clientID + ' AND [xModuleID] = ' + @moduleID
		END
		ELSE
		BEGIN
			SET @sql = @sql + ', [dtUpdated] AS [Updated], [xUserEmail] AS [Updated By] FROM [ModuleData] WHERE [Active] = 1 AND [xCustAcctNum] LIKE ''%' + @CustAcctNum + '%'' AND [xClientID] = ' + @clientID + ' AND [xModuleID] = ' + @moduleID
		END
	END
	ELSE
	BEGIN
		SET @sql = @sql + ', [dtUpdated] AS [Updated], [xUserEmail] AS [Updated By] FROM [ModuleData] WHERE [Active] = 1 AND [ID] = ' + @id
	END
	
	SET @sql = @sql + ' ORDER BY [ID] DESC;'

	EXECUTE sp_executesql @sql
	--SELECT @sql AS [generatedSQL], @fields AS [Fields], @clientID AS [ClientID], @moduleID AS [ModuleID], @CustAcctNum AS [CustAcctNum], @id AS [ID]
END

GO
/****** Object:  StoredProcedure [wdivzylm_utilitywizards].[procTester]    Script Date: 12/15/2016 10:25:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		James Davis
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [wdivzylm_utilitywizards].[procTester] 
	-- Add the parameters for the stored procedure here
	@p1 int = 0, 
	@p2 int = 0
AS
BEGIN
	DECLARE @minQuestionID int,
			@maxQuestionID int,
			@Iterator int,
			@counter int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @minQuestionID = MIN([ID]), @maxQuestionID = MAX([ID]) FROM [Questions] WHERE [xModuleID] = 11

	SET @Iterator = @minQuestionID
	SET @counter = 0

	WHILE (@Iterator <= @maxQuestionID)
	BEGIN
		Set @Iterator = @Iterator + 1
		SET @counter = @counter + 1
	END 	
	
	SELECT @minQuestionID, @maxQuestionID, @counter

END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[42] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Customers"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 225
               Right = 250
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 24
               Left = 317
               Bottom = 275
               Right = 490
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4830
         Alias = 1410
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwCustomerSearch'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwCustomerSearch'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[43] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 361
               Right = 430
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Clients"
            Begin Extent = 
               Top = 6
               Left = 468
               Bottom = 240
               Right = 638
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Sys_History"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1050
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwHistory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwHistory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[55] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 64
               Left = 993
               Bottom = 194
               Right = 1177
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "Modules_1"
            Begin Extent = 
               Top = 14
               Left = 790
               Bottom = 318
               Right = 960
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Modules"
            Begin Extent = 
               Top = 81
               Left = 429
               Bottom = 384
               Right = 599
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Users_1"
            Begin Extent = 
               Top = 344
               Left = 685
               Bottom = 474
               Right = 869
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "Users_2"
            Begin Extent = 
               Top = 290
               Left = 230
               Bottom = 420
               Right = 414
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "vwModuleData"
            Begin Extent = 
               Top = 9
               Left = 15
               Bottom = 360
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWi' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwMobileWorkOrders'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'dths = 11
         Column = 8430
         Alias = 2145
         Table = 1200
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwMobileWorkOrders'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwMobileWorkOrders'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[40] 4[35] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[39] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Locations"
            Begin Extent = 
               Top = 6
               Left = 481
               Bottom = 245
               Right = 654
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Customers"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 244
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ModuleData"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 245
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 7
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1485
         Alias = 1845
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwModuleData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwModuleData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[56] 3) )"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[41] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Questions"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 245
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Modules"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 314
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1320
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwQuestions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwQuestions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[44] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 363
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Clients"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 244
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1650
         Alias = 1545
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwUserInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'wdivzylm_utilitywizards', @level1type=N'VIEW',@level1name=N'vwUserInfo'
GO
USE [master]
GO
ALTER DATABASE [wdivzylm_utilitywizards] SET  READ_WRITE 
GO
