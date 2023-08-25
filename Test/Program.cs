using Core.Objects;
using NBitcoin.DataEncoders;
using NBitcoin;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                // string blockHash = "0000000000000000000117dcc09d31a57fb1ec95490cae32291f7199c9784136";
                var block = await GetBLock.GetAsync("00000000146e28348d962160e9d540ababbd1dababb93d4f92aed9e53bcf36de");
                long totalTx = 0;
                long totalBlock = 0;

                while (true)
                {
                    totalBlock++;

                    // Recorremos
                    long i = 0;
                    foreach (var tx in block.Tx)
                    {

                        // Address
                        Console.WriteLine($"Procesando... TotalTx: {++totalTx:n0} | Total Block: {totalBlock:n0} | Block: {block.Height:n0} | Tx: {++i:n0}/{block.Tx.Count:n0}");
                        foreach (var row in tx.Vout)
                        {
                            string txId = tx.Txid;
                            // Verificamos si está vacía
                            if (string.IsNullOrEmpty(row.ScriptPubKey.Address))
                            {
                                switch (row.ScriptPubKey.Type)
                                {
                                    case "nulldata":
                                        row.ScriptPubKey.Address = "unknown";
                                        break;

                                    case "multisig":
                                        row.ScriptPubKey.Address = "unknown";
                                        break;

                                    case "pubkey":
                                        // ScriptPubKey en formato hexadecimal
                                        string scriptPubKeyHex = row.ScriptPubKey.Hex;

                                        // Convierte el ScriptPubKey hexadecimal a bytes
                                        byte[] scriptPubKeyBytes = Encoders.Hex.DecodeData(scriptPubKeyHex);

                                        // Extrae la clave pública del ScriptPubKey (omitiendo los bytes de control)
                                        byte[] publicKeyBytes = new byte[scriptPubKeyBytes.Length - 2];
                                        Array.Copy(scriptPubKeyBytes, 1, publicKeyBytes, 0, publicKeyBytes.Length);

                                        // Crea un objeto de PubKey a partir de la clave pública
                                        PubKey publicKey = new PubKey(publicKeyBytes);

                                        // Crea una dirección Bitcoin a partir de la clave pública
                                        row.ScriptPubKey.Address = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString();
                                        break;
                                }
                            }

                            if (string.IsNullOrEmpty(row.ScriptPubKey.Address))
                            {
                                Console.WriteLine($"{Guid.NewGuid()}");
                            }
                        }
                    }

                    // Bloque anterior
                    block = await GetBLock.GetAsync(block.Nextblockhash);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}