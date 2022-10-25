using Models;

/// <summary>
/// Class <c>Prompts</c> deals with writing messages to the console and retrieving Employee input.
/// </summary>
public class Prompts
{

    private RequestService _RequestService;

    public Prompts()
    {
     
        _RequestService = new RequestService();
    }

    /// <summary>
    /// <method><c>WelcomePrompt<c></method> 
    /// <para>
    /// This method displays to the Employee his options when he first starts the application. 
    /// </para> 
    /// </summary>
    public int WelcomePrompt()
    {
        Console.WriteLine("Welcome, Please choose an Option: ");
        Console.WriteLine("Enter 1 For login.");
        Console.WriteLine("Enter 2 to register a new acount.");
        Console.WriteLine("Enter 3 to exit\n");
        string? userOptionStr = Console.ReadLine();
        int userOption = 0;
        bool isValidOption = int.TryParse(userOptionStr, out userOption); 

        return userOption;


        
    }

    /// <summary>
    /// <method><c>LoginUserPrompt<c></method> 
    /// <para>
    /// This method asks the Employee for his password and login and sends it to <c>RequestService</c>. 
    /// </para> 
    /// </summary>
    public void LoginUserPrompt(out Employee Employee)
    {


        Console.WriteLine("Please type your username.");
        string? userName = Console.ReadLine();

    
        Console.WriteLine("Please type your password.");
        string? password = Console.ReadLine();
        
        ResponseMessage<Employee> postLoginResponse = _RequestService.PostLogin(userName, password);
        Message.printMessage(postLoginResponse.message!);
        Employee = postLoginResponse.data!;

    }


    /// <summary>
    /// <method><c>AskManagerApprovalPrompt<c></method> 
    /// <para>
    /// This method asks the manager to ethier deny or approve a ticket based on a ticket id. The input is sent to <c>RequestService</c>. 
    /// </para> 
    /// </summary>
    public void AskManagerApprovalPrompt(string token)
    {
        Console.WriteLine("Please enter the ticket id.");
        string? ticketIdstr = Console.ReadLine();
        int ticketId = 0;
        bool isValidTicketId = int.TryParse(ticketIdstr, out ticketId);

        Console.WriteLine("Please enter approve or deny.");
        string? managerInput = Console.ReadLine();

        ResponseMessage<string> UpdateTicketResponse = _RequestService.UpdateTicket(token, ticketId, managerInput);
        Message.printMessage(UpdateTicketResponse.message!);
    }

    /// <summary>
    /// <method><c>CreateUserPrompt<c></method> 
    /// <para>
    /// This method prompts the Employee to create a new username by submitting a name, username and password. 
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
    public int HomePrompt()
    {
        Console.WriteLine("Enter 1 to logout.");
        Console.WriteLine("Enter 2 to create a new ticket");
        Console.WriteLine("Enter 3 to view all your ticket submissions. ");
        Console.WriteLine("Enter 4 to view pending tickets.");
        
        string? userOption = Console.ReadLine();
        int userNum = int.MaxValue; 
        bool isValidOption = int.TryParse(userOption, out userNum); 
        return userNum; 

    }

    /// <summary>
    /// <method><c>CreateTicketPrompt<c></method> 
    /// <para>
    /// This method displays options to logout, create a ticket and view all existing tickets. 
    /// </para> 
    /// <parm name="loggedInUserId">Employee Id </parm> 
    /// </summary>
    public void CreateTicketPrompt(string token)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        Console.WriteLine("Please type a description for your ticket.");
        string? description = Console.ReadLine();

        Console.WriteLine("Please type an Amount.");
        string? amount = Console.ReadLine();

        decimal amountValue = 0; 
        
        bool isValidDecimal = decimal.TryParse(amount, out amountValue); 

        responseMessage = _RequestService.PostTicket(description!, amountValue, token);

        if(!responseMessage.success){
            Message.printErrorMessage(responseMessage.message!);
        } else {
            Message.printMessage(responseMessage.message!); 
        }
       

    }
    /// <summary>
    /// <method><c>printUserTickets<c></method> 
    /// <para>
    /// This method displays Employee tickets. 
    /// </para> 
    /// <parm name="userId">Employee Id </parm> 
    /// </summary>
    public void printUserTickets(string token)
    {
        ResponseMessage<List<Ticket>> getTicketResponse = _RequestService.GetUserTickets(token);

        
        if (getTicketResponse.success)
        {
            foreach (Ticket ticket in getTicketResponse.data!)
            {
                Message.printMessage(ticket.ToString());
            }
        } else {
            Message.printMessage(getTicketResponse.message!); 
        }
    }

    /// <summary>
    /// <method><c>printPendingTickets<c></method> 
    /// <para>
    /// This method displays pending Employee tickets. 
    /// </para> 
    /// </summary>
    public void printPendingTickets(string token)
    {
        ResponseMessage<List<Ticket>> pendingTicketsResponse = _RequestService.GetPendingTickets(token);
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