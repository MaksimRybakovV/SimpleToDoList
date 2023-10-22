using System.Text.Json.Serialization;

namespace Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskCategory
    {
        Work,
        Personal,
        Education,
        Health,
        Fitness,
        Family,
        Friends,
        Travel,
        Finance,
        Creativity,
        Event
    }
}
