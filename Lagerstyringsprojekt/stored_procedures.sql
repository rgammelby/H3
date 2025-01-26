-- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES

USE Lagerstyring;

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('proc_name', 'P') IS NOT NULL
    DROP PROCEDURE proc_name;
GO

-- Creates the procedure
CREATE PROCEDURE proc_name
	-- @parameter type
AS
BEGIN
    -- Logic
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- hvis vi har activity status i en separat UserActivity table
-- Drops the procedure if it already exists
IF OBJECT_ID('DisableUser', 'P') IS NOT NULL
    DROP PROCEDURE DisableUser;
GO

-- Creates the procedure
CREATE PROCEDURE DisableUser
    @user_id INT
AS
BEGIN
    -- Check if the user's activity status is already "disabled"
    IF EXISTS (
        SELECT 1 
        FROM [User] u
        INNER JOIN UserActivity ua ON u.active_status = ua.id
        WHERE u.id = @user_id AND ua.name = 'disabled'
    )
    BEGIN
        -- If the user is already disabled, return a message
        PRINT 'User is already disabled.';
        RETURN;
    END;

    -- Update the user's activity status to "disabled"
    UPDATE [User]
    SET active_status = (
        SELECT id FROM UserActivity WHERE name = 'disabled'
    )
    WHERE id = @user_id;

    PRINT 'User has been disabled successfully.';
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- hvis vi beholder admin/user som en string value i user-tabellen
-- Drops the procedure if it already exists
IF OBJECT_ID('DisableUser', 'P') IS NOT NULL
    DROP PROCEDURE DisableUser;
GO

-- Creates the procedure
CREATE PROCEDURE DisableUser
    @user_id INT
AS
BEGIN
    -- Check if the user's activity status is already "disabled"
    IF EXISTS (
        SELECT 1 
        FROM [User]
        WHERE id = @user_id AND activity = 'disabled'
    )
    BEGIN
        -- If the user is already disabled, return a message
        PRINT 'User is already disabled.';
        RETURN;
    END;

    -- Update the user's activity status to "disabled"
    UPDATE [User]
    SET activity = 'disabled'
    WHERE id = @user_id;

    PRINT 'User has been disabled successfully.';
END;
GO
-- PROCEDURE END



-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetLifecycle', 'P') IS NOT NULL
    DROP PROCEDURE GetLifecycle;
GO

-- Creates the procedure
CREATE PROCEDURE GetLifecycle
	-- @parameter type
	@lifecycle_id VARCHAR(64)
AS
BEGIN
    -- Logic
	SELECT * FROM ActivityHistory
	WHERE lifecycle_id = @lifecycle_id;
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('UpdateUserStringValues', 'P') IS NOT NULL
    DROP PROCEDURE UpdateUserStringValues;
GO
CREATE PROCEDURE UpdateUserStringValues
    @user_first_name VARCHAR(64),
    @user_last_name VARCHAR(64),
    @user_email VARCHAR(64),
    @user_telephone VARCHAR(20),
    @user_active_name VARCHAR(32),
    @user_type_name VARCHAR(32) 
AS
BEGIN
    -- Updates the user in the User table
    UPDATE [User]
    SET 
        first_name = @user_first_name, 
        last_name = @user_last_name, 
        email = @user_email, 
        telephone = @user_telephone, 
        active_status = ua.id;    -- ID from UserType table
    FROM 
        UserActivity ua, 
        UserType ut
    WHERE 
        ua.name = @user_active_name
        AND ut.name = @user_type_name
        AND [User].id = @user_id;    -- Ensure the correct user is updated

END;
GO
-- PROCEDURE END


-- hvis vi sender en string value med; Admin/User, Active/Inactive
-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('CreateUserStringValues', 'P') IS NOT NULL
    DROP PROCEDURE CreateUserStringValues;
GO
CREATE PROCEDURE CreateUserStringValues
    @user_first_name VARCHAR(64),
    @user_last_name VARCHAR(64),
    @user_email VARCHAR(64),
    @user_telephone VARCHAR(20),
    @user_active_name VARCHAR(32),
    @user_type_name VARCHAR(32) 
