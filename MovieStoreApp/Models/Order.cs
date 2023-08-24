using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int MovieId { get; set; }

    public int Quantity { get; set; }

    public virtual CustomerOrder OrderNavigation { get; set; } = null!;
}
