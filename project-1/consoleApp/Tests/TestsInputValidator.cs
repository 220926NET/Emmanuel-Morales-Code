namespace Tests;

public class TestsInputValidator
{
    [Theory]
    [InlineData("abcdefghijklmnopq")]
    [InlineData("abcdefkljskldfjl;asfjasfj")]
    public void Test_Password_Validator_Greater_Than_Sixteen_Characters(string password)
    {
        
        InputValidator inputValidator = new InputValidator();
        InputResponse InputResponse = new InputResponse();
        InputResponse = inputValidator.IsValidPassword(password);
        Assert.False(InputResponse.success);

    }


    [Theory]
    [InlineData("abc")]
    [InlineData("a")]
    [InlineData("aaaab")]
    public void Test_Password_Validator_Less_Than_Eight_Characters(string password)
    {
        InputValidator inputValidator = new InputValidator();
        InputResponse InputResponse = new InputResponse();
        InputResponse = inputValidator.IsValidPassword(password);
        Assert.False(InputResponse.success);


    }

    [Theory]

    [InlineData("password")]
    [InlineData("passwo@#$)U)")]
    public void Test_Password_Contains_No_Numbers(string password)
    {
        InputValidator inputValidator = new InputValidator();
        InputResponse InputResponse = new InputResponse();
        InputResponse = inputValidator.IsValidPassword(password);
        Assert.False(InputResponse.success);

    }


    [Theory]

    [InlineData("password2")]
    [InlineData("passwo@#$)U)1")]
    public void Test_Password_Contains_A_Number(string password)
    {
        InputValidator inputValidator = new InputValidator();
        InputResponse InputResponse = new InputResponse();
        InputResponse = inputValidator.IsValidPassword(password);
        Assert.True(InputResponse.success);

    }

    [Fact]
    public void Test_Null_Input_Password()
    {
        InputValidator inputValidator = new InputValidator();
        InputResponse InputResponse = new InputResponse();
        InputResponse = inputValidator.IsValidPassword(string.Empty);
        Assert.False(InputResponse.success);
    }

    [Theory]

    [InlineData("8 approve")]
    [InlineData("11 approve")]
    [InlineData("9 deny")]
    [InlineData("99 deny")]
    [InlineData("100 approve")]
    [InlineData("100 deny")]
    public void Test_Manager_Input_Accepts_Digits(string choice)
    {
        InputValidator inputValidator = new InputValidator();
        
        Assert.True(inputValidator.IsValidManagerChoice(choice).success);

    }


}