using System.Collections.ObjectModel;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Terminal.Gui;

namespace ThreesTUI.Logging;
public class UILogSink(string outputTemplate = "[{Level:u3}] {Message:lj}")
    : ILogEventSink
{
    public ObservableCollection<string> Logs { get; } = new();
    private readonly ITextFormatter _textFormatter = new MessageTemplateTextFormatter(outputTemplate);

    public void Emit(LogEvent logEvent)
    {
        var writer = new StringWriter();
        _textFormatter.Format(logEvent, writer);
        Application.Invoke(() => Logs.Add(writer.ToString()));
    }
}