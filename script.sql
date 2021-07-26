USE [db_capstone_prototype]
GO
/****** Object:  Table [dbo].[category]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [int] NOT NULL,
	[value] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[company]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[company](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[headquarters] [ntext] NOT NULL,
	[website] [varchar](50) NULL,
	[description] [ntext] NULL,
	[avatar] [text] NULL,
	[create_date] [date] NULL,
	[recruiter_id] [int] NOT NULL,
 CONSTRAINT [PK_company] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cv]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv](
	[student_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[sex] [bit] NULL,
	[dob] [date] NULL,
	[avatar] [text] NULL,
	[school] [nvarchar](100) NULL,
	[experience] [nvarchar](500) NULL,
	[foreign_language] [nvarchar](10) NULL,
	[desired_salary_minimum] [int] NULL,
	[working_form] [int] NULL,
	[create_date] [date] NULL,
	[is_subscribed] [bit] NULL,
 CONSTRAINT [PK_cv] PRIMARY KEY CLUSTERED 
(
	[student_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[job]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[job](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [ntext] NOT NULL,
	[working_form] [int] NOT NULL,
	[location] [int] NOT NULL,
	[working_place] [ntext] NOT NULL,
	[description] [ntext] NOT NULL,
	[requirement] [ntext] NOT NULL,
	[type] [bit] NOT NULL,
	[offer] [ntext] NOT NULL,
	[sex] [int] NULL,
	[quantity] [int] NOT NULL,
	[salary_min] [int] NOT NULL,
	[salary_max] [int] NOT NULL,
	[recruiter_id] [int] NOT NULL,
	[create_date] [date] NOT NULL,
	[manager_id] [int] NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_job] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[job_has_category]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[job_has_category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
	[create_date] [datetime] NOT NULL,
 CONSTRAINT [PK_job_has_category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[manager]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[manager](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NOT NULL,
	[password] [varchar](150) NOT NULL,
	[full_name] [nvarchar](50) NULL,
	[create_date] [date] NULL,
	[role] [varchar](20) NOT NULL,
	[is_banned] [bit] NULL,
 CONSTRAINT [PK_manager] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[recruiter]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recruiter](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NOT NULL,
	[password] [varchar](150) NOT NULL,
	[gmail] [varchar](50) NOT NULL,
	[phone] [text] NOT NULL,
	[avatar] [text] NULL,
	[create_date] [date] NULL,
	[role] [varchar](20) NOT NULL,
	[is_banned] [bit] NULL,
	[full_name] [nvarchar](50) NULL,
 CONSTRAINT [PK_recruiter] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[student](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gmail] [varchar](50) NOT NULL,
	[phone] [varchar](15) NULL,
	[is_banned] [bit] NULL,
	[create_date] [date] NOT NULL,
	[profile_status] [bit] NOT NULL,
	[avatar] [text] NULL,
	[google_id] [varchar](700) NOT NULL,
 CONSTRAINT [PK_student] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student_apply_job]    Script Date: 7/26/2021 2:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[student_apply_job](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [int] NOT NULL,
	[student_id] [int] NOT NULL,
	[create_date] [date] NOT NULL,
 CONSTRAINT [PK_student_apply_job] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[category] ON 

INSERT [dbo].[category] ([id], [code], [value]) VALUES (1, 1, N'Tất cả ngành nghề')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (2, 2, N'Bán hàng')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (3, 3, N'Tư vấn bảo hiểm')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (4, 4, N'Bất động sản')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (5, 5, N'Công nghệ thông tin')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (6, 6, N'Chăm sóc khách hàng')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (7, 7, N'Du lịch/Nhà hàng/Khách sạn')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (8, 8, N'Giải trí/Vui chơi')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (9, 9, N'Giáo dục/Đào tạo/Thư viện')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (10, 10, N'Giao thông/Vận tải/Thủy lợi/Cầu đường')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (11, 11, N'Kho vận/Vật tư/Thu mua')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (12, 12, N'Kinh doanh')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (13, 13, N'Sinh viên/Mới tốt nghiệp/Thực tập')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (14, 14, N'Quảng cáo/Marketing/PR')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (15, 15, N'Thực phẩm/DV ăn uống')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (16, 16, N'Tài xế/Lái xe/Giao nhận')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (17, 17, N'PG/PB/Lễ tân')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (18, 18, N'Phục vụ/Tạp vụ/Giúp việc')
INSERT [dbo].[category] ([id], [code], [value]) VALUES (19, 19, N'Khác')
SET IDENTITY_INSERT [dbo].[category] OFF
GO
SET IDENTITY_INSERT [dbo].[company] ON 

INSERT [dbo].[company] ([id], [name], [headquarters], [website], [description], [avatar], [create_date], [recruiter_id]) VALUES (1, N'test', N'test', NULL, NULL, NULL, CAST(N'2021-07-17' AS Date), 1)
SET IDENTITY_INSERT [dbo].[company] OFF
GO
SET IDENTITY_INSERT [dbo].[job] ON 

INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (1, N'test', 1, 11, N'test', N'test', N'test', 1, N'test', 1, 12, 12, 12, 1, CAST(N'2021-07-10' AS Date), 1008, 3)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (26, N'test3', 3, 12, N'test', N'test', N'test', 1, N'test', 2, 12, 0, 0, 1, CAST(N'2021-07-11' AS Date), 1008, 2)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (27, N'test3', 3, 12, N'test', N'test', N'test', 1, N'test', 2, 120, 1, 10, 1, CAST(N'2021-07-11' AS Date), 1008, 2)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (1002, N'updated 2', 1, 12, N'2', N'test update', N'test update', 1, N'test update', 1, 120, 2, 10, 1, CAST(N'2021-07-12' AS Date), 1008, 2)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (2002, N'test update', 3, 12, N'test update', N'test update', N'test update', 1, N'test update', 2, 120, 1, 10, 1, CAST(N'2021-07-24' AS Date), 1008, 2)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (2003, N'test 999', 3, 12, N'test update', N'test update', N'test update', 1, N'test update', 2, 120, 1, 10, 1, CAST(N'2021-07-24' AS Date), NULL, 1)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (3002, N'test cor', 3, 12, N'test update', N'test update', N'test update', 1, N'test update', 2, 120, 1, 10, 1, CAST(N'2021-07-26' AS Date), NULL, 1)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (4002, N'test cate', 3, 12, N'test update', N'test update', N'test update', 1, N'test update', 2, 120, 1, 10, 1, CAST(N'2021-07-26' AS Date), NULL, 1)
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status]) VALUES (4003, N'test cate2', 3, 12, N'test update', N'test update', N'test update', 1, N'test update', 2, 120, 1, 10, 1, CAST(N'2021-07-26' AS Date), NULL, 1)
SET IDENTITY_INSERT [dbo].[job] OFF
GO
SET IDENTITY_INSERT [dbo].[job_has_category] ON 

INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1, 1, 1, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (2, 1, 2, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (3, 2003, 1, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (4, 2003, 2, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (5, 3002, 12, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (6, 4003, 1, CAST(N'2021-07-26T13:20:44.873' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (7, 4003, 2, CAST(N'2021-07-26T13:20:44.880' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (8, 4003, 3, CAST(N'2021-07-26T13:20:44.880' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (9, 4003, 4, CAST(N'2021-07-26T13:20:44.880' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (10, 26, 1, CAST(N'2021-07-26T14:25:09.770' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (11, 26, 2, CAST(N'2021-07-26T14:25:13.907' AS DateTime))
SET IDENTITY_INSERT [dbo].[job_has_category] OFF
GO
SET IDENTITY_INSERT [dbo].[manager] ON 

INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (1006, N'admin1', N'AIuVNn34ZcEFHXS5PyWU0IZ02d1o4s6ib7siEN/+4gmc7g4BmR/DVLyaTAXoVWBiRw==', N'admin1', CAST(N'2021-07-06' AS Date), N'ROLE_ADMIN', NULL)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (1007, N'admin2', N'AIx/+aHQLoFbEPbYZUPwoY41eENipSvYGLVvxNi5R9nJNcwX3eIRbnFTzhUVYaRExQ==', N'admin2', CAST(N'2021-07-06' AS Date), N'ROLE_ADMIN', NULL)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (1008, N'staff1', N'ACcxCAc9jnwjzOXtZ+hIvc8em8G9Zt+AWsJHmuT/YrbxiQneIkhrKiZ6ZACZZjJKYw==', N'staff1', CAST(N'2021-07-06' AS Date), N'ROLE_STAFF', 0)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (2006, N'staff2', N'ALTkgATd4F/zwzmIgGN649nXXoWWvG9SeLPJy7dgb5kDtqJzm4EIoo08P8NcB2PBSw==', N'asd', CAST(N'2021-07-07' AS Date), N'ROLE_STAFF', 1)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (3006, N'staff3', N'AO4Wo7Ak4KkTHL2kMuK8LA+OccQevnbcV7L6L9X+8+r6L7OnPvs1P0xmQCUr7v4/rQ==', N'asd', CAST(N'2021-07-09' AS Date), N'ROLE_STAFF', NULL)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (3007, N'staff4', N'AEGuz5sdpNrGkgeECRBE7+g9HWkUf/S4Ymlaf2Laql8u6lY1g8mGD7FMsevqSjmclQ==', N'asd', CAST(N'2021-07-09' AS Date), N'ROLE_STAFF', NULL)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (4006, N'staff5', N'AC1+tfN/6tZaKfXtVtAZlXqzzFrRYi2NuUS195dbwmFvssN4pPW0K951GVDjP5Vd5A==', N'asd', CAST(N'2021-07-10' AS Date), N'ROLE_STAFF', NULL)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (5006, N'staff123', N'AB3H0o84aH2L+0uqv6IXMnTxyNRRbn9kPxtYMWoyCULA0DakrXoRm17UX0qXgWOxOA==', N'asd', CAST(N'2021-07-20' AS Date), N'ROLE_STAFF', NULL)
SET IDENTITY_INSERT [dbo].[manager] OFF
GO
SET IDENTITY_INSERT [dbo].[recruiter] ON 

INSERT [dbo].[recruiter] ([id], [username], [password], [gmail], [phone], [avatar], [create_date], [role], [is_banned], [full_name]) VALUES (1, N'recruiter1', N'ACnlwHmP67OLkSyRjhJWCThnhwDmFPqYsT4YSKVDEjK7Peo0oPBVuiBDnMoFbpYlsQ==', N'gmail', N'09123', N'asd', CAST(N'0001-01-01' AS Date), N'ROLE_RECRUITER', 0, NULL)
INSERT [dbo].[recruiter] ([id], [username], [password], [gmail], [phone], [avatar], [create_date], [role], [is_banned], [full_name]) VALUES (2, N'recruiter_test', N'AIdazSZPg7sykkmBeXhRDPd3/OFAQMAV4RaIVpgly9PMir5cEgOZVIJQjN7ShGyF6Q==', N'recruiter_test@gmail.com', N'01478523', N'arecruitertest', CAST(N'2021-07-24' AS Date), N'ROLE_RECRUITER', NULL, N'recruiterrecruiter')
SET IDENTITY_INSERT [dbo].[recruiter] OFF
GO
SET IDENTITY_INSERT [dbo].[student] ON 

INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id]) VALUES (1, N'cnviety2@gmail.com', NULL, 0, CAST(N'2021-07-23' AS Date), 0, N'https://lh3.googleusercontent.com/a/default-user=s96-c', N'113397064656708997803')
INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id]) VALUES (5, N'duytdse63047@fpt.edu.vn', NULL, 0, CAST(N'2021-07-23' AS Date), 0, N'https://lh3.googleusercontent.com/a/default-user=s96-c', N'108570453383030648023')
INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id]) VALUES (6, N'cnviety98@gmail.com', NULL, 0, CAST(N'2021-07-24' AS Date), 0, N'https://lh3.googleusercontent.com/a/default-user=s96-c', N'115199922347111814881')
SET IDENTITY_INSERT [dbo].[student] OFF
GO
SET IDENTITY_INSERT [dbo].[student_apply_job] ON 

INSERT [dbo].[student_apply_job] ([id], [job_id], [student_id], [create_date]) VALUES (1, 2002, 6, CAST(N'2021-07-24' AS Date))
INSERT [dbo].[student_apply_job] ([id], [job_id], [student_id], [create_date]) VALUES (2, 1, 6, CAST(N'2021-07-24' AS Date))
SET IDENTITY_INSERT [dbo].[student_apply_job] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_recruiter]    Script Date: 7/26/2021 2:32:21 PM ******/
ALTER TABLE [dbo].[recruiter] ADD  CONSTRAINT [IX_recruiter] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_recruiter_1]    Script Date: 7/26/2021 2:32:21 PM ******/
ALTER TABLE [dbo].[recruiter] ADD  CONSTRAINT [IX_recruiter_1] UNIQUE NONCLUSTERED 
(
	[gmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_student]    Script Date: 7/26/2021 2:32:21 PM ******/
ALTER TABLE [dbo].[student] ADD  CONSTRAINT [IX_student] UNIQUE NONCLUSTERED 
(
	[gmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[company]  WITH CHECK ADD  CONSTRAINT [FK_company_recruiter] FOREIGN KEY([recruiter_id])
REFERENCES [dbo].[recruiter] ([id])
GO
ALTER TABLE [dbo].[company] CHECK CONSTRAINT [FK_company_recruiter]
GO
ALTER TABLE [dbo].[cv]  WITH CHECK ADD  CONSTRAINT [FK_cv_student] FOREIGN KEY([student_id])
REFERENCES [dbo].[student] ([id])
GO
ALTER TABLE [dbo].[cv] CHECK CONSTRAINT [FK_cv_student]
GO
ALTER TABLE [dbo].[job]  WITH CHECK ADD  CONSTRAINT [FK_job_manager] FOREIGN KEY([manager_id])
REFERENCES [dbo].[manager] ([id])
GO
ALTER TABLE [dbo].[job] CHECK CONSTRAINT [FK_job_manager]
GO
ALTER TABLE [dbo].[job]  WITH CHECK ADD  CONSTRAINT [FK_job_recruiter] FOREIGN KEY([recruiter_id])
REFERENCES [dbo].[recruiter] ([id])
GO
ALTER TABLE [dbo].[job] CHECK CONSTRAINT [FK_job_recruiter]
GO
ALTER TABLE [dbo].[job_has_category]  WITH CHECK ADD  CONSTRAINT [FK_job_has_category_category] FOREIGN KEY([category_id])
REFERENCES [dbo].[category] ([id])
GO
ALTER TABLE [dbo].[job_has_category] CHECK CONSTRAINT [FK_job_has_category_category]
GO
ALTER TABLE [dbo].[job_has_category]  WITH CHECK ADD  CONSTRAINT [FK_job_has_category_job_has_category] FOREIGN KEY([job_id])
REFERENCES [dbo].[job] ([id])
GO
ALTER TABLE [dbo].[job_has_category] CHECK CONSTRAINT [FK_job_has_category_job_has_category]
GO
ALTER TABLE [dbo].[student_apply_job]  WITH CHECK ADD  CONSTRAINT [FK_student_apply_job_job] FOREIGN KEY([job_id])
REFERENCES [dbo].[job] ([id])
GO
ALTER TABLE [dbo].[student_apply_job] CHECK CONSTRAINT [FK_student_apply_job_job]
GO
ALTER TABLE [dbo].[student_apply_job]  WITH CHECK ADD  CONSTRAINT [FK_student_apply_job_student] FOREIGN KEY([student_id])
REFERENCES [dbo].[student] ([id])
GO
ALTER TABLE [dbo].[student_apply_job] CHECK CONSTRAINT [FK_student_apply_job_student]
GO
