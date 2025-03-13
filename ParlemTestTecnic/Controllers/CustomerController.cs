using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ParlemTestTecnic.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}/{partitionKey}")]
    public async Task<IActionResult> GetById(string id, string partitionKey)
    {
        var result = await _customerService.GetCustomerByIdAsync(id, partitionKey);

        return result.Success ? Ok(result) : NotFound(result);
    }
    [HttpGet("{customerId}/products")]
    public async Task<IActionResult> GetCustomerWithProducts(string id, string customerId)
    {
        var result = await _customerService.GetCustomerWithProductsAsync(id, customerId);

        return result.Success ? Ok(result) : NotFound(result);
    }
}
