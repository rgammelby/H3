
-- Insert sample LocationRooms
INSERT INTO LocationRoom (designation) VALUES 
('Room A'), 
('Room B'), 
('Room C');

-- Insert sample LocationCupboards
INSERT INTO LocationCupboard (designation) VALUES 
('Cupboard 1'), 
('Cupboard 2'), 
('Cupboard 3');


-- Insert sample Locations (Mapping between Room & Cupboard)
-- OBS check the values of ids for Room and Cupboard in the above tables
INSERT INTO Location (room_id, cupboard_id) VALUES 
(1, 1), -- Room A - Cupboard 1
(1, 2), -- Room A - Cupboard 2
(1, 3), -- Room A - Cupboard 3
(2, 1), -- Room B - Cupboard 1
(2, 2), -- Room B - Cupboard 2
(3, 1); -- Room C - Cupboard 1

-- Insert sample Device Types
INSERT INTO DeviceType (type_name) VALUES 
('Laptop'), 
('Desktop'), 
('Microphone Set'), 
('Monitor'), 
('Server'), 
('Router'), 
('Switch'), 
('WebCam'), 
('Headset'), 
('Keyboard'), 
('Mouse');


-- Insert sample Users
INSERT INTO [User] (first_name, last_name, email, telephone, is_active, type, salt, hashedpwd) VALUES
('Alice', 'Johnson', 'alice@example.com', '12345678', 1, 'Admin', 'randomsalt1', 'hashedpassword1'),
('Bob', 'Andersen', 'bob@example.com', '87654321', 1, 'User', 'randomsalt2', 'hashedpassword2'),
('Charlie', 'Madsen', 'charlie@example.com', '99887766', 1, 'User', 'randomsalt3', 'hashedpassword3');
-- Insert sample Users with only mandatory fields
INSERT INTO [User] (email,  is_active, type, salt, hashedpwd) VALUES
( 'adfs@exadfssdle.com',  1, 'User', 'rxxandfgdfomsalt1', 'hashedpafdgfdxxxssword1'),
('fdf@edfsple.com', 1, 'User', 'randomsadgfdlt2', 'hashefdgddgfdgdpassword2'),
( 'calice@exafsdfdsple.com', 1, 'User', 'randogdfmfgfdsalt3', 'hasfdhedpgfdgdfassword3');


-- Insert sample Users
INSERT INTO [User] (first_name, last_name, email, telephone, is_active, type, salt, hashedpwd) VALUES

('Eason', 'Chan', 'es@efans.com', '99887766', 1, 'User', 'rghgfandomsalt3', 'hashedphgfhhassword3');
-- Insert sample Users with only mandatory fields
INSERT INTO [User] (email,  is_active, type, salt, hashedpwd) VALUES
( 'teat1345@exadfssdle.com',  1, 'User', 'rxxandfgdfoghmsalt1', 'hashedpafghgdgfdxxxssword1'),
('test44@edfsple.com', 1, 'User', 'randomsadghgfdlt2', 'hashefghgdgddgfdgdpassword2'),
( 'test324g@exafsdfdsple.com', 1, 'User', 'randogdfmfgghgfdsalt3', 'hasfdhedghgpgfdgdfassword3');

-- Update a User
UPDATE [User]
SET first_name = 'Lisa', last_name = 'Andersen', telephone = '23323233'
WHERE id = '08B5F0F9-C924-480A-9F2F-C293A2416C2F'; -- ID of the user you want to update

-- Insert sample DeviceOverview
INSERT INTO DeviceOverview (device_type, model, available_qty, qty, image, last_ordered) VALUES
(1, 'Dell XPS 15', 15, 15, NULL, '2024-12-15'),  -- Laptops
(1, 'HP G11 840"', 20, 20, NULL, '2024-12-10'),
(2, 'Dell OptiPlex 7090', 20, 20, NULL, '2024-11-25'),  -- Desktops
(3, 'Shure MV7 Podcast Mic', 10, 10, NULL, '2024-11-30'),  -- Microphones
(4, 'LG UltraFine 4K 24', 15, 15, NULL, '2024-12-20'),  -- Monitors
(4, 'LG fge27', 30, 30, NULL, '2024-12-20'),  -- Monitors
(4, 'HP g2 24', 50, 50, NULL, '2024-12-20'),  -- Monitors
(5, 'Dell PowerEdge R750', 24, 24, NULL, '2024-10-15');  -- Servers



--  Get all device overviews for Monitors (DeviceType = 4)
SELECT d.id, d.device_type, dt.type_name, d.model, d.available_qty, d.qty, d.image, d.last_ordered
FROM DeviceOverview d
JOIN DeviceType dt ON d.device_type = dt.id
WHERE dt.type_name = 'Monitor';

-- C# LINQ Query (EF Core)
 var laptops = _context.DeviceOverview
    .Where(d => d.DeviceType.Id == 1) // Fetch all laptops
    .Include(d => d.DeviceType) // Ensure DeviceType is loaded
    .ToList();
-- OR, if filtering by name:
var monitors = _context.DeviceOverview
    .Where(d => d.DeviceType.TypeName == "Monitor")
    .Include(d => d.DeviceType)
    .ToList();

