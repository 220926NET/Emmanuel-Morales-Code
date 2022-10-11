using System.Text;

public class InputValidator
{

    //TODO write tests to verify functionality 

    public InputResponse IsValidUserName(string? userName)
    {
        InputResponse inputResponse = new InputResponse();
        if (string.IsNullOrEmpty(userName))
        {
            inputResponse.messages.Add("Please make sure user name is not empty!");
            inputResponse.success = false;
            return inputResponse;
        }

        inputResponse.success = true;
        return inputResponse;

    }


    public InputResponse isValidName(String str)
    {
        InputResponse inputResponse = new InputResponse();
        if (string.IsNullOrEmpty(str))
        {
            inputResponse.messages.Add("Please make sure name is not empty!");
            inputResponse.success = false;
            return inputResponse;
        }

        inputResponse.success = true;
        return inputResponse;
    }
    public InputResponse IsValidPassword(string? password)
    {
        InputResponse InputResponse = new InputResponse();
        bool containsDigit = false;
        for (int i = 0; i < 10; i++)
        {
            if (password.Contains(i.ToString()))
            {
                containsDigit = true;
                InputResponse.success = true;
            }
        }

        if (password.Trim().Length > 16)
        {
            InputResponse.success = false;
            InputResponse.messages.Add("Please ensure your password is 16 characters long!");

        }
        if (password.Trim().Length < 8)
        {
            InputResponse.success = false;
            InputResponse.messages.Add("Please ensure your password is at least 8 characters long!");
        }
        if (ContainsNull(password))
        {
            InputResponse.success = false;
            InputResponse.messages.Add("Please dont leave your password blank!");
        }
        if (!containsDigit)
        {
            InputResponse.success = false;
            InputResponse.messages.Add("Password must have at least one character!");
        }
        return InputResponse;
    }

    public InputResponse IsValidOptionInput(string? input, int maxNumInput)
    {
        InputResponse inputResponse = new InputResponse();
        int num;
        bool inputIsNum = int.TryParse(input, out num);
        if (!inputIsNum || num > maxNumInput || num < 0)
        {

            inputResponse.success = false;
            inputResponse.messages.Add($"Please type a number between 1 and {maxNumInput}");
            return inputResponse;
        }
        else
        {
            inputResponse.success = true;
            return inputResponse;
        }


    }

    public InputResponse IsValidDescriptionAndAmount(string? description, string? amountStr)
    {

        InputResponse inputResponse = new InputResponse();

        if (!IsValidAmount(amountStr))
        {
            inputResponse.messages.Add("Please ensure to type a number for amount!");

        }

        if (String.IsNullOrEmpty(description))
        {
            inputResponse.messages.Add("Please ensure to type a description!");

        }
        if (inputResponse.messages.Count > 2)
        {
            inputResponse.success = false;
        }

        return inputResponse;
    }

    public bool IsValidAmount(string? amountStr)
    {
        if (ContainsNull(amountStr)) return false;
        int amountInt = 0;
        bool isNum = int.TryParse(amountStr, out amountInt);
        if (!isNum) return false;
        return true;
    }

    public InputResponse IsValidManagerChoice(string? decision)
    {
        InputResponse decisionInputRes = new InputResponse();
        if (string.IsNullOrWhiteSpace(decision))
        {
            decisionInputRes.success = false;
            decisionInputRes.messages.Add("Please enter your decision!");
            return decisionInputRes;
        }
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

            if (isValidEmployeeId && IsValidManagerChoice)
            {
                decisionInputRes.success = true;
                return decisionInputRes;
            }

            if (!isValidEmployeeId || !IsValidManagerChoice) decisionInputRes.messages.Add("Please make sure to type employee id followed by a space and your decision!");

            return decisionInputRes;
        }
        else
        {
            decisionInputRes.messages.Add("Please make sure to type the ticket id followed by a space and eitheir \"approve\" or \"deny\"");
            decisionInputRes.success = false;
            return decisionInputRes;
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