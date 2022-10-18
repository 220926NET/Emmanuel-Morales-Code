using System.Text;


/// <summary>
/// Class <c>InputValidator</c> deals with validating strings 
/// </summary>
public class InputValidator
{

    
    
    
    

    /// <summary>
    /// <method><c>IsValidUserName<c></method> 
    /// <para>
    /// This method validates user name by checking if the string is Null or empty
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
    public bool isValidName(String? name)
    {
        if (string.IsNullOrEmpty(name))
        {

            return false;
        }

        return true;
    }

    /// <summary>
    /// <method><c>IsValidPassword<c></method> 
    /// <para>
    /// This method validates user password by checking if it contains at least one digit and is not null or empty, 
    /// 
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
    public bool IsValidPassword(string? password)
    {

        bool containsDigit = false;
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }
        for (int i = 0; i < 10; i++)
        {
            if (password!.Contains(i.ToString()))
            {
                containsDigit = true;

            }
        }

        if (password!.Trim().Length > 16)
        {
            return false;


        }
        if (password.Trim().Length < 8)
        {
            return false;
        }
        
        if (!containsDigit)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// <method><c>IsValidOptionInput<c></method> 
    /// <para>
    /// This method validates user options by ensuring they stay within 0 (exclusive) and maxNumInput(Inclusive)
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
    public bool IsValidOptionInput(string? input, int maxNumInput = 3)
    {
        int num;
        bool inputIsNum = int.TryParse(input, out num);
        if (!inputIsNum || num > maxNumInput || num < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// <method><c>IsValidDescriptionAndAmount<c></method> 
    /// <para>
    /// This method validates a amount string and description str by ensuring 
    /// that amount str can be converted into a decimal and description string is not null 
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
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

    /// <summary>
    /// <method><c>isValidTicketDescription<c></method> 
    /// <para>
    /// This method validates a amount string and description str by ensuring 
    /// that amount str can be converted into a decimal and description string is not null 
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
    public bool isValidTicketDescription(string? description){
        // TODO add more validation 
        
       
        if (String.IsNullOrEmpty(description))
        {

            return false;
        }

         if(description.Length > 50){
            return false; 
        }
    
        
        return true;
    }
    
    /// <summary>
    /// <method><c>IsValidAmount<c></method> 
    /// <para>
    /// This method validates a amount string a by testing if amountStr can be converted into a decimal
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
    public bool IsValidAmount(string? amountStr)
    {
        
        if (String.IsNullOrEmpty(amountStr)) return false;
        foreach(char num in amountStr){
            if(!char.IsDigit(num) && num != '.'){
                return false; 
            }
        }
        decimal amountInt = 0;
        bool isNum = decimal.TryParse(amountStr, out amountInt);

        if (!isNum) return false;
        return true;
    }

    /// <summary>
    /// <method><c>IsValidAmount<c></method> 
    /// <para>
    /// This method validates an id and decision by ensuring that 
    /// id can be converted into an integer and decision is equal to ethier = "approve" or "deny"
    /// </para> 
    ///<returns>
    /// True or false
    /// </returns> 
    /// </summary>
    public bool IsValidManagerChoice(int? id, string? decision)
    {

        if (string.IsNullOrWhiteSpace(decision))
        {

            return false;
        }
        if (decision.ToLower() != "approve" && decision.ToLower() != "deny")
        {
            return false;
        }
        if (id == null)
        {
            return false;
        }
       
        return true;

    }
}