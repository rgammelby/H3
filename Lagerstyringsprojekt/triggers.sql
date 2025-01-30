
---------------------------------qty and available-qty in DeviceOverview table-------------------------------------------------

-- ---------------------------Update qty and available_qty in DeviceOverview when a SingleDevice is Archived---------------------------
USE [Lagerstyring]
GO
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


-- |1 Available|2 Overdue|3 Borrowed|4 Unavailable
-------------------------------------trgUpdateAvailable_qtyUponSingleDeviceStatusChange---------------------------
USE [Lagerstyring]
GO
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


---------------------------------## Sending and Logging Low stock Notification --------------------------------------
USE [Lagerstyring]
GO
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
                   ', Model = ', dv.model, 
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

USE [Lagerstyring]
GO
/****** Object:  Trigger [dbo].[trgLogUser]    Script Date: 30-01-2025 11:35:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   TRIGGER [dbo].[trgLogUser]
ON [dbo].[User]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle INSERT (New User Added)
    IF EXISTS (SELECT 1 FROM Inserted) AND NOT EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Insert',
            CONCAT(
                'New user added: ID = ', CAST(i.id AS NVARCHAR(36)), 
                ', Email = ', i.email, 
                ', Type = ', i.type,
                COALESCE(NULLIF(', First Name = ' + i.first_name, ', First Name = '), ''),
                COALESCE(NULLIF(', Last Name = ' + i.last_name, ', Last Name = '), ''),
                COALESCE(NULLIF(', Telephone = ' + i.telephone, ', Telephone = '), '')
            ),
            GETDATE()
        FROM Inserted i;
    END;

    -- Handle UPDATE (Only Log True Changes)
    IF EXISTS (SELECT 1 FROM Inserted) AND EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        DECLARE @LogMessage NVARCHAR(MAX);

        SELECT 
            @LogMessage = CONCAT(
                'User updated: ID = ', CAST(i.id AS NVARCHAR(36)), 
                ', Email = ', i.email,

                CASE WHEN d.telephone <> i.telephone THEN 
                    CONCAT(', Telephone changed from ', COALESCE(d.telephone, 'NULL'), ' to ', COALESCE(i.telephone, 'NULL'))
                ELSE '' END,

                CASE WHEN d.is_active <> i.is_active THEN 
                    CONCAT(', Active Status changed from ', d.is_active, ' to ', i.is_active) 
                ELSE '' END,

                CASE WHEN d.first_name <> i.first_name THEN 
                    CONCAT(', First Name changed from ', COALESCE(d.first_name, 'NULL'), ' to ', COALESCE(i.first_name, 'NULL')) 
                ELSE '' END,

                CASE WHEN d.last_name <> i.last_name THEN 
                    CONCAT(', Last Name changed from ', COALESCE(d.last_name, 'NULL'), ' to ', COALESCE(i.last_name, 'NULL')) 
                ELSE '' END,

                CASE WHEN d.type <> i.type THEN 
                    CONCAT(', Type changed from ', d.type, ' to ', i.type) 
                ELSE '' END
            )
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id;

        -- Only insert the log if something actually changed
        IF LEN(@LogMessage) > LEN('User updated: ID = ' + CAST((SELECT TOP 1 id FROM Inserted) AS NVARCHAR(36)) + ', Email = ' + (SELECT TOP 1 email FROM Inserted))
        BEGIN
            INSERT INTO Log (log_type, log_message, timestamp)
            VALUES ('Update', @LogMessage, GETDATE());
        END
    END;
END;


-----------------------------Log INSERT and UPDATE operations on DeviceOverview table-----------------------------
USE [Lagerstyring]
GO
CREATE OR ALTER TRIGGER trgLogDeviceOverview
ON DeviceOverview
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle INSERT (New DeviceOverview Added)
    IF EXISTS (SELECT 1 FROM Inserted) AND NOT EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Insert',
            CONCAT(
                'New DeviceOverview added: ID = ', CAST(i.id AS NVARCHAR(36)),
                ', Device Type = ', i.device_type,
                ', Model = ', i.model,
                ', Available Qty = ', COALESCE(CAST(i.available_qty AS NVARCHAR), '0'),
                ', Qty = ', COALESCE(CAST(i.qty AS NVARCHAR), '0'),
                CASE WHEN i.image IS NOT NULL THEN ', Image Provided' ELSE ', No Image' END,
                CASE WHEN i.last_ordered IS NOT NULL THEN CONCAT(', Last Ordered = ', CONVERT(NVARCHAR, i.last_ordered, 120)) ELSE ', Last Ordered = NULL' END
            ),
            GETDATE()
        FROM Inserted i;
    END;

    -- Handle UPDATE (Only Log True Changes)
    IF EXISTS (SELECT 1 FROM Inserted) AND EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        DECLARE @LogMessage NVARCHAR(MAX);

        SELECT 
            @LogMessage = CONCAT(
                'Device updated: ID = ', CAST(i.id AS NVARCHAR(36)),

                CASE WHEN d.device_type <> i.device_type THEN 
                    CONCAT(', Device Type changed from ', d.device_type, ' to ', i.device_type) 
                ELSE '' END,

                CASE WHEN d.model <> i.model THEN 
                    CONCAT(', Model changed from ', d.model, ' to ', i.model) 
                ELSE '' END,

                CASE WHEN d.available_qty <> i.available_qty THEN 
                    CONCAT(', Available Qty changed from ', CAST(d.available_qty AS NVARCHAR), ' to ', CAST(i.available_qty AS NVARCHAR)) 
                ELSE '' END,

                CASE WHEN d.qty <> i.qty THEN 
                    CONCAT(', Qty changed from ', CAST(d.qty AS NVARCHAR), ' to ', CAST(i.qty AS NVARCHAR)) 
                ELSE '' END,

                CASE WHEN d.image <> i.image THEN ', Image updated' ELSE '' END,

                CASE WHEN d.last_ordered <> i.last_ordered THEN 
                    CONCAT(', Last Ordered changed from ', COALESCE(CONVERT(NVARCHAR, d.last_ordered, 120), 'NULL'),
                                             ' to ', COALESCE(CONVERT(NVARCHAR, i.last_ordered, 120), 'NULL')) 
                ELSE '' END
            )
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id;

        -- Only insert the log if something actually changed
        IF LEN(@LogMessage) > LEN('Device updated: ID = ' + CAST((SELECT TOP 1 id FROM Inserted) AS NVARCHAR(36)))
        BEGIN
            INSERT INTO Log (log_type, log_message, timestamp)
            VALUES ('Update', @LogMessage, GETDATE());
        END
    END;
END;


-----------------------------Log INSERT and UPDATE operations on SingleDevice table-----------------------------
USE [Lagerstyring]
GO
CREATE OR ALTER TRIGGER trgLogSingleDevice
ON SingleDevice
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle INSERT (New Single Device Added)
    IF EXISTS (SELECT 1 FROM Inserted) AND NOT EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        INSERT INTO Log (log_type, log_message, timestamp)
        SELECT 
            'Insert',
            CONCAT(
                'New single device added: ID = ', CAST(i.id AS NVARCHAR(36)),
                ', Device Overview ID = ', CAST(i.deviceOverview_id AS NVARCHAR(36)), 
                COALESCE(NULLIF(', Description = ' + i.description, ', Description = '), ''),
                ', Status = ', i.status,
                COALESCE(NULLIF(', Location = ' + i.location, ', Location = '), ''),
                COALESCE(NULLIF(', QR = ' + i.qr, ', QR = '), ''),
                ', Is Archived = ', COALESCE(CAST(i.is_archived AS NVARCHAR), '0')
            ),
            GETDATE()
        FROM Inserted i;
    END;

    -- Handle UPDATE (Only Log True Changes)
    IF EXISTS (SELECT 1 FROM Inserted) AND EXISTS (SELECT 1 FROM Deleted)
    BEGIN
        DECLARE @LogMessage NVARCHAR(MAX);

        SELECT 
            @LogMessage = CONCAT(
                'Single device updated: ID = ', CAST(i.id AS NVARCHAR(36)),

                CASE WHEN d.deviceOverview_id <> i.deviceOverview_id THEN 
                    CONCAT(', Device Overview changed from ', CAST(d.deviceOverview_id AS NVARCHAR(36)), ' to ', CAST(i.deviceOverview_id AS NVARCHAR(36))) 
                ELSE '' END,

                CASE WHEN d.description <> i.description THEN 
                    CONCAT(', Description changed from ', COALESCE(d.description, 'NULL'), ' to ', COALESCE(i.description, 'NULL')) 
                ELSE '' END,

                CASE WHEN d.status <> i.status THEN 
                    CONCAT(', Status changed from ', d.status, ' to ', i.status) 
                ELSE '' END,

                CASE WHEN d.location <> i.location THEN 
                    CONCAT(', Location changed from ', COALESCE(d.location, 'NULL'), ' to ', COALESCE(i.location, 'NULL')) 
                ELSE '' END,

                CASE WHEN d.qr <> i.qr THEN 
                    CONCAT(', QR changed from ', COALESCE(d.qr, 'NULL'), ' to ', COALESCE(i.qr, 'NULL')) 
                ELSE '' END,

                CASE WHEN d.is_archived <> i.is_archived THEN 
                    CONCAT(', Is Archived changed from ', CAST(d.is_archived AS NVARCHAR), ' to ', CAST(i.is_archived AS NVARCHAR)) 
                ELSE '' END
            )
        FROM Inserted i
        INNER JOIN Deleted d ON i.id = d.id;

        -- Only insert the log if something actually changed
        IF LEN(@LogMessage) > LEN('Single device updated: ID = ' + CAST((SELECT TOP 1 id FROM Inserted) AS NVARCHAR(36)))
        BEGIN
            INSERT INTO Log (log_type, log_message, timestamp)
            VALUES ('Update', @LogMessage, GETDATE());
        END
    END;
END;
GO


