﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UsersControllerTests.Models;

public partial class UserGroup
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Code { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
