using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieStoreApp.Models;

public partial class Customer
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    [JsonIgnore]
    public virtual ICollection<CustomerRating> CustomerRatings { get; set; } = new List<CustomerRating>();

    [JsonIgnore]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    [JsonIgnore]
    public virtual ICollection<User> UserEmails { get; set; } = new List<User>();
}
