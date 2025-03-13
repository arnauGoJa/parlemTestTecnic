using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class CustomerValidator: ICustomerValidator
    {
        public Result<bool> ValidateCustomerId(string id, string partitionKey)
        {
            Result<bool> result = Result<bool>.Ok(true);
            if (string.IsNullOrEmpty(id))
            {
                result= Result<bool>.Fail("Customer ID is required.");
            }
            if (string.IsNullOrEmpty(partitionKey))
            {
                result = Result<bool>.Fail("Customer partitionKey is required.");
            }
            return result;
        }
    }
}
