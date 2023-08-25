using Newtonsoft.Json;

namespace Core.Objects
{
    public class Error
    {
        [JsonProperty("code")] public int Code { get; set; } = 0;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}