namespace Domain.Entities
{
    public record Product(

        string id,
    string productName,
    string productTypeName,
    int numeracioTerminal,
     DateTime soldAt,
          string customerId,
           string type
    );
}
