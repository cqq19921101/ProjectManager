USE [NPI_REPORT]
GO
/****** Object:  Table [dbo].[TB_NPI_Step_Handler]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_Step_Handler](
	[CASEID] [int] NULL,
	[FORMNO] [varchar](20) NULL,
	[STEP_NAME] [varchar](20) NULL,
	[DEPT] [varchar](50) NULL,
	[HANDLER] [varchar](20) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[APPROVE_TIME] [datetime] NULL,
	[APPROVE_RESULT] [varchar](50) NULL,
	[APPROVE_REMARK] [varchar](max) NULL,
	[SIGN_FLAG] [nchar](10) NULL,
	[SEQ] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_MEMBER]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_MEMBER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BU] [varchar](50) NULL,
	[BUILDING] [varchar](20) NULL,
	[CATEGORY] [varchar](50) NULL,
	[DEPT] [nvarchar](50) NULL,
	[ENAME] [varchar](30) NULL,
	[CNAME] [nvarchar](20) NULL,
	[EMAIL] [varchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_FMEA]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_FMEA](
	[SubNo] [varchar](50) NULL,
	[Item] [varchar](50) NULL,
	[Source] [nvarchar](400) NULL,
	[Stantion] [varchar](400) NULL,
	[PotentialFailureMode] [nvarchar](400) NULL,
	[Loess] [nvarchar](400) NULL,
	[Sev] [int] NULL,
	[PotentialFailure] [nvarchar](400) NULL,
	[Occ] [int] NULL,
	[CurrentControls] [nvarchar](400) NULL,
	[DET] [int] NULL,
	[RPN] [int] NULL,
	[RecommendedAction] [nvarchar](400) NULL,
	[Resposibility] [nvarchar](50) NULL,
	[TargetCompletionDate] [datetime] NULL,
	[ActionsTaken] [nvarchar](400) NULL,
	[ResultsSev] [int] NULL,
	[ResultsOcc] [int] NULL,
	[ResultsDet] [int] NULL,
	[ResultsRPN] [int] NULL,
	[WriteDept] [nvarchar](50) NULL,
	[ReplyDept] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Update_User] [varchar](50) NULL,
	[Update_Time] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FILE_PATH] [nvarchar](100) NULL,
	[FILE_NAME] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_NPI_FMEA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_DOAConfig]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_DOAConfig](
	[BU] [varchar](10) NOT NULL,
	[BUILDING] [varchar](10) NULL,
	[PHASE] [varchar](10) NULL,
	[STEP_NAME] [nvarchar](50) NOT NULL,
	[SEQ] [int] NULL,
	[JUMP_OPTION] [varchar](1) NULL,
	[CHECK_OPTION] [varchar](1) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[ENABLED] [varchar](1) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_DOA_DETAIL]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_DOA_DETAIL](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BU] [varchar](50) NULL,
	[BUILDING] [varchar](20) NULL,
	[STEP_NAME] [varchar](50) NULL,
	[SEQ] [int] NULL,
	[DEPT] [nvarchar](50) NULL,
	[ENAME] [varchar](30) NULL,
	[CNAME] [nvarchar](20) NULL,
	[EMAIL] [varchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[PHASE] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_CTQ]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_CTQ](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BU] [varchar](20) NULL,
	[BUILDING] [varchar](20) NULL,
	[PROD_GROUP] [varchar](50) NULL,
	[PHASE] [varchar](50) NULL,
	[DEPT] [nvarchar](50) NULL,
	[PROCESS] [nvarchar](50) NULL,
	[CTQ] [nvarchar](50) NULL,
	[UNIT] [varchar](20) NULL,
	[SPC] [nvarchar](10) NULL,
	[SPEC_LIMIT] [nvarchar](50) NULL,
	[CONTROL_TYPE] [varchar](10) NULL,
	[GOAL] [nvarchar](50) NULL,
	[SERVITY] [varchar](50) NULL,
	[flag] [nvarchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[ATTACHMENT_FLAG] [varchar](1) NULL,
 CONSTRAINT [PK_TB_NPI_CTQ] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_SUB]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_SUB](
	[DOC_NO] [varchar](30) NULL,
	[SUB_DOC_NO] [varchar](30) NULL,
	[SUB_DOC_PHASE] [varchar](10) NULL,
	[WorkOrder] [nvarchar](50) NULL,
	[SUB_DOC_PHASE_RESULT] [varchar](10) NULL,
	[SUB_DOC_PHASE_STATUS] [varchar](10) NULL,
	[SUB_DOC_PHASE_VERSION] [int] NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[DFX_STAUTS] [varchar](20) NULL,
	[CTQ_STATUS] [varchar](10) NULL,
	[CLCA_STATUS] [varchar](10) NULL,
	[ISSUES_STATUS] [varchar](10) NULL,
	[PFMA_STATUS] [varchar](10) NULL,
	[CREATE_DATE] [datetime] NULL,
	[CTQ_QTY] [int] NULL,
	[CLCA_QTY] [int] NULL,
	[CLCA_BEGIN_TIME] [varchar](20) NULL,
	[CLCA_END_TIME] [varchar](20) NULL,
	[LOT_QTY] [int] NULL,
	[PCB_REV] [varchar](50) NULL,
	[SPEC_REV] [varchar](50) NULL,
	[ISSUE_DATE] [datetime] NULL,
	[INPUT_DATE] [varchar](50) NULL,
	[CUSTOMER] [nvarchar](50) NULL,
	[LINE] [varchar](10) NULL,
	[BOM_REV] [varchar](50) NULL,
	[CUSTOMER_REV] [varchar](50) NULL,
	[RELEASET_DATE] [varchar](50) NULL,
	[PK_DATE] [varchar](50) NULL,
	[NeedStartItmes] [nvarchar](50) NULL,
	[PROD_GROUP] [nvarchar](50) NULL,
	[REMARKM] [nvarchar](2000) NULL,
	[STATUS] [nvarchar](50) NULL,
	[Result] [nvarchar](50) NULL,
	[CASEID] [int] NULL,
	[MODIFY_FLAG] [varchar](1) NULL,
	[FROMMODEL] [nvarchar](50) NULL,
	[REMARK] [nvarchar](2000) NULL,
	[MODELLINK] [nvarchar](2000) NULL,
	[PDF_FLAG] [nchar](1) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_RESULT]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_RESULT](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SUB_DOC_NO] [varchar](30) NULL,
	[CASEID] [int] NULL,
	[DEPT] [varchar](20) NULL,
	[REMARK] [nvarchar](50) NULL,
	[APPROVER] [varchar](50) NULL,
	[APPROVER_RESULT] [varchar](20) NULL,
	[APPROVER_OPINION] [varchar](2000) NULL,
	[APPROVER_Levels] [varchar](50) NULL,
	[APPROVER_DATE] [datetime] NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_PR_ATTACHFILE]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_PR_ATTACHFILE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SUB_DOC_NO] [nvarchar](50) NULL,
	[DEPT] [nvarchar](50) NULL,
	[CASEID] [int] NULL,
	[FILE_PATH] [nvarchar](2000) NULL,
	[FILE_NAME] [nvarchar](100) NULL,
	[FILE_REMARK] [nvarchar](2000) NULL,
	[UPLOADUSER] [nvarchar](50) NULL,
	[UPDATEUSER] [nvarchar](50) NULL,
	[UPLOAD_TIME] [datetime] NULL,
 CONSTRAINT [PK_TB_NPI_APP_PR_ATTACHFILE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_NPI_APP_MEMBER]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_MEMBER](
	[DOC_NO] [varchar](30) NULL,
	[Category] [varchar](30) NULL,
	[DEPT] [nvarchar](50) NULL,
	[WriteEname] [varchar](30) NULL,
	[WriteCname] [nvarchar](20) NULL,
	[WriteEmail] [varchar](50) NULL,
	[ReplyEName] [varchar](30) NULL,
	[ReplyCname] [varchar](50) NULL,
	[ReplyEmai] [varchar](50) NULL,
	[CheckedEName] [varchar](50) NULL,
	[CheckedEmail] [varchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[CategoryFlag] [varchar](1) NULL,
	[Flag] [varchar](2) NULL,
	[IsReply] [varchar](1) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_MAIN_HIS]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_MAIN_HIS](
	[DOC_NO] [varchar](30) NOT NULL,
	[BU] [varchar](20) NULL,
	[BUILDING] [varchar](20) NULL,
	[APPLY_DATE] [varchar](50) NULL,
	[APPLY_USERID] [varchar](30) NULL,
	[MODEL_NAME] [varchar](50) NULL,
	[CUSTOMER] [nvarchar](50) NULL,
	[PRODUCT_TYPE] [nvarchar](255) NULL,
	[LAYOUT] [nvarchar](255) NULL,
	[PHASE] [nvarchar](50) NULL,
	[NEXTSTAGE_DATE] [nvarchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[NPI_PM] [varchar](50) NULL,
	[SALES_OWNER] [varchar](50) NULL,
	[RD_ENGINEER] [varchar](50) NULL,
	[REMARK] [varchar](1000) NULL,
	[CASEID] [varchar](50) NULL,
	[STATUS] [varchar](20) NULL,
	[PM_LOC] [varchar](50) NULL,
	[PM_EXT] [varchar](20) NULL,
	[SALES_LOC] [varchar](50) NULL,
	[SALES_EXT] [varchar](50) NULL,
	[RD_LOC] [varchar](50) NULL,
	[RD_EXT] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_MAIN]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_MAIN](
	[DOC_NO] [varchar](30) NOT NULL,
	[BU] [varchar](20) NULL,
	[BUILDING] [varchar](20) NULL,
	[PROD_GROUP] [varchar](50) NULL,
	[APPLY_DATE] [datetime] NULL,
	[APPLY_USERID] [varchar](30) NULL,
	[MODEL_NAME] [varchar](50) NULL,
	[CUSTOMER] [nvarchar](50) NULL,
	[PRODUCT_DES] [nvarchar](255) NULL,
	[SALES_AREA] [nvarchar](255) NULL,
	[MP_DATE] [nvarchar](50) NULL,
	[PROJECT_CODE] [nvarchar](50) NULL,
	[TIME_QUANTITY] [nvarchar](255) NULL,
	[PRPhaseTime] [nvarchar](255) NULL,
	[OTHERS] [nvarchar](255) NULL,
	[PRODUCT_DES_REMARK] [nvarchar](255) NULL,
	[SALES_AREA_REMARK] [nvarchar](255) NULL,
	[MP_DATE_REMARK] [nvarchar](255) NULL,
	[PROJECT_CODE_REMARK] [nvarchar](255) NULL,
	[TIME_QUANTITY_REMARK] [nvarchar](255) NULL,
	[PRPhaseTime_Remark] [nvarchar](255) NULL,
	[OTHERS_REMARK] [nvarchar](255) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[NPI_PM] [varchar](50) NULL,
	[SALES_OWNER] [varchar](50) NULL,
	[ME_ENGINEER] [varchar](50) NULL,
	[EE_ENGINEER] [varchar](30) NULL,
	[CAD_ENGINEER] [varchar](50) NULL,
	[TP_PM] [varchar](20) NULL,
 CONSTRAINT [PK_TB_NPI_APP_MAIN] PRIMARY KEY CLUSTERED 
(
	[DOC_NO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_ISSUELIST_ATTACHFILE]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_ISSUELIST_ATTACHFILE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SUB_DOC_NO] [varchar](30) NULL,
	[CASEID] [int] NULL,
	[FILE_PATH] [varchar](255) NULL,
	[FILE_NAME] [varchar](100) NULL,
	[APPROVER] [varchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
 CONSTRAINT [PK_TB_NPI_APP_ISSUELIST_ATTACHFILE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_ISSUELIST]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_ISSUELIST](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](50) NULL,
	[SUB_DOC_NO] [nvarchar](50) NULL,
	[PHASE] [nvarchar](50) NULL,
	[STATION] [nvarchar](50) NULL,
	[ITEMS] [nvarchar](50) NULL,
	[PRIORITYLEVEL] [nvarchar](50) NULL,
	[ISSUE_DESCRIPTION] [nvarchar](2000) NULL,
	[ISSUE_LOSSES] [nvarchar](50) NULL,
	[TEMP_MEASURE] [nvarchar](50) NULL,
	[IMPROVE_MEASURE] [nvarchar](2000) NULL,
	[PERSON_IN_CHARGE] [nvarchar](50) NULL,
	[DUE_DAY] [nvarchar](50) NULL,
	[CURRENT_STATUS] [nvarchar](2000) NULL,
	[AFFIRMACE_MAN] [nvarchar](50) NULL,
	[STAUTS] [nvarchar](200) NULL,
	[TRACKING] [nvarchar](50) NULL,
	[REMARK] [nvarchar](255) NULL,
	[DEPT] [nvarchar](50) NULL,
	[CREATE_TIME] [datetime] NULL,
	[CREATE_USERID] [nvarchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [nvarchar](50) NULL,
	[FILE_PATH] [nvarchar](100) NULL,
	[FILE_NAME] [nvarchar](50) NULL,
	[CLASS] [nvarchar](10) NULL,
	[MEASURE_DEPTREPLY] [nvarchar](2000) NULL,
 CONSTRAINT [PK_TB_NPI_APP_ISSUELIST] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_NPI_APP_CTQ]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_CTQ](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [varchar](30) NULL,
	[SUB_DOC_NO] [varchar](30) NULL,
	[PROD_GROUP] [varchar](50) NULL,
	[PHASE] [varchar](50) NULL,
	[DEPT] [nvarchar](50) NULL,
	[PROCESS] [nvarchar](50) NULL,
	[CTQ] [nvarchar](50) NULL,
	[UNIT] [varchar](20) NULL,
	[SPC] [nvarchar](10) NULL,
	[SPEC_LIMIT] [nvarchar](50) NULL,
	[CONTROL_TYPE] [varchar](10) NULL,
	[GOAL] [nvarchar](50) NULL,
	[ACT] [nvarchar](20) NULL,
	[RESULT] [varchar](10) NULL,
	[Comment] [nvarchar](500) NULL,
	[flag] [nvarchar](50) NULL,
	[STATUS] [varchar](10) NULL,
	[DESCRIPTION] [nvarchar](255) NULL,
	[DUTY_DEPT] [varchar](50) NULL,
	[DUTY_EMP] [varchar](30) NULL,
	[ROOT_CAUSE] [nvarchar](255) NULL,
	[D] [varchar](1) NULL,
	[M] [varchar](1) NULL,
	[P] [varchar](1) NULL,
	[E] [varchar](1) NULL,
	[W] [varchar](1) NULL,
	[O] [varchar](1) NULL,
	[TEMPORARY_ACTION] [nvarchar](255) NULL,
	[CORRECTIVE_PREVENTIVE_ACTION] [nvarchar](255) NULL,
	[COMPLETE_DATE] [nvarchar](255) NULL,
	[IMPROVEMENT_STATUS] [nvarchar](255) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[REPLY_USERID] [varchar](30) NULL,
	[W_FILEPATH] [varchar](1000) NULL,
	[W_FILENAME] [varchar](1000) NULL,
	[R_FILEPATH] [varchar](500) NULL,
	[R_FILENAME] [varchar](200) NULL,
 CONSTRAINT [PK_TB_NPI_APP_CTQ_BACKUP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NPI_APP_ATTACHFILE]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_NPI_APP_ATTACHFILE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SUB_DOC_NO] [varchar](30) NULL,
	[CASEID] [int] NULL,
	[FILE_PATH] [varchar](255) NULL,
	[FILE_TYPE] [varchar](30) NULL,
	[FILE_NAME] [varchar](50) NULL,
	[DEPT] [varchar](20) NULL,
	[APPROVER] [varchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
	[UPDATE_USERID] [varchar](30) NULL,
	[Remark] [varchar](100) NULL,
	[APPROVER_DATE] [datetime] NULL,
	[APPROVER_OPINION] [varchar](100) NULL,
 CONSTRAINT [PK_TB_NPI_APP_ATTACHFILE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_GetNumber]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_GetNumber](
	[Code1] [varchar](50) NOT NULL,
	[Code2] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_DFX_VersionLog]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_DFX_VersionLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Building] [nvarchar](50) NULL,
	[DFXTypeB] [nvarchar](50) NULL,
	[DFXTypeE] [nvarchar](50) NULL,
	[Reason] [nvarchar](2000) NULL,
	[UpdateTime] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_DFX_VersionLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_DFX_Score]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_DFX_Score](
	[DFXNo] [nvarchar](50) NULL,
	[Stage] [nvarchar](50) NULL,
	[Dept] [nvarchar](50) NULL,
	[item] [nvarchar](50) NULL,
	[Score] [nvarchar](50) NULL,
	[PriorityLevel0] [nvarchar](50) NULL,
	[PriorityLevel1] [nvarchar](50) NULL,
	[PriorityLevel2] [nvarchar](50) NULL,
	[PriorityLevel3] [nvarchar](50) NULL,
	[Result] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_DFX_Picture]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_DFX_Picture](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Item] [nvarchar](50) NULL,
	[FILE_NAME] [nvarchar](200) NULL,
	[FILE_PATH] [nvarchar](2000) NULL,
	[UPDATE_USERID] [nvarchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
 CONSTRAINT [PK_TB_DFX_Picture] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_DFX_PARAM]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_DFX_PARAM](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FUNCTION_NAME] [varchar](50) NULL,
	[PARAME_NAME] [varchar](50) NULL,
	[PARAME_ITEM] [varchar](50) NULL,
	[PARAME_VALUE1] [varchar](50) NULL,
	[PARAME_VALUE2] [varchar](50) NULL,
	[PARAME_VALUE3] [varchar](50) NULL,
	[PARAME_VALUE4] [varchar](50) NULL,
	[PARAME_VALUE5] [varchar](50) NULL,
	[Building] [varchar](20) NULL,
	[UPDATE_USER] [varchar](50) NULL,
	[UPDATE_TIME] [datetime] NULL,
 CONSTRAINT [PK_TB_DFX_PARAM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_DFX_ItemBody_Back]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_DFX_ItemBody_Back](
	[ID] [int] NULL,
	[DFXNo] [varchar](50) NULL,
	[Item] [varchar](50) NULL,
	[ItemType] [varchar](50) NULL,
	[ItemName] [varchar](50) NULL,
	[Requirements] [nvarchar](1000) NULL,
	[Losses] [nvarchar](200) NULL,
	[Location] [nvarchar](200) NULL,
	[Severity] [nvarchar](20) NULL,
	[Occurrence] [nvarchar](20) NULL,
	[Detection] [nvarchar](20) NULL,
	[RPN] [nvarchar](20) NULL,
	[Class] [nvarchar](20) NULL,
	[PriorityLevel] [varchar](50) NULL,
	[MaxPoints] [varchar](50) NULL,
	[DFXPoints] [varchar](50) NULL,
	[WriteDept] [varchar](50) NULL,
	[Compliance] [varchar](10) NULL,
	[Comments] [nvarchar](200) NULL,
	[Status] [varchar](50) NULL,
	[Actions] [nvarchar](50) NULL,
	[CompletionDate] [nvarchar](50) NULL,
	[Tracking] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[FilePath] [varchar](100) NULL,
	[OldItemType] [nvarchar](200) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_DFX_ItemBody]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_DFX_ItemBody](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DFXNo] [varchar](50) NULL,
	[ItemID] [varchar](50) NULL,
	[Item] [varchar](50) NULL,
	[ItemType] [varchar](50) NULL,
	[ItemName] [varchar](50) NULL,
	[Requirements] [nvarchar](1000) NULL,
	[Losses] [nvarchar](200) NULL,
	[Location] [nvarchar](200) NULL,
	[Severity] [nvarchar](20) NULL,
	[Occurrence] [nvarchar](20) NULL,
	[Detection] [nvarchar](20) NULL,
	[RPN] [nvarchar](20) NULL,
	[Class] [nvarchar](20) NULL,
	[PriorityLevel] [varchar](50) NULL,
	[MaxPoints] [varchar](50) NULL,
	[DFXPoints] [varchar](50) NULL,
	[WriteDept] [varchar](50) NULL,
	[Compliance] [varchar](10) NULL,
	[Comments] [nvarchar](200) NULL,
	[Status] [varchar](50) NULL,
	[Actions] [nvarchar](50) NULL,
	[CompletionDate] [nvarchar](50) NULL,
	[Tracking] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[FilePath] [varchar](100) NULL,
	[OldItemType] [nvarchar](200) NULL,
 CONSTRAINT [PK_TB_DFX_ItemBody_O] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_DFX_Item]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_DFX_Item](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BU] [varchar](50) NULL,
	[Building] [varchar](50) NULL,
	[ItemID] [varchar](50) NULL,
	[Item] [varchar](50) NOT NULL,
	[ItemType] [varchar](50) NULL,
	[ItemName] [varchar](50) NULL,
	[Requirements] [nvarchar](1000) NULL,
	[ProductType] [nvarchar](50) NULL,
	[PriorityLevel] [int] NULL,
	[Losses] [nvarchar](200) NULL,
	[WriteDept] [varchar](50) NULL,
	[ReplyDept] [varchar](50) NULL,
	[FileName] [nvarchar](50) NULL,
	[FilePath] [nvarchar](200) NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[OldItemType] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_DFX_Item] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_DFX]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_DFX](
	[ID] [float] NULL,
	[BU] [nvarchar](255) NULL,
	[Building] [nvarchar](255) NULL,
	[Item] [nvarchar](255) NULL,
	[ItemType] [nvarchar](255) NULL,
	[ItemName] [nvarchar](255) NULL,
	[Requirements] [nvarchar](255) NULL,
	[ProductType] [nvarchar](255) NULL,
	[PriorityLevel] [float] NULL,
	[Losses] [nvarchar](255) NULL,
	[WriteDept] [nvarchar](255) NULL,
	[ReplyDept] [nvarchar](255) NULL,
	[FileName] [datetime] NULL,
	[FilePath] [datetime] NULL,
	[UpdateUser] [float] NULL,
	[UpdateTime] [float] NULL,
	[OldItemType] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_APPLICATION_PARAM]    Script Date: 07/08/2019 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_APPLICATION_PARAM](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[APPLICATION_NAME] [varchar](50) NULL,
	[FUNCTION_NAME] [varchar](50) NULL,
	[PARAME_NAME] [varchar](50) NULL,
	[PARAME_ITEM] [varchar](50) NULL,
	[PARAME_VALUE1] [nvarchar](50) NULL,
	[PARAME_VALUE2] [nvarchar](50) NULL,
	[PARAME_VALUE3] [nvarchar](50) NULL,
	[PARAME_VALUE4] [nvarchar](100) NULL,
	[REMARK] [varchar](50) NULL,
	[ENABLED] [char](1) NULL,
	[UPDATE_USERID] [varchar](25) NULL,
	[UPDATE_TIME] [datetime] NULL,
 CONSTRAINT [PK_TB_APPLICATION_PARAM] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[P_Get_Prelaucn_Report]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_Get_Prelaucn_Report]
(
  @P_BU VARCHAR(20),
  @P_BUILDING VARCHAR(20),
  @P_STAUTS  VARCHAR(10),
  @P_MODEL VARCHAR(30),
  @P_CUSTOMER VARCHAR(30),
  @P_FORMNO VARCHAR(30),
  @P_BEGDATE varchar(20),
  @P_ENDDATE varchar(20)
)
AS

BEGIN
	
   SET NOCOUNT ON;
   DECLARE @strwhere  varchar(1000)
   DECLARE @sql VARCHAR(8000)
    set @strwhere='AND T1.Bu='+quotename(@P_BU,'''') +' AND T1.Building='+quotename(@P_BUILDING,'''')
    SET @sql=''  --初始化变量@sql
IF @P_MODEL<>''
    SET @strwhere=@strwhere+' And T1.Model  like '+quotename('%'+@P_MODEL+'%','''')
IF @P_CUSTOMER<>''
    SET @strwhere=@strwhere+' And T1.Customer like '+quotename('%'+@P_CUSTOMER+'%','''')
IF @P_FORMNO<>''
    SET @strwhere=@strwhere+' And T1.PiolotRunNo='+quotename(@P_FORMNO,'''')
IF (@P_STAUTS<> '' and @P_STAUTS<>'ALL')
   SET @strwhere=@strwhere+' and T1.Status='+quotename(@P_STAUTS,'''')
IF (@P_BEGDATE<>'' AND @P_ENDDATE <>'')
   SET @strwhere=@strwhere+' AND convert(varchar(10),T1.Date,121)BETWEEN '+quotename(@P_BEGDATE,'''')+ ' AND'+quotename(@P_ENDDATE,'''')
ELSE IF(@P_BEGDATE<>'' AND @P_ENDDATE='')
     SET @strwhere=@strwhere+' AND convert(varchar(10),T1.Date,121)>='+quotename(@P_BEGDATE,'''')
ELSE IF(@P_BEGDATE='' AND @P_ENDDATE<>'') 
     SET @strwhere=@strwhere+' AND convert(varchar(10),T1.Date,121)<='+quotename(@P_ENDDATE,'''')
 


--SET @sql='SELECT DocNo,Plant,Building,Model,DEPT,Name,Quantity,CaseId,DN_NO, 
--      dbo.GetHandler(CaseID) AS handler,Status,AppDate,CaseID
--      FROM TB_ControlRun_Master'
SET @sql='SELECT * ,SPM.dbo.GetHandler(T1.CASEID) AS handler FROM TB_Prelaunch_Main T1'
if (@strwhere<>'')
   
   set @sql=@sql+' WHERE 1=1'+'' +@strwhere+' '+ 'ORDER BY  T1.Date DESC'

exec (@sql)
PRINT(@sql)
END
GO
/****** Object:  StoredProcedure [dbo].[P_GET_NPI_REPORT_HIS]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_GET_NPI_REPORT_HIS]
(
  @P_BU VARCHAR(20),
  @P_BUILDING  VARCHAR(20),
  @P_PHASE VARCHAR(20),
  @P_STAUTS  VARCHAR(10),
  @P_MODEL VARCHAR(30),
  @P_CUSTOMER VARCHAR(30),
  @P_FORMNO VARCHAR(30),
  @P_BEGDATE varchar(20),
  @P_ENDDATE varchar(20)
)
AS

BEGIN
	
   SET NOCOUNT ON;
   DECLARE @strwhere  varchar(1000)
   DECLARE @sql VARCHAR(8000)
    set @strwhere='AND BU='+quotename(@P_BU,'''')
    SET @sql=''  --初始化变量@sql
IF @P_MODEL<>''
    SET @strwhere=@strwhere+' And T1.MODEL_NAME  like '+quotename('%'+@P_MODEL+'%','''')
IF @P_BUILDING<>''
    SET @strwhere=@strwhere+' And T1.BUILDING  like '+quotename('%'+@P_BUILDING+'%','''')
IF @P_CUSTOMER<>''
    SET @strwhere=@strwhere+' And T1.CUSTOMER like  '+quotename('%'+@P_CUSTOMER+'%','''')
IF @P_PHASE<>''
    SET @strwhere=@strwhere+ ' AND T1.PHASE='+quotename(@P_PHASE,'''')
IF @P_FORMNO<>''
    SET @strwhere=@strwhere+' AND T1.DOC_NO LIKE  '+quotename('%'+@P_FORMNO+'%','''')
IF (@P_STAUTS<> '' and @P_STAUTS<>'ALL')
   SET @strwhere=@strwhere+' and T1.STATUS='+quotename(@P_STAUTS,'''')
IF (@P_BEGDATE<>'' AND @P_ENDDATE <>'')
   SET @strwhere=@strwhere+' AND convert(varchar(10),T1.APPLY_DATE,121)BETWEEN '+quotename(@P_BEGDATE,'''')+ ' AND'+quotename(@P_ENDDATE,'''')
ELSE IF(@P_BEGDATE<>'' AND @P_ENDDATE='')
     SET @strwhere=@strwhere+' AND convert(varchar(10),T1.APPLY_DATE,121)>='+quotename(@P_BEGDATE,'''')
ELSE IF(@P_BEGDATE='' AND @P_ENDDATE<>'') 
     SET @strwhere=@strwhere+' AND convert(varchar(10),T1.APPLY_DATE,121)<='+quotename(@P_ENDDATE,'''')
 


--SET @sql='SELECT DocNo,Plant,Building,Model,DEPT,Name,Quantity,CaseId,DN_NO, 
--      dbo.GetHandler(CaseID) AS handler,Status,AppDate,CaseID
--      FROM TB_ControlRun_Master'
SET @sql='SELECT T1.* , T2.APPROVE_RESULT AS result,SPM.dbo.GetHandler(T1.CASEID) AS handler FROM TB_NPI_APP_MAIN_HIS T1 WITH(NOLOCK) 
left join  TB_NPI_Step_Handler T2 on T1.DOC_NO = T2.FORMNO
where T2.STEP_NAME = ''LOB Head'''
if (@strwhere<>'')
   
   set @sql=@sql+'' +@strwhere+''+ 'ORDER BY  APPLY_DATE  DESC'

exec (@sql)
PRINT(@sql)
END
GO
/****** Object:  StoredProcedure [dbo].[P_GET_NPI_REPORT]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_GET_NPI_REPORT]
(
  @P_BU VARCHAR(20),
  @P_BUILDING VARCHAR(20),
  @P_STAUTS  VARCHAR(10),
  @P_MODEL VARCHAR(30),
  @P_CUSTOMER VARCHAR(30),
  @P_FORMNO VARCHAR(30),
  @P_ApplyUser VARCHAR(30),
  @P_ProductType VARCHAR(30),
  @P_BEGDATE varchar(20),
  @P_ENDDATE varchar(20)
)
AS

BEGIN
	
   SET NOCOUNT ON;
   DECLARE @strwhere  varchar(1000)
   DECLARE @sql VARCHAR(8000)
    set @strwhere='AND T2.BU='+quotename(@P_BU,'''') +' AND T2.BUILDING='+quotename(@P_BUILDING,'''')
    SET @sql=''  --初始化变量@sql
IF @P_MODEL<>''
    SET @strwhere=@strwhere+' And T2.MODEL_NAME  like '+quotename('%'+@P_MODEL+'%','''')
IF @P_CUSTOMER<>''
    SET @strwhere=@strwhere+' And T1.CUSTOMER='+quotename(@P_CUSTOMER,'''')
IF @P_FORMNO<>''
    SET @strwhere=@strwhere+' And T1.SUB_DOC_NO='+quotename(@P_FORMNO,'''')
IF @P_ApplyUser<>''
    SET @strwhere=@strwhere+' And T2.NPI_PM='+quotename(@P_ApplyUser,'''')
IF @P_ProductType<>''
    SET @strwhere=@strwhere+' And T1.PROD_GROUP='+quotename(@P_ProductType,'''')
IF (@P_STAUTS<> '' and @P_STAUTS<>'ALL')
   SET @strwhere=@strwhere+' and T1.STATUS='+quotename(@P_STAUTS,'''')
IF (@P_BEGDATE<>'' AND @P_ENDDATE <>'')
   SET @strwhere=@strwhere+' AND convert(varchar(10),T1.CREATE_DATE,121)BETWEEN '+quotename(@P_BEGDATE,'''')+ ' AND'+quotename(@P_ENDDATE,'''')
ELSE IF(@P_BEGDATE<>'' AND @P_ENDDATE='')
     SET @strwhere=@strwhere+' AND convert(varchar(10),T1.CREATE_DATE,121)>='+quotename(@P_BEGDATE,'''')
ELSE IF(@P_BEGDATE='' AND @P_ENDDATE<>'') 
     SET @strwhere=@strwhere+' AND convert(varchar(10),T1.CREATE_DATE,121)<='+quotename(@P_ENDDATE,'''')
 


--SET @sql='SELECT DocNo,Plant,Building,Model,DEPT,Name,Quantity,CaseId,DN_NO, 
--      dbo.GetHandler(CaseID) AS handler,Status,AppDate,CaseID
--      FROM TB_ControlRun_Master'
SET @sql='SELECT T1.*,T2.BU,T2.BUILDING,T2.MODEL_NAME,T2.NPI_PM,
          CONVERT(varchar(10),T1.CREATE_DATE,120) AS Date,
          SPM.dbo.GetHandler(T1.CASEID) AS handler
         FROM TB_NPI_APP_SUB T1
         LEFT JOIN TB_NPI_APP_MAIN T2
         ON T1.DOC_NO=T2.DOC_NO'
         --CONVERT(varchar(10),T1.CREATE_DATE,121) AS CREATE_DATE,
if (@strwhere<>'')
   
   set @sql=@sql+' WHERE 1=1'+'' +@strwhere+' '+ 'ORDER BY  T1.CREATE_DATE '
----+ 'ORDER BY  T1.CREATE_DATE '
--set @sql = 'select A.APPROVER_RESULT,B.* FROM TB_NPI_APP_RESULT A
--LEFT JOIN (' + @sql + ') B on A.SUB_DOC_NO = B.SUB_DOC_NO
--         WHERE A.APPROVER_Levels  = ''Top Manager''
--      ORDER BY  B.CREATE_DATE
--'
exec (@sql)
PRINT(@sql)
END
GO
/****** Object:  StoredProcedure [dbo].[P_CheckDFXNumber]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_CheckDFXNumber]
(
	@DFXNo varchar(50),
	@Model varchar(50),
	@RESULT int Output
)
AS
DECLARE @Code2 int
Declare @used varchar(50)
DECLARE @Count int
BEGIN
	
	select @Count = COUNT(*) from TB_DFX_Main where DFXNo = @DFXNo
	
	if @Count>0
	begin
	   SELECT @Code2=MAX(PARAME_VALUE2) FROM TB_DFX_PARAM WHERE FUNCTION_NAME='Number' AND PARAME_NAME='Model' AND PARAME_VALUE1=@Model 
	   SELECT @used = PARAME_VALUE3  FROM TB_DFX_PARAM WHERE FUNCTION_NAME='Number' AND PARAME_NAME='Model' AND PARAME_VALUE1=@Model 
	   IF @Code2 IS NULL
	   begin
	      SET @Code2=1 
	      UPDATE TB_DFX_PARAM SET PARAME_VALUE2=@Code2, PARAME_VALUE3='Y',UPDATE_TIME=Getdate() WHERE FUNCTION_NAME='Number' AND PARAME_NAME='Model' AND PARAME_VALUE1=@Model  
	   end
	   else
	   begin
	     SET @Code2= @Code2+1;
	     UPDATE TB_DFX_PARAM SET PARAME_VALUE2=@Code2, PARAME_VALUE3=@used,UPDATE_TIME=Getdate() WHERE FUNCTION_NAME='Number' AND PARAME_NAME='Model' AND PARAME_VALUE1=@Model  
	   end
	   --UPDATE TB_DFX_PARAM SET PARAME_VALUE2=@Code2, PARAME_VALUE3='Y',UPDATE_TIME=Getdate() WHERE FUNCTION_NAME='Number' AND PARAME_NAME='Model' AND PARAME_VALUE1=@Model  
	   set @RESULT = 1;
	end
	else 
	begin
	   set @RESULT = 2;
	end
	
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_Prelaunch_NEXT_DOAHandler]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_OP_Prelaunch_NEXT_DOAHandler]
(
@P_BU varchar(10),
@P_CASE_ID int,
@P_STEP_NAME Nvarchar(30),
@P_FORM_NO varchar(30),
@Result varchar(1000) out
)
AS
SET NOCOUNT ON;
BEGIN
DECLARE @T_COUNT INT
DECLARE @T_SEQ INT
DECLARE @T_NEXT_STEP_NAME NVARCHAR(30)
DECLARE @T_NEXT_HANDLER VARCHAR(2000)
SET @Result='NG;'
SET @T_COUNT=0
SET @T_SEQ=0 
   
    
set @T_NEXT_HANDLER = '';
--1.检查LEAVE DOA HANDLER数据是否存在
SELECT @T_COUNT=COUNT(1) FROM TB_Prelaunch_Step_Handler with(nolock) 
WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO

IF @T_COUNT>0
BEGIN
  --2.获取当前关卡sequnce
  SELECT @T_SEQ=SEQ FROM TB_Prelaunch_DOA
  WHERE  STEP_NAME=@P_STEP_NAME
  IF @T_SEQ>0
  BEGIN
    --3.获取下一关卡信息
 
 

    
     select TOP 1 @T_NEXT_STEP_NAME = STEP_NAME  FROM TB_Prelaunch_Step_Handler with(nolock) 
    WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO AND SEQ>@T_SEQ ORDER BY SEQ 
    
    set @T_NEXT_STEP_NAME=ISNULL(@T_NEXT_STEP_NAME,'')
    
    select @T_NEXT_HANDLER = @T_NEXT_HANDLER   +  [HANDLER]  + ','  FROM TB_Prelaunch_Step_Handler with(nolock) 
    WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO AND SEQ>@T_SEQ  AND STEP_NAME=@T_NEXT_STEP_NAME
    ORDER BY SEQ 
    
  
    SET  @T_NEXT_HANDLER=ISNULL(@T_NEXT_HANDLER,'')
    
    if LEN(@T_NEXT_STEP_NAME)>0
      set @Result='OK;'+@T_NEXT_STEP_NAME+'('+@T_NEXT_HANDLER+')';
    else
      set @Result='OK;End'
  END
  ELSE
      SET @Result='NG;無法獲取签核人信息,請聯絡系統管理員!'
      PRINT(@T_NEXT_HANDLER)
END

END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_PRELAUNCH_MAIN]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[P_OP_PRELAUNCH_MAIN] 
(
	@P_OP_TYPE varchar(10),
	@P_Bu varchar(20),
	@P_Building varchar(20),
	@P_PilotRunNO varchar(20),
	@P_Applicant varchar(20),
	@P_Model varchar(20),
	@P_Customer varchar(20),
	@P_PCBInRev varchar(20),
	@P_PCBOutRev varchar(20),
	@P_PLRev varchar(20),
	@P_Date varchar(20),
	@P_TP_ME varchar(20),
	@P_TP_EE varchar(20),
	@P_TP_PM varchar(20),
	@P_PM varchar(20),
	@P_CaseId int,
	@P_Status varchar(10),
	@P_Notes nvarchar(1000),

	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'

IF @P_OP_TYPE='ADD' 
BEGIN
  INSERT INTO TB_Prelaunch_Main
  (
    Bu,Building,PilotRunNO,Model,Customer,PCBInRev,PCBOutRev,
    PLRev,Date,TP_ME,TP_EE,TP_PM,PM,CaseId,Status,Applicant,Notes
   ) 
  VALUES 
  (
	  @P_Bu,@P_Building,@P_PilotRunNO,@P_Model,@P_Customer,
	  @P_PCBInRev,@P_PCBOutRev,@P_PLRev,CONVERT(datetime,@P_Date),@P_TP_ME,@P_TP_EE,@P_TP_PM,
	  @P_PM,@P_CaseId,@P_Status,@P_Applicant,@P_Notes
  ) 
  IF(@P_Status='Pending')
    BEGIN
	   INSERT INTO TB_Prelaunch_AppCheckItem
	   (Bu,Building,Dept,CheckItem,AttachmentFlag,PilotRunNO)
	   SELECT Bu,Building,Dept,CheckItem,AttachmentFlag,@P_PilotRunNO FROM TB_Prelaunch_CheckItemConfig
	   WHERE Bu=@P_Bu AND Building=@P_Building
   END
SET @Result='OK;'
END

IF @P_PM='UPDATE'
  BEGIN
    UPDATE TB_Prelaunch_Main SET Status=@P_Status WHERE CaseId=@P_CaseId
    SET @Result='OK;'
  END


IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_Prelaunch_DOAHandler]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_OP_Prelaunch_DOAHandler]
(
	@P_OP_TYPE VARCHAR(10),
	@P_CASE_ID INT,
	@P_STEP_NAME NVARCHAR(50),
	@P_Handler varchar(20),
	@P_APPROVE_REMARK VARCHAR(1000),
	@P_APPROVE_RESULT VARCHAR(20),
	@P_Reason NVARCHAR(100),
	@Result varchar(1000) out
	
)
AS
SET NOCOUNT ON;
SET XACT_ABORT ON
BEGIN TRAN
DECLARE @T_COUNT INT

  SET @Result='NG;'
IF (@P_OP_TYPE='APPROVE')
BEGIN

  UPDATE  TB_Prelaunch_Step_Handler 
  SET SIGN_FLAG='Y',APPROVE_TIME=GETDATE(),APPROVE_REMARK=@P_APPROVE_REMARK,
  APPROVE_RESULT=@P_APPROVE_RESULT,Reason = @P_Reason
  WHERE CASEID=@P_CASE_ID AND STEP_NAME=@P_STEP_NAME
  AND HANDLER=@P_Handler

IF @P_STEP_NAME='PMC'
BEGIN
  UPDATE TB_Prelaunch_Main SET STATUS='Finished',WorkOrder=@P_APPROVE_REMARK
   WHERE CASEID=@P_CASE_ID
END
  SET @Result='OK;'
END
  
 
    
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_Prelaunch_CheckItemConfig]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_Prelaunch_CheckItemConfig] 
(
	@P_OP_TYPE varchar(10),
	@P_Bu varchar(10),
	@P_Building varchar(10),
	@P_Dept varchar(20),
	@P_CheckItem nvarchar(100),
	@P_AttachmentFlag varchar(1),
	@P_UpdateUser varchar(20),
	@P_UpdateTime datetime,
	@P_ID varchar(20),
	@Result nvarchar(1000)  out
 )
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
declare @Count int
IF @P_OP_TYPE='ADD' 
BEGIN
 SELECT @Count=COUNT(1) from TB_Prelaunch_CheckItemConfig
 WHERE Bu=@P_Bu AND Building=@P_Building and Dept=@P_Dept and CheckItem=@P_CheckItem
 if(@Count>0)
 begin
 SET @Result=@Result+'This Item has already existed'
 end
 ELSE
  BEGIN
	   INSERT INTO TB_PRELAUNCH_CHECKITEMCONFIG 
	    (Bu,Building,Dept,CheckItem,AttachmentFlag,UpdateUser,UpdateTime) 
	   VALUES 
	    (@P_Bu,@P_Building,@P_Dept,@P_CheckItem,@P_AttachmentFlag,@P_UpdateUser,@P_UpdateTime) 
	SET @Result='OK;'
   END
END


IF @P_OP_TYPE='DELETE'
   BEGIN
     DELETE FROM TB_Prelaunch_CheckItemConfig WHERE ID=@P_ID
     SET @Result='OK;'
   END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_NEXT_DOAHandler]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[P_OP_NPI_NEXT_DOAHandler]
(

@P_BU varchar(10),
@P_BUILDING varchar(10),
@P_CASE_ID int,
@P_STEP_NAME Nvarchar(30),
@P_FORM_NO varchar(30),
@P_PHASE varchar(30),
@Result varchar(1000) out
)
AS
SET NOCOUNT ON;
BEGIN
DECLARE @T_COUNT INT
DECLARE @T_SEQ INT
DECLARE @T_NEXT_STEP_NAME NVARCHAR(30)
DECLARE @T_NEXT_HANDLER VARCHAR(1000)
SET @Result='NG;'
SET @T_COUNT=0
SET @T_SEQ=0 
set @T_NEXT_HANDLER = '';
--1.检查LEAVE DOA HANDLER数据是否存在
SELECT @T_COUNT=COUNT(1) FROM TB_NPI_Step_Handler  with(nolock) 
WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO

IF @T_COUNT>0
BEGIN
  --2.获取当前关卡sequnce
  SELECT @T_SEQ=SEQ FROM TB_NPI_DOAConfig   with(nolock) 
  WHERE  BU=@P_BU AND BUILDING=@P_BUILDING
  AND STEP_NAME=@P_STEP_NAME
  AND PHASE=@P_PHASE
  IF @T_SEQ>0
  BEGIN
    --3.获取下一关卡信息
    -- select Top 1 @T_NEXT_STEP_NAME=STEP_NAME,@T_NEXT_HANDLER=HANDLER 
    -- FROM TB_NPI_Step_Handler   with(nolock)
    -- WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO 
    -- AND SEQ>@T_SEQ ORDER BY SEQ 
    
    --set @T_NEXT_STEP_NAME=ISNULL(@T_NEXT_STEP_NAME,'')
    --SET  @T_NEXT_HANDLER=ISNULL(@T_NEXT_HANDLER,'')
    
    select TOP 1 @T_NEXT_STEP_NAME = STEP_NAME  FROM TB_NPI_Step_Handler with(nolock) 
    WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO AND SEQ>@T_SEQ ORDER BY SEQ 
    
	set @T_NEXT_STEP_NAME=ISNULL(@T_NEXT_STEP_NAME,'')
    
    select @T_NEXT_HANDLER = @T_NEXT_HANDLER + [HANDLER]  + ','  FROM TB_NPI_Step_Handler with(nolock) 
    WHERE CASEID=@P_CASE_ID AND FORMNO=@P_FORM_NO AND SEQ>@T_SEQ  AND STEP_NAME=@T_NEXT_STEP_NAME
    ORDER BY SEQ 
    
    SET  @T_NEXT_HANDLER=ISNULL(@T_NEXT_HANDLER,'')
    
    if LEN(@T_NEXT_STEP_NAME)>0
      set @Result='OK;'+@T_NEXT_STEP_NAME+'('+@T_NEXT_HANDLER+')';
    else
      set @Result='OK;End'
  END
  ELSE
      SET @Result='NG;無法獲取DOA設定信息,請聯絡系統管理員!'
END
ELSE
  SET @Result='NG;無法獲取簽核人信息,請聯絡系統管理員!'
  
  END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_MEMBER_HIS]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[P_OP_NPI_MEMBER_HIS] 
(
	@P_OP_TYPE varchar(10),
	@P_ID VARCHAR(10),
	@P_BU varchar(50),
	@P_BUILDING varchar(20),
	@P_CATEGORY varchar(50),
	@P_DEPT nvarchar(100),
	@P_ENAME varchar(30),
	@P_CNAME nvarchar(40),
	@P_EMAIL varchar(50),
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),
	@P_PHASE varchar(30),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @SEQ INT

IF @P_OP_TYPE='ADD' 
BEGIN


 	 if not exists (select * from TB_NPI_DOA_DETAIL where STEP_NAME=@P_CATEGORY
 	   and BU=@P_BU AND BUILDING=@P_BUILDING AND DEPT=@P_DEPT AND PHASE=@P_PHASE )
      BEGIN
        SELECT  @SEQ=SEQ FROM TB_NPI_DOAConfig
        WHERE BU=@P_BU and BUILDING=@P_BUILDING and STEP_NAME=@P_CATEGORY AND PHASE=@P_PHASE
         INSERT INTO TB_NPI_DOA_DETAIL 
           (BU,BUILDING,STEP_NAME,SEQ,DEPT,ENAME,CNAME,EMAIL,UPDATE_TIME,UPDATE_USERID,PHASE) 
         VALUES 
          (@P_BU,@P_BUILDING,@P_CATEGORY,@SEQ,@P_DEPT,@P_ENAME,@P_CNAME,@P_EMAIL,
          @P_UPDATE_TIME,@P_UPDATE_USERID,@P_PHASE) 
         SET @Result='OK;'
     END
     else
       set  @Result='NG;'+'此成員已添加不可重複作業!'
END
IF @P_OP_TYPE='DELETE'
  BEGIN
    DELETE FROM  TB_NPI_DOA_DETAIL WHERE ID=@P_ID
    SET @Result='OK;'
  END

IF @P_OP_TYPE='UPDATE'
  BEGIN
     UPDATE TB_NPI_DOA_DETAIL SET ENAME=@P_ENAME,CNAME=@P_CNAME,EMAIL=@P_EMAIL
     where ID=@P_ID
     set @Result='OK;'
  END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_MEMBER]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_NPI_MEMBER] 
(
	@P_OP_TYPE varchar(10),
	@P_ID VARCHAR(10),
	@P_BU varchar(50),
	@P_BUILDING varchar(20),
	@P_CATEGORY varchar(50),
	@P_DEPT nvarchar(100),
	@P_ENAME varchar(30),
	@P_CNAME nvarchar(40),
	@P_EMAIL varchar(50),
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'

IF @P_OP_TYPE='ADD' 
BEGIN
 	 if not exists (select * from TB_NPI_MEMBER where DEPT =@P_DEPT AND  ENAME=@P_ENAME)
      BEGIN
         INSERT INTO TB_NPI_MEMBER 
           (BU,BUILDING,CATEGORY,DEPT,ENAME,CNAME,EMAIL,UPDATE_TIME,UPDATE_USERID) 
         VALUES 
          (@P_BU,@P_BUILDING,@P_CATEGORY,@P_DEPT,@P_ENAME,@P_CNAME,@P_EMAIL,@P_UPDATE_TIME,@P_UPDATE_USERID) 
         SET @Result='OK;'
     END
     else
       set  @Result='NG;'+'此成員已添加不可重複作業!'
END
IF @P_OP_TYPE='DELETE'
  BEGIN
    DELETE FROM TB_NPI_MEMBER WHERE ID=@P_ID
    SET @Result='OK;'
  END

IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_FMEA]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_NPI_FMEA] 
(
	@P_OP_TYPE varchar(10),
	@P_SubNo varchar(50),
	@P_Item varchar(50),
	@P_Source nvarchar(800),
	@P_Stantion varchar(400),
	@P_PotentialFailureMode nvarchar(800),
	@P_Loess nvarchar(800),
	@P_Sev int,
	@P_PotentialFailure nvarchar(800),
	@P_Occ int,
	@P_CurrentControls nvarchar(800),
	@P_DET int,
	@P_RPN int,
	@P_RecommendedAction nvarchar(800),
	@P_Resposibility varchar(50),
	@P_TargetCompletionDate datetime,
	@P_ActionsTaken nvarchar(800),
	@P_ResultsSev int,
	@P_ResultsOcc int,
	@P_ResultsDet int,
	@P_ResultsRPN int,
	@P_WriteDept varchar(50),
	@P_ReplyDept varchar(50),
	@P_Status varchar(50),
	@P_Update_User varchar(50),
	@P_Update_Time datetime,
	@P_ID INT,
    @P_FILEPATH Varchar(100),
    @P_FILENAME Varchar(50),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'

IF @P_OP_TYPE='ADD' 
BEGIN
    INSERT INTO TB_NPI_FMEA 
     (SubNo,Item,Source,Stantion,PotentialFailureMode,Loess,Sev,PotentialFailure,
     Occ,CurrentControls,DET,RPN,WriteDept,ReplyDept,
     Status,Update_User,Update_Time,FILE_NAME,FILE_PATH) 
     VALUES 
     (@P_SubNo,@P_Item,@P_Source,
     @P_Stantion,@P_PotentialFailureMode,
     @P_Loess,@P_Sev,@P_PotentialFailure,@P_Occ,
     @P_CurrentControls,@P_DET,@P_RPN,@P_WriteDept,
     @P_ReplyDept,@P_Status,
     @P_Update_User,GETDATE(),@P_FILENAME,@P_FILEPATH) 
SET @Result='OK;'
END

IF @P_OP_TYPE='UPDATE'
  BEGIN
      UPDATE TB_NPI_FMEA SET 
		RecommendedAction=@P_RecommendedAction,
		Resposibility=@P_Resposibility,
		TargetCompletionDate=@P_TargetCompletionDate,
		ActionsTaken=@P_ActionsTaken,
		ResultsSev=@P_ResultsSev,
		ResultsOcc=@P_ResultsOcc,
		ResultsDet=@P_ResultsDet,
		ResultsRPN=@P_ResultsRPN,Status=@P_Status
		WHERE id=@P_ID
	SET @Result='OK;'
  END
 
IF @P_OP_TYPE='DELETE'
  BEGIN
        DELETE FROM  TB_NPI_FMEA 
		WHERE id=@P_ID
	SET @Result='OK;'
  END
   

IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_DOAHandler_Reslult]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[P_OP_NPI_DOAHandler_Reslult]
(
@P_OP_TYPE VARCHAR(10),
@P_CASE_ID INT,
@P_STEP_NAME NVARCHAR(50),
@P_HANDLER  VARCHAR(50),
@P_APPROVER_RESULT VARCHAR(50),
@P_APPROVER_REMARK NVARCHAR(200),
@Result varchar(1000) out
)
AS
SET NOCOUNT ON;
SET XACT_ABORT ON
BEGIN TRAN
DECLARE @T_COUNT INT

SET @Result='NG;'
DECLARE @status Varchar(20)
IF @P_OP_TYPE='APPROVE'
BEGIN
  UPDATE TB_NPI_Step_Handler SET APPROVE_TIME=GETDATE(),APPROVE_RESULT=@P_APPROVER_RESULT,
  APPROVE_REMARK=@P_APPROVER_REMARK WHERE CASEID=@P_CASE_ID AND HANDLER=@P_HANDLER
  
  AND STEP_NAME=@P_STEP_NAME
  
  
    IF @P_STEP_NAME='LOB Head'
     SET @status='Finished'
    ELSE
      SET @status='Pending'
      BEGIN
	   update TB_NPI_APP_MAIN_HIS
	   SET STATUS=@status
	   WHERE CASEID=@P_CASE_ID
	   SET @Result='OK;'
	 END
END 
    
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--鈭?瑁?憭望?
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_SUB]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[P_OP_NPI_APP_SUB] 
(

		@P_OP_TYPE varchar(10),
		@P_DOC_NO varchar(30),
		@P_SUB_DOC_NO varchar(30),
		@P_SUB_DOC_PHASE varchar(10),
		@P_SUB_DOC_PHASE_A varchar(10),
		@P_WorkOrder nvarchar(50),
		@P_SUB_DOC_PHASE_RESULT varchar(10),
		@P_SUB_DOC_PHASE_STATUS varchar(10),
		@P_SUB_DOC_PHASE_VERSION int,
		@P_UPDATE_TIME datetime,
		@P_UPDATE_USERID varchar(30),
		
		@P_Building varchar(10),
		@P_CREATE_DATE datetime,
		@P_CTQ_QTY int,
		@P_CLCA_QTY int,
		@P_CLCA_BEGIN_TIME varchar(20),
		@P_CLCA_END_TIME varchar(20),
		@P_LOT_QTY int,
		@P_PCB_REV varchar(50),
		@P_SPEC_REV varchar(50),
		@P_ISSUE_DATE datetime,
		@P_INPUT_DATE varchar(50),
		@P_CUSTOMER nvarchar(100),
		@P_LINE varchar(10),
		@P_BOM_REV varchar(50),
		@P_CUSTOMER_REV varchar(50),
		@P_RELEASET_DATE varchar(50),
		@P_PK_DATE varchar(50),
		@P_NeedStartItmes nvarchar(100),
		@P_GROD_GROUP NVARCHAR(50),
		@P_CASEID INT,
		@P_MODIFYFLAG VARCHAR(1),
		@P_REMARKM NVARCHAR(2000),
		@Result nvarchar(1000)  out
)
AS


SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
SET @Result='NG;'
declare @T_COUNT INT
DECLARE @splitchar varchar(1)					--啟動項分隔符';'
DECLARE @tempCategory nvarchar(10)		        ---單個需啟動項
DECLARE @check_ctq nvarchar(10)		            ---CTQ 部門成員是否有維護
DECLARE @check_dfc  nvarchar(10)		        ---DFX 部門成員是否有維護
DECLARE @ctq_count INT
DECLARE @dfx_count INT
DECLARE @dfx_errmsg nvarchar(50)
DECLARE @ctq_errmsg nvarchar(50)
DECLARE @len_dfx INT

SET @ctq_count=0
SET @dfx_count=0
SET @dfx_errmsg=''
SET @ctq_errmsg=''
SET @check_ctq=''
SET @check_dfc=''
IF @P_OP_TYPE='ADD'  --新增
 BEGIN

  IF len(@P_NeedStartItmes)>0
   BEGIN
       SET @splitchar=';'
     	--申明游標
		DECLARE Category_Cursor CURSOR LOCAL FOR
		--分割啟動項
		SELECT short_str from dbo.FN_SqlServer_Split(@P_NeedStartItmes,@splitchar)
		--打開游標
		OPEN Category_Cursor
		--取得第一筆記錄
		FETCH NEXT FROM Category_Cursor INTO @tempCategory
		--遍歷記錄
		WHILE @@FETCH_STATUS = 0
			 BEGIN  
			   IF @tempCategory='C'
			     BEGIN
			        SELECT @ctq_count=COUNT(1) FROM TB_NPI_CTQ WHERE BU='POWER' AND BUILDING=@P_Building
			        AND PROD_GROUP=@P_GROD_GROUP
			        IF @ctq_count=0
			            SET @ctq_errmsg=@P_GROD_GROUP +' '+ 'CTQ 項目未維護'
			        ELSE
			           BEGIN
					      SELECT @check_ctq= STUFF((
					      SELECT ';' + DEPT FROM
							(select distinct DEPT from TB_NPI_CTQ
							WHERE BU='POWER' AND BUILDING=@P_Building
							AND PROD_GROUP=@P_GROD_GROUP AND DEPT  NOT IN
							(SELECT DISTINCT DEPT FROM TB_NPI_APP_MEMBER
							WHERE DOC_NO=@P_DOC_NO AND Category='CTQ TeamMember')) T1   for xml path('')),1,1,'')  
							
			               IF(LEN(@check_ctq)>0)
			                     SET @ctq_errmsg = 'NG;'+@check_ctq +' CTQ TeamMember 部門成員未維護'
			               ELSE
			                    BEGIN
                              ----寫CTQ資料
										INSERT INTO TB_NPI_APP_CTQ (DOC_NO,SUB_DOC_NO,PROD_GROUP,PHASE,DEPT,PROCESS,CTQ,UNIT,
										SPC,SPEC_LIMIT,CONTROL_TYPE,GOAL,flag,ACT,STATUS,RESULT,Comment,UPDATE_TIME,REPLY_USERID)
										SELECT @P_DOC_NO,@P_SUB_DOC_NO,PROD_GROUP,PHASE,DEPT,PROCESS,CTQ,UNIT,SPC,
										SPEC_LIMIT,CONTROL_TYPE,GOAL,flag,'','Write','','',GETDATE(),@P_UPDATE_USERID FROM TB_NPI_CTQ 
										WHERE PROD_GROUP=@P_GROD_GROUP
										AND PHASE=@P_SUB_DOC_PHASE_A 
										AND BU='POWER' 
										AND BUILDING=@P_Building 
		
                                    END
				        END
				  END
				    
			   IF @tempCategory='D'
			     BEGIN
			        SELECT @dfx_count=COUNT(1) FROM TB_DFX_Item
			        WHERE BU='POWER' AND Building=@P_Building AND ProductType=@P_GROD_GROUP 
			       
			        IF @dfx_count=0
			          SET @dfx_errmsg=@P_GROD_GROUP +' '+ 'DFX 項目未維護'
			        ELSE
			          BEGIN
							SELECT @check_dfc= STUFF((
							SELECT ';' + WriteDept FROM
							(select distinct WriteDept from TB_DFX_Item
							WHERE BU='POWER' AND BUILDING=@P_Building
							AND ProductType=@P_GROD_GROUP AND WriteDept  NOT IN
							(SELECT DISTINCT DEPT FROM TB_NPI_APP_MEMBER
							WHERE DOC_NO=@P_DOC_NO AND Category='DFX TeamMember')) T1   for xml path('')),1,1,'')  
							
							IF(LEN(@check_dfc)>0)
								 SET @dfx_errmsg = 'NG;'+@check_ctq +'DFX TeamMember 部門成員未維護'
							ELSE
							  BEGIN
								  insert into TB_DFX_ItemBody
								  (DFXNo,ItemID,Item,ItemType,ItemName,Requirements,PriorityLevel,Losses,WriteDept,Status,UpdateTime,FilePath,OldItemType)
								   SELECT @P_SUB_DOC_NO,ItemID,Item,ItemType,ItemName,Requirements,PriorityLevel,Losses,WriteDept,
								  'Write', getdate(),FilePath,OldItemType from TB_DFX_ITEM 
								  Where ProductType=@P_GROD_GROUP and BU='POWER' and Building=@P_Building
								  and WriteDept not in 
								  (select distinct DEPT from dbo.TB_NPI_APP_MEMBER
								   where DOC_NO = @P_DOC_NO
								   and WriteEname = '')

									
							 END 
					  END 

			     END
		      
	  		   FETCH NEXT FROM Category_Cursor INTO @tempCategory
		    END 

			--關閉游標
			CLOSE Category_Cursor
			--釋放游標
		DEALLOCATE Category_Cursor
		set @len_dfx=LEN(@dfx_errmsg)
		IF (LEN(@dfx_errmsg)=0 AND LEN(@ctq_errmsg)=0)
		  BEGIN
		      --IF(LEN(@check_ctq)=0 and LEN(@check_dfc)=0)
				 
					 --寫子流程主檔資料
 						INSERT INTO TB_NPI_APP_SUB
					   ( 
							DOC_NO,SUB_DOC_NO,SUB_DOC_PHASE,WorkOrder,SUB_DOC_PHASE_RESULT,
							SUB_DOC_PHASE_STATUS,SUB_DOC_PHASE_VERSION,UPDATE_TIME,
							UPDATE_USERID,
							CREATE_DATE,CTQ_QTY,CLCA_QTY,CLCA_BEGIN_TIME,CLCA_END_TIME,LOT_QTY,
							PCB_REV,SPEC_REV,ISSUE_DATE,INPUT_DATE,CUSTOMER,LINE,BOM_REV,CUSTOMER_REV,RELEASET_DATE,
							PK_DATE,NeedStartItmes,PROD_GROUP,CASEID,STATUS,MODIFY_FLAG,PDF_FLAG,REMARKM
					   ) 
					  VALUES 
					   (
							@P_DOC_NO,@P_SUB_DOC_NO,@P_SUB_DOC_PHASE,@P_WorkOrder,@P_SUB_DOC_PHASE_RESULT,
							'10',@P_SUB_DOC_PHASE_VERSION,@P_UPDATE_TIME,
							@P_UPDATE_USERID,
							@P_CREATE_DATE,@P_CTQ_QTY,@P_CLCA_QTY,@P_CLCA_BEGIN_TIME,
							@P_CLCA_END_TIME,@P_LOT_QTY,@P_PCB_REV,@P_SPEC_REV,@P_ISSUE_DATE,
							@P_INPUT_DATE,@P_CUSTOMER,@P_LINE,@P_BOM_REV,@P_CUSTOMER_REV,
							@P_RELEASET_DATE,@P_PK_DATE,@P_NeedStartItmes,@P_GROD_GROUP
							,@P_CASEID,'Pending',@P_MODIFYFLAG,'N',@P_REMARKM
					   ) 
					  SET @Result='OK;'
				
				 -- IF(LEN(@check_ctq)=0 and LEN(@check_dfc)>0)
					--	SET @Result='NG;'+@check_dfc +' DFX TeamMember 部門成員未維護'
				 -- IF(LEN(@check_ctq)>0 and LEN(@check_dfc)=0)
					--SET @Result='NG;'+@check_ctq +' CTQ TeamMember 部門成員未維護'
				 -- IF(LEN(@check_ctq)>0 and LEN(@check_dfc)>0)
					--SET @Result='NG;'+@check_dfc + 'DFX TeamMember 部門成員未維護' +@check_ctq + ' CTQ TeamMember 部門成員未維護'
		
		  END
		 
		IF (LEN(@dfx_errmsg)=0 AND LEN(@ctq_errmsg)>0)
		   	SET @Result='NG;' +@ctq_errmsg
		IF (LEN(@dfx_errmsg)>0 AND LEN(@ctq_errmsg)=0)
		   	SET @Result='NG;'+ @dfx_errmsg
		   	
		 IF (LEN(@dfx_errmsg)>0 AND LEN(@ctq_errmsg)>0)
		   	SET @Result='NG;'+@dfx_errmsg + ';' +@ctq_errmsg
   END
   
  PRINT(@Result)
 END

IF @P_OP_TYPE='UPDATE'
  BEGIN
     UPDATE TB_NPI_APP_SUB
     set
     PCB_REV=@P_PCB_REV,
     SPEC_REV=@P_SPEC_REV,
     ISSUE_DATE=GETDATE(),
     INPUT_DATE=@P_INPUT_DATE,
     BOM_REV=@P_BOM_REV,
     CUSTOMER_REV=@P_CUSTOMER_REV,
     RELEASET_DATE=@P_RELEASET_DATE,
	 PK_DATE=@P_PK_DATE
     WHERE CASEID=@P_CASEID
      SET @Result = 'OK;'
  END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN

END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_RESULT]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[P_OP_NPI_APP_RESULT] 
(
	@P_OP_TYPE varchar(10),
	@P_SUB_DOC_NO varchar(30),
	@P_CASEID int,
	@P_DEPT varchar(20),
	@P_REMARK nvarchar(100),
	@P_APPROVER varchar(50),
	@P_APPROVER_RESULT varchar(20),
	@P_APPROVER_Levels varchar(50),
	@P_APPROVER_OPINION varchar(2000),
	@P_APPROVER_DATE datetime,
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),

	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @Count int
IF @P_OP_TYPE='ADD' 
  BEGIN
  SELECT @Count=COUNT(1) FROM  TB_NPI_APP_RESULT WHERE CASEID=@P_CASEID
  AND DEPT=@P_DEPT and APPROVER_Levels=@P_APPROVER_Levels and APPROVER = @P_APPROVER
  IF @Count>0
  SET @Result='NG;Data Duplaciton'
  ELSE
   BEGIN
	  INSERT INTO TB_NPI_APP_RESULT 
	  (  
		SUB_DOC_NO,CASEID,DEPT,
		REMARK,APPROVER,APPROVER_RESULT,
		APPROVER_OPINION,APPROVER_DATE,
		UPDATE_TIME,UPDATE_USERID,
		APPROVER_Levels
	   )
		VALUES 
	  (
	     @P_SUB_DOC_NO,@P_CASEID,@P_DEPT,@P_REMARK,
	     @P_APPROVER,@P_APPROVER_RESULT,
	     @P_APPROVER_OPINION,GETDATE(),GETDATE(),
	     @P_UPDATE_USERID,@P_APPROVER_Levels
	   ) 
	   SET @Result='OK;'
   END
END

IF @P_OP_TYPE='UPDATE'
 BEGIN
    UPDATE  TB_NPI_APP_RESULT SET APPROVER_RESULT=@P_APPROVER_RESULT,
    APPROVER_OPINION=@P_APPROVER_OPINION,UPDATE_TIME=GETDATE(),
    APPROVER_Levels=@P_APPROVER_Levels
    WHERE CASEID=@P_CASEID AND DEPT=@P_DEPT AND APPROVER_Levels = @P_APPROVER_Levels
    SET @Result='OK;'
 END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_MEMBER]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_NPI_APP_MEMBER] 
(
	@P_OP_TYPE varchar(10),
	@P_ID int,
	@P_DOC_NO varchar(30),
	@P_Category varchar(30),
	@P_DEPT nvarchar(100),
	@P_WriteEname varchar(30),
	@P_WriteCname nvarchar(40),
	@P_WriteEmail varchar(50),
	@P_ReplyEName varchar(30),
	@P_ReplyCname varchar(50),
	@P_ReplyEmai varchar(50),
	@P_CheckedEname varchar(50),
	@P_CheckedEmai varchar(50),
	@P_CategoryFlag VARCHAR(1),
	@P_Flag VARCHAR(2),
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @Count int
IF @P_OP_TYPE='ADD' 
BEGIN
   SELECT @Count=COUNT(*) FROM TB_NPI_APP_MEMBER Where DOC_NO=@P_DOC_NO and  Category=@P_Category
   AND DEPT=@P_DEPT 
   IF(@Count>0)
     BEGIN
      SET @Result='NG;'+'Data Reduplicate'
     END

   ELSE
      BEGIN
        
         IF(@P_Category='CTQ TeamMember' OR @P_Category='DFX TeamMember' 
            OR @P_Category='ISSUES TeamMember' OR  @P_Category='PFMEA TeamMember')
           SET @P_CategoryFlag='A'

        ELSE IF(@P_Category='Prepared TeamMember')
         SET @P_CategoryFlag='B'
        ELSE IF(@P_Category= 'Manager TeamMember')
           set @P_CategoryFlag='C'
        ELSE IF(@P_Category='PM TeamMember')
          SET @P_CategoryFlag='D'
          
            IF(@P_Category='DFX TeamMember')
           SET @P_Flag='AD'  
            ELSE IF(@P_Category='CTQ TeamMember')
         SET @P_Flag='AC'
          ELSE IF(@P_Category='ISSUES TeamMember')
         SET @P_Flag='AI'
          ELSE IF(@P_Category='PFMEA TeamMember')
         SET @P_Flag='AF'
         
       INSERT INTO TB_NPI_APP_MEMBER 
       (
			DOC_NO,Category,DEPT,
			WriteEname,WriteCname,WriteEmail,
			ReplyEName,ReplyCname,ReplyEmai,
			CheckedEName,CheckedEmail,CategoryFlag,Flag,
			UPDATE_TIME,UPDATE_USERID
       ) 
      VALUES 
       (
          @P_DOC_NO,@P_Category,@P_DEPT,
          @P_WriteEname, @P_WriteCname,@P_WriteEmail,
          @P_ReplyEName,@P_ReplyCname,@P_ReplyEmai,
          @P_CheckedEname,@P_CheckedEmai,@P_CategoryFlag,@P_Flag,
          @P_UPDATE_TIME,@P_UPDATE_USERID
       ) 
      SET @Result='OK;'
     END 
END

IF @P_OP_TYPE='DELETE' 
     BEGIN
       DELETE FROM TB_NPI_APP_MEMBER WHERE ID=@P_ID
       SET @Result='OK;'
     END
IF  @@ERROR = 0   

BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_MAIN_HIS]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[P_OP_NPI_APP_MAIN_HIS] 
(
	@P_OP_TYPE varchar(10),
	@P_DOC_NO varchar(30),
	@P_BU varchar(20),
	@P_BUILDING varchar(20),
	@P_APPLY_DATE  varchar(20),
	@P_APPLY_USERID varchar(30),
	@P_MODEL_NAME varchar(50),
	@P_CUSTOMER nvarchar(100),
	@P_PRODUCT_TYPE nvarchar(510),
	@P_LAYOUT nvarchar(510),
	@P_PHASE nvarchar(100),
	@P_NEXTSTAGE_DATE nvarchar(100),
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),
	@P_NPI_PM varchar(50),
	@P_SALES_OWNER varchar(50),
	@P_RD_ENGINEER varchar(50),
	@P_REMARK varchar(1000),
	@P_CASEID varchar(50),
	@P_STATUS varchar(20),
	@P_PMLOC varchar(50),
	@P_PMEXT varchar(50),
	@P_RDLOC varchar(50),
	@P_RDEXT varchar(50),
	@P_SALESLOC varchar(50),
	@P_SALESEXT varchar(50),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'

IF @P_OP_TYPE='ADD' 
BEGIN
  INSERT INTO TB_NPI_APP_MAIN_HIS 
	(DOC_NO,BU,BUILDING,APPLY_DATE,APPLY_USERID,MODEL_NAME,CUSTOMER,PRODUCT_TYPE,LAYOUT,
	PHASE,NEXTSTAGE_DATE,UPDATE_TIME,UPDATE_USERID,NPI_PM,SALES_OWNER,
	RD_ENGINEER,REMARK,CASEID,STATUS,PM_LOC,PM_EXT,RD_LOC,RD_EXT,SALES_LOC,SALES_EXT) 
  VALUES 
  (
	@P_DOC_NO,@P_BU,@P_BUILDING,@P_APPLY_DATE,@P_APPLY_USERID,
	@P_MODEL_NAME,@P_CUSTOMER,@P_PRODUCT_TYPE,@P_LAYOUT,@P_PHASE,
	@P_NEXTSTAGE_DATE,GETDATE(),@P_UPDATE_USERID,@P_NPI_PM,@P_SALES_OWNER,
	@P_RD_ENGINEER,@P_REMARK,@P_CASEID,'Pending',
	@P_PMLOC,@P_PMEXT,@P_RDLOC,@P_RDEXT,@P_SALESLOC,@P_SALESEXT
  ) 
SET @Result='OK;'
END


IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_ISSUELIST]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_NPI_APP_ISSUELIST] 
(
	@P_OP_TYPE varchar(10),
	@P_ID varchar(30),
	@P_DOC_NO varchar(30),
	@P_SUB_DOC_NO varchar(30),
	@P_PHASE varchar(10),
	@P_STATION nvarchar(100),
	@P_ITEMS nvarchar(100),
	@P_PRIORITYLEVEL varchar(10),
	@P_ISSUE_DESCRIPTION nvarchar(2000),
	@P_ISSUE_LOSSES nvarchar(100),
	@P_TEMP_MEASURE nvarchar(100),
	@P_IMPROVE_MEASURE nvarchar(200),
	@P_PERSON_IN_CHARGE varchar(20),
	@P_DUE_DAY varchar(20),
	@P_CURRENT_STATUS varchar(2000),
	@P_AFFIRMACE_MAN varchar(20),
	@P_STAUTS varchar(200),
	@P_TRACKING varchar(20),
	@P_REMARK nvarchar(40),
	@P_DEPT VARCHAR(20),
	@P_CREATE_TIME datetime,
	@P_CREATE_USERID varchar(30),
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),
	@P_FILENAME VARCHAR(50),
	@P_FILEPATH  varchar(100),
	@P_CLASS VARCHAR(10),
	@P_MEASURE_DEPTREPLY NVARCHAR(2000),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @COUNT INT
IF @P_OP_TYPE='ADD' 
BEGIN
    
    INSERT INTO TB_NPI_APP_ISSUELIST 
    (DOC_NO,SUB_DOC_NO,PHASE,STATION,ITEMS,PRIORITYLEVEL,
     ISSUE_DESCRIPTION,ISSUE_LOSSES,TEMP_MEASURE,IMPROVE_MEASURE,
     CREATE_TIME,CREATE_USERID,UPDATE_TIME,UPDATE_USERID,DEPT,STAUTS,
     FILE_NAME,FILE_PATH,CLASS,REMARK
     ) 
   VALUES 
    (
      @P_DOC_NO,@P_SUB_DOC_NO,@P_PHASE,@P_STATION,@P_ITEMS,
      @P_PRIORITYLEVEL,@P_ISSUE_DESCRIPTION,@P_ISSUE_LOSSES,
      @P_TEMP_MEASURE,@P_IMPROVE_MEASURE,
      @P_CREATE_TIME,@P_CREATE_USERID,@P_UPDATE_TIME,@P_UPDATE_USERID,@P_DEPT,@P_STAUTS,
      @P_FILENAME,@P_FILEPATH,@P_CLASS,@P_REMARK
     ) 
SET @Result='OK;'
END

IF @P_OP_TYPE='DELETE'
  BEGIN
     DELETE FROM  TB_NPI_APP_ISSUELIST
     WHERE ID=@P_ID
     SET @Result='OK;'
  END
IF @P_OP_TYPE='UPDATE'
  BEGIN
    UPDATE TB_NPI_APP_ISSUELIST SET CURRENT_STATUS=@P_CURRENT_STATUS,
    AFFIRMACE_MAN=@P_AFFIRMACE_MAN,
    DUE_DAY=@P_DUE_DAY,
    REMARK=@P_REMARK,
    PERSON_IN_CHARGE=@P_PERSON_IN_CHARGE,
    TRACKING=@P_TRACKING,
    STAUTS=@P_STAUTS,
    MEASURE_DEPTREPLY=@P_MEASURE_DEPTREPLY
    WHERE ID=@P_ID
     set @Result='OK;'
  END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_CTQ]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_NPI_APP_CTQ] 
(
	@P_OP_TYPE varchar(10),
	@P_ID INT,
	@P_DOC_NO varchar(30),
	@P_SUB_DOC_NO varchar(30),
	@P_PROD_GROUP varchar(50),
	@P_PHASE varchar(50),
	@P_DEPT nvarchar(100),
	@P_PROCESS nvarchar(100),
	@P_CTQ nvarchar(100),
	@P_UNIT varchar(20),
	@P_SPC nvarchar(20),
	@P_SPEC_LIMIT nvarchar(100),
	@P_CONTROL_TYPE varchar(10),
	@P_GOAL float,
	@P_ACT nvarchar(40),
	@P_RESULT varchar(10),
	@P_Comment nvarchar(1000),
	@P_STATUS varchar(10),
	@P_DESCRIPTION nvarchar(510),
	@P_DUTY_DEPT varchar(50),
	@P_DUTY_EMP varchar(30),
	@P_ROOT_CAUSE nvarchar(510),
	@P_D varchar(1),
	@P_M varchar(1),
	@P_P varchar(1),
	@P_E varchar(1),
	@P_W varchar(1),
	@P_O varchar(1),
	@P_TEMPORARY_ACTION nvarchar(510),
	@P_CORRECTIVE_PREVENTIVE_ACTION nvarchar(510),
	@P_COMPLETE_DATE nvarchar(510),
	@P_IMPROVEMENT_STATUS nvarchar(510),
	@P_UPDATE_TIME datetime,
	@P_REPLY_USERID varchar(30),
	@P_W_FILENAME  varchar(50),
	@P_W_FILEPATH VARCHAR(100),
	@P_R_FILENAME  varchar(50),
	@P_R_FILEPATH VARCHAR(100),

	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
declare @T_COUNT INT
declare @D_COUNT INT
BEGIN TRAN
SET @Result='NG;'

IF @P_OP_TYPE='Add' 
BEGIN

	update TB_NPI_APP_CTQ set
	 ACT = @P_ACT,
	 RESULT =@P_RESULT,
	 Comment=@P_Comment,
	 UPDATE_TIME=GETDATE(),DESCRIPTION=@P_DESCRIPTION,ROOT_CAUSE=@P_ROOT_CAUSE,
	 D=@P_D,M=@P_M,P=@P_P,E=@P_E,W=@P_W,O=@P_O,STATUS=@P_STATUS,
	 W_FILEPATH=@P_W_FILEPATH,W_FILENAME=@P_W_FILENAME
	
	 where ID=@P_ID

	SELECT @D_COUNT=COUNT(1) FROM TB_NPI_APP_CTQ WHERE DOC_NO=@P_DOC_NO AND SUB_DOC_NO=@P_SUB_DOC_NO
	AND DEPT =@P_DEPT   AND (RESULT='' or RESULT is null)
    if @D_COUNT <=0
		begin
		   SELECT @T_COUNT=COUNT(1) FROM TB_NPI_APP_CTQ WHERE DOC_NO=@P_DOC_NO AND SUB_DOC_NO=@P_SUB_DOC_NO
		   AND (RESULT='' or RESULT is null)
		   if @T_COUNT <= 0
		   begin
			 --CTQ全部維護完成
			 set @Result = 'OK;1';
			 update TB_NPI_APP_SUB set CTQ_STATUS = 'Finished' 
			 where DOC_NO =@P_DOC_NO and SUB_DOC_NO =@P_SUB_DOC_NO
		   end
		   else
		   begin
			  --CTQ當前部門維護完成
			 set @Result = 'OK;2';
		   end
		end
   else
		begin
		   --CTQ當前部門未維護完成
		   set @Result = 'OK;3';
		end


END

IF @P_OP_TYPE='UPDATE'
  begin
      UPDATE TB_NPI_APP_CTQ SET 
      TEMPORARY_ACTION=@P_TEMPORARY_ACTION,
      CORRECTIVE_PREVENTIVE_ACTION=@P_CORRECTIVE_PREVENTIVE_ACTION,
      COMPLETE_DATE=@P_COMPLETE_DATE,STATUS=@P_STATUS,
      IMPROVEMENT_STATUS=@P_IMPROVEMENT_STATUS,
       R_FILEPATH=@P_R_FILEPATH,R_FILENAME=@P_R_FILEPATH
      WHERE ID=@P_ID
      set @Result='OK;'
  end

IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_NPI_APP_ATTACHFILE]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_NPI_APP_ATTACHFILE] 
(
	@P_OP_TYPE varchar(10),
	@P_ID  int,
	@P_SUB_DOC_NO varchar(30),
	@P_CASEID int,
	@P_FILE_PATH varchar(255),
	@P_FILE_TYPE varchar(1),
	@P_FILE_NAME varchar(50),
	@P_DEPT varchar(20),
	@P_REMARK nvarchar(100),
	@P_APPROVER varchar(50),
	@P_APPROVER_OPINION varchar(50),
	
	@P_APPROVER_DATE datetime,
	@P_UPDATE_TIME datetime,
	@P_UPDATE_USERID varchar(30),

	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
Declare @Count int;
IF @P_OP_TYPE='ADD' 
BEGIN
  SELECT @Count=COUNT(1) from TB_NPI_APP_ATTACHFILE
  WHERE CASEID=@P_CASEID AND  DEPT=@P_DEPT
  IF @Count>0
   SET @Result = 'NG;Attachent Duplcaiton'
  ELSE
   BEGIN
	 INSERT INTO TB_NPI_APP_ATTACHFILE 
	 (SUB_DOC_NO,CASEID,FILE_PATH,FILE_TYPE,FILE_NAME,DEPT,REMARK,
	  UPDATE_TIME,UPDATE_USERID) 
	 VALUES 
	 (@P_SUB_DOC_NO,@P_CASEID,@P_FILE_PATH,@P_FILE_TYPE,@P_FILE_NAME,@P_DEPT,@P_REMARK,
	  GETDATE(),@P_UPDATE_USERID) 
	 SET @Result='OK;'
	END
END

IF @P_OP_TYPE='DELETE'
  BEGIN
     DELETE FROM TB_NPI_APP_ATTACHFILE
     WHERE CASEID=@P_CASEID AND ID=@P_ID
     SET @Result='OK;'
  END
IF @P_OP_TYPE='UPDATE'
   BEGIN
     UPDATE TB_NPI_APP_ATTACHFILE SET
     APPROVER=@P_APPROVER,APPROVER_DATE=GETDATE(),APPROVER_OPINION=@P_APPROVER_OPINION
     WHERE ID=@P_ID
     SET @Result='OK;'
   END

IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_DFX_PARAM]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_DFX_PARAM] 
(
	@P_OP_TYPE varchar(10),
	@P_OP_ID  varchar(50),
	@P_FUNCTION_NAME varchar(50),
	@P_PARAME_NAME varchar(50),
	@P_PARAME_ITEM varchar(50),
	@P_PARAME_VALUE1 varchar(50),
	@P_PARAME_VALUE2 varchar(50),
	@P_PARAME_VALUE3 varchar(50),
	@P_PARAME_VALUE4 varchar(50),
	@P_PARAME_VALUE5 varchar(50),
	@P_Building varchar(20),
	@P_UPDATE_USER varchar(50),
	@P_UPDATE_TIME datetime,
	@P_PARAME_TYPE  varchar(2), --- '0'部門信息,'1' 部門成員信息
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
Declare @Count int
Declare @Count_Value2 int
set @Count=0;
set @Count_Value2=0;
IF @P_OP_TYPE='ADD' 
BEGIN
  IF(@P_PARAME_TYPE='0')
   BEGIN
       SELECT @Count=COUNT(1) FROM TB_DFX_PARAM  WHERE FUNCTION_NAME=@P_FUNCTION_NAME AND PARAME_NAME=@P_PARAME_NAME
       AND PARAME_VALUE1=@P_PARAME_VALUE1 AND PARAME_ITEM=@P_PARAME_ITEM
       AND Building=@P_Building
         IF @Count>0
            SET @Result='NG;'+ '部門已存在,不可重複添加'
       SELECT @Count_Value2=COUNT(1) FROM TB_DFX_PARAM  WHERE FUNCTION_NAME=@P_FUNCTION_NAME AND PARAME_NAME=@P_PARAME_NAME
       AND PARAME_VALUE2=@P_PARAME_VALUE2 AND PARAME_ITEM=@P_PARAME_ITEM
       AND Building=@P_Building
         IF @Count_Value2>0
            SET @Result='NG;'+ '此部門編碼已存在,不可重複添加'
         
    END
 ELSE IF(@P_PARAME_TYPE='1')
  BEGIN
       SELECT @Count=COUNT(1) FROM TB_DFX_PARAM  WHERE FUNCTION_NAME=@P_FUNCTION_NAME AND PARAME_NAME=@P_PARAME_NAME
       AND PARAME_VALUE1=@P_PARAME_VALUE1 AND PARAME_ITEM=@P_PARAME_ITEM AND PARAME_VALUE2=@P_PARAME_VALUE2
       AND Building=@P_Building
         IF @Count>0
            SET @Result='NG;'+ '團隊成員不可重複添加'
  END
 ELSE IF(@P_PARAME_TYPE='2')
  BEGIN
       SELECT @Count=COUNT(1) FROM TB_DFX_PARAM  WHERE FUNCTION_NAME=@P_FUNCTION_NAME 
       AND PARAME_NAME=@P_PARAME_NAME
       AND PARAME_VALUE1=@P_PARAME_VALUE1 
       AND PARAME_ITEM=@P_PARAME_ITEM 
       AND Building=@P_Building
         IF @Count>0
            SET @Result='NG;'+ '此編碼已存在,不可重複添加'
  END
   
  IF(@Count<=0 AND @Count_Value2<=0)
    begin
       INSERT INTO TB_DFX_PARAM 
	   ( FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE1,PARAME_VALUE2,
		 PARAME_VALUE3,PARAME_VALUE4,PARAME_VALUE5,
		 Building,UPDATE_USER,UPDATE_TIME
		) 
	   VALUES 
	   ( @P_FUNCTION_NAME,@P_PARAME_NAME,@P_PARAME_ITEM,
		 @P_PARAME_VALUE1,@P_PARAME_VALUE2,@P_PARAME_VALUE3,
		 @P_PARAME_VALUE4,@P_PARAME_VALUE5,@P_Building,@P_UPDATE_USER,@P_UPDATE_TIME
		) 
     SET @Result='OK;'
    end
 
END

IF @P_OP_TYPE='DELETE'
  BEGIN
  
    DELETE FROM TB_DFX_PARAM  WHERE ID=@P_OP_ID
     SET @Result='OK;'
    
  END

IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_DFX_ITEMUpload]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_DFX_ITEMUpload] 
(
	@P_OP_TYPE varchar(10),
	@P_BU varchar(50),
	@P_Building varchar(50),
	@P_ItemID varchar(50),
	@P_Item varchar(50),
	@P_ItemType nvarchar(400),
	@P_ItemName nvarchar(400),
	@P_Requirements nvarchar(400),
	@P_ProductType nvarchar(40),
	@P_PriorityLevel int,
	@P_Losses nvarchar(40),
	@P_WriteDept nvarchar(40),
	@P_ReplyDept nvarchar(40),
	@P_OldItemType varchar(50),

	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @D_COUNT  int
DECLARE @T_COUNT  int




IF @P_OP_TYPE='ADD'
  BEGIN
INSERT INTO [TB_DFX_Item]
           (
            [BU]
           ,[Building]
		   ,ItemID
           ,[Item]
           ,[ItemType]
           ,[ItemName]
           ,[Requirements]
           ,[ProductType]
           ,[PriorityLevel]
           ,[Losses]
           ,[WriteDept]
           ,[ReplyDept]
           ,[OldItemType])
     VALUES
           (@P_BU,
            @P_Building,
            @P_ItemID,
            @P_Item,
            @P_ItemType,
            @P_ItemName,
            @P_Requirements,
            @P_ProductType,
            @P_PriorityLevel,
            @P_Losses,
            @P_WriteDept,
            @P_ReplyDept,
            @P_OldItemType
            )

		      set @Result = 'OK;';
  END


IF @P_OP_TYPE='UPDATE' 
BEGIN

SET @Result='OK;'
END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_DFX_ITEMBODY]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[P_OP_DFX_ITEMBODY] 
(
	@P_OP_TYPE varchar(10),
	@P_DFXNo varchar(50),
	@P_Item varchar(50),
	@P_ItemType varchar(50),
	@P_ItemName varchar(50),
	@P_Requirements nvarchar(400),
	@P_Losses nvarchar(400),
	@P_Location nvarchar(200),
	@P_Severity nvarchar(40),
	@P_Occurrence nvarchar(40),
	@P_Detection nvarchar(40),
	@P_RPN nvarchar(40),
	@P_Class nvarchar(40),
	@P_PriorityLevel varchar(50),
	@P_MaxPoints varchar(50),
	@P_DFXPoints varchar(50),
	@P_WriteDept varchar(50),
	@P_Compliance varchar(10),
	@P_Comments nvarchar(400),
	@P_Status varchar(50),
	@P_UpdateUser varchar(50),
	@P_UpdateTime datetime,
   
    @P_Actions varchar(50),
    @P_CompleteDate VARCHAR(50),
    @P_Remark nvarchar(50),
    @P_Tracking varchar(20),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @D_COUNT  int
DECLARE @T_COUNT  int




IF @P_OP_TYPE='ADD'
  BEGIN
  
       
      UPDATE TB_DFX_ItemBody SET 
      Severity=@P_Severity,Occurrence=@P_Occurrence,Detection=@P_Detection,
      RPN=@P_RPN,Compliance=@P_Compliance,Status=@P_Status,UpdateUser=@P_UpdateUser,
      UpdateTime=GETDATE(),Location=@P_Location,MaxPoints=@P_MaxPoints,DFXPoints=@P_DFXPoints,
      Comments=@P_Comments
      WHERE DFXNo=@P_DFXNo AND Item=@P_Item
      
      
     --SELECT @D_COUNT=COUNT(1) FROM TB_DFX_ItemBody 
     --WHERE DFXNo=@P_DFXNo  AND WriteDept =@P_WriteDept  
     --AND Status<>'Finished'
    
     --if @D_COUNT <=0
  --   begin
	 --  SELECT @T_COUNT=COUNT(1) FROM TB_DFX_ItemBody 
	 --  WHERE DFXNo=@P_DFXNo 
	 --  AND Status<>'Finished'
  --     if @T_COUNT <= 0
		--   begin
		--	 --DFX全部維護完成
		--	 set @Result = 'OK;1';
		--	 update TB_NPI_APP_SUB set DFX_STAUTS='Finished'
		--	 where  SUB_DOC_NO=@P_DFXNo
		--   end
  --   else
		--   begin
		--	  --DFX當前部門維護完成
		--	 set @Result = 'OK;2';
		--   end
  --  end
  --  else
		--begin
		--   --DFX當前部門未維護完成
		--   set @Result = 'OK;3';
		--end
		      set @Result = 'OK;1';
  END


IF @P_OP_TYPE='UPDATE' 
BEGIN
    UPDATE TB_DFX_ItemBody SET Actions=@P_Actions,CompletionDate=@P_CompleteDate,Remark=@P_Remark,Tracking=@P_Tracking
    ,Status=@P_Status
    WHERE DFXNo=@P_DFXNo AND Item=@P_Item
SET @Result='OK;'
END
IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_GetNumber]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_GetNumber] 
(
@Code1 nvarchar(50),
@Code2 int output
)
AS
SET NOCOUNT ON 
BEGIN
select @Code2 = ( Select Max(Code2) From tb_GetNumber  Where Code1 = @Code1)+1
select @Code2=( select isnull(@Code2, 1))
If @Code2=1
  BEGIN
    SET @Code2=1000
    Insert Into tb_GetNumber Values(@Code1, @Code2)
  END
Else
  BEGIN
    Update  tb_GetNumber Set Code2=@Code2 Where Code1=@Code1
  END
END
GO
/****** Object:  StoredProcedure [dbo].[P_GetDFXNumber]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_GetDFXNumber]
(
	@Type varchar(50),
	@Code1 varchar(50),
	@BU VARCHAR(10),
	@BUILDING VARCHAR(10),
	@RESULT int Output
	
)
AS
DECLARE @Code2 int
DECLARE @used varchar(10)
BEGIN
	
	
	
	
	SELECT @Code2=MAX(PARAME_VALUE2) FROM TB_DFX_PARAM WHERE FUNCTION_NAME='Number'
	 AND PARAME_NAME=@Type AND PARAME_VALUE1=@Code1 AND PARAME_ITEM=@BU AND Building=@BUILDING
	
	IF @Code2 IS NULL
		BEGIN
			INSERT INTO TB_DFX_PARAM 
			(FUNCTION_NAME,PARAME_NAME,PARAME_VALUE1,PARAME_VALUE2,PARAME_VALUE3,UPDATE_USER,UPDATE_TIME,PARAME_ITEM,Building)
			VALUES 
			('Number',@Type,@Code1,1,'N','System',Getdate(),@BU,@BUILDING)
			
			SET @Code2=1 
			
		END 
	ELSE
		BEGIN
			
			SELECT @used = PARAME_VALUE3 FROM TB_DFX_PARAM WHERE  FUNCTION_NAME='Number' AND PARAME_NAME=@Type AND PARAME_VALUE1=@Code1
			  AND PARAME_VALUE2=@Code2 AND PARAME_ITEM=@BU AND Building=@BUILDING
			IF @used='Y'
				BEGIN
					SET @Code2=@Code2+1;
					UPDATE TB_DFX_PARAM SET PARAME_VALUE2=@Code2, PARAME_VALUE3='N',UPDATE_TIME=Getdate(),
					PARAME_ITEM=@BU ,Building=@BUILDING
					WHERE FUNCTION_NAME='Number' AND PARAME_NAME=@Type AND PARAME_VALUE1=@Code1  
					AND PARAME_ITEM=@BU AND Building=@BUILDING
				END 			
			
		END 	
	SET @RESULT=@Code2
END
GO
/****** Object:  StoredProcedure [dbo].[P_GET_REPORT]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_GET_REPORT]
(
  
  @P_STAUTS  VARCHAR(10),
  @P_MODEL VARCHAR(10),
  @P_BEGDATE DATETIME,
  @P_ENDDATE DATETIME

)
AS

BEGIN
	
   SET NOCOUNT ON;
   DECLARE @strwhere  varchar(100)
   DECLARE @sql VARCHAR(8000)
    set @strwhere=''
    SET @sql=''  --初始化变量@sql
IF @P_MODEL<>''
    SET @strwhere=@strwhere+' And a.model='+quotename(@P_MODEL,'''')
IF (@P_STAUTS<> '' and @P_STAUTS<>'ALL')
   SET @strwhere=@strwhere+' and a.Status='+quotename(@P_STAUTS,'''')
IF (@P_BEGDATE<>'' AND @P_ENDDATE <>'')
   SET @strwhere=@strwhere+' AND convert(varchar(10),a.AppDate,121)BETWEEN '+quotename(convert(Varchar(10),@P_BEGDATE,121),'''')+' AND'+quotename(convert(varchar(10),@P_ENDDATE,121),'''')
ELSE IF(@P_BEGDATE<>'' AND @P_ENDDATE='')
     SET @strwhere=@strwhere+' AND convert(varchar(10),a.AppDate,121)>='+quotename(convert(varchar(10), @P_BEGDATE,121),'''')
ELSE IF(@P_BEGDATE='' AND @P_ENDDATE<>'') 
     SET @strwhere=@strwhere+' AND convert(varchar(10),a.AppDate,121)<='+quotename(convert(varchar(10),@P_ENDDATE,121),'''')
 
SELECT @sql=@sql+','+WriteDept FROM 
(
  SELECT T1.DFXNo
      ,T1.model
      ,T1.Phase
      ,T1.PCBREV
      ,T1.Category
      ,T1.Customer
      ,T1.Remark
      ,T1.Status
      ,T1.Progress
      ,T1.UpdateUser
      ,T1.AppDate,
      T2.WriteDept,
      T2.SCORE 
 FROM TB_DFX_Main T1,
 (
  SELECT DFXNO,WriteDept,
 cast((SUM(CAST(DFXPoints AS INT))*100.0/SUM(CAST(MaxPoints AS INT))) as   numeric(15,1))
 AS SCORE FROM
 TB_DFX_ItemBody 
 --WHERE MaxPoints<>'NA'
 GROUP BY DFXNo,WriteDept
 )T2
 where T1.DFXNo=T2.DFXNo

 )Tb GROUP BY WriteDept

SET @sql=stuff(@sql,1,1,'')--去掉首个','

SET @sql='select * from 
(  
   SELECT T1.DFXNo
      ,T1.model
      ,T1.Phase
      ,T1.PCBREV
      ,T1.Category
      ,T1.Customer
      ,T1.Remark
      ,T1.Status
      ,T1.Progress
      ,T1.UpdateUser
      ,T1.AppDate,
      T2.WriteDept,
      T2.SCORE 
 FROM TB_DFX_Main T1
 LEFT JOIN 
 (
  SELECT DFXNO,WriteDept,
 cast((SUM(CAST(DFXPoints AS INT))*100.0/SUM(CAST(MaxPoints AS INT))) as   numeric(15,1))
 AS SCORE FROM
 TB_DFX_ItemBody 
 WHERE 1=1
 GROUP BY DFXNo,WriteDept
 )T2
 ON T1.DFXNo=T2.DFXNo
)
tb 
pivot (max(SCORE) for WriteDept in ('+@sql+'))a'

if (@strwhere<>'')
   
   set @sql=@sql+' WHERE 1=1'+'' +@strwhere

exec (@sql)
PRINT(@sql)
END
GO
/****** Object:  StoredProcedure [dbo].[P_Upload_NPI_Files_PR]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/********  FOR HIS ********/


