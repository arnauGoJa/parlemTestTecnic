using Core;
using Domain.Interfaces;
using FluentAssertions;

namespace ParlemTestTecnicUnitTest
{
    public class CustomerValidatorTests
    {
        private readonly ICustomerValidator _customerValidatorService;

        public CustomerValidatorTests()
        {
            _customerValidatorService = new CustomerValidator();
        }

        [Fact]
        public void ValidateCustomerId_ShouldReturnSuccess_WhenBothIdAndPartitionKeyAreValid()
        {
           
            string validId = "12345";
            string validPartitionKey = "partitionKey";

          
            var result = _customerValidatorService.ValidateCustomerId(validId, validPartitionKey);

            
            result.Success.Should().BeTrue();
            result.Message.Should().BeNull(); 
        }

        [Fact]
        public void ValidateCustomerId_ShouldReturnFail_WhenIdIsNull()
        {
           
            string nullId = null;
            string validPartitionKey = "partitionKey";

          
            var result = _customerValidatorService.ValidateCustomerId(nullId, validPartitionKey);

            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer ID is required.");
        }

        [Fact]
        public void ValidateCustomerId_ShouldReturnFail_WhenPartitionKeyIsNull()
        {
           
            string validId = "12345";
            string nullPartitionKey = null;

          
            var result = _customerValidatorService.ValidateCustomerId(validId, nullPartitionKey);

            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer partitionKey is required.");
        }

        [Fact]
        public void ValidateCustomerId_ShouldReturnFail_WhenBothIdAndPartitionKeyAreNull()
        {
           
            string nullId = null;
            string nullPartitionKey = null;

          
            var result = _customerValidatorService.ValidateCustomerId(nullId, nullPartitionKey);

            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer partitionKey is required.");
        }

        [Fact]
        public void ValidateCustomerId_ShouldReturnFail_WhenBothIdAndPartitionKeyAreEmpty()
        {
           
            string emptyId = "";
            string emptyPartitionKey = "";

          
            var result = _customerValidatorService.ValidateCustomerId(emptyId, emptyPartitionKey);

            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer partitionKey is required.");
        }
    }
}
