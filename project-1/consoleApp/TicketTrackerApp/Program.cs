
using Models;


Requests requests = new Requests();
Client form = new Client();
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

        Console.WriteLine("Enter 1 to logout.");
        Console.WriteLine("Enter 2 to create a new ticket");
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

            case 1:
                loggedInUser = null;
                break;

            case 2:
                Console.WriteLine("Please type a description for your ticket.");
                string? description = Console.ReadLine();

                Console.WriteLine("Please type an Amount.");
                string? amountStr = Console.ReadLine();

                bool isValidDescription = inputValidator.isValidDescriptioon(description);
                bool isValidAmount = inputValidator.isValidAmount(amountStr);
                if (!isValidAmount || !isValidDescription)
                {
                    Console.WriteLine("Please ensure Amount contains only numbers and Description contains characters!");

                }
                else
                {
                    int amountInt = int.Parse(amountStr);
                    Ticket newTicket = new Ticket(description, amountInt, loggedInUser.Id);

                    ResponseMessage<string> responseMessage = requests.postTicket(newTicket);
                    printMessage(responseMessage.message);

                }


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

                    bool mgrInputIsValid = inputValidator.isValidManagerChoice(managerInput);

                    if (mgrInputIsValid)
                    {
                        string managerDecision = managerInput!.Substring(managerInput.IndexOf(" ") + 1).ToLower();

                        int employeeTicketId = int.Parse(managerInput.Substring(0, managerInput.IndexOf(" ")).ToLower());

                        ResponseMessage<string> updateTicketResponse = requests.updateTicket((int)employeeTicketId, managerDecision);
                        printMessage(updateTicketResponse.message);
                    }

                }

                break;

        }




    }


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