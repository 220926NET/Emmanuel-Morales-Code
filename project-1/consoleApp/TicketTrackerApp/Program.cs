
using Models;


Requests requests = new Requests();
Form form = new Form();
InputValidator inputValidator = new InputValidator();
User? loggedInUser = null;

bool exit = false;
bool exitCreateUser = false;
bool loggedIn = false;


while (!exit && !loggedIn)
{
    Console.WriteLine("Welcome, Please choose an Option: ");
    Console.WriteLine("Press 1 For login.");
    Console.WriteLine("Press 2 to register a new acount.");
    Console.WriteLine("Press 3 to exit\n");
    string? userOption = Console.ReadLine();
    int? userOptionInt = isValidOptionInput(userOption, 3);


    switch (userOptionInt)
    {

        case 1:

            bool isLoggingIn = true;
            while (isLoggingIn)
            {
                Console.WriteLine("Please type your username.");
                string? userName = Console.ReadLine();
                bool isValidUsername = inputValidator.isValidString(userName);

                Console.WriteLine("Please type your password.");
                string? password = Console.ReadLine();
                bool isValidPassword = inputValidator.isValidString(password);

                if (!isValidUsername || !isValidPassword)
                {
                    Console.WriteLine("Please try again and type a valid username and password. Username and password must contain no null charcters");
                }
                else
                {

                    Login loginCredentials = new Login
                    {
                        UserName = userName!,
                        Password = password!
                    };

                    ResponseMessage<User> responseMessage = requests.postLogin(loginCredentials);
                    printMessage(responseMessage.message);
                    loggedInUser = responseMessage.data;
                    isLoggingIn = !responseMessage.success;
                    loggedIn = responseMessage.success;

                }


            }
            break;


        case 2:


            while (!exitCreateUser)
            {

                Console.WriteLine("Please type your name: ");
                string? name = Console.ReadLine();
                bool isValidName = inputValidator.isValidString(name);

                Console.WriteLine("Please type your username: ");
                string? userName = Console.ReadLine();
                bool isValidUserName = inputValidator.isValidString(userName);

                Console.WriteLine("Please type your password");
                string? password = Console.ReadLine();
                bool isValidPassword = inputValidator.isValidPassword(password);

                if (!isValidName || !isValidName || !isValidPassword)
                {
                    Console.WriteLine("Please ensure your name, username and password contain characters.");
                }
                else
                {

                    Login newLogin = new Login()
                    {
                        UserName = userName!,
                        Password = password!
                    };
                    User newUser = new User()
                    {
                        Name = name,
                        login = newLogin
                    };

                    ResponseMessage<string> response = requests.postCreateUser(newUser);
                    printMessage(response.message!);
                    exitCreateUser = response.success;
                }



            }
            exitCreateUser = false;
            break;

        case 3:
            printMessage("GoodBye");
            exit = true;
            break;
    }


}

printWelcomeMessage(loggedInUser!.Name!.ToUpper());
while (loggedInUser != null)
{

    {

        Console.WriteLine("Enter 0 to logout.");
        Console.WriteLine("Enter 1 to create a new ticket");
        Console.WriteLine("Enter 2 to logout");
        Console.WriteLine("Enter 3 to view all your ticket submissions. ");

        int options = 3;

        if (loggedInUser.IsManager)
        {
            Console.WriteLine("Enter 4 to view employee submissions.");
            options = 4;
        }

        string? userOption = Console.ReadLine();
        int? userOptionInt = isValidOptionInput(userOption, options);

        switch (userOptionInt)
        {
            case 0:
                loggedInUser = null;
                break;
            case 1:
                Console.WriteLine("Please type a description for your ticket.");
                string? description = Console.ReadLine();

                Console.WriteLine("Please type an Amount.");
                string? amountStr = Console.ReadLine();

                ResponseMessage<string> responseMessage = requests.postTicket(loggedInUser.Id, description, amountStr);
                printMessage(responseMessage.message);

                break;
            case 2:
                loggedIn = false;
                break;
            case 3:
                ResponseMessage<List<Ticket>> getTicketResponse = requests.getUserTickets(loggedInUser.Id);
                printMessage(getTicketResponse.message);
                if (getTicketResponse.success)
                {
                    foreach (Ticket ticket in getTicketResponse.data!)
                    {
                        printMessage(ticket.ToString());
                    }
                }
                break;
            case 4:
                ResponseMessage<List<Ticket>> pendingTicketsResponse = requests.getPendingTickets();
                if (pendingTicketsResponse.success)
                {
                    foreach (Ticket ticket in pendingTicketsResponse.data!)
                    {
                        printMessage(ticket.ToString());
                    }
                }
                else
                {
                    printErrorMessage(pendingTicketsResponse.message);
                }
                Console.WriteLine("Please enter the id followed by \"approve\" or \"deny\" to approve or deny a reimbursment ticket.");
                Console.WriteLine("Enter x to leave. \n ");
                string? managerInput = Console.ReadLine();

                if (managerInput != "x")
                {

                    int? employeeTicketId = null;
                    string? managerDecision = null;
                    bool mgrInputIsValid = inputValidator.isValidManagerChoice(managerInput);

                    if (mgrInputIsValid)
                    {
                        managerDecision = managerInput!.Substring(managerInput.IndexOf(" ") + 1).ToLower();
                        string employeeIdStr = managerInput.Substring(0, managerInput.IndexOf(" ")).ToLower();
                        employeeTicketId = int.Parse(employeeIdStr);

                        ResponseMessage<string> updateTicketResponse = requests.updateTicket((int)employeeTicketId, true);
                        printMessage(updateTicketResponse.message);
                    }



                }

                break;

        }




    }


}



static int getUserOption(string userOption)
{
    int userOptionInt;

    bool successParsingInt = int.TryParse(userOption, out userOptionInt);

    return userOptionInt;
}

static int? isValidOptionInput(string? userOption, int options)
{
    InputValidator inputValidator = new InputValidator();
    bool validUserOption = inputValidator.isValidOptionInput(userOption, options);
    int? userOptionInt = null;

    if (!validUserOption)
    {
        printErrorMessage("Please choose a valid option. ");

    }
    else
    {
        userOptionInt = int.Parse(userOption!);

    }
    return userOptionInt;

}

static void printErrorMessage(string message)
{
    Console.WriteLine("-------------error------------------------------");
    Console.WriteLine($"{message}");
    Console.WriteLine("------------------------------------------------");

}

static void printWelcomeMessage(string message)
{
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"\t\t WElCOME {message}");
    Console.WriteLine("------------------------------------------------");

}

static void printMessage(string message)
{
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"\t{message}");
    Console.WriteLine("------------------------------------------------");

}

static void printTicket(Ticket ticket)
{
    Console.WriteLine("-----------------------------------------------");
    Console.WriteLine(ticket.ToString());
    Console.WriteLine("-----------------------------------------------");
}