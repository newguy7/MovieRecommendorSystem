namespace MovieStoreApp.ModelsDTO
{
    public class MovieDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = null!;        

        public decimal Price { get; set; }

        public bool? OnSale { get; set; }

        public bool? OutOfStock { get; set; }

        public DateTime ReleasedYear { get; set; }

        public string MovieImage { get; set; }
    }
}
