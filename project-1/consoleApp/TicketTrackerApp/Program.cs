// See https://aka.ms/new-console-template for more information


Form form = new Form();
Pages pages = new Pages();
// TODO // ASK user for Login, register or Exit

bool exit = false;
bool exitCreateUser = false;
bool loggedIn = false;


while (!exit && !loggedIn)
{
    pages.getHomePage();
    string userOption = Console.ReadLine();
    int userOptionInt = getUserOption(userOption);

    if (userOptionInt == 0)
    {

        //Check if user may login
        Console.WriteLine("Please type your username.");
        string userName = Console.ReadLine();
        Console.WriteLine("Please type your password.");
        string password = Console.ReadLine();

        Login loginCredentials = new Login
        {
            UserName = userName,
            Password = password
        };

        loggedIn = form.login(loginCredentials) == 1 ? true : false;

        

    }



    if (userOptionInt == 1)
    {
        while (!exitCreateUser)
        {

            Console.WriteLine("Please type your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Please type your username: ");
            string userName = Console.ReadLine();
            Console.WriteLine("Please type your password");
            string password = Console.ReadLine();
            // TODO give a third option to view and approve requests if user is manager


            Login newLogin = new Login()
            {
                UserName = userName,
                Password = password
            };
            User newUser = new User()
            {
                Name = name,
                login = newLogin
            };

            if (form.postCreateUser(newUser) == 1)
            {
                exitCreateUser = true;
            }
            else
            {
                Console.WriteLine("Sorry that username is taken, please try again.");
            };

        }
        exitCreateUser = false;
    }

    if (userOptionInt == 2)
    {

        exit = true;

    }



}

// TODO 
// AFTER logging retrieve user data
// 

if (loggedIn)
{
    await form.getUserReq();
    await form.setPendingTickets();
    form.setTicketsById(form.getUserId());
    Console.WriteLine($"Hello {form.getUser().Name}. \n");
    
}


while (loggedIn)
{

    {
        
       
        Console.WriteLine("Please type 1 to create a new ticket");
        Console.WriteLine("Please type 2 to logout");
        Console.WriteLine("Please type 4 to view all your ticket submissions. ");
        if (form.getUser().IsManager)
        {
            Console.WriteLine("Please type 3 to view employee submissions.");
        }
            string input = Console.ReadLine();
            int inputInt = int.Parse(input);
            //TODO if user is a manger 
            // Prompt option 3 to review tickets 



            if (inputInt == 1)
            {
                Console.WriteLine("Please type a description for your ticket.");
                string description = Console.ReadLine();
                Console.WriteLine("Please type an Amount.");
                string amountStr = Console.ReadLine();
                int amount = int.Parse(amountStr);
                Ticket ticket = new Ticket(description, amount, form.getUserId());
                bool postSuccess = form.postTicket(ticket) == 1 ? true : false;

            }
            if (inputInt == 2) loggedIn = false;

            if (inputInt == 3)
            {
                // TODO implement function to retrieve all employee submissions 

                foreach (Ticket ticket in form.getPendingTicket())
                {
                    Console.WriteLine(ticket.ToString());
                }

                Console.WriteLine("Please type the id followed by \"approve\" or \"deny\" to approve or deny a reimbursment ticket.");
                int managerChoice = int.Parse(Console.ReadLine());
                bool isSuccess = form.updateTicket(managerChoice, true);
                if (isSuccess)
                {
                    Console.WriteLine("succesfully approved ticket");
                    await form.setPendingTickets();
                }
                // TOdo implement a function to update a ticket 
                // Get id and send it to form
            }

            if (inputInt == 4)
            {
                //TODO retrieve all user tickets 
                if(form.getSubmittedTickets() != null){
                    foreach (Ticket ticket in form.getSubmittedTickets()){
                            Console.WriteLine(ticket.ToString());
                        }
                } else {

                    Console.WriteLine("No tickets to show.\n");
                }
                
            }

        
    }

}

    // TODO create a new user account if user chooses option 1 



    static int getUserOption(string userOption)
    {
        int userOptionInt;

        bool successParsingInt = int.TryParse(userOption, out userOptionInt);

        return userOptionInt;
    }