using Models;


/// <summary>
/// Keeps track of a logged in user and navigates a menu based on user inputs
///</summary>


Prompts prompts = new Prompts();
User? loggedInUser = null;
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
    }

if (loggedInUser != null) Message.printWelcomeMessage(loggedInUser!.Name!.ToUpper());

while (loggedInUser != null)
{

    {
        int userOption = prompts.HomePrompt(loggedInUser.IsManager);

        switch (userOption)
        {

            case 1:
                Message.printMessage(loggedInUser.Name!.ToUpper() + " has logged out.");
                loggedInUser = null;
                break;

            case 2:

                prompts.CreateTicketPrompt(loggedInUser.Id);

                break;

            case 3:

                prompts.printUserTickets(loggedInUser.Id);

                break;
            case 4:

                prompts.printPendingTickets();

                prompts.AskManagerApprovalPrompt();
                break;
        }
    }
}
}