CREATE  PROCEDURE [dbo].[P_Upload_NPI_Files_PR] 
(
	@PRID INT,
	@P_OP_TYPE varchar(10),
    @DOC_NO nvarchar(50),
    @CASE_ID INT,
    @DEPT nvarchar(50),
	@MFILE_PATH nvarchar(2000),
    @MFILE_NAME nvarchar(100),
    @FILE_REMARK nvarchar(50),
    @UPLOAD_TIME datetime,
    @UPLOADUSER nvarchar(50),
	@Result nvarchar(1000)  out
)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
declare @T_COUNT INT

BEGIN TRAN 
   SET @Result='NG;'
   IF @P_OP_TYPE='ADD' 
   BEGIN
   SELECT @T_COUNT=COUNT(1)from TB_NPI_APP_PR_ATTACHFILE
   WHERE CASEID=@CASE_ID  and [FILE_NAME] =@MFILE_NAME and DEPT=@DEPT
   IF @T_COUNT>0
    BEGIN
      SET @Result=@Result+'The File already exists'
    END
   ELSE
   BEGIN
		INSERT INTO TB_NPI_APP_PR_ATTACHFILE
		   (
		    SUB_DOC_NO
		    ,CASEID
		    ,DEPT
		   ,FILE_PATH
		   ,[FILE_NAME]
		   ,FILE_REMARK
		   ,UPLOAD_TIME
		   ,UPLOADUSER)
		VALUES
		   (
		    @DOC_NO
		   ,@CASE_ID
		   ,@DEPT
		   ,@MFILE_PATH
		   ,@MFILE_NAME
		   ,@FILE_REMARK
		   ,@UPLOAD_TIME
		   ,@UPLOADUSER)
		                   

		set @Result='OK;'
    END
