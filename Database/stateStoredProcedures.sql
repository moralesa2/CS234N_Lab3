DELIMITER // 
CREATE PROCEDURE usp_StateDelete (in code varchar(2), in conCurrId int)
BEGIN
	Delete from states where statecode = code and ConcurrencyID = conCurrId;
END //
DELIMITER ; 

DELIMITER // 
CREATE PROCEDURE usp_StateCreate (in code varchar(2), in name varchar(20))
BEGIN
	Insert into states (statecode, statename) values (code, name);
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_StateSelect (in code varchar(2))
BEGIN
	Select * from states where statecode=code;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_StateSelectAll ()
BEGIN
	Select * from states order by statename;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_StateUpdate (in code varchar(2), in name varchar(20), in conCurrId int)
BEGIN
	Update states
    Set statename = name, concurrencyid = (concurrencyid + 1)
    Where statecode = code and concurrencyid = conCurrId;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_CustomerCreate (out custId int, in name_p varchar(100), in address_p varchar(50), in city_p varchar(20), in state_p varchar(2), in zipcode_p varchar(15))
BEGIN
	Insert into customers (name, address, city, state, zipcode, concurrencyid)
    Values (name_p, address_p, city_p, state_p, zipcode_p, 1);
    Select LAST_INSERT_ID() into custId;
    
END //
DELIMITER ; 

DELIMITER // 
CREATE PROCEDURE usp_CustomerDelete (in custID int, in conCurrId int)
BEGIN
	Delete from customers where customerID = custID and ConcurrencyID = conCurrId;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_CustomerSelect (in CustID int)
BEGIN
	Select * from customers where customerID = CustID;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_CustomerSelectAll ()
BEGIN
	Select * from customers order by name;
END //
DELIMITER ;

DELIMITER // 
CREATE PROCEDURE usp_CustomerUpdate (in custID int, in name varchar(20), in address varchar(50), in city varchar(20), in state char(2), in zipcode char(15), in conCurrId int)
BEGIN
	Update customers
    Set name = name, address = address, city = city, state = state, zipcode = zipcode, concurrencyid = (concurrencyid + 1)
    Where customerID = custID and concurrencyid = conCurrId;
END //
DELIMITER ;