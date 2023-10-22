using System.Text.Json.Serialization;

namespace Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskExecutionStatus
    {
        New,
        InProgress,
        OnHold,
        Waiting,
        Completed,
        Cancelled,
        Overdue,
        Archived, 
        PendingDecision,
        Returned
    }
}
