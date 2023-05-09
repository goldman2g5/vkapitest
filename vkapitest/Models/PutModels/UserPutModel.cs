using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace vkapitest.Models
{
    public class UserPutModel : User
    {
        [JsonIgnore]
        new public DateTime? CreatedDate { get; set; }
        
        [JsonIgnore]
        new public virtual UserGroup? UserGroup { get; set; }

        [JsonIgnore]
        new public virtual UserState? UserState { get; set; }
    }
}
