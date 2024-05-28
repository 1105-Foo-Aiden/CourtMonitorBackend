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
        - Get All Events | R
        - Get Events By Program | R
        - Deleting Event | D
    
    - ProgramController / file
        - Get Admin By Id
        - Get Users By Program Name
        - Get All Programs
        - Get Program by Program ID
        - Get Program by Program Name
        - Get Program by Sport
        - Create Program
        - Add User To Program
        - Move User To Another Status
        - Delete Program
        - Remove User From Program

Servics / folder
    - Context / folder
        - Data Context / file.cs

    - EventService / file.cs
        -  Create Event / fucntion
        -  Delete Event / fucntion
        - Get Program By ID / function
        - Get All Events/ function
        - Get Event By Id / function
        - Get Events By Program Name / function

    - UserService / file.cs
        -  Add user /function | C
        -  Login / function   | R
        -  Update user / function  | U 
        -  Delete user / function  | D
        -  GetuserByUserName / function
        -  GetUserById / function
        -  UpdateUserStatus/function| U
        -  Does Email Exist / function
        -  Hash Password / function
        -  Verify Users Password / function
        -  Search User By Username / function
        -  Get User Id by Username / function
        -  Get All Users / function
        -  Reset Password / function

    - ProgramService / file.cs
        - Create Program / function
        - Get Admin By Id / function
        - Get All Programs / function
        - Get All Events By Program / function
        - Get User By Username / function
        - Delete Program / function
        - Get All Programs By Sport / function
        - Add User To Program / function
        - Get User By ID / function
        - Get Usernames By Program / function
        - Remove User from Program / function
        - Move User In Program / function

Models / folder
    - UserModel / file.cs
        - int    ID
        - string username
        - string Salt
        - string Hash
        - string funFact
        - string Birthday
        - string email
        - string Real Name
        - string Sports
        - string Programs

    -EventModel / file
        - int id
        - string Title
        - string Color
        - bool AllDay
        - int ProgramID
        - string Start
        - string End
    
    -ProgramModel / file
        - int ProgramID 
        - string AdminID 
        - string CoachID 
        - string GenUserID 
        - string ProgramName 
        - string ProgramSport 

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

        -AddUserToProgramDTO / file
            - int ProgramID
            - int UserId
            - string Status
        
        -ProgramUserDTO / file
            - string Status
            - string UserName
            - string RealName
            - string? Image

        - LoginDTO / file.cs
            string username
            string password

        -ProgramDTO / file
            - string ProgramName
            - string ProgramSport
            - string? Description
            - string AdminID

        - CreateAccountDTO / file
            - int ID 
            - string Username
            - string Password
            - string Email
            - string Real Name

        - ResetPasswordDTO / file
            - string email
            - string newPasword 

        - PasswordDTO / file
            - string Salt
            - string Hash
        
        - UserDTO / file
            - int UserID
            - string Username 
            - string RealName 
            - string? Programs 
            - string? Birthday 
            - string? FunFact 
            - string? Image

        - UpdateUserDTO / file
            -string UserName
            -string RealName
            -string? Image
            -string? Birthday
            -string? Funfact
            -string Email
        
        - UseridDTO / file
            - int Id
            - string Username
            - string RealName
            - string Programs
            - string Birthday
            - string FunFact


