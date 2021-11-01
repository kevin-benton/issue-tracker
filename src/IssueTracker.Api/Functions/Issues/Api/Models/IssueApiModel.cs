using System;

using Newtonsoft.Json;

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
    }
}
