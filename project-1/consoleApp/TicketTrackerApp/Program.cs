
using Models;


Requests requests = new Requests();
Prompts prompts = new Prompts();

InputValidator inputValidator = new InputValidator();
User? loggedInUser = null;

bool exit = false;
bool exitCreateUser = false;
bool loggedIn = false;


while (!exit && !loggedIn)
{

    int userOptionInt = prompts.WelcomePrompt();

    if (userOptionInt > 3 || userOptionInt < 1) Message.printMessage("Please choose a valid option. ");

    switch (userOptionInt)
    {
        case 1:
            bool isLoggingIn = true;
            while (isLoggingIn)
            {
                String? userName = null;
                string? password = null;

                bool registeredSuccessfully = prompts.RegisterUserPrompt(out userName, out password);

                if (registeredSuccessfully)
                {
                    Login loginCredentials = new Login
                    {
                        UserName = userName!,
                        Password = password!
                    };

                    ResponseMessage<User> postLoginResponse = requests.PostLogin(loginCredentials);

                    Message.printMessage(postLoginResponse.message);
                    loggedInUser = postLoginResponse.data;
                    isLoggingIn = !postLoginResponse.success;
                    loggedIn = postLoginResponse.success;

                }


            }
            break;

        case 2:


            while (!exitCreateUser)
            {
                string? name = null;
                string? userName = null;
                string? password = null;

                bool inputsAreValid = prompts.CreateUserPrompt(out name, out userName, out password);

                if (!inputsAreValid)
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

                    ResponseMessage<string> CreateUserResponse = requests.PostCreateUser(newUser);
                    Message.printMessage(CreateUserResponse.message!);
                    exitCreateUser = CreateUserResponse.success;
                }


            }
            exitCreateUser = false;
            break;

        case 3:
            Message.printMessage("GoodBye");
            exit = true;
            break;
    }


}

if (loggedInUser != null) Message.printWelcomeMessage(loggedInUser!.Name!.ToUpper());
while (loggedInUser != null)
{

    {
        int userOptionInt = prompts.HomePrompt(loggedInUser.IsManager);

        if (userOptionInt < 0) Message.printErrorMessage("Please enter a valid option");

        switch (userOptionInt)
        {

            case 1:
                loggedInUser = null;
                break;

            case 2:
                string? amountStr = null;
                string? description = null;
                bool successfullyCreatedTicket = prompts.CreateTicketPrompt(out amountStr, out description);

                if (!successfullyCreatedTicket)
                {
                    Console.WriteLine("Please ensure Amount contains only numbers and Description contains characters!");

                }
                else
                {
                    int amountInt = int.Parse(amountStr);
                    Ticket newTicket = new Ticket(description, amountInt, loggedInUser.Id);
                    ResponseMessage<string> responseMessage = requests.PostTicket(newTicket);
                    Message.printMessage(responseMessage.message);

                }


                break;

            case 3:
                ResponseMessage<List<Ticket>> getTicketResponse = requests.GetUserTickets(loggedInUser.Id);
                Message.printMessage(getTicketResponse.message);
                if (getTicketResponse.success)
                {
                    foreach (Ticket ticket in getTicketResponse.data!)
                    {
                        Message.printMessage(ticket.ToString());
                    }
                }
                break;
            case 4:
                ResponseMessage<List<Ticket>> pendingTicketsResponse = requests.GetPendingTickets();
                if (pendingTicketsResponse.success)
                {
                    foreach (Ticket ticket in pendingTicketsResponse.data!)
                    {
                        Message.printMessage(ticket.ToString());
                    }
                }
                else
                {
                    Message.printErrorMessage(pendingTicketsResponse.message);
                }
                Console.WriteLine("Please enter the id followed by \"approve\" or \"deny\" to approve or deny a reimbursment ticket.");
                Console.WriteLine("Enter x to leave. \n ");
                string? managerInput = Console.ReadLine();

                if (managerInput != "x")
                {

                    bool mgrInputIsValid = inputValidator.IsValidManagerChoice(managerInput);

                    if (mgrInputIsValid)
                    {
                        string managerDecision = managerInput!.Substring(managerInput.IndexOf(" ") + 1).ToLower();

                        int employeeTicketId = int.Parse(managerInput.Substring(0, managerInput.IndexOf(" ")).ToLower());

                        ResponseMessage<string> UpdateTicketResponse = requests.UpdateTicket((int)employeeTicketId, managerDecision);
                        Message.printMessage(UpdateTicketResponse.message);
                    }

                }
                break;
        }
    }
}
