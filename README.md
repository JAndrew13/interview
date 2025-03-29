<h3 align="center">iCEV - Technical Interview Project</h3>

<!-- ABOUT THE PROJECT -->
## About The Project
This is a simple demo application to show usage of basic razor pages and how they inteact with transient SQLite tables stored in memory. 
This readme provides a simple walkthrough on how to setup and build the project, along with a few improvements that are required for ideal functionality.

The original (unmodified) repo can be found [here](https://github.com/michaelpaulus/interview)
<!-- GETTING STARTED -->
## Getting Started

### Prerequisites
- Visual Studio or similar IDE
- Windows 10
- .Net 8 SDK 
  - Can be downloaded [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or by running the following command in your developer console:
`winget install --id Microsoft.DotNet.SDK.8`
To make sure the SDK has been installed correctly, run `dotnet --list-sdks` , you should see the package
"8.0.407 [C:\Program Files\dotnet\sdk]" listed in the response.

## Installation Steps
1. Clone the repo
`git clone <https://github.com/michaelpaulus/interview.git`>
2. Open the project in Visual Studio and make sure all the required NuGet packages are installed by running `dotnet restore` in your developer console.
Alternatively, in Visual Studio, go to **Tools -> NuGet Package Manager -> Package Manager Console** and run the following commands.
    - `Update-Package -reinstall`
3. In your AdventureWorks.csproj file, ensure that your ItemGroup contains the following package references, and that the *Installed Packages* listed in your **NuGet Package Manager** match the correct version numbers. The installed packages can be found at **Tools -> NuGet Package Manager -> Manage NuGet packages for solution... -> Installed**
    
    ```csharp
    <PackageReference Include="Microsoft.Data.Sqlite.Core" Version="8.0.4" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.4" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.4" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" Version="8.0.10" />
    <PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="2.1.6" />
    
    ```
    
4. Open the projects program.cs file and add the following code just after the initial using statements.
    
    ```csharp
    // Initialize SQLite Batteries to load native libraries.
    SQLitePCL.Batteries_V2.Init();
    
    ```
    
5. At this point, your should see that after running the following commands, the build succeeds and opens your default web browser.

![image](https://github.com/user-attachments/assets/d9089bbc-005e-4f52-8a5d-84366d25766a)


> **NOTE:**  In some cases,  you may run into an issue that mentions the web address “TCP 127.0.0.1:5000 ” is already in use. If this happens, youll just need to open powershell and run the following commands to view and clear the connection. 
`netstat -ano | findstr :5000`
> 
> 
> ![image](https://github.com/user-attachments/assets/a2fe295a-8cc9-4797-8ae8-29354c68c879)
> 
> To clear the connection, enter `taskkill /PID 39856 /F`  and then try running the project again.
> 
> ![image](https://github.com/user-attachments/assets/26be8259-4792-4409-8a7d-773ca4695915)
> 

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Improvements

Great! Now that the app is running, we can see that there is a `Home Page` and an `Add Users` page available to us, however when testing the functionality out in the *Add Users* page, it seems like were successfully generating new users, but this is not reflected in the `User Count` display on the *home page*! To fix this, we will need to tune this code up a bit. 

### **UserService.cs:**

Our user database is *transient*, so let’s verify that the users are actually getting added to the database by adding a new method in the user service to display a visual list of all the users. 

```csharp
    public List<UserModel> GetAllUsers()
    {
        return _context.Users.ToList();
    }
```

### **Index.cshtml.cs:**

We need to add a new **User List** property to our view model to hold these new values and then modify the Index model’s `OnGet()` method to fetch the list correctly.

```csharp
    public List<UserModel> Users { get; set; } = new List<UserModel>();

    public void OnGet()
    {
        Users = _userService.GetAllUsers();  // Fetch all users
        Count = _userService.GetUserCount();
    }
```

### **Index.cshtml:**

Now we need to add some logic to the `home page` (Index) to display our new list. Adding this unordered list element just below our existing *model.Count* should be fine.

```csharp
<ul>
    @foreach (var user in Model.Users)
    {
        <li>@user.Email</li>
    }
</ul>
```

![image](https://github.com/user-attachments/assets/fdb7cde3-9bea-4e77-8b7b-68f99330474f)

Success! We’re seeing our new users  on the homepage. This means our transient table *is working -*but the counter isn’t right.  Lets fix that too. 

Back in the `UserService.cs` class, we’ve got a few basic “Crud” operations available to us. 

- `AddUsers()` → Generates 5 new users and adds them to the database
- `GetUserCount()` → Returns the total number of users
- `GetUsers()` → Returns a list of all users.

We know the `GetUsers()` list is working, the `AddUsers()`  *seems* to be working, but lets zoom into its logic and break it down.

1. On call, accept a **count** integer, telling the method how many users to create.
2. Iterate through each number within the range of 0 to **count**
    1. Create a new user object with a generated GUID for the ID and Email
    2. Add the new user to the database table.
3. After (**count**) new users have been created and added to the database, Save the database context.

![image](https://github.com/user-attachments/assets/3d5ce80f-aba9-4ee8-b595-00a3580b6386)

While this may seem fine, we’re actually missing a step here! Even though we are updating our database, the **user count** is being stored in memory cache the first time the method is called. Then - when we add new users, the cache is not updated. Simple!

There’s a few ways we can fix this..

**Option 1:**

Modify the `AddUsers()` method to update the `memoryCache` after it completes a *Save* operation

```csharp
_context.SaveChanges();

var userCount = _context.Users.Count();
_memoryCache.Set("USER_COUNT", userCount);
```

**Option 2:** 

Invalidate the cache so that it gets refreshed in the next count request by adding this line after the `AddUsers()`  save operation. 

```csharp
_memoryCache.Remove("USER_COUNT");
```

Option 3: 

Update the cache updating logic in the `GetUserCount()` so that it always refreshes the `userCount` value before it returns the result. Depending on the use case I think this might be the best approach because it keeps the logic directly in the method, ensuring the logic fully independent of the rest of the code.

We can do this by simply removing the if/else block within this method. 

```csharp
    public int GetUserCount()
    {
        var userCount = _context.Users.Count();
        _memoryCache.Set("USER_COUNT", userCount);
        return userCount;
    }
```

With that out of the way,  we can see that the web app functionality now behaves exactly as expected!

![image](https://github.com/user-attachments/assets/2b23ddc2-07ef-49a2-9c8f-ed3b73cbf5d4)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
