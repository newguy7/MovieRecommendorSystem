using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class MoviePicture
{
    public int MovieId { get; set; }

    public int PictureId { get; set; }

    public byte[] Picture { get; set; } = null!;

    public DateTime PictureDate { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
