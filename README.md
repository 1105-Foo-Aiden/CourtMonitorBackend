Objective: create a DB for the front end of Court Manager

Requirements:

- Login/CreateAccount 
- Post 
- Programs/Class 
- Email for notifications and in app

Pages: 
    - Home
        - Calandar with dates and events on each day in a 5 day range 
        - Program dropdown up top

    - Profile (All strings)
        - Name 
        - Program(s)
        - Sport(s)
        - Eamil (optional)
        - Birthday (optional)
        - Fun Fact (optional)
        - Image(optional)

Controllers / folder 
    - UserController / file.cs 
        - Create user / endpoint | C 
        - Login user / endpoint | R 
        - Update user / endpoint | U 
        - Delete user / endpoint | D

    - CalendarController / file.cs
        - Updating Calendar / U
        - Updating Days / U
        - Get Calendar Events
        - Get Day Events

    - EventController / file.cs
        - Creating Event | C
        - Seeing  Events | R
        - Updating Event | U
        - Deleting Event | D

Servics / folder
    - Context / folder
        - Data Context / file.cs

    - EventService / file.cs
        -  Create Event / fucntion
        -  Delete Event / fucntion
        -  Update Event / fucntion
        -  Get Event / function
        - Get Calendar / function

    - UserService / file.cs
        -  Create user /function   | C
        -  Login user / function   | R
        -  Update user / function  | U 
        -  Delete user / function  | D
        -  GetuserByUserName / function
    
    -  PasswordService / file.cs
        -  Hash our Password (Hash and Salt)
        -  Verification Hash

Models / folder
    - UserModel / file.cs
        int      ID
        string   username
        string   Salt
        string   Hash
        string   funFact
        string   Birthday
        string[] Sports
        string[] Programs

    -EventModal / (Model for Each Event Item) .cs
        int ID
        int UserID
        string EventName
        string Summary
        string Program
        int startTime
        int endTime
        bool isPublished
        bool isDeleted
    
    -DTOs/ folder
        - LoginDTO / file.cs
            string username
            string password

        - CreateAccountDTO / file
            - int ID = 0
            - ID ++
            - string Username
            - string Password
        -PasswordDTO / file
            - string Salt
            - string Hash