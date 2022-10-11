public static class Message
{

    public static void printErrorMessage(string message)
    {
        Console.WriteLine("-------------error------------------------------");
        Console.WriteLine($"{message}");
        Console.WriteLine("------------------------------------------------");

    }

    public static void printWelcomeMessage(string message)
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"\t\t WElCOME {message}");
        Console.WriteLine("------------------------------------------------");

    }

    public static void printMessage(string message)
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"\t{message}");
        Console.WriteLine("------------------------------------------------");

    }





}