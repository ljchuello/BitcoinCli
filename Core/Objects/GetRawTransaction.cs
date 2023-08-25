using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NBitcoin.DataEncoders;
using NBitcoin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Objects
{
    public class GetRawTransaction
    {
        [JsonProperty("txid")]
        public string Txid { get; set; } = string.Empty;

        [JsonProperty("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonProperty("version")]
        public long Version { get; set; } = 0;

        [JsonProperty("size")]
        public long Size { get; set; } = 0;

        [JsonProperty("vsize")]
        public long Vsize { get; set; } = 0;

        [JsonProperty("weight")]
        public long Weight { get; set; } = 0;

        [JsonProperty("locktime")]
        public long Locktime { get; set; } = 0;

        [JsonProperty("vin")]
        public List<Generico.Vin> Vin { get; set; } = new List<Generico.Vin>();

        [JsonProperty("vout")]
        public List<Generico.Vout> Vout { get; set; } = new List<Generico.Vout>();

        [JsonProperty("hex")]
        public string Hex { get; set; } = string.Empty;

        [JsonProperty("blockhash")]
        public string Blockhash { get; set; } = string.Empty;

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; } = 0;

        [JsonProperty("time")]
        public long Time { get; set; } = 0;

        [JsonProperty("blocktime")]
        public long Blocktime { get; set; } = 0;

        public static async Task<GetRawTransaction> GetAsync(string txid)
        {
            // Get

            string jsonResponse = await new Core().SendPostRequest($"{{ \"method\": \"getrawtransaction\", \"params\": [ \"{txid}\", true] }}");

            // Set
            JObject result = JObject.Parse(jsonResponse);
            GetRawTransaction getRawTransaction = JsonConvert.DeserializeObject<GetRawTransaction>($"{result["result"]}") ?? new GetRawTransaction();

            // Address
            foreach (var row in getRawTransaction.Vout)
            {
                if (string.IsNullOrEmpty(row.ScriptPubKey.Address) && row.ScriptPubKey.Type != "nulldata")
                {
                    {
                        try
                        {
                            // Convierte el ScriptPubKey hexadecimal a bytes
                            byte[] scriptPubKeyBytes = Encoders.Hex.DecodeData(row.ScriptPubKey.Hex);

                            // Extrae la clave pública del ScriptPubKey (omitiendo los bytes de control)
                            byte[] publicKeyBytes = new byte[scriptPubKeyBytes.Length - 2];
                            Array.Copy(scriptPubKeyBytes, 1, publicKeyBytes, 0, publicKeyBytes.Length);

                            // Crea un objeto de PubKey a partir de la clave pública
                            PubKey publicKey = new PubKey(publicKeyBytes);

                            // Crea una dirección Bitcoin a partir de la clave pública
                            BitcoinAddress address = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main);

                            // Set
                            row.ScriptPubKey.Address = address.ToString();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }


            // Libre pecados
            return getRawTransaction;
        }
    }
}