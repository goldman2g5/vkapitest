using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace vkapitest.Models;

public partial class User
{

    public int Id { get; set; }
    public string? Login { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UserGroupId { get; set; }

    public int? UserStateId { get; set; }

    public virtual UserGroup? UserGroup { get; set; }

    public virtual UserState? UserState { get; set; }
}
