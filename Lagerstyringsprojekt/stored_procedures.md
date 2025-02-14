[[_TOC_]]

# Stored Procedures

## User-oriented procedures

### UpdateUserStringValues

This procedure assumes that both user.active (active/inactive) and user.type (admin/user) are string values. 

The procedure should be amended if user activity status and user types are moved into a separate table. 

### CreateUserStringValues

-//- for this procedure. 'CreateUser' procedure assuming IDs rather than string values has already been created.

### CreateUser

This procedure assumes external IDs rather than string value parameters for user creation. 

### GetSingleUser

Gets a single user by their unique user ID. 

### GetAllUsers

Gets all entries in the User table regardless of activity status. 

### DisableUser (if UserActivity table)

Disables a user if they are not already disabled. Assumes user activity status is external int value. 

### DisableUser (if NO UserActivity table)

Disables a user if they are not already disabled. Assumes user activity is stored on the user as a string value. 

## Activity-oriented procedures

### CreateActivity

Creates an activity record for a device. Takes all parameters except for `created_on`, which is generated automatically. 

### GetAllActivities

Gets every single activity record.

### GetLifecycle

Gets all activity records for a specific lifecycle_id. 

## Device type procedures

### UpdateDeviceType

Updates an existing device type. Takes all parameters.

Assumes that images are stored in DB as base64.

### CreateDevice

Creates a new device. Must belong to an existing device type (keyboard, monitor etc.).

### CreateDeviceType

Must create new device type (keyboard, monitor etc.).

### GetAllDevices (type)

Gets every entry in the Device table. 

## Single device procedures

### ExtendLoan

Extends a user's loan by one week. 

Takes `lifecycle_id`, `device_id` and `activity_type` as parameters.

Creates a new record in the Activity table with an increased `end_date`, and 'Extend' `activity_type`.

### DisableDevice

Sets `is_archived` bool of SingleDevice to `true` if it isn't already.

### CreateSingleDevice

Creates a new SingleDevice record. 

Parameters:

* Takes `device_type` parameter, getting both `name` and `device_type` from the Device table. 
* Takes `device_status` parameter, ID from DeviceStatus table.
* Takes `device_location` parameter, ID from Location table. 
* Takes `device_description` parameter; written note, VARCHAR(MAX).
* Takes `device_qr` parameter; currently also stored as VARCHAR(MAX) pending clarification on QR format. 

### GetFilteredDevices

Gets all devices of a specific `device_type` passed as a parameter. 

Currently accepts string value `device_type`; can be amended to accept INT id from Device table. 

### GetDeviceById

Retrieves single device by its ID. 

### GetAllSingleDevices

Gets a collection of every record in the SingleDevice table.

### GetDeviceByName

Gets a SingleDevice by its Device name. 