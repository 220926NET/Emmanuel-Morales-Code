using Microsoft.Data.SqlClient;
using Models;

class SqlClient : IRequest
{

    private SqlConnection _connection;


    private string _connectionString = "Server=tcp:revaturecsharp.database.windows.net,1433;Initial Catalog=TestDB;Persist Security Info=False;User ID=revatureChsarpAdmin;Password=RevatureCsharp123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public SqlClient()
    {
        _connection = new SqlConnection(_connectionString);
    }

    public ResponseMessage<string> PostCreateUser(User user)
    {
        ResponseMessage<string> postUserResponse = new ResponseMessage<string>();

        if (userAlreadyExists(user.login.UserName))
        {
            postUserResponse.message = "Sorry username already exists";
            postUserResponse.success = false;
            return postUserResponse;
        }
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Logins (User_name, passsword) OUTPUT Inserted.Id VALUES  (@userName, @password);", _connection);
            cmd.Parameters.AddWithValue("@userName", user.login.UserName);
            cmd.Parameters.AddWithValue("@password", user.login.Password);

            SqlDataReader reader = cmd.ExecuteReader();
            int? loginId = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    loginId = (int)reader["id"];

                }

            }
            else
            {
                postUserResponse.success = false;
                postUserResponse.message = "Issue adding user to database.";
                return postUserResponse;
            }
            _connection.Close();
            _connection.Open();
            if (loginId != null)
            {
                SqlCommand cmdEmployee = new SqlCommand("INSERT INTO employees (Name, Login_id) VALUES(@name, @loginId);", _connection);
                cmdEmployee.Parameters.AddWithValue("@name", user.Name);
                cmdEmployee.Parameters.AddWithValue("@loginId", loginId);
                int RowsAffect = cmdEmployee.ExecuteNonQuery();
                postUserResponse.success = true;
                postUserResponse.message = "Successfully created user";
            }
            else
            {
                postUserResponse.success = false;
                return postUserResponse;
            }

        }
        catch (SqlException)
        {

            postUserResponse.message = "error creating user, please try again.";
            postUserResponse.success = false;
        }
        finally
        {
            _connection.Close();
        }

        return postUserResponse;
    }

    // Post the users login information and retrieves the user Id to references resources 

    public ResponseMessage<User> PostLogin(Login login)
    {
        ResponseMessage<User> loginUserResponse = new ResponseMessage<User>();
        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("SELECT Id FROM Logins WHERE User_name = @user_name And passsword=@password;", _connection);
            command.Parameters.AddWithValue("@user_name", login.UserName);
            command.Parameters.AddWithValue("@password", login.Password);

            SqlDataReader reader = command.ExecuteReader();
            int? loginId = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    loginId = (int)reader["Id"];
                }

            }
            else
            {
                loginUserResponse.message = "Sorry could not log you in. Please check your password and username!";
                loginUserResponse.success = false;
                return loginUserResponse;
            }
            _connection.Close();

            _connection.Open();
            SqlCommand getUserCmd = new SqlCommand("SELECT Id, name, Is_Manger FROM Employees WHERE Login_id = @LoginId;", _connection);
            getUserCmd.Parameters.AddWithValue("@LoginId", loginId);
            SqlDataReader getUserReader = getUserCmd.ExecuteReader();
            if (getUserReader.HasRows)
            {
                bool isManger = false;
                while (getUserReader.Read())
                {
                    User newUser = new User()
                    {
                        Id = (int)getUserReader["Id"],
                        Name = getUserReader["name"].ToString(),
                    };
                    if (getUserReader["Is_Manger"].ToString() == "1")
                    {
                        isManger = true;
                    }
                    newUser.IsManager = isManger;
                    loginUserResponse.data = newUser;
                    loginUserResponse.success = true;
                    loginUserResponse.message = "Successfully logged in!";
                    return loginUserResponse;
                }

            }
            else
            {
                loginUserResponse.success = false;
                loginUserResponse.message = "Sorry one or more inputs are incorrect";
            }



        }
        catch (SqlException ex)
        {
            loginUserResponse.message = "Issue connecting to database";
            loginUserResponse.success = false;
            return loginUserResponse;

        }
        finally
        {
            _connection.Close();
        }


        return loginUserResponse;

    }

    public ResponseMessage<string> PostTicket(Ticket ticket)
    {

        ResponseMessage<string> postTicketRes = new ResponseMessage<string>();

        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("Insert into Tickets (Description,Amount, EmployeeId) VALUES(@description,@amount,@employeeId);", _connection);
            cmd.Parameters.AddWithValue("@description", ticket.Description);
            cmd.Parameters.AddWithValue("@amount", ticket.Amount);
            cmd.Parameters.AddWithValue("@employeeId", ticket.EmployeeId);

            int rowsAffected = cmd.ExecuteNonQuery();


            if (rowsAffected == 1)
            {
                postTicketRes.message = "Successfully created Ticket";
                postTicketRes.success = true;

                _connection.Close();
            }
            _connection.Close();

        }
        catch (SqlException e)
        {
            postTicketRes.message = "Error creating ticket, please try again";
            postTicketRes.success = false;

        }
        finally
        {
            _connection.Close();
        }

        return postTicketRes;


    }

    public ResponseMessage<List<Ticket>> GetPendingTickets()
    {
        ResponseMessage<List<Ticket>> getPendingTicketsRes = new ResponseMessage<List<Ticket>>();
        List<Ticket> ticketList = new List<Ticket>();

        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("Select * from tickets where status = 'pending' ", _connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Ticket newTicket = new Ticket(reader["Description"].ToString(), (decimal)reader["amount"], (int)reader["EmployeeId"]);
                    ticketList.Add(newTicket);

                }
                getPendingTicketsRes.data = ticketList;
                getPendingTicketsRes.success = true;
            }
            else
            {
                getPendingTicketsRes.message = "Sorry could not log you in. Please check your password and username!";
                getPendingTicketsRes.success = false;

            }
            _connection.Close();




        }
        catch (SqlException ex)
        {
            getPendingTicketsRes.message = "Issue retrieving tickets, please try again!";
            getPendingTicketsRes.success = false;


        }
        finally
        {
            _connection.Close();
        }
        return getPendingTicketsRes;
    }

    public ResponseMessage<List<Ticket>> GetUserTickets(int id)
    {
        ResponseMessage<List<Ticket>> getUserTicketRes = new ResponseMessage<List<Ticket>>();
        List<Ticket> ticketList = new List<Ticket>();

        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("Select * from tickets where EmployeeId = @employeeId; ", _connection);
            command.Parameters.AddWithValue("@employeeId", id);
            SqlDataReader reader = command.ExecuteReader();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Ticket newTicket = new Ticket(reader["Description"].ToString(), (decimal)reader["amount"], (int)reader["EmployeeId"]);

                    ticketList.Add(newTicket);

                }
                getUserTicketRes.data = ticketList;
                getUserTicketRes.success = true;
            }
            else
            {
                getUserTicketRes.message = "No Tickets to show!";
                getUserTicketRes.success = false;

            }
            _connection.Close();




        }
        catch (SqlException ex)
        {
            getUserTicketRes.message = "Issue retrieving tickets, please try again!";
            getUserTicketRes.success = false;


        }
        finally
        {
            _connection.Close();
        }
        return getUserTicketRes;
    }


    public ResponseMessage<string> UpdateTicket(int id, string newStatus)
    {
        ResponseMessage<string> updateTicketRes = new ResponseMessage<string>();
        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("Update Tickets SET status = @newStatus  WHERE id = @ticketId;  ", _connection);
            command.Parameters.AddWithValue("@ticketId", id);
            command.Parameters.AddWithValue("@newStatus", newStatus);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 1)
            {
                updateTicketRes.message = "Succesfully updated ticket";
                updateTicketRes.success = true;
            }
            else
            {
                updateTicketRes.message = "Sorry unable to update ticket";
                updateTicketRes.success = false;
            }


            _connection.Close();




        }
        catch (SqlException ex)
        {
            updateTicketRes.message = "Issue updating tickets, please try again!";
            updateTicketRes.success = false;


        }
        finally
        {
            _connection.Close();
        }


        return updateTicketRes;

    }


    public bool userAlreadyExists(String userName)
    {
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Logins WHERE User_name = @userName;", _connection);
            cmd.Parameters.AddWithValue("@userName", userName);
        
            SqlDataReader reader = cmd.ExecuteReader();

            if(reader.Read()){
                return true; 
            } else {
                return false;
            }
            

        }
        catch (SqlException)
        {

            return true;
        }
        finally
        {
            _connection.Close();
        }
    }

}