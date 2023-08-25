using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Objects
{
    public class Generico
    {
        public class ScriptPubKey
        {
            [JsonProperty("asm")]
            public string Asm { get; set; } = string.Empty;

            [JsonProperty("desc")]
            public string Desc { get; set; } = string.Empty;

            [JsonProperty("hex")]
            public string Hex { get; set; } = string.Empty;

            [JsonProperty("address")]
            public string Address { get; set; } = string.Empty;

            [JsonProperty("type")]
            public string Type { get; set; } = string.Empty;
        }

        public class ScriptSig
        {
            [JsonProperty("asm")]
            public string Asm { get; set; } = string.Empty;

            [JsonProperty("hex")]
            public string Hex { get; set; } = string.Empty;
        }

        public class Vin
        {
            [JsonProperty("coinbase")]
            public string Coinbase { get; set; } = string.Empty;

            [JsonProperty("txid")]
            public string Txid { get; set; } = string.Empty;

            [JsonProperty("vout")]
            public long Vout { get; set; } = 0;

            [JsonProperty("scriptSig")]
            public ScriptSig ScriptSig { get; set; } = new ScriptSig();

            [JsonProperty("txinwitness")]
            public List<string> Txinwitness { get; set; } = new List<string>();

            [JsonProperty("sequence")]
            public long Sequence { get; set; } = 0;
        }

        public class Vout
        {
            [JsonProperty("value")]
            public decimal Value { get; set; } = 0;

            [JsonProperty("n")]
            public long N { get; set; } = 0;

            [JsonProperty("scriptPubKey")]
            public ScriptPubKey ScriptPubKey { get; set; } = new ScriptPubKey();
        }
    }
}