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
                //string blockHash = "00000000000000241123ba5f49ec423ad3e3a380abe9e4a1fcd699dee8f577da";
                var block = await GetBLock.GetAsync("00000000000000241123ba5f49ec423ad3e3a380abe9e4a1fcd699dee8f577da");
                long totalTx = 0;
                long totalBlock = 0;

                while (true)
                {
                    try
                    {
                        // Bloque anterior
                        totalBlock++;
                        var nextBlock = GetBLock.GetAsync(block.Nextblockhash);

                        // Recorremos
                        Console.WriteLine($"Procesando... TotalTx: {++totalTx:n0} | Total Block: {totalBlock:n0} | Block: {block.Height:n0} | Tx: {block.Tx.Count:n0}");
                        long i = 0;
                        foreach (var tx in block.Tx)
                        {
                            totalTx++;
                            // Address
                            foreach (var row in tx.Vout)
                            {
                                // Verificamos si está vacía
                                if (string.IsNullOrEmpty(row.ScriptPubKey.Address))
                                {
                                    switch (row.ScriptPubKey.Type)
                                    {
                                        case "nulldata":
                                            row.ScriptPubKey.Address = "nulldata";
                                            break;

                                        case "multisig":
                                            row.ScriptPubKey.Address = "multisig";
                                            break;

                                        case "nonstandard":
                                            row.ScriptPubKey.Address = "nonstandard";
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
                                    string txId = tx.Txid;
                                    Console.WriteLine($"{Guid.NewGuid()}");
                                }
                            }
                        }

                        // Proximo
                        await Task.WhenAll(nextBlock);
                        block = nextBlock.Result;
                    }
                    catch (Exception e)
                    {
                        await Task.Delay(2500);
                        Console.WriteLine(e);
                    }
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