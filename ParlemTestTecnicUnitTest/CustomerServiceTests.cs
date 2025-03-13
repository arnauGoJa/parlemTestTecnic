using Moq;
using FluentAssertions;
using Domain.Interfaces;
using Core;
using Domain.Entities;
using Domain.Models;

namespace ParlemTestTecnicUnitTest
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<ICustomerValidator> _mockCustomerValidator;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockCustomerValidator = new Mock<ICustomerValidator>();
            _customerService = new CustomerService(_mockCustomerRepository.Object, _mockCustomerValidator.Object);
        }


        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenValidationSucceedsAndCustomerExists()
        {

            string validId = "12345";
            string validPartitionKey = "partitionKey";
            var customer = new Customer(validId, "nif", "11223344E", "it@parlem.com", "11111", "Enriqueta", "Parlem", "668668668", "customer");
            _mockCustomerValidator.Setup(v => v.ValidateCustomerId(validId, validPartitionKey))
                .Returns(Result<bool>.Ok(true));
            _mockCustomerRepository.Setup(r => r.GetByIdAsync(validId, validPartitionKey))
                .ReturnsAsync(Result<Customer>.Ok(customer));


            var result = await _customerService.GetCustomerByIdAsync(validId, validPartitionKey);


            result.Success.Should().BeTrue();
            result.Data.Should().Be(customer);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnFail_WhenValidationFails()
        {

            string invalidId = "";
            string validPartitionKey = "partitionKey";
            _mockCustomerValidator.Setup(v => v.ValidateCustomerId(invalidId, validPartitionKey))
                .Returns(Result<bool>.Fail("Customer ID is required."));


            var result = await _customerService.GetCustomerByIdAsync(invalidId, validPartitionKey);


            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer ID is required.");
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnFail_WhenCustomerNotFound()
        {

            string validId = "12345";
            string validPartitionKey = "partitionKey";
            _mockCustomerValidator.Setup(v => v.ValidateCustomerId(validId, validPartitionKey))
                .Returns(Result<bool>.Ok(true));
            _mockCustomerRepository.Setup(r => r.GetByIdAsync(validId, validPartitionKey))
                .ReturnsAsync(Result<Customer>.Fail("Customer not found"));


            var result = await _customerService.GetCustomerByIdAsync(validId, validPartitionKey);


            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer not found");
        }

        [Fact]
        public async Task GetCustomerWithProductsAsync_ShouldReturnCustomerWithProducts_WhenValidationSucceedsAndCustomerExists()
        {

            string validId = "12345";
            string validPartitionKey = "partitionKey";
            var customerProducts = new CustomerProducts { id = validId };
            _mockCustomerValidator.Setup(v => v.ValidateCustomerId(validId, validPartitionKey))
                .Returns(Result<bool>.Ok(true));
            _mockCustomerRepository.Setup(r => r.GetCustomerWithProductsAsync(validId, validPartitionKey))
                .ReturnsAsync(Result<CustomerProducts>.Ok(customerProducts));


            var result = await _customerService.GetCustomerWithProductsAsync(validId, validPartitionKey);


            result.Success.Should().BeTrue();
            result.Data.Should().Be(customerProducts);
        }

        [Fact]
        public async Task GetCustomerWithProductsAsync_ShouldReturnFail_WhenValidationFails()
        {

            string invalidId = "";
            string validPartitionKey = "partitionKey";
            _mockCustomerValidator.Setup(v => v.ValidateCustomerId(invalidId, validPartitionKey))
                .Returns(Result<bool>.Fail("Customer ID is required."));


            var result = await _customerService.GetCustomerWithProductsAsync(invalidId, validPartitionKey);


            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer ID is required.");
        }

        [Fact]
        public async Task GetCustomerWithProductsAsync_ShouldReturnFail_WhenCustomerWithProductsNotFound()
        {

            string validId = "12345";
            string validPartitionKey = "partitionKey";
            _mockCustomerValidator.Setup(v => v.ValidateCustomerId(validId, validPartitionKey))
                .Returns(Result<bool>.Ok(true));
            _mockCustomerRepository.Setup(r => r.GetCustomerWithProductsAsync(validId, validPartitionKey))
                .ReturnsAsync(Result<CustomerProducts>.Fail("Customer not found"));


            var result = await _customerService.GetCustomerWithProductsAsync(validId, validPartitionKey);


            result.Success.Should().BeFalse();
            result.Message.Should().Be("Customer not found");
        }

    }
}
