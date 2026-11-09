--CREATE DATABASE SistemaVentasJoyeria;
--GO
--USE SistemaVentasJoyeria;
--GO

--PERSONAS---------------------------------------------------------------
CREATE TABLE Rol(
idRol				INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'ROL' + REPLACE(STR(idRol, 5), ' ', '0'),
nombreRol			NVARCHAR(50) NOT NULL UNIQUE,
descripcion			NVARCHAR(100),
privilegio			INT NOT NULL,			
estado				VARCHAR(8) NOT NULL
);
GO

CREATE TABLE Persona(
idPersona			INT IDENTITY(1,1) PRIMARY KEY,
nombrePer			NVARCHAR(50) NOT NULL,
apellidoPaterno		NVARCHAR(50) NOT NULL,
apellidoMaterno		NVARCHAR(50) NOT NULL,
dni					CHAR(8) NOT NULL,
fechaNacimiento		DATE NOT NULL,
usuario				NVARCHAR(50),
contrasena			NVARCHAR(50),
idRol				INT NOT NULL
);
GO

ALTER TABLE Persona
	ADD CONSTRAINT fkPerRol FOREIGN KEY (idRol) REFERENCES Rol(idRol)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

CREATE TABLE Cliente(
idCliente			INT IDENTITY(1,1) PRIMARY KEY,
idPersona			INT NOT NULL,
codigo				AS 'CLI' + REPLACE(STR(idCliente, 5), ' ', '0'),
telefonoMovil		CHAR(9) NOT NULL,
telefonoFijo		CHAR(7),
direccion			NVARCHAR(100) NOT NULL,
correo				NVARCHAR(100),
estado				VARCHAR(8) NOT NULL
);
GO

ALTER TABLE Cliente
	ADD CONSTRAINT fkCliPer FOREIGN KEY (idPersona) REFERENCES Persona(idPersona)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

CREATE TABLE Empleado(
idEmpleado			INT IDENTITY(1,1) PRIMARY KEY,
idPersona			INT NOT NULL,
codigo				AS 'EMP' + REPLACE(STR(idEmpleado, 5), ' ', '0'),
telefonoMovil		CHAR(9) NOT NULL,
direccion			NVARCHAR(100) NOT NULL,
correo				NVARCHAR(100),
estado				VARCHAR(8) NOT NULL
);
GO

ALTER TABLE Empleado
	ADD CONSTRAINT fkEmpPer FOREIGN KEY (idPersona) REFERENCES Persona(idPersona)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

--PRODUCTO Y DERIVADOS------------------------------------------------------

CREATE TABLE Categoria(
idCategoria			INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'CAT' + REPLACE(STR(idCategoria, 5), ' ', '0'),
nombreCat			NVARCHAR(50) NOT NULL,
descripcion			NVARCHAR(100),
estado				VARCHAR(8) NOT NULL
);
GO

CREATE TABLE UnidadMedida(
idUnidadMedida		INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'UDM' + REPLACE(STR(idUnidadMedida, 5), ' ', '0'),
nombreUnid			NVARCHAR(50) NOT NULL,
estado				VARCHAR(8) NOT NULL
);
GO

CREATE TABLE Proveedor(
idProveedor			INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'PRV' + REPLACE(STR(idProveedor, 5), ' ', '0'),
nombrePvd			NVARCHAR(50) NOT NULL,
ruc					CHAR(11) NOT NULL,
correo				NVARCHAR(50) NOT NULL,
numeroContacto		CHAR(9) NOT NULL,
direccion			NVARCHAR(100) NOT NULL,
estado				VARCHAR(8) NOT NULL
);
GO

CREATE TABLE TipoPago(
idPago				INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'PAG' + REPLACE(STR(idPago, 5), ' ', '0'),
tipo				NVARCHAR(20) NOT NULL,
estado				VARCHAR(8) NOT NULL
);
GO

CREATE TABLE Producto(
idProducto			INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'PRO' + REPLACE(STR(idProducto, 5), ' ', '0'),
idCategoria			INT NOT NULL,
nombrePro			NVARCHAR(50) NOT NULL,
idUnidadMedida		INT NOT NULL,
medida				INT NOT NULL,
idProveedor			INT NOT NULL,
precioCompra		DECIMAL(10,2) NOT NULL,
precioVenta			DECIMAL(10,2) NOT NULL,
stock				INT NOT NULL,
descripcion			NVARCHAR(300),
estado				VARCHAR(8) NOT NULL
);
GO

