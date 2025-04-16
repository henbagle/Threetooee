using Microsoft.Extensions.Logging;
using ThreesTUI.Server;

namespace ThreesTUI.Logging;

public class NakamaSerilogAdapter(ILogger<NakamaClient> logger) : Nakama.ILogger
{
    public void DebugFormat(string format, params object[] args)
    {
        logger.LogDebug(format, args);
    }

    public void InfoFormat(string format, params object[] args)
    {
        logger.LogInformation(format, args);
    }

    public void WarnFormat(string format, params object[] args)
    {
        logger.LogWarning(format, args);
    }
    
    public void ErrorFormat(string format, params object[] args)
    {
        logger.LogError(format, args);
    }
}