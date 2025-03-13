using Core;
using Microsoft.AspNetCore.Mvc;

namespace ParlemTestTecnic.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}/{partitionKey}")]
    public async Task<IActionResult> GetById(string id, string partitionKey)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, partitionKey);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }
}