END
IF @P_OP_TYPE='UPDATE'
 BEGIN
    UPDATE  TB_NPI_APP_PR_ATTACHFILE SET FILE_PATH=@MFILE_PATH,
    [FILE_NAME]=@MFILE_NAME,UPLOAD_TIME=GETDATE(),UPLOADUSER=@UPLOADUSER,
    FILE_REMARK=@FILE_REMARK
    WHERE CASEID=@CASE_ID AND DEPT=@DEPT AND ID = @PRID
    SET @Result='OK;'
 END
IF  @@ERROR = 0   
BEGIN
    COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_Upload_NPI_Files_Issue]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/********  FOR HIS ********/


CREATE  PROCEDURE [dbo].[P_Upload_NPI_Files_Issue] 
(
    @DOC_NO varchar(30),
    @CASE_ID INT,
	@MFILE_PATH varchar(255),
    @MFILE_NAME varchar(50),
    @APPROVER varchar(50),
    @UPDATE_TIME datetime,
    @UPDATE_USERID varchar(30),
	@Result nvarchar(1000)  out
)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
declare @T_COUNT INT

BEGIN TRAN 
   SET @Result='NG;'
   SELECT @T_COUNT=COUNT(1)from TB_NPI_APP_ISSUELIST_ATTACHFILE
   WHERE CASEID=@CASE_ID 
   IF @T_COUNT>0
    BEGIN
      SET @Result=@Result+'The File already exists'
    END
   ELSE
   BEGIN
		INSERT INTO TB_NPI_APP_ISSUELIST_ATTACHFILE
		   (
		    SUB_DOC_NO
		    ,CASEID
		   ,FILE_PATH
		   ,[FILE_NAME]
		   ,APPROVER
		   ,UPDATE_TIME
		   ,UPDATE_USERID)
		VALUES
		   (
		    @DOC_NO
		   ,@CASE_ID
		   ,@MFILE_PATH
		   ,@MFILE_NAME
		   ,@APPROVER
		   ,@UPDATE_TIME
		   ,@UPDATE_USERID)
		                   

		set @Result='OK;'
    END

