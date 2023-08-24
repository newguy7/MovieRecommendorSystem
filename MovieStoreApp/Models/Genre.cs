using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    [JsonIgnore]
    public virtual ICollection<Movie> MoviesNavigation { get; set; } = new List<Movie>();
}
