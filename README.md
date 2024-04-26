Objective: create a Data Base for the front end of Court Manager

Requirements:

- Login/CreateAccount 
- Post 
- Programs/Class 

Optional:
- Email for notifications and in app -(Maybe)

Controllers / folder 
    - UserController / file.cs 
        - Create user / endpoint | C 
        - Login user / endpoint | R 
        - Update user / endpoint | U 
        - Delete user / endpoint | D

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
        -  GetUserById / function
        -  UpdateUserStatus/function| U
    
    -  PasswordService / file.cs
        -  Hash our Password (Hash and Salt)
        -  Verification Hash

Models / folder
    - UserModel / file.cs
        - int      ID
        - string   username
        - string   Salt
        - string   Hash
        - string   funFact
        - string   Birthday
        - string   email
        - string   Real Name
        - string   Sports
        - string   Programs

    -EventModel / (Model for Each Event Item) .cs
        - int ID
        - int UserID
        - string EventName
        - int ProgramID
        - int startTime
        - int endTime
        - bool isPublished
        - bool isDeleted
    
    -ProgramModel / file
        - int ProgramID 
        - int AdminID 
        - int CoachID 
        - int GenUserID 
        - string ProgramName 
        - string ProgramSport 
        - int EventID 

    -AdminModel / file
        - int ID
        - int UserID
        - int? ProgramID
    -CoachModel / file
        - int ID
        - int UserID
        - int? ProgramID
    -GenUserModel / file
        - int ID
        - int UserID
        - int? ProgramID
    -DTOs/ folder
        - LoginDTO / file.cs
            string username
            string password

        - CreateAccountDTO / file
            - int ID 
            - string Username
            - string Password
            - string Email
            - string Real Name

        - PasswordDTO / file
            - string Salt
            - string Hash
        
        - UserDTO / file
            - int UserID
            - string Username 
            - string RealName 
            - string Programs 
            - string Birthday 
            - string FunFact 
            - bool IsAdmin 
            - bool IsCoach 
            - bool IsUser
        
        -UseridDTO / file
            - int Id
            - string Username
            - string RealName
            - string Programs
            - string Birthday
            - string FunFact
            - bool IsAdmin
            - bool IsCoach
            - bool IsUser


