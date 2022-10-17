
/// <summary>
/// Class <c>Message</c> deals with printing strings that increase visibilty inside a terminal
/// </summary>

public static class Message
{
    /// <summary>
    /// <method><c>printErrorMessage<c></method> 
    /// <para>
    /// This method prints a string for an error and adds visibility inside terminal
    /// </para> 
    /// </summary>
    public static void printErrorMessage(string message)
    {
        Console.WriteLine("-------------error------------------------------");
        Console.WriteLine($"{message}");
        Console.WriteLine("------------------------------------------------");

    }

    /// <summary>
    /// <method><c>printWelcomeMessage<c></method> 
    /// <para>
    /// This method prints a string and adds visibility inside terminal
    /// </para> 
    /// </summary>
    public static void printWelcomeMessage(string message)
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"\t\t WELCOME {message}");
        Console.WriteLine("--------------------------------------------------");

    }

    /// <summary>
    /// <method><c>printMessage<c></method> 
    /// <para>
    /// This method prints a string to allow for better visibility inside terminal
    /// </para> 
    /// </summary>
    public static void printMessage(string message)
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"\t{message}");
        Console.WriteLine("--------------------------------------------------");

    }





}