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
    public ServiceResponse<string> isValidName(String? name)
    {
         ServiceResponse<string> isValidNameRes = new ServiceResponse<string>(); 
        isValidNameRes.Data = null;
        isValidNameRes.Success = false; 
        if (string.IsNullOrEmpty(name))
        {
            isValidNameRes.Message = "Name must not be empty!";
            return isValidNameRes;
        }

        if(name.Length > 20){
            isValidNameRes.Message = "Name must not be more than 20 characters!";
            return isValidNameRes;
        }

        if(name.Length < 4){
            isValidNameRes.Message = "Name must not be less than 4 characters!";
            return isValidNameRes;
        }

        isValidNameRes.Success = true;
        isValidNameRes.Message = "Validated name."; 

        return isValidNameRes;
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
    public ServiceResponse<string> IsValidPassword(string? password)
    {
        ServiceResponse<string> isValidPassRes = new ServiceResponse<string>(); 
        isValidPassRes.Data = null;
        isValidPassRes.Success = false; 
        bool containsDigit = false;
        if (string.IsNullOrEmpty(password))
        {
            isValidPassRes.Message = "Password must not be null or empty"; 
            return isValidPassRes; 
            
        }
        for (int i = 0; i < 10; i++)
        {
            if (password!.Contains(i.ToString()))
            {
                containsDigit = true;

            }
        }
        if (!containsDigit)
        {
             isValidPassRes.Message = "Password must container at least one digit"; 
            return isValidPassRes; 
        }


        if (password!.Trim().Length > 30)
        {
             isValidPassRes.Message = "Password must not be greater than 30 characters!"; 
            return isValidPassRes; 


        }
        if (password.Trim().Length < 8)
        {
            isValidPassRes.Message = "Password must be greater than 8 characters!"; 
            return isValidPassRes; 
        }
        isValidPassRes.Success = true;
        isValidPassRes.Message = "Password was validated!"; 
       
        return isValidPassRes;
    }

    public ServiceResponse<string> isValidUserName(string userName){
        ServiceResponse<string> isValidUserNameRes = new ServiceResponse<string>(); 
        isValidUserNameRes.Data = null;
        isValidUserNameRes.Success = false; 
   
        if (string.IsNullOrEmpty(userName))
        {
            isValidUserNameRes.Message = "Employee username must not be empty!"; 
            return isValidUserNameRes; 
            
        }
        if(userName.Length > 20){
            isValidUserNameRes.Message = "Employee username must not be greater than 20 characters!"; 
            return isValidUserNameRes; 
        }
        if(userName.Length < 5){
            isValidUserNameRes.Message = "Employee username must not be less than 5 characters"; 
            return isValidUserNameRes; 
        }

        isValidUserNameRes.Success = true; 
        isValidUserNameRes.Message = "Validated employee username."; 

        return isValidUserNameRes; 

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
    public ServiceResponse<string> IsValidDescriptionAndAmount(string? description, decimal amount)
    {
        ServiceResponse<string> isValidDescriptionAmountRes = new ServiceResponse<string>();
        isValidDescriptionAmountRes.Data = null; 
        isValidDescriptionAmountRes.Success = false; 
        if(description == "string"){
            isValidDescriptionAmountRes.Message = "Please replace string with a value";
            return isValidDescriptionAmountRes; 
        }
        
        if (String.IsNullOrEmpty(description))
        {
            isValidDescriptionAmountRes.Message = "Description must not be empty";
            return isValidDescriptionAmountRes;
        }
        
        if(amount < 1){
            isValidDescriptionAmountRes.Message = "Amount must not be less than $1.";
            return isValidDescriptionAmountRes; 
        }
    
        isValidDescriptionAmountRes.Message = "Succesfully validated ticket";
        isValidDescriptionAmountRes.Success = true; 

        return isValidDescriptionAmountRes;
    }

    public ServiceResponse<string> ValidateUpdateTicket(string? ticketStatus, int ticketId)
    {
        // TODO add validation to check that ticketstatus is set to pending
        // else return unable to update 
        ServiceResponse<string> newTicketStatusRes = new ServiceResponse<string>(); 
        newTicketStatusRes.Data = null; 
        newTicketStatusRes.Success = false; 

        if(ticketId < 1){
            newTicketStatusRes.Message = "Please verify ticket id."; 
            return newTicketStatusRes; 
        }

        

        if(string.IsNullOrEmpty(ticketStatus)){
            newTicketStatusRes.Message = "new ticket status must not be blank!"; 
            return newTicketStatusRes; 
        }

        if(ticketStatus!.ToLower() != "approve" && ticketStatus.ToLower() != "deny"){
            newTicketStatusRes.Message = "new Ticket status can only be Approve or Deny. Please enter Approve or Deny."; 
            return newTicketStatusRes; 
        }
        
        
        
        newTicketStatusRes.Success = true;
        newTicketStatusRes.Message = "Validated new ticket status"; 
        
        return newTicketStatusRes;
        
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