ALTER TABLE Producto
	ADD CONSTRAINT fkProCat FOREIGN KEY (idCategoria) REFERENCES Categoria(idCategoria)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE Producto
	ADD CONSTRAINT fkProUni FOREIGN KEY (idUnidadMedida) REFERENCES UnidadMedida(idUnidadMedida)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE Producto
	ADD CONSTRAINT fkProPrv FOREIGN KEY (idProveedor) REFERENCES Proveedor(idProveedor)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

CREATE TABLE Venta(
idVenta				INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'VEN' + REPLACE(STR(idVenta, 5), ' ', '0'),
idCliente			INT NOT NULL,
idEmpleado			INT NOT NULL,
tipoComprobante		VARCHAR(50) NOT NULL,
serieComprobante	AS '0' + REPLACE(STR(idVenta, 2), ' ', '0') + ' N° ' + REPLACE(STR(idVenta, 7), ' ', '0'),
fechaVenta			DATE NOT NULL,
horaVenta			TIME(2) NOT NULL,
idPago				INT NOT NULL,
observaciones		NVARCHAR(100)
);
GO

ALTER TABLE Venta
	ADD CONSTRAINT fkVenCli FOREIGN KEY (idCliente) REFERENCES Cliente(idCliente)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE Venta
	ADD CONSTRAINT fkVenEmp FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE Venta
	ADD CONSTRAINT fkVenPag FOREIGN KEY (idPago) REFERENCES TipoPago(idPago)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

CREATE TABLE Compra(
idCompra			INT IDENTITY(1,1) PRIMARY KEY,
codigo				AS 'COM' + REPLACE(STR(idCompra, 5), ' ', '0'),
idProveedor			INT NOT NULL,
idEmpleado			INT NOT NULL,
tipoComprobante		VARCHAR(50) NOT NULL,
serieComprobante	AS '0' + REPLACE(STR(idCompra, 2), ' ', '0') + '-COM' + REPLACE(STR(idCompra, 7), ' ', '0'),
fechaCompra			DATE NOT NULL,
horaCompra			TIME(2) NOT NULL,
idPago				INT NOT NULL,
observaciones		NVARCHAR(100)
);
GO

ALTER TABLE Compra
	ADD CONSTRAINT fkComPrv FOREIGN KEY (idProveedor) REFERENCES Proveedor(idProveedor)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE Compra
	ADD CONSTRAINT fkComEmp FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE Compra
	ADD CONSTRAINT fkComPag FOREIGN KEY (idPago) REFERENCES TipoPago(idPago)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

CREATE TABLE DetalleVenta(
idVenta				INT NOT NULL,
idProducto			INT NOT NULL,
precioUnitario		DECIMAL(10,2) NOT NULL,
cantidad			INT	NOT NULL
);
GO

ALTER TABLE DetalleVenta
	ADD CONSTRAINT fkDVVen FOREIGN KEY (idVenta) REFERENCES Venta(idVenta)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE DetalleVenta
	ADD CONSTRAINT fkDVPro FOREIGN KEY (idProducto) REFERENCES Producto(idProducto)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

CREATE TABLE DetalleCompra(
idCompra			INT NOT NULL,
idProducto			INT NOT NULL,
precioCompra		DECIMAL(10,2) NOT NULL,
cantidad			INT	NOT NULL
);
GO

ALTER TABLE DetalleCompra
	ADD CONSTRAINT fkDCCom FOREIGN KEY (idCompra) REFERENCES Compra(idCompra)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO

ALTER TABLE DetalleCompra
	ADD CONSTRAINT fkDCPro FOREIGN KEY (idProducto) REFERENCES Producto(idProducto)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
GO


insert into Rol values('Administrador','Poder absoluto',1,'Activo');
insert into Rol values('Vendedor','Poder medio',2,'Activo');
insert into Rol values('Cliente','Poder básico',3,'Activo');
GO

insert into Persona (nombrePer, apellidoPaterno, apellidoMaterno, dni, fechaNacimiento, usuario, contrasena, idRol) values('Erick','Gutierrez','Molina','11202032','1998/07/12','admin','Idat123',1);
GO
insert into Empleado(idPersona, telefonoMovil, direccion, correo, estado) values(1,'994152859','Los molinos 1256 Ate','erick@gmail.com','Activo');
GO
insert into Persona (nombrePer, apellidoPaterno, apellidoMaterno, dni, fechaNacimiento, usuario, contrasena, idRol) values('Bryan','Huaman','Chavez','41528596','1998/05/24','bryan123','Idat123',1);
GO
insert into Empleado(idPersona, telefonoMovil, direccion, correo, estado) values(2,'945128745','Frutales 1452 Santa Anita','bryan@gmail.com','Activo');
GO
insert into Persona (nombrePer, apellidoPaterno, apellidoMaterno, dni, fechaNacimiento, usuario, contrasena, idRol) values('Sergio','Condori','Huachez','41859656','1998/11/18','sergio123','Idat123',1);
GO
insert into Empleado(idPersona, telefonoMovil, direccion, correo, estado) values(3,'947856321','El pontifice 1452 La victoria','sergio@gmail.com','Activo');
GO

insert into Persona (nombrePer, apellidoPaterno, apellidoMaterno, dni, fechaNacimiento, usuario, contrasena, idRol) values('Laura','Medina','Suarez','47851625','1992/10/12','','',3);
GO
insert into Cliente(idPersona, telefonoMovil, direccion, correo, estado) values(4,'999415285','Los mirlos 1256 Ate','laura@gmail.com','Activo');
GO
insert into Persona (nombrePer, apellidoPaterno, apellidoMaterno, dni, fechaNacimiento, usuario, contrasena, idRol) values('Ricardo','Marin','Torres','91874852','1991/04/24','','',3);
GO
insert into Cliente(idPersona, telefonoMovil, direccion, correo, estado) values(5,'994718523','Rosales 4152 Santa Anita','ricardo@gmail.com','Activo');
GO

insert into Proveedor values('Piarte','11452635263','piarte@outlook.es','994152632','Miraflores 1254 cd 10','Activo');
insert into Proveedor values('Icaro joyeros','14785415263','icarojoyeros@gmail.com','991452365','San Isidro 1236 cd 2','Activo');
GO

insert into TipoPago values('Contado en efectivo','Activo');
insert into TipoPago values('Contado con tarjeta','Activo');
GO

insert into UnidadMedida values('Gramo','Activo');
insert into UnidadMedida values('Quilate','Activo');
GO

insert into Categoria values('Anillos','','Activo');
insert into Categoria values('Collares','','Activo');
insert into Categoria values('Cadenas','','Activo');
GO

insert into Producto values(1,'Anillo de oro con degradado',2,18,1,900,1020.99,20,'Anillo con efecto de degradado','Activo');
insert into Producto values(2,'Collar oro amarillo',2,18,2,700,970.99,20,'Collar hecho de oro amarillo','Activo');
insert into Producto values(3,'Cadena fina con gravado',1,10,1,300,420.99,20,'Cadena fina para gravado','Activo');
GO

insert into Compra values(1,1,'Factura','2020/08/03','10:20:52',1,'');
insert into Compra values(1,2,'Factura','2020/08/03','12:40:21',1,'');
insert into Compra values(2,3,'Factura','2020/08/03','12:50:24',2,'');
GO

insert into DetalleCompra values(1,1,900,10);
insert into DetalleCompra values(2,2,700,10);
insert into DetalleCompra values(3,3,300,5);
GO

insert into Venta values(1,1,'Boleta','2020/08/04','09:20:25',1,'');
insert into Venta values(2,2,'Boleta','2020/08/04','10:34:26',1,'');
insert into Venta values(2,3,'Boleta','2020/08/04','12:15:18',1,'');
GO

insert into DetalleVenta values(1,1,1020.99,1);
insert into DetalleVenta values(2,2,970.99,1);
insert into DetalleVenta values(3,3,420.99,1);
GO

--select * from Persona;
--select * from Rol;
--select * from Empleado;
--select * from Cliente;
--select * from Producto;
--select * from Proveedor;
--select * from TipoPago;
--select * from UnidadMedida;
--select * from Categoria;
--select * from Compra;
--select * from DetalleCompra;
--select * from Venta;
--select * from DetalleVenta;
