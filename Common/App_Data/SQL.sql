USE [TutorGroup.SmsPlatform]

--新增[雙向簡訊CallBackUrl]
IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'UserReturnUrl' AND Object_ID = Object_ID(N'SmsPlatformAppInfo'))
BEGIN
	ALTER TABLE "SmsPlatformAppInfo" DROP "UserReturnUrl";
	GO
END

ALTER TABLE SmsPlatformAppInfo ADD UserReturnUrl nvarchar(300);
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'雙向簡訊CallBackUrl' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SmsPlatformAppInfo', @level2type=N'COLUMN',@level2name=N'UserReturnUrl'
GO

--新增[任務編號]
IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'JobId' AND Object_ID = Object_ID(N'SmsPlatformRecord'))
BEGIN
    -- Column Exists
	ALTER TABLE "SmsPlatformRecord" DROP "JobId";
END

ALTER TABLE SmsPlatformRecord ADD JobId nvarchar(25);
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任務編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SmsPlatformRecord', @level2type=N'COLUMN',@level2name=N'JobId'
GO

--新增[國碼]
IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Country' AND Object_ID = Object_ID(N'SmsPlatformRecord'))
BEGIN
	ALTER TABLE SmsPlatformRecord DROP COLUMN Country
END
GO

ALTER TABLE SmsPlatformRecord ADD Country int;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SmsPlatformRecord', @level2type=N'COLUMN',@level2name=N'Country'
GO

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Configs'))
BEGIN
    drop table [dbo].[Configs]
END