IF  @@ERROR = 0   
BEGIN
    COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_Upload_NPI_Files]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/********  FOR HIS ********/


CREATE  PROCEDURE [dbo].[P_Upload_NPI_Files] 
(
    @DOC_NO varchar(30),
    @CASE_ID INT,
	@MFILE_PATH varchar(255),
	@MFILE_TYPE  varchar(50),
    @MFILE_NAME varchar(50),
    @UPDATE_TIME datetime,
    @UPDATE_USERID varchar(30),
	@Result nvarchar(1000)  out
)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
declare @T_COUNT INT

BEGIN TRAN 
   SET @Result='NG;'
   SELECT @T_COUNT=COUNT(1)from TB_NPI_APP_ATTACHFILE
   WHERE CASEID=@CASE_ID AND FILE_TYPE=@MFILE_TYPE
   IF @T_COUNT>0
    BEGIN
      SET @Result=@Result+'FILE_TYPE Duplication'
    END
   ELSE
   BEGIN
		INSERT INTO TB_NPI_APP_ATTACHFILE
		   (
		    SUB_DOC_NO
		    ,CASEID
		   ,FILE_PATH
		   ,FILE_TYPE
		   ,[FILE_NAME]
		   ,UPDATE_TIME
		   ,UPDATE_USERID)
		VALUES
		   (
		    @DOC_NO
		    ,@CASE_ID
		   ,@MFILE_PATH
		   ,@MFILE_TYPE
		   ,@MFILE_NAME
		   ,@UPDATE_TIME
		   ,@UPDATE_USERID)
		                   

		set @Result='OK;'
    END

