using System.Text.Json.Serialization;

namespace UsersControllerTests.Models
{
    public class UserPostModel : User
    {
        [JsonIgnore]
        new public virtual UserGroup? UserGroup { get; set; }

        [JsonIgnore]
        new public virtual UserState? UserState { get; set; }
    }
}
