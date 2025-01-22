# Databaseskitse

Dette er et overblik over vores nuværende databaseskitse. Alle er velkomne til at komme med indvendinger/tilføjelser/rettelser-

## Tabeller

### DeviceType

DeviceType-tabellen dækker over specifikke typer af devices. Den vil indeholde f.eks. flere forskellige slags monitors, keyboards osv. Hver af deres navne, billeder og kvantiteter vil kunne findes i denne tabel. 

|id|name|type|qty|image|lifecycle_id|booking_id
|---|---|---|---|---|---|---|
|#seq|ThinkVision 9000|monitor|#|base64|asdf|fdsqa|

### SingleDevice

SingleDevice-tabellen er en oversigt over hvert individuelle device. Hvis der findes 3 stk. Lenovo Whatever keyboards, vil hver af dem have en entry i denne tabel. Deres `type` kalder ud til DeviceType-tabellen. Deres `location` kalder ud til den samlede `Location`-tabel. `lifecycle`- og `booking`-ID'erne er unikke for hvert enkelte enhed.

|id|type|status|location|qr|image|lending_period|
|---|---|---|---|---|---|---|
|#seq|device_type_id|status_type_id|location_id|idk|image_id|1 week from current date?|

# StatusType

|id|status_type|
|---|---|
|#seq|Loaned|
||Available|
||Reserved|
||Processing|
||Not returned|
||Borrowed|

# Activity
|id|activity|
|---|---|
|#seq|Book|
||Borrow|
||Return|
||Extend|
||Cancel|

### Lender

Lender-tabellen er en oversigt over brugere i systemet, som har mulighed for at booke/låne devices. 

|id|first_name|last_name|email|telephone|
|---|---|---|---|---|
|#seq|anne|petersen|ap@mail.dk|1234 5678|

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

### Log

Log-tabellen indeholder logs, og vil løbende bliver opdateret/fyldt, i takt med at programmets processer bliver brugt.

|id|log_type|log_message|
|---|---|---|