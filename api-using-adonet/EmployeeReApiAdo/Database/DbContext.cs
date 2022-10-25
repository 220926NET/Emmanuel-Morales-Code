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

            SqlCommand command = new SqlCommand("exec loginUser @user_name, @password;", _connection);
            command.Parameters.AddWithValue("@user_name", login.UserName);
            command.Parameters.AddWithValue("@password", login.Password);

            SqlDataReader reader = command.ExecuteReader();
            int? loginId = null;
            string? userName = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    loginId = (int)reader["Id"];
                    userName = reader["User_name"].ToString();
                    // get userName s
                }

                LoginDto loginDto = new LoginDto
                {
                    Id = (int)loginId!,
                    UserName = userName!,
                };
                _connection.Close();
                loginResponse.Data = loginDto;
                loginResponse.Success = true;
                loginResponse.Message = "User exists";
                return loginResponse;

            }
            else
            {
                _connection.Close();
                loginResponse.Message = "Sorry could not log you in. Please make sure your password and username are correct.";
                loginResponse.Success = false;
                return loginResponse;
            }
            {

            }
            {

            }


        }
        catch (SqlException)
        {
            _connection.Close();
            loginResponse.Message = "There was an issue connecting to the database. Try again later.";
            loginResponse.Success = false;
            return loginResponse;

        }
        finally
        {
            _connection.Close();
        }




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

            }

        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
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
                    TicketDto newTicket = new TicketDto((int)reader["TicketId"], reader["Description"].ToString()!, reader["Name"].ToString()!, (decimal)reader["Amount"], reader["Status"].ToString()!, (int)reader["EmployeeId"]);
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
        catch (SqlException)
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

        ServiceResponse<List<TicketDto>> getUserTicketRes = new ServiceResponse<List<TicketDto>>();


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
                    TicketDto newTicket = new TicketDto((int)reader["TicketId"], reader["Description"].ToString()!, reader["Name"].ToString()!, (decimal)reader["Amount"], reader["Status"].ToString()!, (int)reader["EmployeeId"]);
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


    public ServiceResponse<string> UpdateTicket(UpdateTicketDto updateTicketDto)
    {

        ServiceResponse<string> updateTicketRes = new ServiceResponse<string>();

        try
        {
            _connection.Open();

            SqlCommand command = new SqlCommand("exec updateTicket @newStatus, @ticketId", _connection);
            command.Parameters.AddWithValue("@ticketId", updateTicketDto.TicketId);
            command.Parameters.AddWithValue("@newStatus", updateTicketDto.TicketStatus);
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


    public ServiceResponse<string> AddEmployeeDetails(EmployeeDetails employeeDetails, int employeeId)
    {
        ServiceResponse<string> addPhotoRes = new ServiceResponse<string>();


        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec addEmployeeDetails @Address , @State , @Zipcode , @EmployeeId , @Country;", _connection);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@Address", employeeDetails.Address);
            cmd.Parameters.AddWithValue("@State", employeeDetails.State);
            cmd.Parameters.AddWithValue("@Country", employeeDetails.Country);
            cmd.Parameters.AddWithValue("@Zipcode", employeeDetails.ZipCode);


            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 1)
            {
                addPhotoRes.Success = true;
                addPhotoRes.Message = "Successfully added employee details";
            }

        }
        catch (SqlException)
        {
            addPhotoRes.Success = false;
            addPhotoRes.Message = "Please delete your current details before updating.";
            return addPhotoRes;

        }
        finally
        {
            _connection.Close();
        }
        return addPhotoRes;
    }

    public ServiceResponse<string> UploadEmployeePhoto(int employeeId, string url, string extension)
    {

        ServiceResponse<string> addPhotoRes = new ServiceResponse<string>();
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec uploadUserPhotoUrl @url, @EmployeeId, @Extension", _connection);
            cmd.Parameters.AddWithValue("@url", url);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@Extension", extension);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 1)
            {
                addPhotoRes.Success = true;
                addPhotoRes.Message = "Successfully added employee photo";
            }
            else
            {

                addPhotoRes.Success = false;
                addPhotoRes.Message = "Unable to add employee photo, try again later";
            }


        }
        catch (SqlException)
        {
            addPhotoRes.Success = false;
            addPhotoRes.Message = "Unexpected error adding photo to database, please try again later!";
            return addPhotoRes;

        }
        finally
        {
            _connection.Close();
        }
        return addPhotoRes;
    }

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


    public bool IsManager(int employeeId)
    {

        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec getManagerStatus @employeeId;", _connection);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    bool isManager = (bool)reader["IsManager"];
                    return isManager;
                }


            }


            _connection.Close();
        }
        catch (SqlException)
        {

            return false;
        }
        finally
        {
            _connection.Close();
        }

        return false;
    }
    public bool hasProfilePhoto(int employeeId)
    {

        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec hasPhoto @employeeId;", _connection);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                _connection.Close();
                return true;
            }
            _connection.Close();
            return false;


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

    public ServiceResponse<string> getPhotoUrl(int employeeId)
    {
        ServiceResponse<string> employeePhotoRes = new ServiceResponse<string>();
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec getPhotoUrl @employeeId;", _connection);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            SqlDataReader reader = cmd.ExecuteReader();
            string url = string.Empty;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    url = reader["Photo"].ToString()!;
                }

                employeePhotoRes.Data = url;
                employeePhotoRes.Message = "Succesfully retreived photo url.";
                employeePhotoRes.Success = true;
            }
            else
            {
                employeePhotoRes.Data = null;
                employeePhotoRes.Message = "You do not have a photo set";
                employeePhotoRes.Success = true;
            }


            _connection.Close();



        }
        catch (SqlException)
        {

            employeePhotoRes.Message = "Error connecting to server, please try again later.";
            employeePhotoRes.Success = false;

        }
        finally
        {
            _connection.Close();


        }

        return employeePhotoRes;

    }

    public ServiceResponse<string> deleteUserPhoto(int employeeId)
    {
        ServiceResponse<string> deleteEmployeeRes = new ServiceResponse<string>();
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec deleteEmployeePhoto @employeeId;", _connection);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            SqlDataReader reader = cmd.ExecuteReader();
            string fileExtension = string.Empty;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    fileExtension = reader["Extension"].ToString()!;
                }

                deleteEmployeeRes.Data = fileExtension;
                deleteEmployeeRes.Success = true;
                deleteEmployeeRes.Message = "Succesfully deleted photo.";
            }
            else
            {
                deleteEmployeeRes.Data = null;
                deleteEmployeeRes.Success = true;
                deleteEmployeeRes.Message = "No Photo to delete.";
            }


        }
        catch (SqlException)
        {
            deleteEmployeeRes.Data = null;
            deleteEmployeeRes.Message = "Error connecting to server, please try again later.";
            deleteEmployeeRes.Success = false;

        }
        finally
        {
            _connection.Close();


        }

        return deleteEmployeeRes;

    }

    public ServiceResponse<string> UploadEmployeeReceipts(int employeeId, List<string> receiptsUrl, List<string> fileNames)
    {
        ServiceResponse<string> uploadReceiptsRes = new ServiceResponse<string>();
        try
        {
            _connection.Open();
            for (int i = 0; i < receiptsUrl.Count; i++)
            {
                SqlCommand cmd = new SqlCommand("exec addEmployeeReceipts @EmployeeId, @ReceiptUrl, @FileName;", _connection);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                cmd.Parameters.AddWithValue("@ReceiptUrl", receiptsUrl[i]);
                cmd.Parameters.AddWithValue("@FileName", fileNames[i]);
                cmd.ExecuteNonQuery();
            }

            uploadReceiptsRes.Data = null;
            uploadReceiptsRes.Message = "Added employee urls.";
            uploadReceiptsRes.Success = true;

        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            uploadReceiptsRes.Data = null;
            uploadReceiptsRes.Message = "Error connecting to server, please try again later.";
            uploadReceiptsRes.Success = false;

        }
        finally
        {
            _connection.Close();


        }

        return uploadReceiptsRes;

    }

    public bool EmployeeReceiptExists(int employeeId, string fileName)
    {

        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec getEmployeeReceipt @FileName,  @EmployeeId;", _connection);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                return true;
            }




        }
        catch (SqlException e)
        {
            Console.WriteLine("inside sql exception");
            Console.WriteLine(e);
            return false;

        }
        finally
        {
            _connection.Close();

        }

        return false;


    }

    public ServiceResponse<List<Receipt>> GetAllEmployeeReceipts(int employeeId)
    {
        ServiceResponse<List<Receipt>> getAllReceiptsRes = new ServiceResponse<List<Receipt>>();
        List<Receipt> receiptList = new List<Receipt>();
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec getAllEmployeeReceipts @EmployeeId;", _connection);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = int.Parse(reader["Id"].ToString()!);
                    string url = reader["URL"].ToString()!;

                    Receipt receipt = new Receipt
                    {
                        Id = id,
                        Url = url
                    };
                    receiptList.Add(receipt);

                }
                getAllReceiptsRes.Data = receiptList;
                getAllReceiptsRes.Message = "Receipts retrieved!";
                getAllReceiptsRes.Success = true;

            }
            else
            {

                getAllReceiptsRes.Message = "No receipts to show!";
                getAllReceiptsRes.Success = true;

            }




        }
        catch (SqlException)
        {
            getAllReceiptsRes.Message = "Error connecting to server, try again later.";
            getAllReceiptsRes.Success = false;

        }
        finally
        {
            _connection.Close();

        }

        return getAllReceiptsRes;

    }

    public ServiceResponse<List<string>> DeleteAllReceipts(int employeeId)
    {
        ServiceResponse<List<string>> getAllReceiptsRes = new ServiceResponse<List<string>>();
        List<string> fileNames = new List<string>();
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec deleteAllEmployeeReceipts @EmployeeId;", _connection);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            SqlDataReader reader = cmd.ExecuteReader();
            int rowCounter = 0;
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    rowCounter++;
                    string filename = reader["Filename"].ToString()!;
                    fileNames.Add(filename);

                }
                getAllReceiptsRes.Data = fileNames;
                getAllReceiptsRes.Message = $"Deleted {rowCounter} receipts.";
                getAllReceiptsRes.Success = true;

            }
            else
            {
                getAllReceiptsRes.Data = null;
                getAllReceiptsRes.Message = "No receipts to delete";
                getAllReceiptsRes.Success = true;

            }




        }
        catch (SqlException)
        {
            getAllReceiptsRes.Message = "Error connecting to server, try again later.";
            getAllReceiptsRes.Success = false;

        }
        finally
        {
            _connection.Close();

        }

        return getAllReceiptsRes;

    }

    public ServiceResponse<string> ChangeRoleToManger(int employeeId)
    {
        ServiceResponse<string> changeRoleRes = new ServiceResponse<string>();
        try
        {
            _connection.Open();
            SqlCommand cmd = new SqlCommand("exec changeRoleToManager @EmployeeId;", _connection);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {

                changeRoleRes.Message = "Changed employee role to manager!";
                changeRoleRes.Success = true;

            }




        }
        catch (SqlException)
        {
            changeRoleRes.Message = "Error connecting to server, try again later.";
            changeRoleRes.Success = false;

        }
        finally
        {
            _connection.Close();

        }

        return changeRoleRes;

    }


}