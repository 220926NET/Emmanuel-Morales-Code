

public static class ConnectionFactory
{

    public static IRequest getSqlConnection()
    {
        SqlClient sqlclient = new SqlClient();
        return sqlclient;
    }


    public static IRequest getApiConnection()
    {
        ApiClient apiClient = new ApiClient();
        return apiClient;
    }
}