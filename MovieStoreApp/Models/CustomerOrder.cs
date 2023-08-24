using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class CustomerOrder
{
    public int UserId { get; set; }

    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Customer User { get; set; } = null!;
}
