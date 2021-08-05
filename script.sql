USE [capstone2021]
GO
/****** Object:  Table [dbo].[category]    Script Date: 8/5/2021 9:48:31 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[company]    Script Date: 8/5/2021 9:48:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cv]    Script Date: 8/5/2021 9:48:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NOT NULL,
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
	[is_public] [bit] NULL,
	[cv_name] [ntext] NOT NULL,
 CONSTRAINT [PK_cv] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[job]    Script Date: 8/5/2021 9:48:32 PM ******/
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
	[string_for_suggestion] [varchar](50) NULL,
 CONSTRAINT [PK_job] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[job_has_category]    Script Date: 8/5/2021 9:48:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[manager]    Script Date: 8/5/2021 9:48:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[recruiter]    Script Date: 8/5/2021 9:48:32 PM ******/
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
	[sex] [bit] NOT NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[last_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_recruiter] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student]    Script Date: 8/5/2021 9:48:32 PM ******/
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
	[last_applied_job_string] [varchar](50) NULL,
 CONSTRAINT [PK_student] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student_apply_job]    Script Date: 8/5/2021 9:48:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student_save_job]    Script Date: 8/5/2021 9:48:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[student_save_job](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [int] NOT NULL,
	[student_id] [int] NOT NULL,
	[createDate] [date] NOT NULL,
 CONSTRAINT [PK_student_save_job] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
SET IDENTITY_INSERT [dbo].[cv] ON 

INSERT [dbo].[cv] ([id], [student_id], [name], [sex], [dob], [avatar], [school], [experience], [foreign_language], [desired_salary_minimum], [working_form], [create_date], [is_subscribed], [is_public], [cv_name]) VALUES (5, 5, N'name', 1, CAST(N'2021-08-04' AS Date), N'avatar', N'school', N'exp', N'langu', 1000000, 1, CAST(N'2021-08-04' AS Date), 1, 0, N'CV lập trình')
INSERT [dbo].[cv] ([id], [student_id], [name], [sex], [dob], [avatar], [school], [experience], [foreign_language], [desired_salary_minimum], [working_form], [create_date], [is_subscribed], [is_public], [cv_name]) VALUES (6, 1, N'duy', 1, CAST(N'2021-08-04' AS Date), NULL, N'fpt', N'tester', N'english', 100000, 1, CAST(N'2021-08-04' AS Date), 0, 0, N'CV partime job')
SET IDENTITY_INSERT [dbo].[cv] OFF
GO
SET IDENTITY_INSERT [dbo].[job] ON 

INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4010, N'Lái Xe Nâng - Đi Làm Ngay', 1, 1, N'194 Lạc Long Quân', N'Vận hành các loại XE NÂNG ĐIỆN để thực hiện các công việc được phân công của thủ kho, giám sát kho trong việc bốc dỡ, sắp xếp, di chuyển hàng hóa, vệ sinh kho bãi và các việc liên quan khác', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4011, N'Nhân Viên Giao Nhận- Đi Làm Ngay', 1, 1, N'194 Lạc Long Quân', N'Chăm sóc cửa hàng, đại lý... có sẵn.', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4012, N'Nhân Viên Lái Xe Công Vụ', 1, 1, N'194 Lạc Long Quân', N'Lái xe cho Ban lãnh đạo Công ty và Cán bộ văn phòng đi liên hệ làm việc bên ngoài', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4013, N'Nhân Viên Thẩm Định Tín Dụng', 1, 1, N'194 Lạc Long Quân', N'Tiếp nhận hồ sơ khách hàng cần xác minh từ hệ thống và tiến hành đặt hẹn với các khách hàng và sắp xếp tuyến đường hợp lý để đến thẩm định tại nhà/công ty của khách hàng.', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4014, N'Nhân Viên Bảo Vệ', 1, 1, N'194 Lạc Long Quân', N'Trực tiếp thực hiện công tác bảo vệ an ninh và đảm bảo an toàn về người, cơ sở vật chất, tài sản, tiền vốn, an toàn phòng chống cháy nổ tại Trụ sở chính.', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4015, N'Nhân Viên Kỹ Thuật Giao Lắp', 1, 1, N'194 Lạc Long Quân', N'Thực hiện công việc vận chuyển hàng hóa từ kho của công ty tới nhà khách hàng.', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4016, N'Công Nhân Lao Động Phổ Thông', 1, 1, N'194 Lạc Long Quân', N'Phụ cơ khí , đánh mài các sản phẩm cơ khí', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4017, N'Nhân Viên Thu Ngân - Bán Hàng Lotte Mart', 1, 1, N'194 Lạc Long Quân', N'Tính tiền cho khách hàng tham gia mua sắm tại Trung siêu thị', N'Tốt nghiệp THPT/ Cấp 3 trở lên', 1, N'Cơ hội thăng tiến lên các vị trí', 2, 120, 7000000, 14000000, 1, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-4;5-1')
INSERT [dbo].[job] ([id], [name], [working_form], [location], [working_place], [description], [requirement], [type], [offer], [sex], [quantity], [salary_min], [salary_max], [recruiter_id], [create_date], [manager_id], [status], [string_for_suggestion]) VALUES (4018, N'Bán Hàng Online', 2, 2, N'123 Nguyễn Văn A', N'Bán hàng trên mạng xã hội facebook', N'Có khả năng bán hàng online', 1, N'Được hưởng lương tháng 13', 2, 5, 30000000, 50000000, 2, CAST(N'2021-08-05' AS Date), 1006, 2, N'1-2-2')
SET IDENTITY_INSERT [dbo].[job] OFF
GO
SET IDENTITY_INSERT [dbo].[job_has_category] ON 

INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1020, 4010, 4, CAST(N'2021-08-05T04:07:47.407' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1021, 4010, 5, CAST(N'2021-08-05T04:07:47.407' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1022, 4011, 4, CAST(N'2021-08-05T04:09:03.700' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1023, 4011, 5, CAST(N'2021-08-05T04:09:03.700' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1024, 4012, 4, CAST(N'2021-08-05T05:11:43.603' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1025, 4012, 5, CAST(N'2021-08-05T05:11:43.620' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1026, 4013, 4, CAST(N'2021-08-05T05:12:08.783' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1027, 4013, 5, CAST(N'2021-08-05T05:12:08.783' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1028, 4014, 4, CAST(N'2021-08-05T05:12:35.877' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1029, 4014, 5, CAST(N'2021-08-05T05:12:35.877' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1030, 4015, 4, CAST(N'2021-08-05T05:13:29.387' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1031, 4015, 5, CAST(N'2021-08-05T05:13:29.387' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1032, 4016, 4, CAST(N'2021-08-05T05:13:51.527' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1033, 4016, 5, CAST(N'2021-08-05T05:13:51.527' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1034, 4017, 4, CAST(N'2021-08-05T05:14:18.517' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1035, 4017, 5, CAST(N'2021-08-05T05:14:18.517' AS DateTime))
INSERT [dbo].[job_has_category] ([id], [job_id], [category_id], [create_date]) VALUES (1036, 4018, 2, CAST(N'2021-08-05T06:44:56.973' AS DateTime))
SET IDENTITY_INSERT [dbo].[job_has_category] OFF
GO
SET IDENTITY_INSERT [dbo].[manager] ON 

INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (1006, N'admin1', N'AIuVNn34ZcEFHXS5PyWU0IZ02d1o4s6ib7siEN/+4gmc7g4BmR/DVLyaTAXoVWBiRw==', N'admin1', CAST(N'2021-07-06' AS Date), N'ROLE_ADMIN', NULL)
INSERT [dbo].[manager] ([id], [username], [password], [full_name], [create_date], [role], [is_banned]) VALUES (1008, N'staff1', N'ACcxCAc9jnwjzOXtZ+hIvc8em8G9Zt+AWsJHmuT/YrbxiQneIkhrKiZ6ZACZZjJKYw==', N'staff1', CAST(N'2021-07-06' AS Date), N'ROLE_STAFF', 0)
SET IDENTITY_INSERT [dbo].[manager] OFF
GO
SET IDENTITY_INSERT [dbo].[recruiter] ON 

INSERT [dbo].[recruiter] ([id], [username], [password], [gmail], [phone], [avatar], [create_date], [role], [is_banned], [sex], [first_name], [last_name]) VALUES (1, N'recruiter1', N'ACnlwHmP67OLkSyRjhJWCThnhwDmFPqYsT4YSKVDEjK7Peo0oPBVuiBDnMoFbpYlsQ==', N'gmail', N'09123', N'asd', CAST(N'2021-07-24' AS Date), N'ROLE_RECRUITER', 0, 1, N'trần', N'duy')
INSERT [dbo].[recruiter] ([id], [username], [password], [gmail], [phone], [avatar], [create_date], [role], [is_banned], [sex], [first_name], [last_name]) VALUES (2, N'recruiter_test', N'AIdazSZPg7sykkmBeXhRDPd3/OFAQMAV4RaIVpgly9PMir5cEgOZVIJQjN7ShGyF6Q==', N'recruiter_test@gmail.com', N'01478523', N'arecruitertest', CAST(N'2021-07-24' AS Date), N'ROLE_RECRUITER', NULL, 1, N'nguyễn', N'đinh')
INSERT [dbo].[recruiter] ([id], [username], [password], [gmail], [phone], [avatar], [create_date], [role], [is_banned], [sex], [first_name], [last_name]) VALUES (3, N'recruiter3', N'AHKDu+CCIeesFRgVf/OLVHOGByCLly/nVrY13r/Y4T10qMN8pYf0Q1VDWhy88chOIA==', N'recruiter3@gmail.com', N'0147852233', NULL, CAST(N'2021-08-05' AS Date), N'ROLE_RECRUITER', NULL, 1, N'tran', N'duy')
SET IDENTITY_INSERT [dbo].[recruiter] OFF
GO
SET IDENTITY_INSERT [dbo].[student] ON 

INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id], [last_applied_job_string]) VALUES (1, N'cnviety2@gmail.com', NULL, 0, CAST(N'2021-07-23' AS Date), 1, N'https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/d0a1fa80-5bbb-4f33-9494-929eb30934b7113397064656708997803.jpeg', N'113397064656708997803', N'1-4;5-1')
INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id], [last_applied_job_string]) VALUES (5, N'duytdse63047@fpt.edu.vn', NULL, 0, CAST(N'2021-07-23' AS Date), 1, N'https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/77e6be25-f434-4321-800c-ef2f86801ef2108570453383030648023.jpeg', N'108570453383030648023', N'1-4;5-1')
INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id], [last_applied_job_string]) VALUES (2018, N'thangpv250997@gmail.com', NULL, 0, CAST(N'2021-08-05' AS Date), 0, N'https://lh3.googleusercontent.com/a-/AOh14Gg1xzMyKmIbP-cAuGIr_LTOJqXuc8o7-Jo33FGz=s96-c', N'115467286264441821229', NULL)
INSERT [dbo].[student] ([id], [gmail], [phone], [is_banned], [create_date], [profile_status], [avatar], [google_id], [last_applied_job_string]) VALUES (2019, N'thangpvse63541@fpt.edu.vn', NULL, 0, CAST(N'2021-08-05' AS Date), 0, N'https://lh3.googleusercontent.com/a-/AOh14GjcBDAOIhPgWlWsO1TyE65dqulO2FwLkqY1kn_SJg=s96-c', N'103962831546942956871', NULL)
SET IDENTITY_INSERT [dbo].[student] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_recruiter]    Script Date: 8/5/2021 9:48:35 PM ******/
ALTER TABLE [dbo].[recruiter] ADD  CONSTRAINT [IX_recruiter] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_recruiter_1]    Script Date: 8/5/2021 9:48:35 PM ******/
ALTER TABLE [dbo].[recruiter] ADD  CONSTRAINT [IX_recruiter_1] UNIQUE NONCLUSTERED 
(
	[gmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_student]    Script Date: 8/5/2021 9:48:35 PM ******/
ALTER TABLE [dbo].[student] ADD  CONSTRAINT [IX_student] UNIQUE NONCLUSTERED 
(
	[gmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ALTER TABLE [dbo].[student_save_job]  WITH CHECK ADD  CONSTRAINT [FK_student_save_job_job] FOREIGN KEY([job_id])
REFERENCES [dbo].[job] ([id])
GO
ALTER TABLE [dbo].[student_save_job] CHECK CONSTRAINT [FK_student_save_job_job]
GO
ALTER TABLE [dbo].[student_save_job]  WITH CHECK ADD  CONSTRAINT [FK_student_save_job_student] FOREIGN KEY([student_id])
REFERENCES [dbo].[student] ([id])
GO
ALTER TABLE [dbo].[student_save_job] CHECK CONSTRAINT [FK_student_save_job_student]
GO
