using System;

using Newtonsoft.Json;

namespace IssueTracker.CQRS.Domain.Models
{
    public class EventDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "aggregateId")]
        public string AggregateId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "commandId")]
        public string CommandId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "sequence")]
        public int Sequence { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; } = string.Empty;
    }
}
