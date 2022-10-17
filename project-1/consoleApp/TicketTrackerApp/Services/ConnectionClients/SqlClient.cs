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

        if (userAlreadyExists(user.login!.UserName!))
        {
            postUserResponse.message = "Sorry username already exists";
            postUserResponse.success = false;
            return postUserResponse;
        }
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec createLogin @userName, @password ", _connection);
            cmd.Parameters.AddWithValue("@userName", user.login.UserName);
            cmd.Parameters.AddWithValue("@password", user.login.getPassword());
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
                postUserResponse.message = "There was an issue adding user to database.";
                return postUserResponse;
            }
            _connection.Close();
            _connection.Open();
            if (loginId != null)
            {
                SqlCommand cmdEmployee = new SqlCommand("exec createEmployee @name, @loginId;", _connection);
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
            postUserResponse.message = "There was an error creating user, please try again.";
            postUserResponse.success = false;
        }
        finally
        {
            _connection.Close();
        }

        return postUserResponse;
    }

  

    public ResponseMessage<User> PostLogin(Login login)
    {
        ResponseMessage<User> loginUserResponse = new ResponseMessage<User>();
        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("exec sp_getUserLoginId @user_name, @password;", _connection);
            command.Parameters.AddWithValue("@user_name", login.UserName);
            command.Parameters.AddWithValue("@password", login.getPassword());

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
                loginUserResponse.message = "Sorry could not log you in. Please verify your password and username!";
                loginUserResponse.success = false;
                return loginUserResponse;
            }
            _connection.Close();
            _connection.Open();
            SqlCommand getUserCmd = new SqlCommand("exec sp_getLoginInfo @LoginId;", _connection);
            getUserCmd.Parameters.AddWithValue("@LoginId", loginId);
            SqlDataReader getUserReader = getUserCmd.ExecuteReader();
            if (getUserReader.HasRows)
            {
                
                while (getUserReader.Read())
                {
                    User newUser = new User()
                    {
                        
                        Id = (int)getUserReader["Id"],
                        Name = getUserReader["name"].ToString(),
                        IsManager = bool.Parse(getUserReader["IsManager"].ToString()!.ToLower())
                    };
                   
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
        catch (SqlException)
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
            SqlCommand cmd = new SqlCommand("exec createTicket @description, @amount, @employeeId;", _connection);
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
        
        }
        catch (SqlException)
        {
            postTicketRes.message = "There was an error creating ticket, please try again";
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

            SqlCommand command = new SqlCommand("exec sp_getPendingTickets ", _connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {   
                    Ticket newTicket = new Ticket((int)reader["TicketId"], reader["Description"].ToString()!,reader["Name"].ToString()!,(decimal)reader["Amount"],reader["Status"].ToString()!,(int)reader["EmployeeId"]);
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
        }
        catch (SqlException )
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

            SqlCommand command = new SqlCommand("exec sp_getTicketsByid @employeeId; ", _connection);
            command.Parameters.AddWithValue("@employeeId", id);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Ticket newTicket = new Ticket((int)reader["TicketId"], reader["Description"].ToString()!,reader["Name"].ToString()!,(decimal)reader["Amount"],reader["Status"].ToString()!,(int)reader["EmployeeId"]);

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

        }
        catch (SqlException)
        {
            getUserTicketRes.message = "There was an issue retrieving tickets, please contact your admin!";
            getUserTicketRes.success = false;

        }
        finally
        {
            _connection.Close();
        }
        return getUserTicketRes;
    }


    public ResponseMessage<string> UpdateTicket(int? id, string? newStatus)
    {
        ResponseMessage<string> updateTicketRes = new ResponseMessage<string>();
        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("exec updateTicket @newStatus, @ticketId", _connection);
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


        }
        catch (SqlException)
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
            SqlCommand cmd = new SqlCommand("exec userAlreadyExists @userName;", _connection);
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

            return false;
        }
        finally
        {
            _connection.Close();
        }
    }

}