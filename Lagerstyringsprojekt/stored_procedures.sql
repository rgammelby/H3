-- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES

USE Lagerstyring;

-- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE -- TEMPLATE 

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

-- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER 

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

-- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER -- USER 

-- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY 

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('CreateActivity', 'P') IS NOT NULL
    DROP PROCEDURE CreateActivity;
GO

-- PROCEDURE BEGIN
-- Creates the procedure
CREATE PROCEDURE CreateActivity
    -- @parameter type
    @device_id INT,
    @user_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @created_on DATETIME,
    @notes VARCHAR(MAX),
	@lifecycle_id VARCHAR(MAX)
AS
BEGIN
    -- Insert logic
    INSERT INTO Activity (
        device_id,
        user_id,
        start_date,
        end_date,
        created_on,
        notes,
        lifecycle_id
    )
    VALUES (
        @device_id,
        @user_id,
        @start_date,
        @end_date,
        CURRENT_TIMESTAMP,
        @notes,
        @lifecycle_id
    );
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetAllActivities', 'P') IS NOT NULL
    DROP PROCEDURE GetAllActivities;
GO

-- Creates the procedure
CREATE PROCEDURE GetAllActivities
AS
BEGIN
    -- Retrieves all entries from the ActivityHistory table
    SELECT * FROM Activity;
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
	SELECT * FROM Activity
	WHERE lifecycle_id = @lifecycle_id;
END;
GO
-- PROCEDURE END


-- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY -- ACTIVITY 

-- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE 

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
IF OBJECT_ID('CreateDevice', 'P') IS NOT NULL
    DROP PROCEDURE CreateDevice;
GO

-- Creates the procedure
CREATE PROCEDURE CreateDevice
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
-- PROCEDEURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('CreateDeviceType', 'P') IS NOT NULL
    DROP PROCEDURE CreateDeviceType;
GO

-- Creates the procedure
CREATE PROCEDURE CreateDeviceType
	-- @parameter type
	@device_type VARCHAR(64)
AS
BEGIN
    -- Logic
	INSERT INTO Device(
		name
	)
	VALUES(
		device_type
	);
END;
GO
-- PROCEDEURE END

-- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE -- DEVICE TYPE  

-- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE 

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('ExtendLoan', 'P') IS NOT NULL
    DROP PROCEDURE ExtendLoan;
GO

-- Creates the procedure
CREATE PROCEDURE ExtendLoan
    -- @parameter type
    @lifecycle_id INT,
    @device_id INT,
    @activity_type INT
AS
BEGIN
    -- Declare variables to hold the activity details
    DECLARE @user_id INT,
            @start_date DATETIME,
            @end_date DATETIME,
            @created_on DATETIME,
            @notes VARCHAR(MAX);

    -- Retrieve the activity details
    SELECT 
        @user_id = user_id,
        @start_date = start_date,
        @end_date = end_date,
        @created_on = created_on,
        @notes = notes
    FROM 
        GetLifecycle(@lifecycle_id);

    -- Update the end_date and activity_type
    SET @end_date = DATEADD(WEEK, 1, @end_date); -- Add 1 week to end_date

    -- Call CreateActivity with updated values
    EXEC CreateActivity 
        @device_id = @device_id,
        @user_id = @user_id,
        @start_date = @start_date,
        @end_date = @end_date,
        @created_on = CURRENT_TIMESTAMP, -- Use the current timestamp
        @notes = @notes;
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- hvis vi har activity status i en separat UserActivity table
-- Drops the procedure if it already exists
IF OBJECT_ID('DisableDevice', 'P') IS NOT NULL
    DROP PROCEDURE DisableDevice;
GO

-- Creates the procedure
CREATE PROCEDURE DisableDevice
    @single_device_id INT
AS
BEGIN
    -- Check if the user's activity status is already "disabled"
    IF EXISTS (
        SELECT 1 
        FROM SingleDevice d
        WHERE d.id = @single_device_id AND is_archived = false;
    )
    BEGIN
        -- If the user is already disabled, return a message
        PRINT 'Device is already inactive. ';
        RETURN;
    END;

    -- Update the user's activity status to "disabled"
    UPDATE SingleDevice
    SET is_archived = true
    WHERE id = @single_device_id;

    PRINT 'User has been disabled successfully.';
END;
GO
-- PROCEDURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('CreateSingleDevice', 'P') IS NOT NULL
    DROP PROCEDURE CreateSingleDevice;
GO

CREATE PROCEDURE CreateSingleDevice
    @device_type VARCHAR(12),
    @device_status VARCHAR(12),
    @device_location VARCHAR(64),
    @device_description VARCHAR(MAX),
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
	-- @filter INT  -- assuming device_type passed as ID from Device table
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

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetAllSingleDevices', 'P') IS NOT NULL
    DROP PROCEDURE GetAllSingleDevices;
GO

-- Creates the procedure
CREATE PROCEDURE GetAllSingleDevices
	-- @parameter type
AS
BEGIN
    -- Logic
	SELECT * FROM SingleDevice
    WHERE is_archived = false;
END;
GO
-- PROCEDEURE END

-- PROCEDURE BEGIN
-- Drops the procedure if it already exists
IF OBJECT_ID('GetDeviceByName', 'P') IS NOT NULL
    DROP PROCEDURE GetDeviceByName;
GO

-- Creates the procedure
CREATE PROCEDURE GetDeviceByName
	-- @parameter type
	@device_name VARCHAR(64)
AS
BEGIN
    -- Logic
	SELECT sd.*
	FROM SingleDevice sd
	INNER JOIN Device d on sd.device_id = d.id
	WHERE d.name = @device_name;
END;
GO
-- PROCEDURE END


-- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE -- SINGLE DEVICE 

-- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES -- STORED PROCEDURES
