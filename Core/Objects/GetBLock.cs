using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Objects
{
    public class GetBLock
    {
        [JsonProperty("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; } = 0;

        [JsonProperty("height")]
        public long Height { get; set; } = 0;

        [JsonProperty("version")]
        public long Version { get; set; } = 0;

        [JsonProperty("versionHex")]
        public string VersionHex { get; set; } = string.Empty;

        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; } = string.Empty;

        [JsonProperty("time")]
        public long Time { get; set; } = 0;

        [JsonProperty("mediantime")]
        public long Mediantime { get; set; } = 0;

        [JsonProperty("nonce")]
        public long Nonce { get; set; } = 0;

        [JsonProperty("bits")]
        public string Bits { get; set; } = string.Empty;

        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; } = 0;

        [JsonProperty("chainwork")]
        public string Chainwork { get; set; } = string.Empty;

        [JsonProperty("nTx")]
        public long NTx { get; set; } = 0;

        [JsonProperty("previousblockhash")]
        public string Previousblockhash { get; set; } = string.Empty;

        [JsonProperty("nextblockhash")]
        public string Nextblockhash { get; set; } = string.Empty;

        [JsonProperty("strippedsize")]
        public long Strippedsize { get; set; } = 0;

        [JsonProperty("size")]
        public long Size { get; set; } = 0;

        [JsonProperty("weight")]
        public long Weight { get; set; } = 0;

        [JsonProperty("tx")]
        //public List<string> Tx { get; set; } = new List<string>();
        public List<GetRawTransaction> Tx { get; set; } = new List<GetRawTransaction>();

        public static async Task<GetBLock> GetAsync(string blokHash)
        {
            // Get
            string jsonResponse = await new Core().SendPostRequest($"{{ \"method\": \"getblock\", \"params\": [ \"{blokHash}\", 2 ] }}");

            // Set
            JObject result = JObject.Parse(jsonResponse);
            GetBLock getRawTransaction = JsonConvert.DeserializeObject<GetBLock>($"{result["result"]}") ?? new GetBLock();

            // Libre pecados
            return getRawTransaction;
        }
    }
}
