SELECT TOP (1000) Id, Nombre, Codigo, Rol, Ubicacion, RowState, LastUpdate
FROM     Antena
SELECT Id, Codigo, Nombre, LocalId, LastUpdate, RowState
FROM     Modulo
SELECT TOP (1000) Id, Nombre, Dirección, EmpresaId, LastUpdate, RowState
FROM     Local
SELECT TOP (1000) Id, Nombre, RUC, Direccion, LastUpdate, RowState
FROM     Empresa
SELECT TOP (1000) Id, TAG, EPC, TID, InvTimes, RSSI, AntID, LastTime, FirstReadTime, Color, ModuloId, ModuloRol
FROM     ReadTag
SELECT ReadTag.Id, ReadTag.TAG, ReadTag.EPC, ReadTag.TID, ReadTag.InvTimes, ReadTag.RSSI, ReadTag.AntID, ReadTag.LastTime, ReadTag.FirstReadTime, ReadTag.Color, ReadTag.ModuloId, ReadTag.ModuloRol, Antena.Id AS Expr1, 
                  Antena.Nombre, Antena.Codigo, Antena.Rol, Antena.Ubicacion, Antena.RowState, Antena.LastUpdate, Modulo.Id AS Expr2, Modulo.Codigo AS Expr3, Modulo.Nombre AS Expr4, Modulo.LocalId, Modulo.LastUpdate AS Expr5, 
                  Modulo.RowState AS Expr6, Local.Id AS Expr7, Local.Nombre AS Expr8, Local.Dirección, Local.EmpresaId, Local.LastUpdate AS Expr9, Local.RowState AS Expr10, Empresa.Id AS Expr11, Empresa.Nombre AS Expr12, Empresa.RUC, 
                  Empresa.Direccion, Empresa.LastUpdate AS Expr13, Empresa.RowState AS Expr14
FROM     ReadTag INNER JOIN
                  Antena ON ReadTag.AntID = Antena.Id INNER JOIN
                  Modulo ON ReadTag.ModuloId = Modulo.Codigo INNER JOIN
                  Local ON Modulo.LocalId = Local.Id INNER JOIN
                  Empresa ON Local.EmpresaId = Empresa.Id	