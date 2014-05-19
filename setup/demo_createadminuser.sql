-- password: zSNfdPTj
-- hashed password: VUyChSNrWlgN98gw7HK4EnUslHdfMQCMSm6Hn1ZI8EfbQjor+/7QohelQDlRx0ocn/H2AzPfi7o00mRNUONJRg==
-- salt: 100000.f7374XrdznZdrprFRQzJ+dMRVRal6MgNcvFvst9yCLqLtg==
-- generated via ignored unit test

-- intentionally no values set for cultures column
INSERT INTO [dbo].[Users]
           ([UserName]
           ,[PasswordHash]
           ,[PasswordSalt]
           ,[EmailAddress]
           ,[FirstName]
           ,[LastName]
           ,[IsActive]
           ,[IsAdmin])
     VALUES
           ('admin',
           'VUyChSNrWlgN98gw7HK4EnUslHdfMQCMSm6Hn1ZI8EfbQjor+/7QohelQDlRx0ocn/H2AzPfi7o00mRNUONJRg==',
           '100000.f7374XrdznZdrprFRQzJ+dMRVRal6MgNcvFvst9yCLqLtg==',
           'admin@yourdomain.com',
           'John',
           'Doe',
           1,
           1)
GO