AS
BEGIN
    -- Insert the new user into the User table
    INSERT INTO [User] (
        first_name, 
        last_name, 
        email, 
        telephone, 
        active_status, 
        user_type
    )
    SELECT 
        @user_first_name, 
        @user_last_name, 
        @user_email, 
        @user_telephone, 
        ua.id,       -- ID from UserActivity table
        ut.id        -- ID from UserType table
    FROM 
        UserActivity ua, 
        UserType ut
    WHERE 
        ua.name = @user_active_name
        AND ut.name = @user_type_name;

END;
GO
-- PROCEDURE END

-- hvis vi sender et reelt ID med
-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('CreateUser', 'P') IS NOT NULL
    DROP PROCEDURE CreateUser;
GO
CREATE PROCEDURE CreateUser
    @user_first_name VARCHAR(64),
    @user_last_name VARCHAR(64),
    @user_email VARCHAR(64),
    @user_telephone VARCHAR(20),
    @user_active INT,  -- vi er nok nødt til også at have tabeller for user active level (Active/Inactive)
    @user_type INT      -- og for user_type; (User/Admin)
AS
BEGIN
    -- Insert the new user into the User table
    INSERT INTO [User] (
        first_name, 
        last_name, 
        email, 
        telephone, 
        active_status, 
        user_type
    )
    VALUES (
        @user_first_name, 
        @user_last_name, 
        @user_email, 
        @user_telephone, 
        @user_active, 
        @user_type
    );

END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetSingleUser', 'P') IS NOT NULL
    DROP PROCEDURE GetSingleUser;
GO

-- Creates the procedure
CREATE PROCEDURE GetSingleUser
	-- @parameter type
	@user_id INT
AS
BEGIN
    -- Logic
	SELECT * FROM User
	WHERE id = @user_id;
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetAllUsers', 'P') IS NOT NULL
    DROP PROCEDURE GetAllUsers;
GO

-- Creates the procedure
CREATE PROCEDURE GetAllUsers
	-- @parameter type
AS
BEGIN
    -- Logic
	SELECT * FROM User;
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('DeleteSingleDevice', 'P') IS NOT NULL
    DROP PROCEDURE DeleteSingleDevice;
GO

-- Creates the procedure
CREATE PROCEDURE DeleteSingleDevice
	-- @parameter type
	@single_device_id INT
AS
BEGIN
    -- Logic
	DELETE FROM SingleDevice
	WHERE id = @single_device_id;
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
IF OBJECT_ID('UpdateDeviceType', 'P') IS NOT NULL
    DROP PROCEDURE UpdateDeviceType;
GO

CREATE PROCEDURE UpdateDeviceType
    @device_id INT,           -- Assuming there is an ID to identify the device
    @device_name VARCHAR(256),
    @device_type VARCHAR(64),
    @qty INT,
    @image VARCHAR(MAX)       -- Assuming base64 encoding as a placeholder for the image
AS
BEGIN
    -- Update an existing device in the Device table
    UPDATE Device
    SET
        name = @device_name,        -- Update the device name
        device_type = dt.id,        -- Update the device_type with the corresponding ID from DeviceType table
        qty = @qty,                 -- Update the quantity
        image = @image              -- Update the image (base64 or as needed)
    FROM DeviceType dt
    WHERE Device.id = @device_id   -- Identify the device to update by its ID
    AND dt.name = @device_type;    -- Match the device type to ensure correct ID mapping

END;
GO
-- PROCEDURE END


-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('CreateDeviceType', 'P') IS NOT NULL
    DROP PROCEDURE CreateDeviceType;
GO

-- Creates the procedure
CREATE PROCEDURE CreateDeviceType
	-- @parameter type
	@device_name VARCHAR(256),
	@device_type VARCHAR(64),
	@qty INT,
    @image VARCHAR(MAX) -- Assuming base64 encoding as a placeholder for the image
AS
BEGIN
    -- Insert a new device into the Device table
    INSERT INTO Device (
        name, 
        device_type, 
        qty, 
        image
    )
    SELECT 
        @device_name, -- Directly uses the input device name
        dt.id,        -- Device type ID from DeviceType table where DeviceType.name matches @device_type
        @qty,         -- Directly uses the input quantity
        @image        -- Directly uses the input image (base64 encoded or whatever the format is)
    FROM DeviceType dt
    WHERE dt.name = @device_type;

