# User stories

Dette er en samling af user stories som vi bruger til at planlægge hvilken funktionalitet vi har behov for at implementere.

# User management

## Story - Profile Creation

As a user, I want to create a profile with my personal details (e.g., name, email, and password) via the website so that I can securely access my account. 

### Acceptance criteria

 * User is able to access the profile creation page
 * User is able to input relevant, obligatory data
 * User creation successfully creates a user profile in the database
 * User is informed of successful user creation

 ## Story - Profile Login

 As a user, I need to be able to log in to my profile, so I can book equipment and track the status and history of my loans.

 ### Acceptance criteria

 * User is able to successfully login
 * User is able to access their loan history
 * User has permission to book and loan devices

 ## Story - Update Profile Information

As a user, I need to be able to update my profile information to ensure its accuracy

 ### Acceptance criteria

 * User is able to access their profile page
 * User has access to input fields containing relevant information
 * User is able to edit and save changes to input fields containing relevant information

 ## Story - Book Equipment

As a user, I need to be able to book equipment via the website. 

 ### Acceptance criteria
 * User is able to access the device page and enter DeviceType they need
 * User has access to input fields containing relevant information: startDate and endDate
 * User is able to see a list of available devices
 * User is able to pick one and send Book Request, waiting for confirmation from an Admin

## Story - Send Extend Request

As a user, I need to be able to send request to extend a current borrow.

 ### Acceptance criteria

 * User is able to access their borrowing page
 * User had a valid ongoing borrow
 * User has access to input fields for new return date
 * User is able to save changes to input fields containing relevant information
 * A request will be sent to admin, waniting to be confirmed

 ## Story - Cancle a booking

 As a user, I need to be able to cancle a future booking, without needing admin confirmation

 ### Acceptance criteria

 * User is able to access their borrowing page
 * User had a valid future booking (being confirmed by an admin)
 * User can click a cancle button 
 * User is able to confirm cancellation

 ## Story - Search for Equipment by Device Name

 As a user, I want to be able to search for specific devices by their name, so that I can find the device I would like to loan.

 ### Acceptance criteria

 * User is able to access a search bar and input the name of the device they are looking for
 * Upon executing the search, a call should be made to the database returning the relevant devices
 * The search query should also return partial matches
 * Seach query responses should be displayed on a search page

 ## Story - Filter by Device Type

 As a user, I need to be able to view collections of devices by their type (e.g., monitors, keyboards etc.)

 ### Acceptance criteria

 * User is able to select a filter on the Devices page, deciding which device types are displayed
 * Database queries are able to return collections of specific devices filtered by device type

 # Admin requirements

 ## Story - Device Creation and Deletion

 As an admin, I must be able to create new devices and delete existing devices on the site to keep inventory up-to-date

 ### Acceptance criteria

 * Admin is able to access the device handling interface
 * Site is able to send a database query for device creation and deletion 

 ## Story - Update Device Status

 As an admin, I must be able to update device status upon loan/reservation in order to manage loans and resolve conflicts

 ### Acceptance criteria

 * Admin is able to access the device handling interface
 * Admin is able to change the status of existing devices

 ## Story - Device Overview

 As an admin, I need to be able to see a complete collection of all devices

 ### Acceptance criteria

 * Admin is able to access device summary page
 * Site is able to retrieve complete collection of devices from the database

## Story - User Overview

 As an admin, I need to be able to see a list of all users

 ### Acceptance criteria

 * Admin is able to access user admin page
 * Site is able to retrieve complete list of users from the database


## Story - UserActivity Overview

 As an admin, I need to be able to see a list of all users' activities

 ### Acceptance criteria

 * Admin is able to access user admin page
 * Site is able to retrieve complete list of users' activities from the database

## Story - Confirm User Request: Book

 As an admin, I need to be able to confirm Book Request from user

 ### Acceptance criteria

 * Admin is able to access user/Activity admin page
 * Admin is able to confirm user's Book request
 * When date is starting date of the booking, a helper class automatically set deviceStatus from Available to Reserved, lock up the device for the user
   
## Story - Confirm User Request: Extend

 As an admin, I need to be able to confirm Extend Request from user

 ### Acceptance criteria

 * Admin is able to access user/Activity admin page
 * User who sent Extend Request has a valid ongoing Borrow activity
 * Admin is able to confirm user's extend request
 * The recent Borrow Activity's endDate will be updated to the new endDate
   
## Story - Admin Borrow Immediately A Device for A User

 As an admin, I need to be able to borrow a device to a user without a Booking

 ### Acceptance criteria

 * Admin is able to access Activity admin page
 * Admin is able to create a Borrow Activity(stating date is now) without needing a Confirmation
 * Admin is able to enter info: lenders's username and return date, then POST to db
   
