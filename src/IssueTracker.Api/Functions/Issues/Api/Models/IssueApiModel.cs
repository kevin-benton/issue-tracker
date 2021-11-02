using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using IssueTracker.CQRS.Events;

namespace IssueTracker.Api.Functions.Issues.Api.Models
{
    public class IssueApiModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "issueId")]
        public string IssueId { get; set; } = "1";

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; } = 5;

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "updated")]
        public DateTime Updated { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "deleted")]
        public DateTime? Deleted { get; set; }

        [JsonProperty(PropertyName = "history")]
        public List<Event> History { get; set; } = new List<Event>();
    }
}