END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Creates the procedure
CREATE PROCEDURE CreateSingleDevice
    @device_type VARCHAR(12),
    @device_status VARCHAR(12),
    @device_location VARCHAR(64),
    @device_description VARCHAR(256),
    @device_qr VARCHAR(MAX) -- Placeholder for QR, change as needed later
AS
BEGIN
    -- Insert a new row into SingleDevice table
    INSERT INTO SingleDevice (
        name, 
        type, 
        status, 
        location, 
        description, 
        qr
    )
    SELECT 
        d.device_name, -- Assuming 'device_name' is the column in Device for the name
        d.id,          -- ID from Device table where Device.type matches @device_type
        st.id,         -- ID from StatusType table where StatusType.status_type matches @device_status
        l.id,          -- ID from Location table where Location.designation matches @device_location
        @device_description, -- Directly uses the input parameter for description
        NULL           -- QR code left as NULL for now
    FROM Device d
    CROSS JOIN StatusType st
    CROSS JOIN Location l
    WHERE d.device_type = @device_type
      AND st.status_type = @device_status
      AND l.designation = @device_location;
END;
GO
-- PROCEDURE END



-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetFilteredDevices', 'P') IS NOT NULL
    DROP PROCEDURE GetFilteredDevices;
GO

-- PROCEDURE BEGIN
CREATE PROCEDURE GetFilteredDevices
    @filter VARCHAR(12) -- Parameter to filter devices by type
AS
BEGIN
    -- Selects all entries from SingleDevice where the type matches the filter
    SELECT sd.*
    FROM SingleDevice sd
    INNER JOIN Device d ON sd.type = d.id
    WHERE d.device_type = @filter;
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetDeviceById', 'P') IS NOT NULL
    DROP PROCEDURE GetDeviceById;
GO

-- Creates the procedure
CREATE PROCEDURE GetDeviceById
	-- @parameter type
	@single_device_id INT
AS
BEGIN
    -- Logic
	SELECT * FROM SingleDevice
	WHERE id = @single_device_id;
END;
GO
-- PROCEDURE END

-- Drops the procedure if it already exists
IF OBJECT_ID('GetAllDevices', 'P') IS NOT NULL
    DROP PROCEDURE GetAllDevices;
GO

-- Creates the procedure
CREATE PROCEDURE GetAllDevices
	-- @parameter type
AS
BEGIN
    -- Logic
	SELECT * FROM Device;
END;
GO


-- Drops the procedure if it already exists
IF OBJECT_ID('GetAllActivities', 'P') IS NOT NULL
    DROP PROCEDURE GetAllActivities;
GO

-- Creates the procedure
CREATE PROCEDURE GetAllActivities
AS
BEGIN
    -- Retrieves all entries from the ActivityHistory table
    SELECT * FROM ActivityHistory;
END;
GO


DROP PROCEDURE IF EXISTS proc_name;

DELIMITER //
CREATE PROCEDURE proc_name(OUT (return) row_name TYPE)
BEGIN
	-- LOGIC
END//
DELIMITER;
-- Generate a random number
DROP PROCEDURE IF EXISTS GenerateOrderNumber;

DELIMITER //
CREATE PROCEDURE GenerateOrderNumber(OUT orderNumber INT)
BEGIN
	DECLARE randomNumber INT;

	REPEAT
        SET randomNumber = (100000 + RAND() * 900000);
    UNTIL NOT EXISTS (SELECT 1 FROM Purchase WHERE order_number = randomNumber) END REPEAT;
    
    SET orderNumber = randomNumber;
END//
DELIMITER ;

-- Creates a new order and generates an order number
DROP PROCEDURE IF EXISTS CreateNewOrder;

DELIMITER //
CREATE PROCEDURE CreateNewOrder (IN customerUsername VARCHAR(64))
BEGIN
	DECLARE orderNumber INT;
	CALL GenerateOrderNumber(orderNumber);
    SELECT orderNumber;
    INSERT INTO Purchase
    VALUES (DEFAULT, orderNumber, (SELECT customer_id FROM Customer WHERE username LIKE CONCAT('%', customerUsername, '%')));
    
