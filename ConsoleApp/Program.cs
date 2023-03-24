using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var myContainer = new MyLocalContainer(new Uri("https://localhost:5001/odata"));

            

        }
    }
}
