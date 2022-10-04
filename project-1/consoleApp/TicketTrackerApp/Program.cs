// See https://aka.ms/new-console-template for more information


Form form = new Form();
Pages pages = new Pages();
// TODO // ASK user for Login, register or Exit

bool exit = false;
bool exitCreateUser = false;
bool loggedIn = false;
bool exitLogin = false;
bool isHomePage = true;
bool logout = false;

while (!exit)
{
    while (isHomePage)
    {
        pages.getHomePage();
        string userOption = Console.ReadLine();
        int userOptionInt = getUserOption(userOption);
        if (userOptionInt == 0)
        {
            while (!loggedIn && !exitLogin)
            {
                //Check if user may login
                Console.WriteLine("Please type your username:");
                string userName = Console.ReadLine();
                Console.WriteLine("Please type your password");
                string password = Console.ReadLine();

                Login loginCredentials = new Login
                {
                    UserName = userName,
                    Password = password
                };

                loggedIn = form.login(loginCredentials) == 1 ? true : false;
                isHomePage = !loggedIn;
            }

            if (userOptionInt == 1)
            {
                while (!exitCreateUser)
                {
                    Console.WriteLine("Please type your name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Please type your username: ");
                    string userName = Console.ReadLine();
                    Console.WriteLine("Please type your password");
                    string password = Console.ReadLine();

                    Login newLogin = new Login()
                    {
                        UserName = userName,
                        Password = password
                    };

                    User newUser = new User()
                    {
                        Name = name,
                        login = newLogin
                    };

                    if (form.postCreateUser(newUser) == 1)
                    {
                        exitCreateUser = true;
                    }
                    else
                    {
                        Console.WriteLine("Sorry that username is taken, please try again.");
                    };

                }

            }

            if (userOptionInt == 2)
            {

                exit = true;

            }




            // }


        }
    }


    if (loggedIn)
    {
        Console.WriteLine(@"
        Welcome to your ticket tracker home page!
        Please type 1 to create a new reimbursment ticket. 
        Please type 2 to logout.");
        Console.ReadLine();
    }

}


// TODO create a new user account if user chooses option 1 






static int getUserOption(string userOption)
{
    int userOptionInt;

    bool successParsingInt = int.TryParse(userOption, out userOptionInt);

    return userOptionInt;
}