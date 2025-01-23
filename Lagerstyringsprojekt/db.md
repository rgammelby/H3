# Databaseskitse

Dette er et overblik over vores nuværende databaseskitse. Alle er velkomne til at komme med indvendinger/tilføjelser/rettelser-

## Tabeller

### DeviceOverview

DeviceOverview-tabellen dækker over alle typer af devices og dynamicaly opdaterer device antal ved hjælpe af triggers (after e.g. BorrowActivity and ReturnActivity, or DeviceStatus changed to Archived)
Den vil indeholde f.eks. flere forskellige slags monitors, keyboards osv. Hver af deres navne, billeder og kvantiteter vil kunne findes i denne tabel. 

|id|deviceType|model|availableCount|totalCount|admin|image|lastOrdered|
|---|---|---|---|---|---|---|---|
|#seq|3|ThinkVision 9000|42|90|admin1|base64|25/1/2025|

### SingleDevice

SingleDevice-tabellen er en oversigt over hvert individuelle device. Hvis der findes 3 stk. Lenovo Whatever keyboards, vil hver af dem have en entry i denne tabel. Deres `type` kalder ud til DeviceType-tabellen. Deres `location` kalder ud til den samlede `Location`-tabel. `lifecycle`- og `booking`-ID'erne er unikke for hvert enkelte enhed.

|id|name|type|description|status|location|qr|
|---|---|---|---|---|---|---|
|#seq|name|device_type_id|blalbalbla|status_type_id|location_id|idk|  

## DeviceType

|id|typeName|
|---|---|
|#seq|laptop|
||desktop|
||microfon set|
||monitor|
||server|
||router|
||switch|
||webCam|
||headset|
||keyboard|
||mousse|
||...|

# StatusType

|id|status_type|
|---|---|
|#seq|Available|
||Reserved|
||Not returned|
||Borrowed|
||Unavailable|
||Archived|

## ActivityHistory

|id|device_id|activity_type|start_date|end_date|created_at|notes|lifecycle_id|booking_id|
|---|---|---|---|---|---|---|---|---|
|#seq|213|1|i dag|om en uge|22-01-2025 09:15|Booket til afhentning i dag|li-cy-123|b-1234|

## ActivityType

|id|activity_type|
|---|---|
|1|Book|
||Borrow|
||Return|
||Extend|
||Cancel|

OVERVEJ: Om vi skal introducere nye ActivityTypes;
* Extend Overdue;
* Return Overdue.

Extend ActivityType.end_date

### User

User-tabellen er en oversigt over brugere i systemet, som har mulighed for at booke/låne devices. 

|id|first_name|last_name|email|telephone|activity|type|
|---|---|---|---|---|---|---|
|#seq|anne|petersen|ap@mail.dk|1234 5678|active/inactive|user/admin|

### SingleDeviceLender

SingleDeviceLender er en samletabel, som agerer samlet oversigt over hvilke brugere har booket/lånt hvilke devices.

|id|device_id|lender_id|
|---|---|---|

### LocationRoom

LocationRoom er en lille tabel, som indeholder alle de forskellige rumdesigneringer vi kan komme på.

|id|designation|
|---|---|

### LocationCupboard

LocationCupboard er ligeledes en lille tabel, som indeholder alle de forskellige skabsdesigneringer vi kan komme på.

|id|designation|
|---|---|

### Location

Location-tabellen er en tabel som samler rum- og skabsdesigneringer.

|id|room_id|cupboard_id|
|---|---|---|


## RequestStatus

|id|request_status|
|---|---|
|0|Pending|
||Approved|
||Denied|

### Request

Request-tabellen indeholder alle user requests, der kræver admin godkendelse (Book, Extend)
? more info needed for single request? For create  Book or Extend Activity?

|id|request_status|request_type|userId|deviceId|adminID|startDate|endDate|createdAt|note|
|---|---|---|---|---|---|---|---|---|---|
|123|1|book/extend|456|545|-|25/1-2025|6/2-2025|23/1-2025|-|

### Log

Log-tabellen indeholder logs, og vil løbende bliver opdateret/fyldt, i takt med at programmets processer bliver brugt.

|id|log_type|log_message|
|---|---|---|
