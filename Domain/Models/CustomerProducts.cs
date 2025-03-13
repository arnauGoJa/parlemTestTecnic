using Domain.Entities;

namespace Domain.Models
{
    public class CustomerProducts()
    {

        public CustomerProducts(Customer costumer) :this()
        {
            this.id = costumer.id;
            this.docNum = costumer.docNum;
            this.email = costumer.email;
            this.customerId = costumer.customerId;
            this.phone = costumer.phone;
            this.familyName1 = costumer.familyName1;
            this.docType = costumer.docType;
            this.givenName = costumer.givenName;

        }
        public string id { get; set; }
        public string partitionKey { get; set; }
        public string docType { get; set; }
        public string docNum { get; set; }
        public string email { get; set; }
        public string customerId { get; set; }
        public string givenName { get; set; }
        public string familyName1 { get; set; }
        public string phone { get; set; }
        public List<Product> Products { get; set; }
    }
}
