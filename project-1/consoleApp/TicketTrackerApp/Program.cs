


Form form = new Form();
InputValidator inputValidator = new InputValidator();

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

                    loggedIn = form.login(loginCredentials) == 1 ? true : false;
                    isLoggingIn = false;
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

                    // TODO check if username is available before submitting to create user

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

                    if (form.postCreateUser(newUser))
                    {
                        exitCreateUser = true;
                    }
                    else
                    {
                        Console.WriteLine("Sorry that username is taken, please try again.");
                    };

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

/*
   Ensure user form is updated with pending tickets and updates user id; 
*/

if (loggedIn)
{
    await form.getUserReq();
    await form.setPendingTickets();
    //TODO fix set tickets by Id
    //Bug: awaiting causes newtwon parse error
    printWelcomeMessage(form.getUser().Name!.ToUpper());
}


while (loggedIn)
{

    {


        Console.WriteLine("Please type 1 to create a new ticket");
        Console.WriteLine("Please type 2 to logout");
        Console.WriteLine("Please type 3 to view all your ticket submissions. ");

        int options = 3;

        if (form.getUser().IsManager)
        {
            Console.WriteLine("Please type 4 to view employee submissions.");
            options = 4;
        }

        string? userOption = Console.ReadLine();
        int? userOptionInt = isValidOptionInput(userOption, options);

        switch (userOptionInt)
        {
            case 1:
                Console.WriteLine("Please type a description for your ticket.");
                string? description = Console.ReadLine();
                bool isValidDescription = inputValidator.isValidDescriptioon(description);

                Console.WriteLine("Please type an Amount.");
                string? amountStr = Console.ReadLine();
                bool isValidAmount = inputValidator.isValidAmount(amountStr);
                int amount = 0;

                if (isValidAmount)
                {
                    amount = int.Parse(amountStr!);
                }

                if (!isValidAmount || !isValidDescription)
                {
                    printErrorMessage("Please ensure Amount contains only numbers and Description contains characters!");
                }
                else
                {
                    Ticket ticket = new Ticket(description!, amount!, form.getUserId());
                    bool postSuccess = form.postTicket(ticket) == 1 ? true : false;
                    if (!postSuccess)
                    {
                        printErrorMessage("Error creating ticket, please try again later.");
                    }
                    else
                    {
                        printMessage("Succesfully created ticket.");
                    }
                }
                break;
            case 2:
                loggedIn = false;
                break;
            case 3:
                await form.setTicketsById();
                if (form.getSubmittedTickets() != null)
                {
                    foreach (Ticket ticket in form.getSubmittedTickets())
                    {
                        printMessage(ticket.ToString());
                    }
                }
                else
                {

                    printMessage("You currently have no tickets.");
                }
                break;
            case 4:
                bool isViewingTickets = true;
                while (isViewingTickets)
                {
                    foreach (Ticket ticket in form.getPendingTicket())
                    {
                        printTicket(ticket);

                    }

                    Console.WriteLine("Please enter the id followed by \"approve\" or \"deny\" to approve or deny a reimbursment ticket.");
                    Console.WriteLine("Enter x to leave. \n ");

                    string? managerInput = Console.ReadLine();

                    if (managerInput == "x")
                    {
                        isViewingTickets = false;
                    }
                    else
                    {
                        //TODO implement validation for choosing tickets 
                        int? employeeTicketId = null;
                        string? managerDecision = null;
                        bool mgrInputIsValid = inputValidator.isValidManagerChoice(managerInput);

                        if (mgrInputIsValid)
                        {
                            managerDecision = managerInput!.Substring(managerInput.IndexOf(" ") + 1).ToLower();
                            string employeeIdStr = managerInput.Substring(0, managerInput.IndexOf(" ")).ToLower();
                            employeeTicketId = int.Parse(employeeIdStr);

                            bool isSuccess = form.updateTicket(employeeTicketId.Value, true);
                            if (isSuccess)
                            {
                                Console.WriteLine("succesfully approved ticket");
                                await form.setPendingTickets();
                            }

                        }



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