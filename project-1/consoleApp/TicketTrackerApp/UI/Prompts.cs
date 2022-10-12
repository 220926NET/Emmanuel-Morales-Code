using Models;

public class Prompts
{
    private InputResponse _inputResponse;
    private InputValidator _inputvalidator;

    private Requests _requests;
    public Prompts()
    {
        _inputvalidator = new InputValidator();
        _inputResponse = new InputResponse();
        _requests = new Requests(ConnectionFactory.getSqlConnection());
    }

    public int WelcomePrompt()
    {
        InputValidator inputValidator = new InputValidator();
        Console.WriteLine("Welcome, Please choose an Option: ");
        Console.WriteLine("Press 1 For login.");
        Console.WriteLine("Press 2 to register a new acount.");
        Console.WriteLine("Press 3 to exit\n");
        string? userOptionStr = Console.ReadLine();

        _inputResponse = inputValidator.IsValidOptionInput(userOptionStr, 3);
        bool isValid = _inputResponse.success;
        if (!isValid)
        {
            foreach (string message in _inputResponse.messages)
            {
                Message.printErrorMessage(message);
            }
            return -1;
        }
        else
        {
            return int.Parse(userOptionStr!);
        }

    }

    public void LoginUserPrompt(out User user)
    {


        Console.WriteLine("Please type your username.");
        string? userName = Console.ReadLine();
        _inputResponse = _inputvalidator.IsValidUserName(userName);

        InputResponse isValidPasswordRes = new InputResponse();
        Console.WriteLine("Please type your password.");
        string? password = Console.ReadLine();
        isValidPasswordRes = _inputvalidator.IsValidPassword(password);

        if (_inputResponse.success && isValidPasswordRes.success)
        {
            Login loginCredentials = new Login
            {
                UserName = userName!,
                Password = password!
            };

            ResponseMessage<User> postLoginResponse = _requests.PostLogin(loginCredentials);
            if (postLoginResponse.success)
            {
                user = new User
                {
                    Id = postLoginResponse.data.Id!,
                    Name = postLoginResponse.data.Name!,
                    IsManager = postLoginResponse.data.IsManager
                };
            }
            else
            {
                user = null;
            }
            Message.printMessage(postLoginResponse.message!);


        }
        else
        {
            _inputResponse.messages = new List<string>() { "Please dont leave user name blank!" };
            printInputReponseMessages(_inputResponse);
            printInputReponseMessages(isValidPasswordRes);
            user = null;
        }



    }

    public void AskManagerApprovalPrompt()
    {
        Console.WriteLine("Please enter the id followed by \"approve\" or \"deny\" to approve or deny a reimbursment ticket.");
        Console.WriteLine("Enter x to leave. \n ");
        string? managerInput = Console.ReadLine();
        if (managerInput.Trim().ToLower() == "x") return;


        _inputResponse = _inputvalidator.IsValidManagerChoice(managerInput);

        if (_inputResponse.success)
        {
            string managerDecision = managerInput!.Substring(managerInput.IndexOf(" ") + 1).ToLower();

            int employeeTicketId = int.Parse(managerInput.Substring(0, managerInput.IndexOf(" ")).ToLower());

            ResponseMessage<string> UpdateTicketResponse = _requests.UpdateTicket((int)employeeTicketId, managerDecision);
            Message.printMessage(UpdateTicketResponse.message);
        }
        else
        {
            printInputReponseMessages(_inputResponse);
        }


    }

    public void CreateUserPrompt()
    {

        InputResponse isValidNameRes = new InputResponse();
        Console.WriteLine("Please type your name: ");
        string? nameInput = Console.ReadLine();
        isValidNameRes = _inputvalidator.isValidName(nameInput);

        InputResponse isValidUserNameRes = new InputResponse();
        Console.WriteLine("Please type your username: ");
        string? userNameInput = Console.ReadLine();
        isValidUserNameRes = _inputvalidator.IsValidUserName(userNameInput);

        InputResponse isValidPassRes = new InputResponse();
        Console.WriteLine("Please type your password");
        string? passwordInput = Console.ReadLine();
        isValidPassRes = _inputvalidator.IsValidPassword(passwordInput);


        if (!isValidPassRes.success || !isValidNameRes.success || !isValidUserNameRes.success)
        {
            printInputReponseMessages(isValidNameRes);
            printInputReponseMessages(isValidPassRes);
            printInputReponseMessages(isValidUserNameRes);

        }
        else
        {
            Login newLogin = new Login()
            {
                UserName = userNameInput!,
                Password = passwordInput!
            };
            User newUser = new User()
            {
                Name = nameInput,
                login = newLogin
            };

            ResponseMessage<string> CreateUserResponse = _requests.PostCreateUser(newUser);
            Message.printMessage(CreateUserResponse.message!);

        }

    }

    public int HomePrompt(bool isManager)
    {
        Console.WriteLine("Enter 1 to logout.");
        Console.WriteLine("Enter 2 to create a new ticket");
        Console.WriteLine("Enter 3 to view all your ticket submissions. ");
        int options = 3;
        if (isManager)
        {
            Console.WriteLine("Enter 4 to view pending tickets.");
            options = 4;
        }
        string? userOption = Console.ReadLine();
        _inputResponse = _inputvalidator.IsValidOptionInput(userOption, options);
        bool isValidOption = _inputResponse.success;

        if (!isValidOption)
        {
            foreach (string message in _inputResponse.messages)
            {
                Message.printErrorMessage(message);
            }
            return -1;
        }
        return int.Parse(userOption!);

    }

    public void CreateTicketPrompt(int loggedInUserId)
    {
        Console.WriteLine("Please type a description for your ticket.");
        string? descriptionStr = Console.ReadLine();

        Console.WriteLine("Please type an Amount.");
        string? amountStr = Console.ReadLine();


        _inputResponse = _inputvalidator.IsValidDescriptionAndAmount(descriptionStr, amountStr);
        bool isValidResponse = _inputResponse.success;

        if (!isValidResponse)
        {
            foreach (string message in _inputResponse.messages)
            {
                Message.printErrorMessage(message);
            }
        }
        else
        {
            int amountInt = int.Parse(amountStr);
            Ticket newTicket = new Ticket(descriptionStr, amountInt, loggedInUserId);
            ResponseMessage<string> responseMessage = _requests.PostTicket(newTicket);
            Message.printMessage(responseMessage.message);
        }

    }

    public void printUserTickets(int userId)
    {
        ResponseMessage<List<Ticket>> getTicketResponse = _requests.GetUserTickets(userId);

        Message.printMessage(getTicketResponse.message!);
        if (getTicketResponse.success)
        {
            foreach (Ticket ticket in getTicketResponse.data!)
            {
                Message.printMessage(ticket.ToString());
            }
        }
    }

    public void printPendingTickets()
    {
        ResponseMessage<List<Ticket>> pendingTicketsResponse = _requests.GetPendingTickets();
        if (pendingTicketsResponse.success)
        {
            foreach (Ticket ticket in pendingTicketsResponse.data!)
            {
                Message.printMessage(ticket.ToString());
            }
        }
        else
        {
            Message.printErrorMessage(pendingTicketsResponse.message!);
        }
    }

    public void printInputReponseMessages(InputResponse inputResponse)
    {
        foreach (string message in inputResponse.messages)
        {
            Message.printErrorMessage(message);
        }
    }
}