using System.Text;

public class InputValidator
{

    //TODO write tests to verify functionality 

    public bool IsValidString(string? userName)
    {

        if (ContainsNull(userName))
        {
            return false;
        }
        return true;
    }

//     public int? IsValidOption(string? userOption, int options)
//     {
//     InputValidator inputValidator = new InputValidator();
//     bool validUserOption = inputValidator.IsValidOptionInput(userOption, options);
//     int? userOptionInt = null;

//     if (!validUserOption)
//     {
//         printErrorMessage("Please choose a valid option. ");

//     }
//     else
//     {
//         userOptionInt = int.Parse(userOption!);

//     }
//     return userOptionInt;

// }


    public bool IsValidPassword(string? password)
    {
        if (ContainsNull(password)) return false;

        return true;
    }

    public bool IsValidOptionInput(string? input, int maxNumInput)
    {
        int num;
        bool inputIsNum = int.TryParse(input, out num);
        if (!inputIsNum || num > maxNumInput || num < 0)
        {
            
            return false;
        }
        return true;
    }

    public bool IsValidDescription(string? description)
    {

        if (String.IsNullOrEmpty(description)) return false;


        return true;
    }

    public bool IsValidAmount(string? amountStr)
    {
        if (ContainsNull(amountStr)) return false;
        int amountInt = 0;
        bool isNum = int.TryParse(amountStr, out amountInt);
        if (!isNum) return false;
        return true;
    }

    public bool IsValidManagerChoice(string? decision)
    {
        if (string.IsNullOrWhiteSpace(decision)) return false;
        // Todo check that there exists at least two substrings 
        var decisionStrArr = decision.Split();
        if (decisionStrArr.Length == 2)
        {
            string? employeeId = decision.Substring(0, decision.IndexOf(" ")).ToLower();
            int num = 0;
            bool isValidEmployeeId = int.TryParse(employeeId, out num);



            string managerDecision = decision.Substring(decision.IndexOf(" ") + 1).ToLower();
            bool IsValidManagerChoice = false;

            if (managerDecision == "approve" || managerDecision == "deny")
            {
                IsValidManagerChoice = true;
            }

            return isValidEmployeeId && IsValidManagerChoice;
        }
        else
        {
            return false;
        }

    }

    public bool ContainsNull(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }
        return false;
    }

}