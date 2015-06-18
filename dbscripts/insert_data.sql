

insert into Personas (ID, Nombre, Apellido, CorreoElectronico, FechaNacimiento) 
  values (default, 'Homer', 'Simpson', 'hsimpson@pp.com', convert(date, '20/05/1968', 103))

insert into Personas (ID, Nombre, Apellido, CorreoElectronico, FechaNacimiento) 
  values (default, 'Abraham', 'Simpson', 'asimpson@pp.com', convert(date, '01/06/1923', 103))

insert into Personas (ID, Nombre, Apellido, CorreoElectronico, FechaNacimiento) 
  values (default, 'Montgomery', 'Burns', 'mburns@centralnuclear.com', convert(date, '13/04/1875', 103))

insert into Personas (ID, Nombre, Apellido, CorreoElectronico, FechaNacimiento) 
  values (default, 'Marge', 'Bouvier', 'mbouvier@pp.com', convert(date, '01/03/1971', 103))


insert into Perfiles values ('SysAdmin', 'Administracion total del Sistema')
insert into Perfiles values ('StockAdmin', 'Maneja el ingreso/egreso de productos al deposito')
insert into Perfiles values ('PDV', 'Atencion al cliente, facturacion, caja')
insert into Perfiles values ('AsistenciaCliente', 'Acceso a terminales de ayuda al cliente, busqueda de productos y precios')


insert into Usuarios (Login, Password, ID_Persona)
  values ('hsimpson', '123456', 'B9493449-D006-4270-B8EA-9CFE34DF7123')

insert into Usuarios (Login, Password, ID_Persona)
  values ('mburns', 'monty', '3008DEAE-F9F6-4BDB-88CB-904DE6A5348D')

insert into Usuarios (Login, Password, ID_Persona)
  values ('mbouvier', 'ringo', '3D1094EB-ED22-493A-87CD-7A78193F2B33')

insert into Usuarios_Perfiles values ('hsimpson', 4)
insert into Usuarios_Perfiles values ('mburns', 1)
insert into Usuarios_Perfiles values ('mbouvier', 3)
insert into Usuarios_Perfiles values ('mbouvier', 4)


select * from Personas
select * from Perfiles
select * from Usuarios
select * from Usuarios_Perfiles


