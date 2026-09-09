
USE master;
GO

-- 1. Crear Base de Datos usando la configuración por defecto del motor
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'AuthDB')
BEGIN
    CREATE DATABASE AuthDB;
END
GO

-- 2. Crear Login a nivel de Servidor (si existen permisos de sysadmin/securityadmin)
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'AuthUser')
BEGIN
    CREATE LOGIN AuthUser WITH PASSWORD = 'Skrillex1.', CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF;
END
GO

USE AuthDB;
GO

-- 3. Crear Usuario de Base de Datos y asignar permisos de administración de esquema
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'AuthUser')
BEGIN
    CREATE USER AuthUser FOR LOGIN AuthUser;
    ALTER ROLE db_owner ADD MEMBER AuthUser;
END
GO

-- 4. Tabla de Usuarios
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id INT IDENTITY(1,1) NOT NULL,
        Username NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100) NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        Rol NVARCHAR(20) NOT NULL CONSTRAINT DF_Usuarios_Rol DEFAULT 'User',
        Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT 1,
        FechaCreacion DATETIME NOT NULL CONSTRAINT DF_Usuarios_FechaCreacion DEFAULT GETDATE(),
        CONSTRAINT PK_Usuarios PRIMARY KEY CLUSTERED (Id ASC),
        CONSTRAINT UQ_Usuarios_Username UNIQUE (Username)
    );
END
GO

-- 5. Tabla de RefreshTokens (Gestión de JWT)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RefreshTokens')
BEGIN
    CREATE TABLE RefreshTokens (
        Id INT IDENTITY(1,1) NOT NULL,
        UsuarioId INT NOT NULL,
        Token NVARCHAR(500) NOT NULL,
        FechaExpiracion DATETIME NOT NULL,
        Revocado BIT NOT NULL CONSTRAINT DF_RefreshTokens_Revocado DEFAULT 0,
        CONSTRAINT PK_RefreshTokens PRIMARY KEY CLUSTERED (Id ASC),
        CONSTRAINT FK_RefreshTokens_Usuarios FOREIGN KEY (UsuarioId) 
            REFERENCES Usuarios(Id) ON DELETE CASCADE
    );
END
GO

-- 6. Insertar registros iniciales si la tabla está vacía
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'admin')
BEGIN
    INSERT INTO Usuarios (Username, Email, PasswordHash, Rol, Activo)
    VALUES ('admin', 'admin@ejemplo.com', 'Password123!', 'Admin', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'usuario')
BEGIN
    INSERT INTO Usuarios (Username, Email, PasswordHash, Rol, Activo)
    VALUES ('usuario', 'user@ejemplo.com', 'Password123!', 'User', 1);
END
GO