--新增[共用資料表]
CREATE TABLE [dbo].[Configs](
	[Groups] [nvarchar](20) NOT NULL,
	[Category] [nvarchar](20) NOT NULL,
	[Keys] [nvarchar](200) NOT NULL,
	[Value] [nvarchar](500) NULL,
	[ShowYN] [nvarchar](1) NOT NULL   DEFAULT (N'Y'),
	[Description] [nvarchar](200) NOT NULL   DEFAULT (N'Y'),
	[Order] [int] NOT NULL    DEFAULT ((0)),
	[CreatorId] [nvarchar](20) NULL   DEFAULT (N'0'),
	[CreateOn] [datetime] NOT NULL  DEFAULT (getdate()),
	[LastEditorId] [nvarchar](20) NULL,
	[LastEditOn] [datetime] NULL,
 CONSTRAINT [PK_Configs] PRIMARY KEY CLUSTERED 
(
	[Groups] ASC,
	[Category] ASC,
	[Keys] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'共用參數表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Configs';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'參數群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'Groups'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'參數類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'參數鍵值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'Keys'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'參數值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'ShowYN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'參數敘述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'Order'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'CreatorId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'CreateOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'LastEditorId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configs', @level2type=N'COLUMN',@level2name=N'LastEditOn'
GO



--寫入[參數]
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','EToneNet','Password','va782pi','Y','[SmsProvider][EToneNet][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','EToneNet','Account','7782','Y','[SmsProvider][EToneNet][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','EToneNet','Url','http://esms.etonenet.com/sms/mt','Y','[SmsProvider][EToneNet][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouMarketing','Password','61713030','Y','[SmsProvider][JianZhouMarketing][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouMarketing','Account','sdk_vipabc3','Y','[SmsProvider][JianZhouMarketing][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouMarketing','Url','http://www.jianzhou.sh.cn/JianzhouSMSWSServer/http/','Y','[SmsProvider][JianZhouMarketing][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','EmayExpress','ApiKey','3SDK-EMY-0130-JBQUL','Y','[SmsProvider][EmayExpress][ApiKey]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','EmayExpress','ApiPassword','341738','Y','[SmsProvider][EmayExpress][ApiPassword]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSeaNotify','Password','3852C9CA9EF3A1CB7C34AFF1265E655A','Y','[SmsProvider][SunSeaNotify][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSeaNotify','Account','SDK-LRF-010-00067','Y','[SmsProvider][SunSeaNotify][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSeaNotify','Url','http://sdk.entinfo.cn:80/','Y','[SmsProvider][SunSeaNotify][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','ChtSmsSc','SendRetryCount','5','Y','[SmsProvider][ChtSmsSc][SendRetryCount]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','MDNList','0985012968,0985015968,0985006568,0985016368,0985015368','Y','[SmsProvider][AsiaPacific][MDNList]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','MDNHkList','0985012968,0985015968,0985006568,0985016368,0985015368','Y','[SmsProvider][AsiaPacific][MDNHkList]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','Url','http://xsms.aptg.com.tw/XSMSAP/api/','Y','[SmsProvider][AsiaPacific][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','Account','tutorabc','Y','[SmsProvider][AsiaPacific][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','SendRetryCount','5','Y','[SmsProvider][AsiaPacific][SendRetryCount]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','MDN','0985012968','Y','[SmsProvider][AsiaPacific][MDN]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','AsiaPacific','Password','TutorAbc','Y','[SmsProvider][AsiaPacific][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouNotify','Password','61713030','Y','[SmsProvider][JianZhouNotify][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouNotify','Account','sdk_vipabc2','Y','[SmsProvider][JianZhouNotify][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouNotify','Url','http://www.jianzhou.sh.cn/JianzhouSMSWSServer/http/','Y','[SmsProvider][JianZhouNotify][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouVerify','Password','61713030','Y','[SmsProvider][JianZhouVerify][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouVerify','Account','sdk_vipabc','Y','[SmsProvider][JianZhouVerify][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhouVerify','Url','http://www.jianzhou.sh.cn/JianzhouSMSWSServer/http/','Y','[SmsProvider][JianZhouVerify][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','HiNet','SendRetryCount','5','Y','[SmsProvider][HiNet][SendRetryCount]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','HiNet','Passwd','tabcpc08','Y','[SmsProvider][HiNet][Passwd]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','HiNet','Server','api.hiair.hinet.net','Y','[SmsProvider][HiNet][Server]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','HiNet','UserId','89919415','Y','[SmsProvider][HiNet][UserId]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','HiNet','Port','8000','Y','[SmsProvider][HiNet][Port]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhou','Password','61713030','Y','[SmsProvider][JianZhou][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhou','Account','sdk_vipabc2','Y','[SmsProvider][JianZhou][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhou','Url','http://www.jianzhou.sh.cn/JianzhouSMSWSServer/http/','Y','[SmsProvider][JianZhou][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','JianZhou','SendRetryCount','5','Y','[SmsProvider][JianZhou][SendRetryCount]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','DXTon','Password','Vipabc123','Y','[SmsProvider][DXTon][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','DXTon','Account','5220248','Y','[SmsProvider][DXTon][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','DXTon','Url','http://http.chinasms.com.cn/','Y','[SmsProvider][DXTon][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','DXTon','SendRetryCount','5','Y','[SmsProvider][DXTon][SendRetryCount]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Montnets','Password','332620','Y','[SmsProvider][Montnets][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Montnets','Url','http://61.135.198.131:8023/MWGate/wmgw.asmx','Y','[SmsProvider][Montnets][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Montnets','UserId','J70126','Y','[SmsProvider][Montnets][UserId]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Emay','ApiKey','3SDK-EMY-0130-MKSPO','Y','[SmsProvider][Emay][ApiKey]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Emay','ApiPassword','187065','Y','[SmsProvider][Emay][ApiPassword]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSeaCode','Password','B1C85A7DDD6DBE3884362183B0E4FF24','Y','[SmsProvider][SunSeaCode][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSeaCode','Account','SDK-LRF-010-00068','Y','[SmsProvider][SunSeaCode][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSeaCode','Url','http://sdk.entinfo.cn:80/','Y','[SmsProvider][SunSeaCode][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','CallBackPassword','fDtNhT13','Y','[SmsProvider][Every8d][CallBackPassword]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','CallBackDomain','TutorABC','Y','[SmsProvider][Every8d][CallBackDomain]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','Pwd','1234','Y','[SmsProvider][Every8d][Pwd]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','Url','http://biz2.every8d.com/tutorabc/API21/HTTP/sendsms.ashx','Y','[SmsProvider][Every8d][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','CallBackAccount','appuser','Y','[SmsProvider][Every8d][CallBackAccount]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','UID','ent_tutorabc','Y','[SmsProvider][Every8d][UID]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','Every8d','CheckBalanceUrl','http://biz2.every8d.com/tutorabc/API21/HTTP/getCredit.ashx','Y','[SmsProvider][Every8d][CheckBalanceUrl]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSea','Password','6876EFA8789AC39C4DAE6D2B7D9EDC76','Y','[SmsProvider][SunSea][Password]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSea','Account','SDK-LRF-010-00069','Y','[SmsProvider][SunSea][Account]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSea','Url','http://sdk.entinfo.cn:80/','Y','[SmsProvider][SunSea][Url]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SmsProvider','SunSea','Channel','1','Y','[SmsProvider][SunSea][Channel]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsPlatformTaskSerial','SourceType,TaskSn,CreatorId,CreateOn,LastEditorId,LastEditOn','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsPlatformTaskSerial]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsSchedule','Id,ApplicationSn,JobId,SmsCorpId,Country,Phone,ClientSn,LeadSn,BrandId,DepartmentId,SendSubject,SendMessage,ScheduleTime,ExpireTime,DefaultSuffix,CallBackUrl,IsProccessed,SendStatus,CreatorId,CreateOn,LastEditorId,LastEditOn','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsSchedule]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsPlatformRecord','Id,AppInfoId,SmsCorpId,Phone,ExtSerial,Message,SentTime,DepartmentId,IsReply,CallbackUrl,Result,CreatorId,CreateOn,ClientSn,LeadSn,EventId,IsSuccess,TaskId,RawResponseData,SmsSendingStatus,SmsPlatformTaskId,SmsCorpNotifyUrl,JobId,Country','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsPlatformRecord]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsEvent','SmsEventId,EventName,SmsSubject,DefaultSmsContent,Valid,[Count],EventStartTime,EventEndTime,CreatorId,CreateOn,LastEditorId,LastEditOn','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsEvent]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsPlatformAppInfo','Id,ApplicationName,DepartmentId,ApplicationSn,[Count],Valid,IsTest,CreatorId,CreateOn,LastEditorId,LastEditOn','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsPlatformAppInfo]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsPlatformStatusReportHistory','Id,SmsPlatformRecordId,LastStatus,ReturnCode,ReturnCodeDesc,RawResponseData,CreateOn,CreatorId','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsPlatformStatusReportHistory]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsCorp','Id,CorpName,CorpCName,CanWaitReply,CanCallback,[Count],Valid,CreatorId,CreateOn,LastEditorId,LastEditOn,BatchLimit,MessageLimit,CMessageLimit,Area','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsCorp]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsPlatformSendingStatus','Id,StatusCode,StatusDesc','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsPlatformSendingStatus]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-Configs','Groups,Category,Keys,Value,ShowYN,Description,[Order],CreatorId,CreateOn,LastEditorId,LastEditOn','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-Configs]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('Package','Tools.DataAccess','TutorGroup.SmsPlatform-SmsPlatformAppSmsCorp','Id,AppInfoId,SmsCorpId,Sort,[Count],CreatorId,CreateOn,LastEditorId,LastEditOn,IsValid','Y','[Package][Tools.DataAccess][TutorGroup.SmsPlatform-SmsPlatformAppSmsCorp]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','EmayChangeTo','AsiaPacificCN','Y','[SystemConfig][AppSettings][EmayChangeTo]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','TestLimit','50','Y','[SystemConfig][AppSettings][TestLimit]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','ForceToAsyncAppSns','5320A679-5833-4AF4-9947-EE23215BCA76,48C9950B-4299-4384-8396-95E0EF6CCD05,D88AA82C-5345-4D90-8206-110AF45E5BFF,C3C78FC7-9F81-4307-89AE-9A76B60A67BC,3A7186F3-41C4-46DF-A746-4B211EB2A05E,1F6B516F-C26E-44D0-BADF-54CED2C61B58,5D05488F-6B44-46D8-A77E-FA52ADEB6EB9,453B3575-2AC8-4358-A708-7DDE24FE7FA5,6721213E-2966-45F5-9F0D-12CCAFC9A282,C30956E9-971A-40B8-9E16-B14CBFAC0B89,D7A2A69B-72E8-484E-8471-0BADF2456B5F,0E885F54-CD9B-4C22-AC6E-679A032E00B6','Y','[SystemConfig][AppSettings][ForceToAsyncAppSns]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','VIPABCSuffix','(退订回复TD)','Y','[SystemConfig][AppSettings][VIPABCSuffix]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','SmsCorpAssemblyName','TutorGroup.SmsPlatform.Sms','Y','[SystemConfig][AppSettings][SmsCorpAssemblyName]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','RegisterEmailTemplate','~/App_Data/RegisterNotifyMail.htm','Y','[SystemConfig][AppSettings][RegisterEmailTemplate]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','ForceToAsyncSmsCorpIds','8','Y','[SystemConfig][AppSettings][ForceToAsyncSmsCorpIds]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','UnregisterAppSn','E7203ED7-4D0F-4986-85FC-4CEFCEA230B0','Y','[SystemConfig][AppSettings][UnregisterAppSn]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','SunSeaChangeTo','AsiaPacificCN','Y','[SystemConfig][AppSettings][SunSeaChangeTo]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','SunSeaNotifyChangeTo','AsiaPacificCN','Y','[SystemConfig][AppSettings][SunSeaNotifyChangeTo]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','VIPABCPrefix','【vipabc】','Y','[SystemConfig][AppSettings][VIPABCPrefix]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','EmayExpressChangeTo','AsiaPacificCN','Y','[SystemConfig][AppSettings][EmayExpressChangeTo]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','ServerIPs','webapi.tutorabc.com/sms;services.tutorabc.com/sms','Y','[SystemConfig][AppSettings][ServerIPs]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','SunSeaCodeChangeTo','AsiaPacificCN','Y','[SystemConfig][AppSettings][SunSeaCodeChangeTo]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','SmsCorpTypeNameFormat','TutorGroup.SmsPlatform.Sms.Corporation.{0}Sms','Y','[SystemConfig][AppSettings][SmsCorpTypeNameFormat]',1)
GO
INSERT INTO [TutorGroup.SmsPlatform].[dbo].[Configs] ([Groups],[Category],[Keys],[Value],[ShowYN],[Description],[Order]) 
 VALUES ('SystemConfig','AppSettings','SystemSuffix','(系統)','Y','[SystemConfig][AppSettings][SystemSuffix]',1)
GO