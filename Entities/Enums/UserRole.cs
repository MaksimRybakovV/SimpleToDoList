using System.Text.Json.Serialization;

namespace Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        CommonUser,
        Admin
    }
}
