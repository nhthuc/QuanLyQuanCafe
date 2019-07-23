CREATE DATABASE QuanLyQuanCafe 
GO 

USE QuanLyQuanCafe 
GO

CREATE TABLE TableFood 
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	status NVARCHAR(100) DEFAULT N'Trống'-- 0 : bàn trống ||  1 có người 
)
GO

CREATE TABLE Account 
(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'NHT',
	Password NVARCHAR(100) NOT NULL DEFAULT N'0',
	Type INT NOT NULL DEFAULT 0
)
go

 
CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE Food 
(	
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	Status INT NOT NULL -- đã thanh toán hay chưa 1 - thanh toán - 0 là k thanh toán
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO

INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          Password ,
          Type
        )
VALUES  ( N'NHT' , -- UserName - nvarchar(100)
          N'NHTk2' , -- DisplayName - nvarchar(100)
          N'nht' , -- Password - nvarchar(100)
          0  -- Type - int
        )

		INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          Password ,
          Type
        )
VALUES  ( N'admin' , -- UserName - nvarchar(100)
          N'admin' , -- DisplayName - nvarchar(100)
          N'admin' , -- Password - nvarchar(100)
          1  -- Type - int
        )
GO

CREATE PROC USP_GetAccountByUserName
@userName NVARCHAR(100)
AS 
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

--EXEC dbo.USP_GetAccountByUserName @userName = N'admin' -- nvarchar(100) thực thi store proceduce

--SELECT * FROM dbo.Account WHERE UserName = N'admin' AND Password = N'' -- "OR 1 =1 --" lỗi SQL
--GO

CREATE PROC USP_Login 
@userName NVARCHAR(100), @passWord NVARCHAR(100)
AS 
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND Password = @Password
END
GO
-- Thêm bàn ăn
DECLARE @i INT = 11
WHILE @i <= 20
BEGIN
	INSERT INTO dbo.TableFood
	        ( name)
	VALUES  ( N'Bàn ' + CAST(@i AS VARCHAR(100)))
	SET @i = @i + 1
END
GO

-- DBCC CHECKIDENT(TableFood,RESEED,0) -- reset id lại 0

CREATE PROC USP_TableLoad
AS 
SELECT * FROM dbo.TableFood
GO

-- thêm danh mục các món ăn

INSERT INTO dbo.FoodCategory
        ( name )
VALUES  ( N'Hải Sản'
          )
INSERT INTO dbo.FoodCategory
        ( name )
VALUES  ( N'Lẫu'  -- name - nvarchar(100)
          )
INSERT INTO dbo.FoodCategory
        ( name )
VALUES  ( N'Món Cơm'  -- name - nvarchar(100)
          )
INSERT INTO dbo.FoodCategory
        ( name )
VALUES  ( N'Món khai vị'  -- name - nvarchar(100)
          )
INSERT INTO dbo.FoodCategory
        ( name )
VALUES  ( N'Món Nướng'  -- name - nvarchar(100)
          )
INSERT INTO dbo.FoodCategory
        ( name )
VALUES  ( N'Thức uống'  -- name - nvarchar(100)
          )
-- Thêm món ăn 
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Nghêu hấp xã', 1, 45000) 
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Sò huyết rang me', 1, 55000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Tôm nướng muối ớt', 1, 85000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Bạch tuột táp lửa', 1, 75000)

INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Lẫu cua đồng', 2, 255000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Lẫu gà lá giang', 2, 300000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Lẫu thập cẩm', 2, 400000)

INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Cơm thập cẩm', 3, 20000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Cơm gà', 3, 25000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Cơm chiên dương châu', 3, 30000)

INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Bacon Cuộn Cá', 4, 55000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Chả giò Mayonnaise', 4, 35000)

INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Cá nướng muối ớt', 5, 45000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Mực nang nướng sa tế', 5, 55000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Vú heo nướng', 5, 45000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Sườn nướng tiêu', 5, 60000)

INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Trà sữa đậu xanh', 6, 25000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'Cafe ý', 6, 30000)
INSERT INTO dbo.Food( name, idCategory, price )
VALUES  ( N'cookie mocha', 6, 28000)

-- thêm thông tin bill

INSERT INTO dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 1, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT INTO dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 1, -- idBill - int
          3, -- idFood - int
          4  -- count - int
          )
INSERT INTO dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 2, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT INTO dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 2, -- idBill - int
          5, -- idFood - int
          4  -- count - int
          )
INSERT INTO dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 3, -- idBill - int
          6, -- idFood - int
          3  -- count - int
          )
GO

CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
INSERT INTO dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          Status,
		  discount
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          @idTable , -- idTable - int
          0,  -- Status - int
		  0
        )
END
GO


CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
INSERT INTO dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
END
GO

CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	DECLARE @isExitBillInfo INT		
	DECLARE @foodCount INT = 1

	SELECT @isExitBillInfo = id, @foodCount = count FROM dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood

    IF (@isExitBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo SET count = @foodCount + @count WHERE idFood = @idFood AND idBill = @idBill
		ELSE 
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE 
	BEGIN
		INSERT INTO dbo.BillInfo
		  ( idBill, idFood, count )
		VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
	END

END
GO

CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT 
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idTable INT 
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND Status = 0

	UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable
END
GO

CREATE TRIGGER UTG_InsertBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = id FROM Inserted

	DECLARE @idTable INT 
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND Status = 0

	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
END
GO

ALTER TABLE dbo.Bill
ADD discount INT
UPDATE dbo.Bill SET discount = 0

ALTER TABLE dbo.Bill ADD totalPrice FLOAT
GO

CREATE PROC USP_GetListDateByDate
@checkIn DATE, @checkOut DATE
AS
BEGIN
	SELECT b.name AS [Tên bàn], a.totalPrice AS [Tổng tiền], a.DateCheckIn AS [Ngày vào], a.DateCheckOut AS [Ngày ra], a.discount AS [Giảm giá] 
	FROM dbo.Bill AS a, dbo.TableFood AS b
	WHERE a.DateCheckIn >= @checkIn AND a.DateCheckOut <= @checkOut AND a.Status = 1
	AND a.idTable = b.id 
END
GO

CREATE PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @passWord NVARCHAR(100), @newPassWord NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPassword INT = 0

	SELECT @isRightPassword = COUNT(*) FROM dbo.Account WHERE UserName = @userName AND Password = @passWord
	IF (@isRightPassword = 1)
	BEGIN
		IF (@newPassWord = NULL OR @newPassWord = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, Password = @newPassWord WHERE UserName = @userName
	END
    
END
GO

CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS
BEGIN
	DECLARE @idBillInfo INT 
	DECLARE @idBill INT 
	SELECT @idBillInfo = id, @idBill = Deleted.idBill FROM Deleted

	DECLARE @idTable INT 
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.Bill AS b, dbo.BillInfo AS bi WHERE b.id = bi.idBill AND b.id = @idBill AND b.Status = 0

	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable

END
GO 

--hàm chuyển ký tự có dấu thành không dấu
CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO 


SELECT a.id AS [ID], a.name AS [Tên món ăn],b.name AS [Danh mục],b.id AS [id Danh mục], a.price AS [Giá] FROM dbo.Food AS a, dbo.FoodCategory AS b 
WHERE a.idCategory = b.id AND dbo.fuConvertToUnsign1(a.name) LIKE N'%' + dbo.fuConvertToUnsign1(N'muc') +'%'

SELECT UserName,DisplayName,Type FROM dbo.Account 


DELETE dbo.BillInfo
DELETE dbo.Bill


SELECT * FROM dbo.Account
SELECT * FROM dbo.FoodCategory
SELECT * FROM dbo.Food
SELECT * FROM dbo.Bill
SELECT * FROM dbo.BillInfo
SELECT * FROM dbo.TableFood