USE [DB_DoctorBooking]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DoctorId] [int] NOT NULL,
	[AppointmentDate] [date] NOT NULL,
	[AppointmentTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[Status] [nvarchar](20) NULL,
	[PatientNotes] [nvarchar](500) NULL,
	[DoctorNotes] [nvarchar](500) NULL,
	[ConsultationFee] [decimal](10, 2) NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[QueueNumber] [varchar](20) NULL,
	[PaymentId] [int] NULL,
	[Symptoms] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blogs]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](300) NULL,
	[CreatedAt] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctors]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctors](
	[DoctorId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Photo] [nvarchar](200) NULL,
	[SpecializationId] [int] NOT NULL,
	[ProviderId] [int] NOT NULL,
	[LicenseNumber] [nvarchar](50) NULL,
	[ConsultationFee] [decimal](10, 2) NULL,
	[Biography] [nvarchar](1000) NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[DoctorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DoctorSchedules]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorSchedules](
	[SchedulesID] [int] IDENTITY(1,1) NOT NULL,
	[DoctorID] [int] NOT NULL,
	[DayOfWeek] [tinyint] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[SlotDuration] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[SchedulesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods](
	[PaymentMethodId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Code] [nvarchar](50) NULL,
	[Icon] [nvarchar](200) NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentId] [int] NOT NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[Amount] [decimal](10, 2) NULL,
	[TransactionId] [nvarchar](100) NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CompletedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Providers]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Providers](
	[ProviderId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](500) NULL,
	[City] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProviderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[DoctorId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[AppointmentId] [int] NOT NULL,
	[Rating] [float] NULL,
	[Comment] [nvarchar](1000) NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specializations]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specializations](
	[SpecializationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Icon] [nvarchar](200) NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[SpecializationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 22/07/2025 18.51.25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Gender] [nvarchar](10) NULL,
	[DateOfBirth] [date] NULL,
	[Address] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Appointments] ON 

INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (1, 1, 6, CAST(N'2025-07-27' AS Date), CAST(N'10:00:00' AS Time), CAST(N'10:30:00' AS Time), N'Scheduled', N'Update: Sakit kepala sudah berkurang, tapi masih ingin konsultasi', NULL, CAST(120000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (2, 1, 7, CAST(N'2025-07-17' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Scheduled', N'Saya ingin konsultasi', NULL, CAST(150000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (3, 1, 8, CAST(N'2025-07-17' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Scheduled', N'Saya ingin konsultasi', NULL, CAST(130000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (4, 1, 9, CAST(N'2025-07-17' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Scheduled', N'Saya ingin konsultasi', NULL, CAST(145000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (5, 1, 10, CAST(N'2025-07-17' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Scheduled', N'Saya ingin konsultasi', NULL, CAST(100000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (6, 1, 11, CAST(N'2025-07-17' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Scheduled', N'Saya ingin konsultasi', NULL, CAST(160000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (7, 1, 12, CAST(N'2025-07-17' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Scheduled', N'Saya ingin konsultasi', NULL, CAST(155000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (8, 1, 13, CAST(N'2025-07-21' AS Date), CAST(N'16:00:00' AS Time), CAST(N'16:30:00' AS Time), N'Scheduled', N'Saya butuh pengecekan', NULL, CAST(170000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), CAST(N'2025-07-17T19:47:08.5033333' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (9, 1, 6, CAST(N'2025-07-21' AS Date), CAST(N'08:00:00' AS Time), CAST(N'08:30:00' AS Time), N'Scheduled', N'Keluhan pusing sejak pagi', NULL, CAST(120000.00 AS Decimal(10, 2)), 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (1009, 3, 7, CAST(N'2025-07-21' AS Date), CAST(N'16:00:00' AS Time), CAST(N'16:30:00' AS Time), N'Completed', N'Tes Senin sore', NULL, CAST(150000.00 AS Decimal(10, 2)), 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'A-001', NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (1011, 3, 8, CAST(N'2025-07-21' AS Date), CAST(N'16:00:00' AS Time), CAST(N'16:30:00' AS Time), N'Cancelled', N'Tes Senin', NULL, CAST(130000.00 AS Decimal(10, 2)), 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-20T17:19:55.0818320' AS DateTime2), N'A-001', NULL, NULL)
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (1026, 1, 7, CAST(N'2025-07-25' AS Date), CAST(N'09:00:00' AS Time), CAST(N'09:30:00' AS Time), N'Completed', N'Kontrol rutin', NULL, CAST(150000.00 AS Decimal(10, 2)), 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-20T18:38:30.6513891' AS DateTime2), N'D7-2507-1', NULL, N'Demam dan batuk')
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (1028, 1, 7, CAST(N'2025-07-25' AS Date), CAST(N'11:00:00' AS Time), CAST(N'11:30:00' AS Time), N'Scheduled', N'Test booking via QR code', NULL, CAST(150000.00 AS Decimal(10, 2)), 1, CAST(N'2025-07-20T19:31:10.9184727' AS DateTime2), CAST(N'2025-07-20T19:31:10.9197233' AS DateTime2), N'D7-2507-2', NULL, N'Sakit kepala ringan')
INSERT [dbo].[Appointments] ([AppointmentId], [UserId], [DoctorId], [AppointmentDate], [AppointmentTime], [EndTime], [Status], [PatientNotes], [DoctorNotes], [ConsultationFee], [IsActive], [CreatedAt], [UpdatedAt], [QueueNumber], [PaymentId], [Symptoms]) VALUES (1029, 1, 7, CAST(N'2025-07-25' AS Date), CAST(N'09:30:00' AS Time), CAST(N'10:00:00' AS Time), N'Scheduled', N'Kontrol rutin', NULL, CAST(150000.00 AS Decimal(10, 2)), 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'D7-2507-3', NULL, N'Demam dan batuk')
SET IDENTITY_INSERT [dbo].[Appointments] OFF
GO
SET IDENTITY_INSERT [dbo].[Blogs] ON 

INSERT [dbo].[Blogs] ([Id], [Title], [Description], [ImageUrl], [CreatedAt], [IsActive]) VALUES (1, N'Prevent the spread of Bird Flu Virus', N'Learn how to prevent bird flu virus and stay healthy with simple steps.', N'birdflu.jpeg', CAST(N'2025-07-21T17:48:31.133' AS DateTime), 1)
INSERT [dbo].[Blogs] ([Id], [Title], [Description], [ImageUrl], [CreatedAt], [IsActive]) VALUES (2, N'New Specialist Doctor Joined', N'We are proud to announce that Dr. Amanda Smith, a cardiologist, has joined our team.', N'doctor-join.jpeg', CAST(N'2025-07-21T17:48:31.133' AS DateTime), 1)
INSERT [dbo].[Blogs] ([Id], [Title], [Description], [ImageUrl], [CreatedAt], [IsActive]) VALUES (3, N'Free Health Checkup Week', N'Enjoy free health checkups from 1st to 7th August for all registered members.', N'health-check.jpeg', CAST(N'2025-07-21T17:48:31.133' AS DateTime), 1)
INSERT [dbo].[Blogs] ([Id], [Title], [Description], [ImageUrl], [CreatedAt], [IsActive]) VALUES (4, N'Berita Teknologi Terbaru', N'Artikel ini membahas tren teknologi terkini di tahun 2025.', N'https://example.com/images/tech2025.jpg', CAST(N'2025-07-22T12:53:54.590' AS DateTime), 0)
INSERT [dbo].[Blogs] ([Id], [Title], [Description], [ImageUrl], [CreatedAt], [IsActive]) VALUES (6, N'Website Edufir Smart', N'Fitur new telah dirilis di Edufir Smart', N'Edufir.jpg', CAST(N'2025-07-22T13:25:16.000' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Blogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Doctors] ON 

INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (6, N'Dr. Patricia Ahoy', N'patricia@hermina.com', N'08199990000', N'dr_patricia_ahoy.jpeg', 1, 1, N'ENT-0101', CAST(120000.00 AS Decimal(10, 2)), N'Dr. Patricia Ahoy is a senior ENT specialist at RS Hermina Malang. She provides ear, nose, and throat consultation.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (7, N'Dr. Nadia Prameswari', N'nadia@siloam.com', N'08123456789', N'dr_nadia.jpeg', 2, 2, N'PSY-9090', CAST(150000.00 AS Decimal(10, 2)), N'Dr. Nadia is a psychiatrist at RS Siloam, specializing in mental health.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (8, N'Dr. Tommy Santoso', N'tommy@mayapada.com', N'08222333444', N'dr_tommy.jpeg', 3, 3, N'DEN-1234', CAST(130000.00 AS Decimal(10, 2)), N'Dr. Tommy is a dentist with years of experience at RS Mayapada.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (9, N'Dr. Liana Putri', N'liana@kimiafarma.com', N'085500112233', N'dr_liana.jpeg', 4, 4, N'DER-8989', CAST(145000.00 AS Decimal(10, 2)), N'Dr. Liana is a skin specialist at Klinik Kimia Farma.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (10, N'Dr. Aldi Rahman', N'aldi@hermina.com', N'08111222333', N'dr_aldi.jpeg', 5, 1, N'GEN-4444', CAST(100000.00 AS Decimal(10, 2)), N'Dr. Aldi is a general practitioner at RS Hermina Malang.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (11, N'Dr. Maya Lestari', N'maya@siloam.com', N'08177778888', N'dr_maya.jpeg', 6, 2, N'EYE-9911', CAST(160000.00 AS Decimal(10, 2)), N'Dr. Maya is an eye specialist at RS Siloam Jakarta.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (12, N'Dr. Kevin Nugraha', N'kevin@mayapada.com', N'08334445566', N'dr_kevin.jpeg', 7, 3, N'PED-4455', CAST(155000.00 AS Decimal(10, 2)), N'Dr. Kevin is a pediatrician caring for children at RS Mayapada.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (13, N'Dr. Susi Hartati', N'susi@kimiafarma.com', N'08567788990', N'dr_susi.jpeg', 8, 4, N'CAR-7878', CAST(170000.00 AS Decimal(10, 2)), N'Dr. Susi is a cardiologist at Klinik Kimia Farma.', 1, CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2), CAST(N'2025-07-17T19:47:08.4566667' AS DateTime2))
INSERT [dbo].[Doctors] ([DoctorId], [FullName], [Email], [PhoneNumber], [Photo], [SpecializationId], [ProviderId], [LicenseNumber], [ConsultationFee], [Biography], [IsActive], [CreatedAt], [UpdatedAt]) VALUES (25, N'Dr. Siti Rahma', N'siti@clinic.com', N'08123456789', N'https://example.com/photo.jpg', 1, 1, N'ABC123', CAST(150000.00 AS Decimal(10, 2)), N'Spesialis jantung dengan pengalaman 10 tahun.', 0, CAST(N'2025-07-20T05:35:53.3271336' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Doctors] OFF
GO
SET IDENTITY_INSERT [dbo].[DoctorSchedules] ON 

INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (1, 6, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (2, 7, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (3, 8, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (4, 9, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (5, 10, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (6, 11, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (7, 12, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (8, 13, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (9, 6, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (10, 7, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (11, 8, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (12, 9, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (13, 10, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (14, 11, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (15, 12, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (16, 13, 2, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (17, 6, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (18, 7, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (19, 8, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (20, 9, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (21, 10, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (22, 11, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (23, 12, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (24, 13, 3, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (25, 6, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (26, 7, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (27, 8, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (28, 9, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (29, 10, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (30, 11, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (31, 12, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (32, 13, 4, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (33, 6, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (34, 7, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (35, 8, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (36, 9, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (37, 10, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (38, 11, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (39, 12, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (40, 13, 5, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-17T19:47:08.4866667' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (42, 7, 1, CAST(N'15:00:00' AS Time), CAST(N'19:00:00' AS Time), 30, 1, CAST(N'2025-07-20T14:26:03.3200000' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (43, 8, 7, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 30, 1, CAST(N'2025-07-20T14:26:03.3200000' AS DateTime2))
INSERT [dbo].[DoctorSchedules] ([SchedulesID], [DoctorID], [DayOfWeek], [StartTime], [EndTime], [SlotDuration], [IsActive], [CreatedAt]) VALUES (44, 9, 6, CAST(N'14:00:00' AS Time), CAST(N'18:00:00' AS Time), 30, 0, CAST(N'2025-07-20T14:26:03.3200000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[DoctorSchedules] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentMethods] ON 

INSERT [dbo].[PaymentMethods] ([PaymentMethodId], [Name], [Code], [Icon], [IsActive], [CreatedAt]) VALUES (1, N'Credit Card', N'credit_card', N'creditcard.png', 1, CAST(N'2025-07-17T20:44:20.6633333' AS DateTime2))
INSERT [dbo].[PaymentMethods] ([PaymentMethodId], [Name], [Code], [Icon], [IsActive], [CreatedAt]) VALUES (2, N'Mandiri', N'mandiri', N'mandiri.png', 1, CAST(N'2025-07-17T20:44:20.6633333' AS DateTime2))
INSERT [dbo].[PaymentMethods] ([PaymentMethodId], [Name], [Code], [Icon], [IsActive], [CreatedAt]) VALUES (3, N'BCA', N'bca', N'bca.png', 1, CAST(N'2025-07-17T20:44:20.6633333' AS DateTime2))
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
GO
SET IDENTITY_INSERT [dbo].[Providers] ON 

INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (1, N'RS. Hermina', N'Jl. Tangkuban Perahu No 31-33, Kauman, Kec. Klojen, Kota Malang, Jawa Timur 65119', N'Malang', 1, CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), -7.977, 112.634)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (2, N'RS Siloam', N'Jl. Raya TB Simatupang, Jakarta', N'Jakarta', 1, CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), -6.2917, 106.7845)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (3, N'RS Mayapada', N'Jl. Lebak Bulus, Jakarta', N'Jakarta', 1, CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), -6.3038, 106.7825)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (4, N'Klinik Kimia Farma', N'Jl. Sudirman, Jakarta', N'Jakarta', 1, CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), CAST(N'2025-07-15T05:59:22.9066667' AS DateTime2), -6.2115, 106.8236)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (5, N'RS Pondok Kelapa', N'Jl. Metro Duta Kav. UE Pondok Kelapa, Jakarta Selatan', N'Jakarta', 1, CAST(N'2025-07-20T06:48:48.0009492' AS DateTime2), CAST(N'2025-07-20T06:53:25.1549395' AS DateTime2), -6.2434, 106.8981)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (6, N'RSUD Kota Malang', N'Jl. Rajasa No. 1, Malang', N'Malang', 0, CAST(N'2025-07-20T06:49:49.2860308' AS DateTime2), CAST(N'2025-07-20T07:00:44.3272599' AS DateTime2), -7.9778, 112.638)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (7, N'Klinik Sehat Sentosa', N'Jl. Kenanga Raya No.88, Bandung', N'Bandung', 1, CAST(N'2025-07-20T06:51:21.1133629' AS DateTime2), CAST(N'2025-07-20T06:51:21.1135407' AS DateTime2), -6.9147, 107.6098)
INSERT [dbo].[Providers] ([ProviderId], [Name], [Address], [City], [IsActive], [CreatedAt], [UpdatedAt], [Latitude], [Longitude]) VALUES (8, N'RS Premier Jatinegara', N'Jl. Jatinegara Barat No.126, Bali Mester, Jakarta Timur', N'Jakarta', 1, CAST(N'2025-07-20T07:12:00.4100416' AS DateTime2), CAST(N'2025-07-20T07:12:00.4100427' AS DateTime2), -6.2215, 106.8705)
SET IDENTITY_INSERT [dbo].[Providers] OFF
GO
SET IDENTITY_INSERT [dbo].[Reviews] ON 

INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (1, 6, 1, 1, 5, N'Dokter Patricia sangat ramah dan membantu.', CAST(N'2025-07-20T12:44:09.1800000' AS DateTime2))
INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (2, 7, 1, 2, 4, N'Konsultasi dengan Dr. Nadia cukup jelas.', CAST(N'2025-07-20T12:44:09.1800000' AS DateTime2))
INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (3, 8, 1, 3, 5, N'Dr. Tommy memberikan penjelasan detail.', CAST(N'2025-07-20T12:44:09.1800000' AS DateTime2))
INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (4, 9, 1, 4, 3, N'Pelayanan cukup baik.', CAST(N'2025-07-20T12:44:09.1800000' AS DateTime2))
INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (5, 10, 1, 5, 4, N'Dokter Aldi baik, tapi antriannya lama.', CAST(N'2025-07-20T12:44:09.1800000' AS DateTime2))
INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (9, 6, 1, 8, 4, N'Pelayanan cepat, terima kasih dok!', CAST(N'2025-07-20T12:46:13.6226239' AS DateTime2))
INSERT [dbo].[Reviews] ([ReviewId], [DoctorId], [UserId], [AppointmentId], [Rating], [Comment], [CreatedAt]) VALUES (10, 9, 2, 4, 3.5, N'bagus tapi kurang', CAST(N'2025-07-20T15:37:33.6725804' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Reviews] OFF
GO
SET IDENTITY_INSERT [dbo].[Specializations] ON 

INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (1, N'Ear, Nose & Throat', N'ENT Specialist', N'ear_nose_throat.png', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (2, N'Psychiatrist', N'Mental Health Specialist', N'psychiatrist.jpeg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (3, N'Dentist', N'Dental Care Specialist', N'dentist.jpeg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (4, N'Dermato-veneorologis', N'Skin & Venereal Disease Specialist', N'dermatology.jpeg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (5, N'General Practitioner', N'General Medicine', N'general_doctor.jpeg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (6, N'Ophthalmologist', N'Eye Care Specialist', N'eye_doctor.jpeg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (7, N'Pediatrician', N'Child Health Specialist', N'pediatric.jpg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
INSERT [dbo].[Specializations] ([SpecializationId], [Name], [Description], [Icon], [CreatedAt]) VALUES (8, N'Cardiologist', N'Heart Specialist', N'heart_doctor.jpeg', CAST(N'2025-07-15T05:59:22.8900000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Specializations] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [FullName], [Email], [PhoneNumber], [PasswordHash], [CreatedAt], [UpdatedAt], [Gender], [DateOfBirth], [Address], [IsActive]) VALUES (1, N'Syafira', N'syafira@gmail.com', N'0987654323', N'$2a$11$3EBeahTv.tWUarYZ1WUfmOoh5rZVrrMNgrhtyL.PAHotth64cTFX6', CAST(N'2025-07-17T06:33:37.6023309' AS DateTime2), CAST(N'2025-07-22T11:48:20.4686709' AS DateTime2), N'F', CAST(N'2025-07-17' AS Date), N'kp.gaga', 1)
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [PhoneNumber], [PasswordHash], [CreatedAt], [UpdatedAt], [Gender], [DateOfBirth], [Address], [IsActive]) VALUES (2, N'Orang2', N'orang@gmail.com', N'0987654323', N'$2a$11$sK1OWwN5ceYQ4tAYpi.GLu/WDiGNwKW7WsyM4EymURCCsJEsWNdPy', CAST(N'2025-07-17T06:36:15.7023437' AS DateTime2), CAST(N'2025-07-22T11:48:05.1966017' AS DateTime2), N'M', CAST(N'2025-07-17' AS Date), N'kp.orang', 1)
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [PhoneNumber], [PasswordHash], [CreatedAt], [UpdatedAt], [Gender], [DateOfBirth], [Address], [IsActive]) VALUES (3, N'Budi Santoso', N'budi@example.com', N'08123456789', N'$2a$11$yo8aTeOwvguFRR27.PRPMeYPF4dNo0xoaAcLb8Ojf6I25vuSWj3HS', CAST(N'2025-07-20T03:22:21.0869013' AS DateTime2), CAST(N'2025-07-20T04:12:17.5532559' AS DateTime2), N'M', CAST(N'2000-01-15' AS Date), N'Jl. Puncak No. 20, Jakarta', 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534E204F8FE]    Script Date: 22/07/2025 18.51.25 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointments] ADD  DEFAULT ('Scheduled') FOR [Status]
GO
ALTER TABLE [dbo].[Appointments] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Appointments] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Appointments] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Blogs] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Blogs] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Doctors] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Doctors] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Doctors] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[DoctorSchedules] ADD  DEFAULT ((30)) FOR [SlotDuration]
GO
ALTER TABLE [dbo].[DoctorSchedules] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[DoctorSchedules] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Providers] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Providers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Specializations] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([DoctorId])
REFERENCES [dbo].[Doctors] ([DoctorId])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payments] ([PaymentId])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Payment]
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD FOREIGN KEY([ProviderId])
REFERENCES [dbo].[Providers] ([ProviderId])
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD FOREIGN KEY([SpecializationId])
REFERENCES [dbo].[Specializations] ([SpecializationId])
GO
ALTER TABLE [dbo].[DoctorSchedules]  WITH CHECK ADD FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctors] ([DoctorId])
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointments] ([AppointmentId])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointments] ([AppointmentId])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([DoctorId])
REFERENCES [dbo].[Doctors] ([DoctorId])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD CHECK  (([Status]='NoShow' OR [Status]='Cancelled' OR [Status]='Completed' OR [Status]='Scheduled'))
GO
ALTER TABLE [dbo].[DoctorSchedules]  WITH CHECK ADD CHECK  (([DayOfWeek]>=(1) AND [DayOfWeek]<=(7)))
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
