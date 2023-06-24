
IF NOT EXISTS (SELECT * FROM AspNetRoles) BEGIN
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (NEWID(), 'Admin', 'ADMIN');
end

IF NOT EXISTS (SELECT * FROM AspNetUsers) BEGIN
INSERT INTO [AspNetUsers] ([Id], [UserName], [Email], [NormalizedEmail], [NormalizedUserName], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [AccessFailedCount], [Discriminator], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [IsDeleted])
VALUES ( NEWID(), 'admin', 'admin@gmail.com', 'ADMIN@GMAIL.COM', 'ADMIN', 0, 'AQAAAAIAAYagAAAAEBtWmWPRWhAePW7/CyuQ6NPRF+FCCe73X5PNx7jQeeDEaKnGNBYBnkik3DTP86QgQw==', 'UQJUD4BXGAF2JYYBFGLHXSTJ23Y4L5R3', '79484be8-0696-41ed-abcb-c2afb8010b58', 0, 'User', 0, 0, 1, 0);
end

IF NOT EXISTS (SELECT * FROM AspNetUserRoles) BEGIN
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
SELECT [Id], (SELECT [Id] FROM [AspNetRoles] WHERE [Name] = 'Admin')
FROM [AspNetUsers] WHERE [NormalizedUserName] = 'ADMIN';
end