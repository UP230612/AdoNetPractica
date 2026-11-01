-- ============================================================
-- Script para crear la base de datos y la tabla de Productos
-- Solo hay que correr esto UNA vez antes de arrancar el proyecto
-- ============================================================

-- Primero creo la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProductosDB')
BEGIN
    CREATE DATABASE ProductosDB;
END
GO

-- Me cambio a usar esa base de datos
USE ProductosDB;
GO

-- Creo la tabla Productos si no existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Productos')
BEGIN
    CREATE TABLE Productos (
        -- IDENTITY(1,1) significa que el Id empieza en 1 y sube de uno en uno solo
        Id          INT           IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(255) NOT NULL,
        -- DECIMAL(10,2) significa: hasta 10 digitos en total, con 2 decimales
        Precio      DECIMAL(10,2) NOT NULL,
        Existencia  INT           NOT NULL
    );
END
GO

-- Agrego unos productos de ejemplo para poder probar el swagger desde el principio
INSERT INTO Productos (Nombre, Descripcion, Precio, Existencia) VALUES
('Laptop HP 15',        'Laptop para trabajo y estudio, 8GB RAM, 256GB SSD',  12500.00, 10),
('Mouse Logitech M170', 'Mouse inalambrico comodo para uso diario',               350.00, 25),
('Teclado Mecanico',    'Teclado con switches azules ideal para programar',        800.00, 15),
('Monitor Samsung 24',  'Monitor Full HD 24 pulgadas, perfecto para la escuela', 3200.00,  8);
GO

-- Verifico que se crearon bien los datos
SELECT * FROM Productos;
GO
