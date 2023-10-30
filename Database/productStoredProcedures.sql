DELIMITER // 
CREATE PROCEDURE usp_ProductCreate (out prodId int, in productcode_p char(10), in description_p varchar(50), in unitprice_p decimal(10), in onhandquantity_p int)
BEGIN
	Insert into products (productcode, description, unitprice, onhandquantity, concurrencyid)
    Values (productcode_p, description_p, unitprice_p, onhandquantity_p, 1);
    Select LAST_INSERT_ID() into prodId;
    
END //
DELIMITER ; 

DELIMITER // 
CREATE PROCEDURE usp_ProductDelete (in prodID int, in conCurrId int)
BEGIN
	Delete from products where ProductID = prodID and ConcurrencyID = conCurrId;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_ProductSelect (in ProdID int)
BEGIN
	Select * from products where productID = ProdID;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_ProductSelectAll ()
BEGIN
	Select * from products order by productcode;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_ProductUpdate (in prodID int, in productCode char(10), in conCurrId int)
BEGIN
	Update products
    Set productcode = productCode, concurrencyid = (concurrencyid + 1)
    Where productID = prodID and concurrencyid = conCurrId;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_ProductUpdate (in prodID int, in productcode char(10), in description varchar(50), in unitprice decimal(10, 4), in onhandquantity int, in conCurrId int)
BEGIN
	Update products
    Set productcode = productcode, description = description, unitprice = unitprice, onhandquantity = onhandquantity, concurrencyid = (concurrencyid + 1)
    Where productID = prodID and concurrencyid = conCurrId;
END //
DELIMITER ;