IF  @@ERROR = 0   
BEGIN
    COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_Upload_DFXPicture]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_Upload_DFXPicture] 
(
	@P_Type nvarchar(30),
    @Item nvarchar(30),
	@MFILE_PATH nvarchar(255),
    @MFILE_NAME nvarchar(50),
    @UPDATE_TIME datetime,
    @UPDATE_USERID nvarchar(30),
	@Result nvarchar(1000)  out
)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
declare @T_COUNT INT


BEGIN TRAN 
   IF @P_Type='ADD' 
   BEGIN
		INSERT INTO TB_DFX_Picture
		   (
		    Item
		   ,FILE_PATH
		   ,[FILE_NAME]
		   ,UPDATE_TIME
		   ,UPDATE_USERID
		   )
		VALUES
		   (
		    @Item
		   ,@MFILE_PATH
		   ,@MFILE_NAME
		   ,@UPDATE_TIME
		   ,@UPDATE_USERID
		   )
		                   

		set @Result='OK;'
END
IF @P_Type='UPDATE'
 BEGIN
		Update  TB_DFX_Picture set [FILE_NAME] = @MFILE_NAME,FILE_PATH = @MFILE_PATH
		where Item = @Item
    SET @Result='OK;'
 END
IF  @@ERROR = 0   
BEGIN
    COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_Update_NPI_CTQ]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_Update_NPI_CTQ] 
