namespace ECommerceOrderSystem.Models.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }


        public List<Order> Orders {get; set;} = new();
        public List<Address> Addresses{get; set;} = new();
    }
}