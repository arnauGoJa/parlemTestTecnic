namespace Domain.Entities
{
    public record Customer(

        string id,
        string partitionKey,
    string docType,
    string docNum,
    string email,
    string customerId,     
      string givenName,
        string familyName1,
          string phone,
        string type
    );
}
