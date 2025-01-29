USE Lagerstyring;

---------------------------------qty and available-qty in DeviceOverview table-------------------------------------------------

-- ---------------------------Update qty and available_qty in DeviceOverview when a SingleDevice is Archived---------------------------

CREATE TRIGGER trgUpdateQtyAndAvailable_qtyWhenSingleDeviceIsArchived
ON SingleDevice
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Decrement qty and available_qty in DeviceOverview when SingleDevice is archived
    UPDATE DeviceOverview
    SET 
        qty = qty - 1,
        available_qty = available_qty - 1
    FROM DeviceOverview
    INNER JOIN Inserted i ON 
        -- DeviceOverview.model = i.name -- Match the model in DeviceOverview with the name in SingleDevice
        -- AND DeviceOverview.device_type = i.type -- Match the device_type
        DeviceOverview.id = i.deviceOverview_id -- Match by deviceOverview_id
    WHERE i.is_archived = 1 -- Check if SingleDevice is archived
      AND qty > 0          -- Ensure qty doesn't go below zero
      AND available_qty > 0; -- Ensure available_qty doesn't go below zero
END;
GO


-- |1 Available|2 Overdue|3 Borrowed|4 Unavailable
-------------------------------------trgUpdateAvailable_qtyUponSingleDeviceStatusChange---------------------------

CREATE TRIGGER trgUpdateAvailable_qtyUponSingleDeviceStatusChange
ON SingleDevice
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Increment available_qty when status changes to 1 Available (status_type_id = 1)
    UPDATE DeviceOverview
    SET
        available_qty = available_qty + 1
        FROM DeviceOverview
        INNER JOIN Inserted i ON 
            -- DeviceOverview.model = i.name -- Match the model in DeviceOverview with the name in SingleDevice
            -- AND DeviceOverview.device_type = i.type -- Match the device_type
            DeviceOverview.id = i.deviceOverview_id -- Match by deviceOverview_id
        INNER JOIN Deleted d ON d.id = i.id
        WHERE d.status != 1  -- Previous status was not Available (1)
        AND i.status = 1 -- Current status is Available (1)

    -- Decrement available_qty when status changes from 1 Available to any other status
    UPDATE DeviceOverview
    SET
        available_qty = available_qty - 1
        FROM DeviceOverview
        INNER JOIN Inserted i ON
            -- DeviceOverview.model = i.name -- Match the model in DeviceOverview with the name in SingleDevice
            -- AND DeviceOverview.device_type = i.type -- Match the device_type
            DeviceOverview.id = i.deviceOverview_id
        INNER JOIN Deleted d ON d.id = i.id
        WHERE d.status = 1  -- Previous status was Available (1)
        AND i.status != 1; -- Current status is not Available (1)
END;
GO


---------------------------------## Sending and Logging Low stock Notification --------------------------------------

CREATE TRIGGER trgLowStockLogAndNotification
ON DeviceOverview
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if available_qty drops below 10
    IF EXISTS (
        SELECT 1
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id
        WHERE i.available_qty < 10  -- New value is less than 10
          AND d.available_qty >= 10 -- Old value was 10 or more
    )
    -- Send low stock notification when qty is less than or equal to 10
    BEGIN
        -- Insert a log entry into the Log table
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'LowStockNotification',  -- Log type
            CONCAT('Low stock alert: Device ID = ', i.id, 
                   ', Model = ', dv.model, -- 'dv' represents DeviceOverview, fetching model
                   ', Available Qty = ', i.available_qty), -- Log message
            GETDATE() -- Timestamp
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id
        INNER JOIN DeviceOverview dv ON dv.id = i.id -- Fetch model using `dv`
        WHERE i.available_qty < 10
          AND d.available_qty >= 10;

        -- Simulate sending a notification (optional)
        PRINT 'Low stock alert logged. Check the Log table for details.';
    END;
END;

------------------------------Log INSERT and UPDATE operations on User table----------------------------

CREATE TRIGGER trgLogUser
ON [User]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle INSERT
    IF EXISTS (SELECT 1 FROM Inserted) AND NOT EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Insert',
            CONCAT(
                'New user added: ID = ', i.id, 
                ', Email = ', i.email, -- Email is mandatory, so always included
                ', Type = ', i.type,   -- Type is mandatory, so always included
                ', Salt = ', i.salt, -- Log salt for debugging security
                CASE 
                    WHEN i.first_name IS NOT NULL THEN CONCAT(', First Name = ', i.first_name) 
                ELSE ', First Name = NULL'
                END,
                CASE 
                    WHEN i.last_name IS NOT NULL THEN CONCAT(', Last Name = ', i.last_name) 
                    ELSE ', Last Name = NULL'
                END,
                CASE 
                    WHEN i.telephone IS NOT NULL THEN CONCAT(', Telephone = ', i.telephone) 
                    ELSE ', Telephone = NULL'
                END
            ),
            GETDATE()
        FROM Inserted i;
    END

    -- Handle UPDATE
    IF EXISTS (SELECT 1 FROM Inserted) AND EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Update',
            CONCAT(
                'User updated: ID = ', i.id, -- ID is static and included for context
                ', Email = ', i.email,       -- Email is static and included for context
                CASE 
                    WHEN d.telephone <> i.telephone THEN CONCAT(', Telephone changed from ', d.telephone, ' to ', i.telephone) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.is_active <> i.is_active THEN CONCAT(', Active Status changed from ', d.is_active, ' to ', i.is_active) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.first_name <> i.first_name THEN CONCAT(', First Name changed from ', d.first_name, ' to ', i.first_name) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.last_name <> i.last_name THEN CONCAT(', Last Name changed from ', d.last_name, ' to ', i.last_name) 
                    ELSE ''
                END
                CASE 
                    WHEN d.type <> i.type THEN CONCAT(', Type changed from ', d.type, ' to ', i.type) 
                    ELSE ''
                END
            ),
            GETDATE()
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id;
    END
