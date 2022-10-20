using Microsoft.Data.SqlClient;


public class DbContext 
{

    private SqlConnection _connection;


    private string _connectionString = "Server=tcp:revaturecsharp.database.windows.net,1433;Initial Catalog=TestDB;Persist Security Info=False;User ID=revatureChsarpAdmin;Password=RevatureCsharp123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public DbContext()
    {
        _connection = new SqlConnection(_connectionString);
    }

    public ServiceResponse<string> CreateUser(RegisterAccountDto newEmployee)
    {
            ServiceResponse<string> createUserResponse = new ServiceResponse<string>(); 

        if (userAlreadyExists(newEmployee.UserName))
        {
            createUserResponse.Message = "Sorry username already exists.";
            createUserResponse.Success = false;
            return createUserResponse;
        }
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec createLogin @userName, @password ", _connection);
            cmd.Parameters.AddWithValue("@userName", newEmployee.UserName);
            cmd.Parameters.AddWithValue("@password", newEmployee.Password);
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
                createUserResponse.Success = false;
                createUserResponse.Message = "There was an issue adding user to database.";
                return createUserResponse;
            }
            _connection.Close();
            _connection.Open();
            if (loginId != null)
            {
                SqlCommand cmdEmployee = new SqlCommand("exec createEmployee @name, @loginId;", _connection);
                cmdEmployee.Parameters.AddWithValue("@name", newEmployee.Name);
                cmdEmployee.Parameters.AddWithValue("@loginId", loginId);
                int RowsAffect = cmdEmployee.ExecuteNonQuery();
                createUserResponse.Success = true;
                createUserResponse.Message = "User created successfully!";
            }
            else
            {
                createUserResponse.Success = false;
                return createUserResponse;
            }

        }
        catch (SqlException)
        {
            createUserResponse.Message = "There was an error creating user, please try again.";
            createUserResponse.Success = false;
        }
        finally
        {
            _connection.Close();
        }

        return createUserResponse;
    }

  

    public ServiceResponse<LoginDto> Login(Login login)
    {

        ServiceResponse<LoginDto> loginResponse = new ServiceResponse<LoginDto>(); 
       
        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("exec sp_getUserLoginId @user_name, @password;", _connection);
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
               
                loginResponse.Message = "Sorry could not log you in. Please make sure your password and username are correct.";
                loginResponse.Success = false;
                return loginResponse;
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
                    LoginDto loginDto = new LoginDto()
                    {
                        
                        Id = (int)getUserReader["Id"],
                        Name = getUserReader["name"].ToString()!,
                        IsManager = bool.Parse(getUserReader["IsManager"].ToString()!.ToLower())
                    };
                   
                    loginResponse.Data = loginDto;
                    loginResponse.Success = true;
                    loginResponse.Message = "Successfully logged in!";
                    return loginResponse;
                }

            }
            else
            {
                loginResponse.Success = false;
                loginResponse.Message = "Sorry one or more inputs are incorrect";
            }



        }
        catch (SqlException)
        {
            loginResponse.Message = "Issue connecting to database";
            loginResponse.Success = false;
            return loginResponse;

        }
        finally
        {
            _connection.Close();
        }


        return loginResponse;

    }

    public ServiceResponse<string> createTicket(Ticket ticket)
    {
        ServiceResponse<string> createTicketRes = new ServiceResponse<string>();

    

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
                createTicketRes.Message = "Successfully created Ticket.";
                createTicketRes.Success = true;
                _connection.Close();
            }
        
        }
        catch (SqlException)
        {
            createTicketRes.Message = "There was an error creating ticket, please try again";
            createTicketRes.Success = false;

        }
        finally
        {
            _connection.Close();
        }

        return createTicketRes;
    }

    public ServiceResponse<List<TicketDto>> GetPendingTickets()
    {

        ServiceResponse<List<TicketDto>> createTicketRes = new ServiceResponse<List<TicketDto>>();
        List<TicketDto> ticketList = new List<TicketDto>();

        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("exec getPendingTickets ", _connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {   
                    TicketDto newTicket = new TicketDto((int)reader["TicketId"], reader["Description"].ToString()!,reader["Name"].ToString()!,(decimal)reader["Amount"],reader["Status"].ToString()!,(int)reader["EmployeeId"]);
                    ticketList.Add(newTicket);

                }
                createTicketRes.Data = ticketList;
                createTicketRes.Success = true;
            }
            else
            {
                createTicketRes.Message = "Sorry could not log you in. Please check your password and username!";
                createTicketRes.Success = false;

            }
        }
        catch (SqlException )
        {
            createTicketRes.Message = "Issue retrieving tickets, please try again!";
            createTicketRes.Success = false;


        } 
        finally
        {
            _connection.Close();
        }
        return createTicketRes;
    }



    public ServiceResponse<List<TicketDto>> getTicketsById(int id)
    {
       
        ServiceResponse<List<TicketDto>> getUserTicketRes  = new ServiceResponse<List<TicketDto>>(); 


        List<TicketDto> ticketList = new List<TicketDto>();

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
                    TicketDto newTicket = new TicketDto((int)reader["TicketId"], reader["Description"].ToString()!,reader["Name"].ToString()!,(decimal)reader["Amount"],reader["Status"].ToString()!,(int)reader["EmployeeId"]);
                    ticketList.Add(newTicket);

                }
                getUserTicketRes.Data = ticketList;
                getUserTicketRes.Success = true;
            }
            else
            {
                getUserTicketRes.Message = "No Tickets to show!";
                getUserTicketRes.Success = false;

            }

        }
        catch (SqlException)
        {
            getUserTicketRes.Message = "There was an issue retrieving tickets!";
            getUserTicketRes.Success = false;

        }
        finally
        {
            _connection.Close();
        }
        return getUserTicketRes;
    }


    public ServiceResponse<string> UpdateTicket(int? id, string? newStatus)
    {

        ServiceResponse<string> updateTicketRes = new ServiceResponse<string>(); 
       
        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("exec updateTicket @newStatus, @ticketId", _connection);
            command.Parameters.AddWithValue("@ticketId", id);
            command.Parameters.AddWithValue("@newStatus", newStatus);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 1)
            {
                updateTicketRes.Message = "Succesfully updated ticket";
                updateTicketRes.Success = true;
            }
            else
            {
                updateTicketRes.Message = "Sorry unable to update ticket";
                updateTicketRes.Success = false;
            }


        }
        catch (SqlException)
        {
            updateTicketRes.Message = "Issue updating tickets, please try again!";
            updateTicketRes.Success = false;


        }
        finally
        {
            _connection.Close();
        }


        return updateTicketRes;

    }


    // public ServiceResponse<string> AddEmployeePhoto(byte[] photo, int employeeId){
    //     ServiceResponse<string> addPhotoRes = new ServiceResponse<string>(); 
    //     try
    //     {
    //         _connection.Open();
    //         SqlCommand cmd = new SqlCommand("exec setEmployeeImage @photo, @EmployeeId;", _connection);
    //         cmd.Parameters.AddWithValue("@photo", photo);
    //         cmd.Parameters.AddWithValue("@employeeId", employeeId);

    //         int rowsAffected = cmd.ExecuteNonQuery();

    //         if(rowsAffected == 1){
    //             addPhotoRes.Success = true; 
    //             addPhotoRes.Message = "Successfully Added Photo."; 
    //         } else {

    //             addPhotoRes.Success = false;
    //             addPhotoRes.Message = "Unable to add photo, please try again."; 
    //         }

            


    //     }
    //     catch (SqlException)
    //     {
    //         addPhotoRes.Success = false; 
    //         addPhotoRes.Message = "Unexpected error please try again later!"; 
    //         return addPhotoRes; 
            
    //     }
    //     finally
    //     {
    //         _connection.Close();
    //     }
    //     return addPhotoRes; 
    // }

    public bool userAlreadyExists(String userName)
    {
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec userAlreadyExists @userName;", _connection);
            cmd.Parameters.AddWithValue("@userName", userName);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return true;
            }
            else
            {
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