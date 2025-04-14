namespace ThreesTUI.Server;

public class NakamaConsoleLogger : Nakama.ILogger
{
    public void DebugFormat(string format, params object[] args)
    {
        Console.WriteLine("DEBUG: "+ format, args);
    }

    public void ErrorFormat(string format, params object[] args)
    {
        Console.Error.WriteLine(format, args);
    }

    public void InfoFormat(string format, params object[] args)
    {
        Console.WriteLine("INFO: " + format, args);
    }

    public void WarnFormat(string format, params object[] args)
    {
        Console.WriteLine("WARN: " + format, args);
    }
}