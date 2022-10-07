using System.Text;

public class InputValidator
{

    //TODO write tests to verify functionality 

    public bool isValidString(string? userName)
    {

        if (containsNull(userName))
        {
            return false;
        }
        return true;
    }

    public bool isValidPassword(string? password)
    {
        if (containsNull(password)) return false;

        return true;
    }

    public bool isValidOptionInput(string? input, int maxNumInput)
    {
        int num;
        bool inputIsNum = int.TryParse(input, out num);
        if (!inputIsNum || num > maxNumInput || num < 0)
        {
            return false;
        }
        return true;
    }

    public bool isValidDescriptioon(string? description)
    {

        if (String.IsNullOrEmpty(description)) return false;


        return true;
    }

    public bool isValidAmount(string? amountStr)
    {
        if (containsNull(amountStr)) return false;
        int amountInt = 0;
        bool isNum = int.TryParse(amountStr, out amountInt);
        if (!isNum) return false;
        return true;
    }

    public bool isValidManagerChoice(string? decision)
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
            bool isValidManagerChoice = false;

            if (managerDecision == "approve" || managerDecision == "deny")
            {
                isValidManagerChoice = true;
            }

            return isValidEmployeeId && isValidManagerChoice;
        }
        else
        {
            return false;
        }

    }

    public bool containsNull(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }
        return false;
    }

}