END //
DELIMITER ;

-- Fetches logs made in a span of dates 
DROP PROCEDURE IF EXISTS GetLogsBetweenDates;

DELIMITER //
CREATE PROCEDURE GetLogsBetweenDates (
    IN firstDay INT, 
    IN firstMonth INT, 
    IN firstYear VARCHAR(4), 
    IN lastDay INT, 
    IN lastMonth INT, 
    IN lastYear VARCHAR(4)
)

BEGIN
    IF firstYear = '' THEN SET firstYear = YEAR(CURDATE()); END IF;
    IF lastYear = '' THEN SET lastYear = YEAR(CURDATE()); END IF;
    SELECT log_id as 'Log no.', change_type, table_name, id_key as Id, log_time as 'Time and Date' 
    FROM bogreden_log
    WHERE log_time BETWEEN CONCAT(firstYear, '-', firstMonth, '-', firstDay)
    AND CONCAT(lastYear, '-', lastMonth, '-', lastDay)
    ORDER BY log_time;
END //
DELIMITER ;

-- Get books by author
DROP PROCEDURE IF EXISTS GetBooksByAuthor;

DELIMITER //
CREATE PROCEDURE GetBooksByAuthor (IN authorName VARCHAR(50))
BEGIN 
	SELECT DISTINCT b.title AS Title, b.price AS 'Price (kr.)', CONCAT(a.first_name, a.last_name) AS Author 
    FROM Book b
    JOIN Author a 
    ON b.author = a.author_id
    WHERE b.author IN (SELECT author_id FROM Author WHERE first_name LIKE CONCAT('%', authorName, '%') OR last_name LIKE CONCAT('%', authorName, '%') ORDER BY last_name ASC);
END //
DELIMITER ;

-- Get author of book
DROP PROCEDURE IF EXISTS GetAuthorByBookTitle;

DELIMITER //
CREATE PROCEDURE GetAuthorByBookTitle (IN bookTitle VARCHAR(256)) 
BEGIN
	SELECT b.title AS Title, concat(a.first_name, a.last_name) AS Author
    FROM Author a
    JOIN Book b ON a.author_id = b.author
    WHERE b.title LIKE CONCAT('%', bookTitle, '%');
END //
DELIMITER ;

-- Get customer info by customer
DROP PROCEDURE IF EXISTS GetCustomerInfoByCustomerName;

DELIMITER //
CREATE PROCEDURE GetCustomerInfoByUsername (IN customerUsername VARCHAR(50))
BEGIN
	SELECT CONCAT(c.first_name, c.last_name) AS Name, c.email AS 'E-mail', c.road_and_number AS Address, a.postcode AS 'Postcode', a.city AS City
    FROM Customer c
    JOIN Address a ON c.address = a.address_id
    WHERE c.username = customerUsername;
END //
DELIMITER ;

-- Get orders by customer
DROP PROCEDURE IF EXISTS GetOrdersByCustomerUsername;

DELIMITER //
CREATE PROCEDURE GetOrdersByCustomer (IN customerUsername VARCHAR(50))
BEGIN
	SELECT p.order_number, b.title, c.name FROM BookOrder o
    JOIN Purchase p ON o.order_number = p.order_id
    JOIN Customer c ON p.customer = c.customer_id
    JOIN Book b ON o.book = b.book_id
    WHERE c.username = customerUsername;
END //
DELIMITER ;

-- Get book info by book
DROP PROCEDURE IF EXISTS GetBookInfoByBookTitle;

DELIMITER //
CREATE PROCEDURE GetBookInfoByBookTitle (IN bookTitle VARCHAR(256))
BEGIN
	SELECT b.title AS Title, CONCAT(a.first_name, a.last_name) AS Author, b.price AS 'Price (kr.)', g.name AS Genre
    FROM Book b
    JOIN Author a ON b.author = a.author_id
    JOIN Genre g ON b.genre = g.genre_id
    WHERE title LIKE CONCAT('%', bookTitle, '%');
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS CreateNewUser;

