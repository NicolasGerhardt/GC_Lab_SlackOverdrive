CREATE DATABASE [SlackOverload]
GO
USE [SlackOverload]?

CREATE TABLE [dbo].[Answers](

	[Id] [int] IDENTITY(1,1) NOT NULL,

	[Username] [nvarchar](30) NOT NULL,

	[Detail] [nvarchar](4000) NOT NULL,

	[QuestionId] [int] NOT NULL,

	[Posted] [datetime] NOT NULL,

	[Upvotes] [int] NOT NULL,

 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED 

(

	[Id] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

?

CREATE TABLE [dbo].[Questions](

	[Id] [int] IDENTITY(1000,1) NOT NULL,

	[Username] [nvarchar](30) NOT NULL,

	[Title] [nvarchar](200) NOT NULL,

	[Detail] [nvarchar](4000) NOT NULL,

	[Posted] [datetime] NOT NULL,

	[Category] [nvarchar](30) NOT NULL,

	[Tags] [nvarchar](200) NULL,

	[Status] [int] NOT NULL,

 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 

(

	[Id] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Answers] FOREIGN KEY([QuestionId])

REFERENCES [dbo].[Questions] ([Id])

GO

ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Questions_Answers]

GO

USE [master]

GO

ALTER DATABASE [SlackOverload] SET  READ_WRITE 

GO