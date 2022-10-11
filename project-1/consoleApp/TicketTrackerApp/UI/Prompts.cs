public class Prompts
{

    private InputValidator _inputvalidator;
    public Prompts()
    {
        _inputvalidator = new InputValidator();
    }

    public int WelcomePrompt()
    {
        InputValidator inputValidator = new InputValidator();
        Console.WriteLine("Welcome, Please choose an Option: ");
        Console.WriteLine("Press 1 For login.");
        Console.WriteLine("Press 2 to register a new acount.");
        Console.WriteLine("Press 3 to exit\n");
        string? userOptionStr = Console.ReadLine();
        bool isValid = inputValidator.IsValidOptionInput(userOptionStr, 3);
        if (isValid)
        {
            return int.Parse(userOptionStr!);
        }
        return -1;
    }

    public bool RegisterUserPrompt(out string userName, out string password)
    {
        Console.WriteLine("Please type your username.");
        string? userNameStr = Console.ReadLine();
        bool isValidUsername = _inputvalidator.IsValidString(userNameStr);

        Console.WriteLine("Please type your password.");
        string? passwordStr = Console.ReadLine();
        bool IsValidPassword = _inputvalidator.IsValidString(passwordStr);

        if (isValidUsername && IsValidPassword)
        {
            userName = userNameStr;
            password = passwordStr;
            return true;
        }
        userName = null;
        password = null;
        return false;
    }


    public bool CreateUserPrompt(out string name, out string userName, out string password)
    {
        Console.WriteLine("Please type your name: ");
        string? nameInput = Console.ReadLine();
        bool isValidName = _inputvalidator.IsValidString(nameInput);

        Console.WriteLine("Please type your username: ");
        string? userNameInput = Console.ReadLine();
        bool isValidUserName = _inputvalidator.IsValidString(userNameInput);

        Console.WriteLine("Please type your password");
        string? passwordInput = Console.ReadLine();
        bool IsValidPassword = _inputvalidator.IsValidPassword(passwordInput);

        if (isValidName && isValidUserName && IsValidPassword)
        {
            name = nameInput;
            userName = userNameInput;
            password = passwordInput;
            return true;
        }

        name = null;
        userName = null;
        password = null;
        return false;


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
        bool isValidOption = _inputvalidator.IsValidOptionInput(userOption, options);
        if (isValidOption)
        {
            return int.Parse(userOption);
        }
        return -1;
    }

    public bool CreateTicketPrompt(out string amount, out string description)
    {
        Console.WriteLine("Please type a description for your ticket.");
        string? descriptionStr = Console.ReadLine();

        Console.WriteLine("Please type an Amount.");
        string? amountStr = Console.ReadLine();

        bool isValidDescription = _inputvalidator.IsValidDescription(descriptionStr);
        bool IsValidAmount = _inputvalidator.IsValidAmount(amountStr);
        if (isValidDescription && IsValidAmount)
        {
            amount = amountStr;
            description = descriptionStr;
            return true;
        }
        amount = null;
        description = null;
        return false;
    }
}