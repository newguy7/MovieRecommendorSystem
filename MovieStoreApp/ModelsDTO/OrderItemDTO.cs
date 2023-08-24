namespace MovieStoreApp.ModelsDTO
{
    public class OrderItemDTO
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
