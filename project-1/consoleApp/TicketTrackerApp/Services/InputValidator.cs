using System.Text;

public class InputValidator
{

    //TODO write tests to verify functionality 

    public bool IsValidUserName(string? userName)
    {
        if (string.IsNullOrEmpty(userName))
        {

            return false;
        }

        return true;

    }


    public bool isValidName(String str)
    {
        if (string.IsNullOrEmpty(str))
        {

            return false;
        }

        return true;
    }
    public bool IsValidPassword(string? password)
    {

        bool containsDigit = false;
        for (int i = 0; i < 10; i++)
        {
            if (password.Contains(i.ToString()))
            {
                containsDigit = true;

            }
        }

        if (password.Trim().Length > 16)
        {
            return false;


        }
        if (password.Trim().Length < 8)
        {
            return false;
        }
        if (ContainsNull(password))
        {
            return false;
        }
        if (!containsDigit)
        {
            return false;
        }
        return true;
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

    public bool IsValidDescriptionAndAmount(string? description, string? amountStr)
    {



        if (!IsValidAmount(amountStr))
        {

            return false;
        }

        if (String.IsNullOrEmpty(description))
        {

            return false;
        }



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

    public bool IsValidManagerChoice(int? id, string? decision)
    {

        if (string.IsNullOrWhiteSpace(decision))
        {

            return false;
        }
        if (decision.ToLower() != "approve" || decision.ToLower() != "deny")
        {
            return false;
        }
        if (id == null)
        {
            return false;
        }
        // Todo check that there exists at least two substrings 
        return true;


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