namespace MovieStoreApp.ModelsDTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }

    }
}
