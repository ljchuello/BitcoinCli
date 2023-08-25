using Core.Objects;

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
                var a = await GetRawTransaction.GetAsync("88322203cf7eadbe8ae73c09d5577358d86d60246e8b2147eb053c76a85bb73e");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}