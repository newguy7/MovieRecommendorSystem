using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class User
{
    public string UserEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Customer> Users { get; set; } = new List<Customer>();
}