DELIMITER //
CREATE PROCEDURE CreateNewUser (
	IN customerUsername VARCHAR(64),
    IN customerPassword VARCHAR(64),
    IN customerFirstName VARCHAR(50), 
    IN customerLastName VARCHAR(50),
    IN customerMail VARCHAR(50), 
    IN customerAddress VARCHAR(50), 
    IN addressIdFromPostcode SMALLINT
)
BEGIN
	INSERT INTO Customer
    VALUES (DEFAULT, customerUsername, SHA2(customerPassword, 256), CONCAT(customerFirstName, ' '), customerLastName, customerMail, customerAddress, (SELECT address_id FROM Address WHERE postcode = addressIdFromPostcode));
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS CreateNewAuthor;

DELIMITER //
CREATE PROCEDURE CreateNewAuthor (IN authorName VARCHAR(50), IN authorLastName varchar(50))
BEGIN
	DECLARE existingAuthor INT;
	SELECT author_id FROM Author WHERE first_name LIKE CONCAT('%', authorName, '%') AND last_name LIKE CONCAT('%', authorLastName, '%') INTO existingAuthor;
    
    IF existingAuthor IS NULL THEN
		INSERT INTO Author
		VALUES (DEFAULT, CONCAT(authorName, ' '), authorLastName);
	
    ELSE SELECT "This author already exists in the database. " AS Error;
    END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS CreateNewGenre;

DELIMITER //
CREATE PROCEDURE CreateNewGenre (IN genreName VARCHAR(50))
BEGIN
	DECLARE existingGenre INT;
    SELECT genre_id FROM Genre WHERE name = genreName INTO existingGenre;
    IF existingGenre IS NULL THEN
		INSERT INTO Genre
		VALUES (DEFAULT, genreName);
        
	ELSE SELECT "This genre already exists in the database. " AS Error;
    END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS CreateNewBook;

DELIMITER //
CREATE PROCEDURE CreateNewBook (
    IN bookTitle VARCHAR(256), 
    IN bookAuthor VARCHAR(50), 
    IN bookAuthorLastName VARCHAR(50),
    IN bookPrice SMALLINT, 
    IN bookGenre VARCHAR(50)
)
BEGIN
	-- Declare author and genre ids to ensure the book gets created even if the author or genre is not currently in the database
	DECLARE authorId SMALLINT;
    DECLARE genreId SMALLINT;
    DECLARE existingBookTitle SMALLINT;
    
    -- Checks validity of author and genre in procedure call
    SELECT author_id INTO authorId FROM Author WHERE first_name LIKE CONCAT('%', bookAuthor, '%') OR last_name LIKE CONCAT('%', bookAuthorLastName, '%');
    SELECT genre_id INTO genreId FROM Genre WHERE name LIKE CONCAT('%', bookGenre, '%');
    SELECT book_id INTO existingBookTitle FROM Book WHERE title = bookTitle;
    
    IF existingBookTitle IS NULL THEN
		-- If author or genre from procedure call does not exist, create record for that author or genre
		IF authorId IS NULL THEN
			CALL CreateNewAuthor(bookAuthor, bookAuthorLastName);
			SELECT LAST_INSERT_ID() INTO authorId;
		END IF;
    
		IF genreId IS NULL THEN
			CALL CreateNewGenre(bookGenre);
			SELECT LAST_INSERT_ID() INTO genreId;
		END IF;
        
	INSERT INTO Book
    VALUES (DEFAULT, bookTitle, authorId, bookPrice, genreId);
    
    ELSE SELECT "This book already exists in the database. " AS Error;
    END IF;
    
END //
DELIMITER ;

-- Assigns a product to a specific order number using a pre-generated order number from Purchase and the Book(book_id) - call for each book purchased
DROP PROCEDURE IF EXISTS CreateNewBookOrder;

DELIMITER //
CREATE PROCEDURE CreateNewBookOrder (IN orderNumber SMALLINT, IN orderedBook SMALLINT)
BEGIN
	INSERT INTO BookOrder
    VALUES (DEFAULT, orderNumber, (SELECT book_id FROM Book WHERE title LIKE CONCAT('%', orderedBook, '%')));
END //
DELIMITER ; 

-- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES
