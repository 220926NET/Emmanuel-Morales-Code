namespace Tests;

public class TestsInputValidator
{
    [Theory]
    [InlineData("abcdefghijklmnopq")]
    [InlineData("abcdefkljskldfjl;asfjasfj")]
    public void Test_Password_Validator_Greater_Than_Sixteen_Characters(string password)
    {
        InputValidator inputValidator = new InputValidator();
        
        Assert.False(inputValidator.IsValidPassword(password));

    }


    [Theory]
    [InlineData("abc")]
    [InlineData("a")]
    [InlineData("aaaab")]
    public void Test_Password_Validator_Less_Than_Eight_Characters(string password)
    {
        InputValidator inputValidator = new InputValidator();
       
        bool isValidPassword = inputValidator.IsValidPassword(password);
        Assert.False(isValidPassword);


    }

    [Theory]

    [InlineData("password")]
    [InlineData("passwo@#$)U)")]
    public void Test_Password_Contains_No_Numbers(string password)
    {
        InputValidator inputValidator = new InputValidator();
    
        bool isValidPassword = inputValidator.IsValidPassword(password);
        Assert.False(isValidPassword);

    }


    [Theory]

    [InlineData("password2")]
    [InlineData("passwo@#$)U)1")]
    public void Test_Password_Contains_A_Number(string password)
    {
        InputValidator inputValidator = new InputValidator();
     
        bool isValidPassword = inputValidator.IsValidPassword(password);
        Assert.True(isValidPassword);

    }

    [Fact]
    public void Test_Null_Input_Password()
    {
        InputValidator inputValidator = new InputValidator();
        
        bool isValidPassword = inputValidator.IsValidPassword(string.Empty);
        Assert.False(isValidPassword);
    }

    [Theory]
    [InlineData("8 approve")]
    [InlineData("11 approve")]
    [InlineData("9 deny")]
    [InlineData("99 deny")]
    [InlineData("100 approve")]
    [InlineData("100 deny")]
    public void Test_Manager_Decision_Is_Valid(string choice)
    {
        
        InputValidator inputValidator = new InputValidator();
        string[] choices = choice.Split(" "); 
        
        string id = choices[0]; 
        string decision = choices[1]; 
        int idInt =0; 

        bool canParseNum = int.TryParse(choices[0], out idInt); 
        
        bool isValidChoice = inputValidator.IsValidManagerChoice(idInt, decision);
      
        Assert.True(isValidChoice);
   

    }

    [Theory]

    [InlineData("8 aprove")]
    [InlineData("11 dey")]
    [InlineData("9 appprove")]
    [InlineData("99 d")]
    [InlineData("100 a")]
    [InlineData("100 sadf")]
    public void Test_Manager_Decision_Is_Not_Valid(string choice)
    {
        InputValidator inputValidator = new InputValidator();
        string[] choices = choice.Split(" "); 
        
        string id = choices[0]; 
        string decision = choices[1]; 
        int idInt =0; 

        bool canParseNum = int.TryParse(choices[0], out idInt); 
        
        bool isValidChoice = inputValidator.IsValidManagerChoice(idInt, decision);
      
        Assert.False(isValidChoice);
    }


    [Theory]

    [InlineData("88.00")]
    [InlineData("1.00")]
    [InlineData("2.00")]
    [InlineData("100000.00")]
    [InlineData(".01")]
    [InlineData("1")]
    [InlineData("1.")]
    public void Test_Amount_Is_Valid(string amount)
    {
        InputValidator inputValidator = new InputValidator();
    
        bool isValidChoice = inputValidator.IsValidAmount(amount);
      
        Assert.True(isValidChoice);
    }

    [Theory]

    [InlineData("-1.00")]
    [InlineData("100asdf")]
    [InlineData("1..00")]
    public void Test_Amount_Is_Not_Valid(string amount)
    {
        InputValidator inputValidator = new InputValidator();
    
        bool isValidAmount = inputValidator.IsValidAmount(amount);
      
        Assert.False(isValidAmount);
    }

    [Theory]

    [InlineData("Flight to DC")]
    [InlineData("Flight To MIAMI")]
    [InlineData("Car rentals")]
    public void Test_Description_Length_Is_Valid(string description)
    {
        InputValidator inputValidator = new InputValidator();
    
        bool isValidDescription = inputValidator.isValidTicketDescription(description);
      
        Assert.True(isValidDescription);
    }

    [Theory]

    [InlineData("Flight to DC kasdjfkljasdklfjasdkljfklasd;flkasdjl;fkjasdklf")]
    [InlineData("Flight to DC kasdjfkljasdklfjsdfasdfadsfasdfasdfddx")]
    public void Test_Description_Length_Is_Not_Valid(string description)
    {
        InputValidator inputValidator = new InputValidator();
    
        bool isValidDescription = inputValidator.isValidTicketDescription(description);
      
        Assert.False(isValidDescription);
    }

    
}