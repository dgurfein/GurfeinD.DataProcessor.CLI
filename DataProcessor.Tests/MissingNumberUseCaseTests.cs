using DataProcessor.Application.DTOs;
using DataProcessor.Application.Interfaces;
using DataProcessor.Application.Registration;

using Microsoft.Extensions.DependencyInjection;

namespace DataProcessor.Tests
{
    public class MissingNumberUseCaseTests
    {
        private readonly IUseCase _useCase;

        public MissingNumberUseCaseTests()
        {
            var services = new ServiceCollection();
            ApplicationServiceInitializer.ServicesRegistration(services);
            var provider = services.BuildServiceProvider();
            var resolver = provider.GetRequiredService<IUseCaseResolver>();
            _useCase = resolver.Resolve("MissingNumberArray");
        }

        [Theory]
        [InlineData("[3,0,1]", 2)]
        [InlineData("[9,6,4,2,3,5,7,0,1]", 8)]
        [InlineData("[0]", 1)]
        [InlineData("[1,0]", 2)]
        public void Execute_ReturnsMissingNumber_ForValidInput(string input, int expected)
        {
            var parsed = _useCase.ParseInput(input);
            _useCase.ValidateInput(parsed);
            var result = _useCase.Process(parsed);

            Assert.Equal(expected, ((MissingNumberResult)result).MissingNumber);
        }

        // ---------------------------
        // Parser exceptions
        // ---------------------------
        [Theory]
        [InlineData("[1,2,abc]")]
        [InlineData("[x,y,z]")]
        public void Parser_ThrowsException_ForInvalidInteger(string input)
        {
            Assert.Throws<Exception>(() => _useCase.ParseInput(input));
        }

        // ---------------------------
        // Validator exceptions
        // ---------------------------
        [Theory]
        [InlineData("[0,1,1,2]")]
        [InlineData("[2,2,0]")]
        public void Validator_ThrowsException_ForDuplicateValues(string input)
        {
            var parsed = _useCase.ParseInput(input);
            Assert.Throws<Exception>(() => _useCase.ValidateInput(parsed));
        }

        [Theory]
        [InlineData("[0,1,4]")]
        [InlineData("[0,5,1]")]
        public void Validator_ThrowsException_ForOutOfRangeValue(string input)
        {
            var parsed = _useCase.ParseInput(input);
            Assert.Throws<Exception>(() => _useCase.ValidateInput(parsed));
        }

        // ---------------------------
        // Empty input
        // ---------------------------
        [Fact]
        public void Parser_AllowsEmptyArray()
        {
            string input = "[]";
            var parsed = _useCase.ParseInput(input);
            Assert.Empty(((MissingNumberRequest)parsed).NumberArray);
        }
    }
}