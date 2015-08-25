use OMB
go

create procedure CambiarPasswordForLogin
  @login  varchar(20),
  @newPassword varchar(50)
as begin
  declare @rows int

  set nocount on

  begin try
    update USR
      set Password = @newPassword
    from Usuarios as USR
    where Login = @login

    set @rows = @@rowcount
  end try
  begin catch
    set @rows = -1 
    --  execute GetErrorInfo
  end catch

  select convert(int, @rows)
end
go

create procedure ValidarPasswordForLogin
  @login  varchar(20),
  @pass   varchar(50)
as begin
  if exists(select * from Usuarios where Login=@login and Password=@pass)
    select ValidarPass = 1
  else
    select ValidarPass = -1
end


--  execute CambiarPasswordForLogin 'mbouvier', 'ringo'

--  execute ValidarPasswordForLogin 'lsimpson', 'lisa-12'

/*

create procedure GetErrorInfo
as begin
  select
    NumError      = ERROR_NUMBER(),
    Severidad     = ERROR_SEVERITY(),
    Estado        = ERROR_STATE(),
    Procedimiento = ERROR_PROCEDURE(),
    Linea         = ERROR_LINE(),
    Mensaje       = ERROR_MESSAGE()
end
go;

alter procedure CambiarPasswordForLogin
  @login  varchar(20),
  @newPassword varchar(50)
as begin
  declare @rows int

  begin try
    update USR
      set Password = @newPassword
    from Usuarios as USR
    where Login = @login

    select FilasAfectadas = @@rowcount
  end try
  begin catch
    select FilasAfectadas = -1 
    execute GetErrorInfo
  end catch
end

*/

