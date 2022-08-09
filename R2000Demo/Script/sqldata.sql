Select @@version
GO
Select @@servername
GO
-- [Using OS Commands]
!!MD C:\DataToProcess
!!DIR C:\DataToProcess

!!if 4 == 4 DIR C:\DaraToProcess
!!if not 4 == 5 DIR c:\DataToProcess
!!if exist C:\DataToProcess DIR c:\DataToProcess
!!if not exist C:\DataToProcessZZZ DIR C:\DataToProcess
!!if not exist C:\DataToProcessZZZ DIR C:\DataToProcess

:!!DIR C:\DataToProcess -- WORKS! The : is optional here
!!out C:\DataToProcess\ProductList.txt --<<< Error
Select ProdcutName, UnitPrice From Northwind.dbo.Products;