(
    @ID int,
    @DOC_NO varchar(30),
    @SUB_DOC_NO varchar(30),
	@ACT nvarchar(20),
	@RESULTVAR  varchar(10),
	@Comment nvarchar(500),
	@DEPT nvarchar(50),
    @UPDATE_TIME datetime,
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
declare @T_COUNT INT
declare @D_COUNT INT
BEGIN TRAN
SET @Result='NG;'


update TB_NPI_APP_CTQ set
 ACT = @ACT,
 RESULT = @RESULTVAR,
 Comment = @Comment,
 UPDATE_TIME=@UPDATE_TIME 
where ID=@ID

SELECT @D_COUNT=COUNT(1) FROM TB_NPI_APP_CTQ WHERE DOC_NO=@DOC_NO AND SUB_DOC_NO=@SUB_DOC_NO
AND DEPT = @DEPT  AND (RESULT='' or RESULT is null)
if @D_COUNT <=0
begin
   SELECT @T_COUNT=COUNT(1) FROM TB_NPI_APP_CTQ WHERE DOC_NO=@DOC_NO AND SUB_DOC_NO=@SUB_DOC_NO
   AND (RESULT='' or RESULT is null)
   if @T_COUNT <= 0
   begin
     --CTQ全部維護完成
     set @Result = 'OK;1';
     update TB_NPI_APP_SUB set CTQ_STATUS = 'Finished' where DOC_NO = @DOC_NO and SUB_DOC_NO = @SUB_DOC_NO
   end
   else
   begin
      --CTQ當前部門維護完成
     set @Result = 'OK;2';
   end
end
else
begin
   --CTQ當前部門未維護完成
   set @Result = 'OK;3';
end

--SET @Result = 'OK;'
IF  @@ERROR = 0   
BEGIN
    COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  StoredProcedure [dbo].[P_OP_PRELAUNCH_STEP_HANDLER]    Script Date: 07/08/2019 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[P_OP_PRELAUNCH_STEP_HANDLER] 
(
	@P_OP_TYPE varchar(10),
	@P_CASEID int,
	@P_FORMNO varchar(20),
	@P_STEP_NAME varchar(20),
	@P_DEPT varchar(50),
	@P_HANDLER varchar(20),
	@P_UPDATE_TIME datetime,
	@P_BU VARCHAR(20),
	@P_BUILDING VARCHAR(20),
	@Result nvarchar(1000)  out
	)
AS


SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRAN
SET @Result='NG;'
DECLARE @Flow_Count INT
DECLARE @Dept_Count Int
DECLARE @Hander_Count int
DECLARE @SEQ  int
DECLARE @Handler_IsExist int
DECLARE @DPDept_Count int --Dept.Prepared部門統計
DECLARE @MPDept_Count int --MP/Leader.Checked部門統計
DECLARE @MADept_Count int --Manager.Approved部門統計
DECLARE @TM_Count int     --NPI Manager人數統計
DECLARE @TMDept_Count int --Top Manager部門統計
DECLARE @NTM_Count int    --NPI Top Manager人數統計
DECLARE @PMC_Count int    --PMC人數統計
DECLARE @ErroCode int
SET @ErroCode=0
IF @P_OP_TYPE='ADD' 
  BEGIN
       SELECT @Flow_Count=COUNT(1) FROM TB_Prelaunch_DOA
       WHERE STEP_NAME=@P_STEP_NAME
       AND BU=@P_BU 
       
       IF (@Flow_Count=0)
           BEGIN
                SET @ErroCode=1
                SET @Result='NG;关卡'+@P_STEP_NAME +'不存在!'
            END
       ELSE
           BEGIN
                   SELECT @Hander_Count=COUNT(1) FROM SPM.dbo.[USER] WHERE LOGONID=@P_HANDLER
                   AND ENABLE='Y'
                   IF (@Hander_Count=0) 
                      BEGIN
                        SET @ErroCode=2
                         SET @Result='NG;部門'+ @P_DEPT +'簽核人不存在,請維護完整'
                         
                      END
                   ELSE
                      BEGIN
                        IF (@P_STEP_NAME='Dept.Prepared' OR @P_STEP_NAME='Dept.Checked' OR @P_STEP_NAME = 'MP/Leader.Checked')
                               SELECT @Dept_Count= COUNT (distinct Dept)  from TB_Prelaunch_CheckItemConfig
                               WHERE Dept=@P_DEPT 
                               AND Bu=@P_BU and Building=@P_BUILDING
                               IF (@Dept_Count=0)
								  SET @ErroCode=2
								  SET @Result='NG;'+@P_DEPT+'未設定PrelaunchCheckItem'
                       END
              END
        
       IF (@ErroCode=0)      
           BEGIN 
			   SELECT @SEQ=SEQ FROM TB_Prelaunch_DOA
			    WHERE STEP_NAME=@P_STEP_NAME
			    AND BU=@P_BU
			   
			    SELECT @Handler_IsExist=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME=@P_STEP_NAME AND HANDLER=@P_HANDLER AND DEPT=@P_DEPT
			    AND CASEID=@P_CASEID 
			    IF(@Handler_IsExist>0)
				SET @Result='NG;'+@P_STEP_NAME+'签核人重复设定!'
				
				--Dept.Prepared邏輯設定
			    IF(@P_STEP_NAME='Dept.Prepared')
				BEGIN			
				--Dept.Prepared設定部門不重複
				SELECT @DPDept_Count=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME='Dept.Prepared' AND DEPT=@P_DEPT
			    AND CASEID=@P_CASEID
			    IF(@DPDept_Count > 0)
			    SET @Result='NG;Dept關卡'+@P_DEPT+'部門設定重複!' 
			    
			    ELSE
			     BEGIN
					INSERT INTO TB_PRELAUNCH_STEP_HANDLER 
					(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ) 
					VALUES 
					(@P_CASEID,@P_FORMNO,@P_STEP_NAME,@P_DEPT,@P_HANDLER,@P_UPDATE_TIME,'N',@SEQ) 
					SET @Result='OK;'
				 END
				END
				
				--MP/Leader.Checked邏輯設定
				ELSE IF(@P_STEP_NAME='MP/Leader.Checked')
				BEGIN			
				--Dept.Prepared設定部門不重複
				SELECT @MPDept_Count=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME='MP/Leader.Checked' AND DEPT=@P_DEPT
			    AND CASEID=@P_CASEID
			    IF(@MPDept_Count > 0)
			    SET @Result='NG;MP/Leader關卡'+@P_DEPT+'部門設定重複!' 
			    
			    ELSE
			     BEGIN
					INSERT INTO TB_PRELAUNCH_STEP_HANDLER 
					(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ) 
					VALUES 
					(@P_CASEID,@P_FORMNO,@P_STEP_NAME,@P_DEPT,@P_HANDLER,@P_UPDATE_TIME,'N',@SEQ) 
					SET @Result='OK;'
				 END
				END
				
				
				--NPI Manager邏輯設定
				ELSE IF(@P_STEP_NAME='NPI Manager')
				BEGIN
				--NPI Manager設定人數不超過兩人
			    SELECT @TM_Count=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME='NPI Manager' AND CASEID=@P_CASEID
			    
			    --NPI Manager設定部門不重複
				SELECT @TMDept_Count=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME='NPI Manager' AND DEPT=@P_DEPT 
			    AND CASEID=@P_CASEID 
			    
			    IF(@TM_Count>1)	    
				SET @Result='NG;NPI Manager設定不得超過兩人!'
			    ELSE IF(@TMDept_Count > 0)
			    SET @Result='NG;NPI Manager,'+@P_DEPT+'部門設定重複!' 
			    
			    ELSE
			     BEGIN
					INSERT INTO TB_PRELAUNCH_STEP_HANDLER 
					(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ) 
					VALUES 
					(@P_CASEID,@P_FORMNO,@P_STEP_NAME,@P_DEPT,@P_HANDLER,@P_UPDATE_TIME,'N',@SEQ) 
					SET @Result='OK;'
				 END
				END
				
				
				--NPI Top Manager邏輯設定
				ELSE IF(@P_STEP_NAME='NPI Top Manager')
				BEGIN
			    SELECT @NTM_Count=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME='NPI Top Manager' AND CASEID=@P_CASEID 
			    IF(@NTM_Count>0)	    
				SET @Result='NG;NPI Top Manager設定不得超過一人!'
				
			    ELSE
			     BEGIN
					INSERT INTO TB_PRELAUNCH_STEP_HANDLER 
					(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ) 
					VALUES 
					(@P_CASEID,@P_FORMNO,@P_STEP_NAME,@P_DEPT,@P_HANDLER,@P_UPDATE_TIME,'N',@SEQ) 
					SET @Result='OK;'
				 END
				END
				
				--PMC邏輯設定
				ELSE IF(@P_STEP_NAME='PMC')
				BEGIN
			    SELECT @PMC_Count=COUNT(1) FROM TB_Prelaunch_Step_Handler
			    WHERE STEP_NAME='PMC' AND CASEID=@P_CASEID 
			    IF(@PMC_Count>0)	    
				SET @Result='NG;PMC設定不得超過一人!'
				
			    ELSE
			     BEGIN
					INSERT INTO TB_PRELAUNCH_STEP_HANDLER 
					(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ) 
					VALUES 
					(@P_CASEID,@P_FORMNO,@P_STEP_NAME,@P_DEPT,@P_HANDLER,@P_UPDATE_TIME,'N',@SEQ) 
					SET @Result='OK;'
				 END
				END			
								
			    ELSE
			     BEGIN
					INSERT INTO TB_PRELAUNCH_STEP_HANDLER 
					(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ) 
					VALUES 
					(@P_CASEID,@P_FORMNO,@P_STEP_NAME,@P_DEPT,@P_HANDLER,@P_UPDATE_TIME,'N',@SEQ) 
					SET @Result='OK;'
				 END
				 
		    END
                   
     
    PRINT(@Result)
END


IF  @@ERROR = 0   
BEGIN
      COMMIT TRAN
END
ELSE
BEGIN   
      ROLLBACK  TRAN
      SET @Result = 'NG;DB ERROR'--事物執行失敗
END
GO
/****** Object:  Default [DF_TB_APPLICATION_PARAM_ENABLED]    Script Date: 07/08/2019 22:05:48 ******/
ALTER TABLE [dbo].[TB_APPLICATION_PARAM] ADD  CONSTRAINT [DF_TB_APPLICATION_PARAM_ENABLED]  DEFAULT ('Y') FOR [ENABLED]
GO
/****** Object:  Default [DF_TB_APPLICATION_PARAM_UPDATE_TIME]    Script Date: 07/08/2019 22:05:48 ******/
ALTER TABLE [dbo].[TB_APPLICATION_PARAM] ADD  CONSTRAINT [DF_TB_APPLICATION_PARAM_UPDATE_TIME]  DEFAULT (getdate()) FOR [UPDATE_TIME]
GO
