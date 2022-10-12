
using Models;
public class TestSqlclient
{



    // [Fact]
    // public void Test_Can_Create_User()
    // {
    //     // Arrange
    //     SqlClient sqlClient = new SqlClient();
    //     ResponseMessage<string> createUserResponse = new ResponseMessage<string>();

    //     User user = new User()
    //     {
    //         Name = "Emmanuel",
    //         login = new Login()
    //         {
    //             UserName = "test123",
    //             Password = "password123",
    //         },
    //         IsManager = false,
    //     };
    //     // Act 
    //     createUserResponse = sqlClient.PostCreateUser(user);
    //     // Assert

    //     Assert.True(createUserResponse.success);
    //     Assert.Contains("User created successfully!", createUserResponse.message);
    // }

    [Fact]
    public void TEST_CREATE_DUPLICATE_USERNAME()
    {
        // Arrange
        SqlClient sqlClient = new SqlClient();
        ResponseMessage<string> createUserResponse = new ResponseMessage<string>();

        User user = new User()
        {
            Name = "Emmanuel",
            login = new Login()
            {
                UserName = "test123",
                Password = "password123",
            },
            IsManager = false,
        };
        // Act 
        createUserResponse = sqlClient.PostCreateUser(user);
        // Assert

        Assert.False(createUserResponse.success);
        Assert.Contains("Sorry username already exists.", createUserResponse.message);
    }

    [Fact]
    public void TEST_CAN_RETREIVE_ID()
    {
        SqlClient sqlClient = new SqlClient();
        ResponseMessage<User> loginUserRes = new ResponseMessage<User>();
        Login login = new Login()
        {
            UserName = "test123",
            Password = "password123",
        };

        loginUserRes = sqlClient.PostLogin(login);


        Assert.Contains("Emmanuel", loginUserRes.data.Name);
        Assert.True(loginUserRes.data.Id > 0);
    }

    // ensure no username exists with userName "johnny123" with pass : "password123"
    [Fact]
    public void TEST_LOGIN_BAD_LOGIN()
    {
        SqlClient sqlClient = new SqlClient();
        ResponseMessage<User> loginUserRes = new ResponseMessage<User>();
        Login login = new Login()
        {
            UserName = "johnny123",
            Password = "password123",
        };

        loginUserRes = sqlClient.PostLogin(login);

        Assert.Contains("Sorry could not log you in. Please make sure your password and username are correct.", loginUserRes.message);
        Assert.False(loginUserRes.success);
    }

    // ensure an employee with user id 1 exists
    [Fact]
    public void TEST_CAN_POST_VALID_TICKET(){
        SqlClient sqlClient = new SqlClient();
        ResponseMessage<string> postTicketRes = new ResponseMessage<string>();
        Ticket ticket = new Ticket("Flight to new York", 200m, 1); 

        postTicketRes = sqlClient.PostTicket(ticket);

        Assert.Contains("Successfully created Ticket.", postTicketRes.message);
        Assert.True(postTicketRes.success);
    }


    [Fact]

    public void TEST_BAD_TICKET_POST(){
        
    }
}








