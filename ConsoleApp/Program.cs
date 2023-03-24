using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private const string BaseUrl = "https://localhost:5001/odata";// link for api or backend 
        static async Task Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            var myContainer = new MyLocalContainer(new Uri(BaseUrl));

            // to load people from source to conatiner
            var people = myContainer.People.Execute();
            var peopleCondition = myContainer.People.Where(a => a.PersonId == 1);
            var peopleAsync = await myContainer.People.ExecuteAsync();
            foreach (var person in people)
            {
                Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
            }

        }
    }
}
