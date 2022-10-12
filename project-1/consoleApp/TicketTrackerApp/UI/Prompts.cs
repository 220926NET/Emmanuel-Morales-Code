using Models;

public class Prompts
{
    private InputResponse _inputResponse;
    private InputValidator _inputvalidator;

    private RequestService _RequestService;


    public Prompts()
    {
        _inputvalidator = new InputValidator();
        _inputResponse = new InputResponse();
        _RequestService = new RequestService();
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

        InputResponse isValidPasswordRes = new InputResponse();
        Console.WriteLine("Please type your password.");
        string? password = Console.ReadLine();

        Login loginCreds = new Login()
        {
            UserName = userName,
            Password = password
        };
        ResponseMessage<User> postLoginResponse = _RequestService.PostLogin(loginCreds);
        Message.printMessage(postLoginResponse.message);
        user = postLoginResponse.data;

    }

    public void AskManagerApprovalPrompt()
    {
        Console.WriteLine("Please enter the ticket id.");
        string? ticketIdstr = Console.ReadLine();
        int ticketId = 0;
        bool isValidTicketId = int.TryParse(ticketIdstr, out ticketId);
        Console.WriteLine("Please enter approve or deny.");
        string? managerInput = Console.ReadLine();
        ResponseMessage<string> UpdateTicketResponse = _RequestService.UpdateTicket(ticketId, managerInput);
        Message.printMessage(UpdateTicketResponse.message!);
    }

    public void CreateUserPrompt()
    {


        Console.WriteLine("Please type your name: ");
        string? nameInput = Console.ReadLine();


        Console.WriteLine("Please type your username: ");
        string? userNameInput = Console.ReadLine();



        Console.WriteLine("Please type your password");
        string? passwordInput = Console.ReadLine();

        User newUser = new User()
        {
            Name = nameInput,
            login = new Login()
            {
                UserName = userNameInput,
                Password = passwordInput
            }
        };

        ResponseMessage<string> CreateUserResponse = _RequestService.PostCreateUser(newUser);
        Message.printMessage(CreateUserResponse.message!);




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
        bool isValidInput = _inputvalidator.IsValidDescriptionAndAmount(descriptionStr, amountStr);
        if (isValidInput)
        {
            Ticket newTicket = new Ticket(descriptionStr, decimal.Parse(amountStr), loggedInUserId);
            ResponseMessage<string> responseMessage = _RequestService.PostTicket(newTicket);
        }





    }

    public void printUserTickets(int userId)
    {
        ResponseMessage<List<Ticket>> getTicketResponse = _RequestService.GetUserTickets(userId);

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
        ResponseMessage<List<Ticket>> pendingTicketsResponse = _RequestService.GetPendingTickets();
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