END;
GO


-----------------------------Log INSERT and UPDATE operations on DeviceOverview table-----------------------------

CREATE TRIGGER trgLogDeviceOverview
ON DeviceOverview
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle INSERT
    IF EXISTS (SELECT 1 FROM Inserted) AND NOT EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Insert',
            CONCAT(
                'New DeviceOverview added: ID = ', i.id,
                ', Device Type = ', i.device_type,
                ', Model = ', i.model,
                ', Available Qty = ', COALESCE(CAST(i.available_qty AS NVARCHAR), '0'),
                ', Qty = ', COALESCE(CAST(i.qty AS NVARCHAR), '0'),
                CASE 
                    WHEN i.image IS NOT NULL THEN CONCAT(', Image Provided') 
                    ELSE ', No Image'
                END,
                CASE 
                    WHEN i.last_ordered IS NOT NULL THEN CONCAT(', Last Ordered = ', CONVERT(NVARCHAR, i.last_ordered, 120)) 
                    ELSE ', Last Ordered = NULL'
                END
            ),
            GETDATE()
        FROM Inserted i;
    END;

    -- Handle UPDATE
    IF EXISTS (SELECT 1 FROM Inserted) AND EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Update',
            CONCAT(
                'Device updated: ID = ', i.id,
                CASE 
                    WHEN d.device_type <> i.device_type THEN CONCAT(', Device Type changed from ', d.device_type, ' to ', i.device_type) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.model <> i.model THEN CONCAT(', Model changed from ', d.model, ' to ', i.model) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.available_qty <> i.available_qty THEN CONCAT(', Available Qty changed from ', d.available_qty, ' to ', i.available_qty) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.qty <> i.qty THEN CONCAT(', Qty changed from ', d.qty, ' to ', i.qty) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.image <> i.image THEN CONCAT(', Image updated') 
                    ELSE ''
                END,
                CASE 
                    WHEN d.last_ordered <> i.last_ordered THEN CONCAT(', Last Ordered changed from ', CONVERT(NVARCHAR, d.last_ordered, 120), ' to ', CONVERT(NVARCHAR, i.last_ordered, 120)) 
                    ELSE ''
                END
            ),
            GETDATE()
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id
        WHERE 
            -- Log only if at least one field has changed
            d.device_type <> i.device_type OR
            d.model <> i.model OR
            d.available_qty <> i.available_qty OR
            d.qty <> i.qty OR
            d.image <> i.image OR
            d.last_ordered <> i.last_ordered;
    END;
END;
GO


-----------------------------Log INSERT and UPDATE operations on SingleDevice table-----------------------------
CREATE TRIGGER trgLogSingleDevice
ON SingleDevice
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle INSERT
    IF EXISTS (SELECT 1 FROM Inserted) AND NOT EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Insert',
            CONCAT(
                'New single device added: ID = ', i.id,
                ', Device Overview ID = ', i.deviceOverview_id, --  Updated column
                CASE 
                    WHEN i.description IS NOT NULL THEN CONCAT(', Description = ', i.description)
                    ELSE ', Description = NULL'
                END,
                ', Status = ', i.status,
                CASE 
                    WHEN i.location IS NOT NULL THEN CONCAT(', Location = ', i.location)
                    ELSE ', Location = NULL'
                END,
                CASE 
                    WHEN i.qr IS NOT NULL THEN CONCAT(', QR = ', i.qr)
                    ELSE ', QR = NULL'
                END,
                ', Is Archived = ', COALESCE(CAST(i.is_archived AS NVARCHAR), '0') -- Defaults to '0' (false)
            ),
            GETDATE()
        FROM Inserted i;
    END;

    -- Handle UPDATE
    IF EXISTS (SELECT 1 FROM Inserted) AND EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Update',
            CONCAT(
                'Single device updated: ID = ', i.id,
                CASE 
                    WHEN d.deviceOverview_id <> i.deviceOverview_id THEN CONCAT(', Device Overview changed from ', d.deviceOverview_id, ' to ', i.deviceOverview_id) 
                    ELSE ''
                END,
                CASE 
                    WHEN d.description <> i.description THEN CONCAT(', Description changed from ', d.description, ' to ', i.description)
                    ELSE ''
                END,
                CASE 
                    WHEN d.status <> i.status THEN CONCAT(', Status changed from ', d.status, ' to ', i.status)
                    ELSE ''
                END,
                CASE 
                    WHEN d.location <> i.location THEN CONCAT(', Location changed from ', d.location, ' to ', i.location)
                    ELSE ''
                END,
                CASE 
                    WHEN d.qr <> i.qr THEN CONCAT(', QR changed from ', d.qr, ' to ', i.qr)
                    ELSE ''
                END,
                CASE 
                    WHEN d.is_archived <> i.is_archived THEN CONCAT(', Is Archived changed from ', CAST(d.is_archived AS NVARCHAR), ' to ', CAST(i.is_archived AS NVARCHAR))
                    ELSE ''
                END
            ),
            GETDATE()
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id
        WHERE 
            -- Log only if at least one field has changed
            d.deviceOverview_id <> i.deviceOverview_id OR
            d.description <> i.description OR
            d.status <> i.status OR
            d.location <> i.location OR
            d.qr <> i.qr OR
            d.is_archived <> i.is_archived;
    END;
END;
GO


