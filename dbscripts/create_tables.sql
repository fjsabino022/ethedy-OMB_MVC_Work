use OMB

create table Personas
(
  ID                      uniqueidentifier        not null    default(newid()) primary key,
  Nombre                  varchar(100)            not null,
  Apellido                varchar(50)             not null,
  Direccion               varchar(250),
  Localidad               varchar(100),
  Provincia               varchar(50),
  CodigoPostal            varchar(10),
  CorreoElectronico       varchar(50),
  Telefono                varchar(50),
  FechaNacimiento         date                    not null
)

create table Perfiles
(
  ID_Perfil             int             identity not null primary key,
  Nombre                varchar(30)     not null,
  Descripcion           varchar(200)    not null
)

create table Usuarios
(
  Login                       varchar(20)           not null      primary key,
  Password                    varchar(50)           not null	    default('alskdjfhg'),
  FechaExpiracionPassword     date,
  FechaLastLogin              smalldatetime,
  MustChangePass              bit,
  EnforceExpiration           bit,
  EnforceStrong               bit,
  Enabled                     bit                   not null default(1),
  ID_Persona                  uniqueidentifier      not null,
  constraint FK_Usuarios_Personas foreign key (ID_Persona) references Personas(ID)
)

--  alter table Usuarios
--	add constraint Password_Default
--	default 'alskdjfhg' for Password

create table Usuarios_Perfiles
(
  Login         varchar(20)       not null,
  ID_Perfil     int               not null,
  constraint FK_Ususarios_Perfiles_Usuarios foreign key (Login) references Usuarios(Login),
  constraint FK_Ususarios_Perfiles_Perfiles foreign key (ID_Perfil) references Perfiles(ID_Perfil)
)
