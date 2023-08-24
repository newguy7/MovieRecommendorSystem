using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class CustomerRating
{
    public int UserId { get; set; }

    public int MovieId { get; set; }

    public int? Rating { get; set; }

    [JsonIgnore]
    public virtual Movie Movie { get; set; } = null!;

    [JsonIgnore]
    public virtual Customer User { get; set; } = null!;
}
