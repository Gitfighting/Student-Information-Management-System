-- ====================================
-- 管理员登录问题排查 SQL 脚本
-- ====================================

-- 1. 检查数据库是否存在
USE master;
IF DB_ID('StudentManageDB2') IS NOT NULL
    PRINT '✅ 数据库 StudentManageDB2 存在'
ELSE
    PRINT '❌ 数据库不存在，请检查 Web.config 连接字符串'

GO

-- 2. 切换到目标数据库
USE StudentManageDB2;
GO

-- 3. 检查表是否存在
IF OBJECT_ID('admin_info', 'U') IS NOT NULL
    PRINT '✅ 表 admin_info 存在'
ELSE
BEGIN
    PRINT '❌ 表不存在，EF Code First 可能未初始化'
    PRINT '解决方案：运行一次 Web API 项目让 EF 自动建表'
END

GO

-- 4. 查看所有管理员数据
SELECT 
    admin_id AS [ID],
    admin_name AS [用户名],
    admin_pwd AS [密码],
    admin_nickname AS [昵称],
    LEN(admin_pwd) AS [密码长度]
FROM admin_info;

-- 5. 检查 'admin' 用户是否存在
IF EXISTS(SELECT 1 FROM admin_info WHERE admin_name = 'admin')
BEGIN
    PRINT '✅ 用户 admin 存在'
    
    DECLARE @pwd NVARCHAR(50)
    SELECT @pwd = admin_pwd FROM admin_info WHERE admin_name = 'admin'
    
    IF @pwd = '123456'
        PRINT '✅ 密码为明文 123456'
    ELSE
        PRINT '⚠️ 密码不是 123456，当前值：' + @pwd
END
ELSE
BEGIN
    PRINT '❌ 用户 admin 不存在，请插入初始数据'
    PRINT '执行以下命令创建：'
    PRINT 'INSERT INTO admin_info(admin_name, admin_pwd, admin_nickname) VALUES(''admin'',''123456'',''超级管理员'')'
END

GO

-- 6. 如果表存在但无数据，插入默认管理员
IF OBJECT_ID('admin_info', 'U') IS NOT NULL AND NOT EXISTS(SELECT 1 FROM admin_info WHERE admin_name = 'admin')
BEGIN
    INSERT INTO admin_info(admin_name, admin_pwd, admin_nickname) 
    VALUES('admin','123456','超级管理员');
    PRINT '✅ 已自动插入默认管理员：admin/123456'
END
