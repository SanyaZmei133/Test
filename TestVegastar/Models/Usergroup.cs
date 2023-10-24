using System;
using System.Collections.Generic;

namespace TestVegastar.Models;

public partial class Usergroup
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public ICollection<User> Users { get; set; }
}
