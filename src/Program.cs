using CommandLine;

namespace BioFileDeleter;

public class Options
{
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
}

public static class Program
{
    private static Options? _options;
    private static CancellationTokenSource _cts = new();

    public static void Main(string[] args)
    {
        Console.CancelKeyPress += ConsoleOnCancelKeyPress;
        
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                _options = o;
                Task.Run(() => CheckForRemovalCycle());
            });
    }

    private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        WriteIfVerbose(e.Exception.ToString());
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        WriteIfVerbose(e.ExceptionObject.ToString() ?? "Unhandled not an exception");
    }

    private static void ConsoleOnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        _cts.Cancel();
    }

    private static void WriteIfVerbose(string msg)
    {
        if (_options is { Verbose: true })
            Console.WriteLine(msg);
    }

    private static void CheckForRemovalCycle()
    {
        while (!_cts.IsCancellationRequested)
        {
            
        }
    }
}