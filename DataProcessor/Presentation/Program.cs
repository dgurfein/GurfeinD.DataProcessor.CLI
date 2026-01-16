using Microsoft.Extensions.DependencyInjection;
using DataProcessor.Application.DTOs;
using DataProcessor.Application.Interfaces;
using DataProcessor.Presentation;
using DataProcessor.Application.UseCases;
using DataProcessor.Application.Registration;

class Program
{
    static string CLI_USAGE = "DataProcessor [-u <useCaseName>] [<input>]";
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        // Register Presentation
        services.AddSingleton<IInputProvider, ConsoleReader>();
        services.AddSingleton<IOutputWriter, ConsoleWriter>();

        // Register Application
        ApplicationServiceInitializer.ServicesRegistration(services);

        var provider = services.BuildServiceProvider();

        var inputProvider = provider.GetRequiredService<IInputProvider>();
        var outputWriter = provider.GetRequiredService<IOutputWriter>();
        var parser = provider.GetRequiredService<IParser<MissingNumberRequest>>();
        var validator = provider.GetRequiredService<IValidator<MissingNumberRequest>>();
        var processor = provider.GetRequiredService<IProcessor<MissingNumberRequest, MissingNumberResult>>();
        var resolver = provider.GetRequiredService<IUseCaseResolver>();

        string? expectedInput = null; // used for exceptions
        try
        {
            // Parse args
            ParseArgs(args, out string useCaseName, out string? rawInput);
            // Get the name of the injected use case based on its name
            var useCase = resolver.Resolve(useCaseName);
            // set the command line usage for cases of exception
            expectedInput = useCase.CommandLineUsage;
            // If user did not provide input at command line, prompt for input
            if (rawInput == null)
            {
                rawInput = inputProvider.Read(useCase.Prompt);
            }
            // parse the input to the request DTO structure
            var request = useCase.ParseInput(rawInput);
            // validate the input
            useCase.ValidateInput(request);
            // process the data to get result
            var result = useCase.Process(request);

            outputWriter.Write(useCase.FormatOutput(result));
        }
        catch (Exception ex)
        {
            outputWriter.Write($"Error: {ex.Message}");
            outputWriter.Write($"\nUsage: {CLI_USAGE}");
            outputWriter.Write($"Available use cases: {string.Join(", ", Enum.GetNames(typeof(UseCaseNames)))}");
            outputWriter.Write($"Default use case: {UseCaseNames.MissingNumberArray.ToString()}");
            // Show input details if available on use case
            if (expectedInput != null) 
            {
                outputWriter.Write($"Where\n\t{expectedInput}");
            }
            outputWriter.Write("If input not provided on command line provide it when prompted");
        }
    }
    /// <summary>
    /// Command line argument parser. Expected format:
    /// DataProcessor [-u <UseCase>] rest_of_input
    /// </summary>
    /// <param name="args">array of the command line arguments</param>
    /// <param name="useCaseName">Output - Name of use case</param>
    /// <param name="rawInput">Output - additional input if present</param>
    /// <exception cref="Exception">If command line not per specification</exception>
    private static void ParseArgs(string[] args, out string useCaseName, out string? rawInput)
    {
        useCaseName = "MissingNumberArray";
        rawInput = null;

        //if (args == null || args.Length == 0)
        //    throw new Exception("No input provided. Usage: DataProcessor [-u <UseCase>] <input>");

        int index = 0;
        if (args.Length > 0)
        {
            if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length < 2)
                {
                    throw new Exception("Invalid usage. Must provide use case name after -u.");
                }
                useCaseName = args[1];
                index = 2;
            }
        }

        if (index < args.Length)
            rawInput = string.Join(' ', args[index..]);
        // else rawInput remains null → use case can prompt if needed
    }
}