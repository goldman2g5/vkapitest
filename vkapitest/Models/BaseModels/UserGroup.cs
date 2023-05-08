using System;
using System.Collections.Generic;

namespace vkapitest.Models;

public partial class UserGroup
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
