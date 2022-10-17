using Models;

/// <summary>
/// Class <c>Prompts</c> deals with writing messages to the console and retrieving user input.
/// </summary>
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

    /// <summary>
    /// <method><c>WelcomePrompt<c></method> 
    /// <para>
    /// This method displays to the user his options when he first starts the application. 
    /// </para> 
    /// </summary>
    public int WelcomePrompt()
    {
        Console.WriteLine("Welcome, Please choose an Option: ");
        Console.WriteLine("Enter 1 For login.");
        Console.WriteLine("Enter 2 to register a new acount.");
        Console.WriteLine("Enter 3 to exit\n");
        string? userOptionStr = Console.ReadLine();

        bool inputIsValid = _inputvalidator.IsValidOptionInput(userOptionStr);
        
        if (inputIsValid)
        {
            return int.Parse(userOptionStr!); 
        }
        else
        {
            Message.printErrorMessage("Please choose a valid option.");
            return -1; 
        }

    }

    /// <summary>
    /// <method><c>LoginUserPrompt<c></method> 
    /// <para>
    /// This method asks the user for his password and login and sends it to <c>RequestService</c>. 
    /// </para> 
    /// </summary>
    public void LoginUserPrompt(out User user)
    {


        Console.WriteLine("Please type your username.");
        string? userName = Console.ReadLine();

        InputResponse isValidPasswordRes = new InputResponse();
        Console.WriteLine("Please type your password.");
        string? password = Console.ReadLine();
        
        ResponseMessage<User> postLoginResponse = _RequestService.PostLogin(userName, password);
        Message.printMessage(postLoginResponse.message!);
        user = postLoginResponse.data!;

    }


    /// <summary>
    /// <method><c>AskManagerApprovalPrompt<c></method> 
    /// <para>
    /// This method asks the manager to ethier deny or approve a ticket based on a ticket id. The input is sent to <c>RequestService</c>. 
    /// </para> 
    /// </summary>
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

    /// <summary>
    /// <method><c>CreateUserPrompt<c></method> 
    /// <para>
    /// This method prompts the user to create a new username by submitting a name, username and password. 
    /// </para> 
    /// </summary>
    public void CreateUserPrompt()
    {


        Console.WriteLine("Please type your name: ");
        string? name = Console.ReadLine();


        Console.WriteLine("Please type your username: ");
        string? userName = Console.ReadLine();



        Console.WriteLine("Please type your password");
        string? password = Console.ReadLine();

        ResponseMessage<string> CreateUserResponse = _RequestService.CreateUser(name, userName, password);
        Message.printMessage(CreateUserResponse.message!);

    }
    /// <summary>
    /// <method><c>HomePrompt<c></method> 
    /// <para>
    /// This method displays options to logout, create a ticket and view all existing tickets. 
    /// </para> 
    /// </summary>
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

        bool isValidInput = _inputvalidator.IsValidOptionInput(userOption, options);
        bool isValidOption = _inputResponse.success;

        if (isValidInput){
            return int.Parse(userOption!);
        } else {
            Message.printErrorMessage("Please choose a valid option!");
            return -1; 
        }

    }

    /// <summary>
    /// <method><c>CreateTicketPrompt<c></method> 
    /// <para>
    /// This method displays options to logout, create a ticket and view all existing tickets. 
    /// </para> 
    /// <parm name="loggedInUserId">user Id </parm> 
    /// </summary>
    public void CreateTicketPrompt(int loggedInUserId)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        Console.WriteLine("Please type a description for your ticket.");
        string? description = Console.ReadLine();

        while(!_inputvalidator.isValidTicketDescription(description)){
            Message.printErrorMessage("Please input a valid description!"); 
            Console.WriteLine("Please type a description for your ticket.");
            description = Console.ReadLine();
        }; 

        Console.WriteLine("Please type an Amount.");
        string? amount = Console.ReadLine();

         while(!_inputvalidator.IsValidAmount(amount)){
            Message.printErrorMessage("Please input a valid Amount!"); 
            Console.WriteLine("Please type an Amount.");
             amount = Console.ReadLine();
        }; 

        responseMessage = _RequestService.PostTicket(description!, decimal.Parse(amount!), loggedInUserId);


    }
    /// <summary>
    /// <method><c>printUserTickets<c></method> 
    /// <para>
    /// This method displays user tickets. 
    /// </para> 
    /// <parm name="userId">user Id </parm> 
    /// </summary>
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

    /// <summary>
    /// <method><c>printPendingTickets<c></method> 
    /// <para>
    /// This method displays pending user tickets. 
    /// </para> 
    /// </summary>
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

}