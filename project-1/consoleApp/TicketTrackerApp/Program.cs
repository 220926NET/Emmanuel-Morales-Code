using Models;


/// <summary>
/// Keeps track of a logged in Employee and navigates a menu based on Employee inputs
///</summary>


Prompts prompts = new Prompts();
Employee? loggedInUser = null;
bool exit = false;

while ( !exit)
{

    int userOptionInt = prompts.WelcomePrompt();

    switch (userOptionInt)
    {
        case 1:

            prompts.LoginUserPrompt(out loggedInUser);

            break;

        case 2:
            prompts.CreateUserPrompt();

            break;

        case 3:
            Message.printMessage("GoodBye");

            exit = true;
            break;

        default:
            Message.printErrorMessage("Please choose a valid option"); 
            break;
    }

if (loggedInUser != null) Message.printWelcomeMessage(loggedInUser!.UserName!.ToUpper());

while (loggedInUser != null)
{

    {
        int userOption = prompts.HomePrompt();

        switch (userOption)
        {

            case 1:
                Message.printMessage(loggedInUser.UserName!.ToUpper() + " has logged out.");
                loggedInUser = null;
                break;

            case 2:

                prompts.CreateTicketPrompt(loggedInUser.Token);

                break;

            case 3:

                prompts.printUserTickets(loggedInUser.Token);

                break;
            case 4:

                prompts.printPendingTickets(loggedInUser.Token);

                prompts.AskManagerApprovalPrompt(loggedInUser.Token);
                break;
            default:
                Message.printErrorMessage("Please choose a valid option"); 
                break;

        }
    }
}
}


