
/*
  EngineEdition --> 
    3 => Enterprise
    4 => Express
    5 => Azure

  ProductLevel 
    RTM => original release version 
    CTP => community technology preview
    SP => service pack

  Version Producto (caracteres numericos hasta el primer punto)
    12  => 2014
    11  => 2012
    10  => 2008
     9  => 2005
     8  => 2000
     7  => SQL Server 7
     6  => SQL Server 6.5

     11.0.9230.282  => Azure 

  https://support.microsoft.com/en-us/kb/321185

*/
--
--  PASO 1: chequear datos del servidor
--
select 
	SERVERPROPERTY('Edition') as Edicion
	, SERVERPROPERTY('EngineEdition') as Edicion_Motor
	, SERVERPROPERTY('ProductVersion') as Version_Producto
	, SERVERPROPERTY('ProductLevel') as Nivel_Producto


--
--  PASO 2: drop de la DB si existe
--
use master

drop database OMB

--
--  PASO 3: creamos la base de datos 
--
create database OMB 
on primary 
(
  name = OMB_data,                                    --  nombre interno del objeto FILE (SQL)
  filename = 'F:\DESARROLLO\OMB_MVC_Work\database\OMB.mdf',    --  nombre windows del archivo
  size = 50MB,
  maxsize = unlimited,
  filegrowth = 10%
)
log on                                                      --  log file, para guardar transacciones temporalmente
(
  name = OMB_log,
  filename = 'F:\DESARROLLO\OMB_MVC_Work\database\OMB_log.mdf',
  size = 5MB,
  maxsize = unlimited,
  filegrowth = 10%
)



