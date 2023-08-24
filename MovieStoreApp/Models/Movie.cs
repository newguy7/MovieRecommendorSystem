using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public decimal Price { get; set; }

    public bool? OnSale { get; set; }

    public bool? OutOfStock { get; set; }

    public DateTime ReleasedYear { get; set; }

    public int GenreId { get; set; }

    public int ImdbId { get; set; }

    public int TmdbId { get; set; }

    public virtual ICollection<CustomerRating> CustomerRatings { get; set; } = new List<CustomerRating>();

    public virtual Genre GenreNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<MoviePicture> MoviePictures { get; set; } = new List<MoviePicture>();

    [JsonIgnore]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    [JsonIgnore]
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
