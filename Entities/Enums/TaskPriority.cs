using System.Text.Json.Serialization;

namespace Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskPriority
    {
        Low, 
        Medium, 
        High,
        Critical,
        No
    }
}
