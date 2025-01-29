### SQL Server
For Microsoft SQL Server, you can use SQL Server Agent to create the scheduled job.

Steps to Create a Scheduled Job in SQL Server:
## 1. Enable SQL Server Agent:

Ensure that SQL Server Agent is running in the SQL Server Management Studio (SSMS).
If it's not running, start it from the "SQL Server Configuration Manager."

## 2. Open SQL Server Agent:

In SSMS, expand the SQL Server Agent node in the Object Explorer.

## 3. Create a New Job:

Right-click Jobs under SQL Server Agent and select New Job.

## 4. Define the Job:

### 4.1 General Tab:  
Enter a name for the job, e.g., CheckOverdueDevices.  

### 4.2 Steps Tab:  
Add a new step.  
Set the type to Transact-SQL script (T-SQL).  
Write the query to check overdue devices:   

 |1 Available|2 Overdue|3 Borrowed|4 Unavailable

>-- Update overdue devices  
UPDATE sd  
SET status = 2 -- Change to Overdue status  
FROM SingleDevice sd  
INNER JOIN Activity a ON sd.id = a.device_id  
WHERE   
    sd.status = 3  
    AND CAST(a.end_date AS DATE) = CAST(GETDATE() - 1 AS DATE);  

## 5. Schedule the Job:
Go to the Schedules Tab and click New.  
Define the schedule:  
>   Name: Run Daily at 1 AM  
Frequency: Daily.  
Time: 1:00 AM.  

## 6. Save and Start:  

Click OK to save the job.  
The job will now run daily at 1:00 AM.
