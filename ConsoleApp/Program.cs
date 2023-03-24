using ConsoleApp.Models;
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

            // to insert into people 
            myContainer.AddPerson(new Person()
            {
                FirstName = "John the First",
                LastName = "Smith",
                AmountOfCashToSpend = 400,
                DateOfBirth = new DateTimeOffset(new DateTime(1980, 5, 10)),
                Email = "someaddress@someserver.com",
                NumberOfRecordsOnWishList = 10,
            });
            // note not pass the id it is auto generation 
            // view status before save 
            LogTrackedPerson(myContainer);
            await myContainer.SaveChangesAsync();
            // view status after save 
            LogTrackedPerson(myContainer);


        }
        public static void LogTrackedPerson(MyLocalContainer container)
        {
            //view the status of all items 
            foreach (var entityDescriptor in container.EntityTracker.Entities)
            {
                if (entityDescriptor.Entity is Person castedPerson)
                {
                    Console.WriteLine($"{entityDescriptor.State} - {castedPerson.PersonId} " + $"{castedPerson.FirstName} {castedPerson.LastName}");
                }
            }
        }
    }
}
