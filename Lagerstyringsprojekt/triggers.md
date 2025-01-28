# Triggers

## Updating: qty and available-qty in DeviceOverview table

### trgUpdateQtyAndAvailable_qtyWhenSingleDeviceIsArchived

Stored procedure takes care of qty and available_qty update.  
When is_archived in SingleDevice Table is set to true or 1:  
>     qty --  
>     available_qty --  
Retrieve the corresponding DeviceOverview row using the SingleDevice.name (which corresponds to DeviceOverview.model) and device_type_id.


### trgUpdateAvailable_qtyUponSingleDeviceStatusChange

When status_type_id for a SingleDevice has been changed, update available_qty in DeviceOverview Table  
>    available_qty ++, when: Borrowed/Overdue/Unavailable -> Available
>      available_qty --, when: Available -> Borrowed/Unavailable 
  
## Sending and Logging Low stock Notification 

### trgLowStockLogAndNotification
This trigger monitors updates to the available_qty column in the DeviceOverview table. If the available_qty drops below 10, it will:

>   Log the Event into the Log table.  
>   Send a Notification (simulated as a PRINT statement or could call a stored procedure).

## Log All Create and Update in User, DeviceOverView and SingleDevice tables

## Monitor Overdue Devices
Monitor end_date for all Borrowed SingleDevice in SingleDevice Table

### trgMonitorOverdueDevicesAndSwitchDevideStatus
This trigger involves checking devices in the SingleDevice table where the end_date has passed but the device's status_type_id in the SingleDevice table is still Borrowed.  
Then change SingleDeviceStatus from Borrowed to Overdue.