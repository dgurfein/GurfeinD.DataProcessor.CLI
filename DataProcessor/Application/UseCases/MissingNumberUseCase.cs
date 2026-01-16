using DataProcessor.Application.DTOs;
using DataProcessor.Application.Interfaces;
using DataProcessor.Application.Parsers;

namespace DataProcessor.Application.UseCases
{
    // This file holds the use case for the missing number case and all the processing implementations
    // All included in single file as they are short and simple. Consider splitting if it grows bigger.

    /// <summary>
    /// The missing number use case. 
    /// Takes an array containing numbers taken from the range 0 to n, find the one number that is missing from the array. and returns it    
    /// </summary>
    public sealed class MissingNumberUseCase : IUseCase
    {
        private readonly IParser<MissingNumberRequest> _parser;
        private readonly IValidator<MissingNumberRequest> _validator;
        private readonly IProcessor<MissingNumberRequest, MissingNumberResult> _processor;

        public string Name => UseCaseNames.MissingNumberArray.ToString();

        public string Prompt =>
            "Enter an array of n integers values must be between 0..n with no duplicates (e.g. [ 3, 0, 1 ]):";

        public string CommandLineUsage =>
            "input is an array of n integers with values between 0..n with no duplicates (e.g. [ 3, 0, 1 ])";

        public MissingNumberUseCase(
            IParser<MissingNumberRequest> parser,
            IValidator<MissingNumberRequest> validator,
            IProcessor<MissingNumberRequest, MissingNumberResult> processor)
        {
            _parser = parser;
            _validator = validator;
            _processor = processor;
        }

        public object ParseInput(string rawInput) => _parser.Parse(rawInput);

        public void ValidateInput(object input) => _validator.Validate((MissingNumberRequest)input);

        public object Process(object input) => _processor.Process((MissingNumberRequest)input);

        // can probably do with IFormatter for more complex output presentation, decided to keep implementation here at this stage
        public string FormatOutput(object result) => $"Missing number: {((MissingNumberResult)result).MissingNumber.ToString()}";
    }

    internal sealed class MissingNumberParser : IParser<MissingNumberRequest>
    {
        private readonly IntArrayParser _intParser = new();

        public MissingNumberRequest Parse(string rawInput)
        {
            // check if input is within closing [] brackets
            // remove array brackets if exists
            var trimmedInput = rawInput.Trim();
            if (trimmedInput.StartsWith('[') && trimmedInput.EndsWith(']')) 
            {
                trimmedInput = trimmedInput.TrimStart('[').TrimEnd(']');
            }
            // else - any other invalid prefix/sufix will result in parsing error
            return new MissingNumberRequest(_intParser.Parse(trimmedInput));
        } 
    }
    internal sealed class MissingNumberValidator : IValidator<MissingNumberRequest>
    {
        public void Validate(MissingNumberRequest input)
        {
            var nums = input.NumberArray;

            if (nums.Length == 0)
                throw new Exception("Array cannot be empty.");

            var n = nums.Length;
            if (nums.Any(x => x < 0 || x > n))
            {
                throw new Exception($"Values must be in range 0..{n}.");
            }
            // this rule will never fire if we parse with option to eliminate duplicates
            if (nums.Distinct().Count() != nums.Length)
            {
                throw new Exception("Duplicate values are not allowed.");
            }
        }
    }
    internal sealed class MissingNumberProcessor : IProcessor<MissingNumberRequest, MissingNumberResult>
    {
        public MissingNumberResult Process(MissingNumberRequest request)
        {
            var nums = request.NumberArray;

            var n = nums.Length;
            // Get the total sum of the complete array from 0..n  (good old Gauss technic never disapoints)
            var expectedTotal = n * (n + 1) / 2;
            var actual = nums.Sum();

            return new MissingNumberResult
            {
                MissingNumber = (expectedTotal - actual)
            }; 
        }
    }

}