-- Update a DeviceOverview
UPDATE DeviceOverview
SET model = 'Update to New Laptop Model' -- New name
WHERE id = 1; -- ID of the device overview you want to update





-- 🔹 C# Entity Framework Core: Update DeviceOverview
--  Fetch, Modify, and Save
// Find the DeviceOverview by ID
var deviceOverview = _context.DeviceOverview.FirstOrDefault(d => d.Id == 1);

if (deviceOverview != null)
{
    deviceOverview.Model = "New Laptop Model"; // Change name
    _context.SaveChanges(); // Commit changes to the database
}

--🔹 API Endpoint: Update DeviceOverview (REST API)
-- If you're building an API, you can create an endpoint:
// PUT: api/deviceoverview/{id}
[HttpPut("{id}")]
public IActionResult UpdateDeviceOverview(int id, [FromBody] DeviceOverview updatedDevice)
{
    var device = _context.DeviceOverview.FirstOrDefault(d => d.Id == id);

    if (device == null)
    {
        return NotFound();
    }

    device.Model = updatedDevice.Model; // Update model
    _context.SaveChanges();

    return Ok(device);
}

-- Insert sample SingleDevices with qr
INSERT INTO SingleDevice (deviceOverview_id, status, location, description, qr, is_archived)
VALUES 
(1, 1, 3, 'Dell XPS Laptop', 'QR12345', 0),  -- Available Dell Laptop in Room A
(1, 1, 3, 'Lenovo ThinkPad', 'QR67890', 0),  -- Available Lenovo Laptop in Room A
(2, 1, 6, 'HP EliteBook', 'QR11111', 0),    -- Available HP Laptop in Room B
(2, 1, 6, 'HP EliteBook', 'QR11n11', 0);    -- Available HP Laptop in Room B
INSERT INTO SingleDevice (deviceOverview_id, status, location, description, is_archived)
VALUES 
(1, 1, 3, 'fgdf',  0),  -- Available Dell Laptop in Room A
(1, 1, 3, 'fgdgs',  0),  -- Available Lenovo Laptop in Room A
(2, 1, 6, 'fdk',  0),    -- Available HP Laptop in Room B
(2, 1, 6, 'fdgdgdf', 0),    -- Available HP Laptop in Room B
(2, 1, 5, 'fdgdgdf', 0);    -- Available HP Laptop in Room B
-- insert monitors: 
INSERT INTO SingleDevice (deviceOverview_id, status, location, description, is_archived)
VALUES 
(7, 1, 3, 'fgdf',  0),  
(7, 1, 3, 'fgdgs',  0),  
(7, 1, 7, 'fdk',  0)   


-- Get all info about SingleDevices
SELECT 
    sd.id AS DeviceID, 
    sd.description AS DeviceDescription, 
    sd.qr AS QRCode, 
    sd.is_archived AS Archived, 
    do.model AS DeviceModel, 
	dt.type_name AS DeviceType,  -- Added Device type_name
    st.status_type AS Status, 
    l.room_id AS Room, 
	r.designation AS RoomName,
    l.cupboard_id AS Cupboard,
	c.designation AS CupboardName
FROM SingleDevice sd
JOIN DeviceOverview do ON sd.deviceOverview_id = do.id
JOIN DeviceType dt ON do.device_type = dt.id  -- Joining DeviceType to get type_name
JOIN StatusType st ON sd.status = st.id
JOIN Location l ON sd.location = l.id
JOIN LocationRoom r ON l.room_id = r.id
JOIN LocationCupboard c ON l.cupboard_id = c.id;




-- Insert sample Activities
INSERT INTO Activity (device_id, activity_type, user_id, start_date, end_date, created_at,notes, lifecycle_id)
VALUES 
('2D44C685-80CE-4617-A61E-26B0890802FA', 1, '17BCD416-CF2F-4F66-A85E-A478D0B0AA8E', '2024-01-01','2024-01-30', GETDATE(), 'Borrowed by Bob', NEWID()),
('BBF0EEBD-E5A8-46A5-9982-38E95B4339C6', 1, 'B71616AB-7EBD-46F6-8C99-FA156229B5CB', '2024-01-15','2024-02-05', GETDATE(), 'Borrowed by Charlie', NEWID());

-- OSB above code wont change SingleDevice status to 2 (Borrowed) as it is not updated in the above code, 
-- as BLL handles logic of statuschange after inserting a new Activity

-- this is just checking : update available_qty in DeviceOverview, and add 2 new logs after updating the status of SingleDevice
UPDATE SingleDevice
SET status = 3 -- Borrowed
WHERE id = '2D44C685-80CE-4617-A61E-26B0890802FA';

-- see Activities for a specific user:
SELECT 
    a.id AS ActivityID,
    a.device_id,
    d.model AS DeviceName,
    a.activity_type,
    a.start_date,
    a.end_date,
    a.created_at,
    a.notes,
    s.status AS DeviceStatus,
	u.email AS UserEmail
FROM Activity a
JOIN SingleDevice s ON a.device_id = s.id
JOIN DeviceOverview d ON s.deviceOverview_id = d.id
JOIN [User] u ON a.user_id = u.id  -- Join with Users table to get email
WHERE a.user_id = '17BCD416-CF2F-4F66-A85E-A478D0B0AA8E'
ORDER BY a.created_at DESC;
