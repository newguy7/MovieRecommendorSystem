using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class Recommendation
{
    public int UserId { get; set; }

    public int MovieId { get; set; }

    public double RecommendationScore { get; set; }

    public DateTime RecimmendedOn { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Customer User { get; set; } = null!;
}
