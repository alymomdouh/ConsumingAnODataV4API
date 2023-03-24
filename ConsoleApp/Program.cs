using AirVinyl;
using Microsoft.OData.Client;
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
            var autoContainer = new AirVinylContainer(new Uri(BaseUrl));

            // make Batch

            // var people = await autoContainer.People.ExecuteAsync();
            // var recordStores = await autoContainer.RecordStores.ExecuteAsync();

            var peopleQuery = autoContainer.People.Where(p => p.PersonId == 1) as DataServiceQuery;
            var recordStoresQuery = autoContainer.RecordStores; 
            var batchResponse = await autoContainer.ExecuteBatchAsync(peopleQuery, recordStoresQuery);
            // view the batch 
            foreach (var operationResponse in batchResponse)
            {
                var peopleResponse = operationResponse as QueryOperationResponse<AirVinyl.Person>;
                if (peopleResponse != null)
                {
                    foreach (AirVinyl.Person person in peopleResponse)
                    {
                        Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
                    }
                }
                var recordsStoresResponse = operationResponse as QueryOperationResponse<RecordStore>;
                if (recordsStoresResponse != null)
                {
                    foreach (var recordStore in recordsStoresResponse)
                    {
                        Console.WriteLine($"{recordStore.RecordStoreId} {recordStore.Name}");
                    }
                }
            }
        }
        public async static Task CodeGeneration()
        {
            // call auto generated debcontext container 

            var autoContainer = new AirVinylContainer(new Uri(BaseUrl));
            var people = autoContainer.People.Execute();

            // working with query options 
            // filters by QueryOptions
            var filterpeople = await autoContainer.People
                .AddQueryOption("$expand", "VinylRecords")
                .AddQueryOption("$select", "PersonId,FirstName")
                .AddQueryOption("$orderby", "FirstName")
                .AddQueryOption("$top", "2")
                .AddQueryOption("$skip", "4")
                .ExecuteAsync();

            // filters by Linq
            var filterpeople2 = autoContainer.People
                //.Expand(p => p.VinylRecords)
                .OrderBy(p => p.FirstName)
                .Skip(4)
                .Take(2)//that mean top2
                .Select(p => new { p.PersonId, p.FirstName, VinylRecords = p.VinylRecords });

            foreach (var person in people.ToList())
            {
                Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
                // view NavigationProperty Name="VinylRecords" Type="Collection"

                //autoContainer.LoadProperty(person, "VinylRecords"); 
                //foreach (var vinylRecord in person.VinylRecords)
                //{
                //    Console.WriteLine($"---- {vinylRecord.VinylRecordId} {vinylRecord.Title}");
                //}
            }
        }
        public async static Task NoCodeGeneration()
        {
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
            myContainer.AddPerson(new Models.Person()
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
            // to update 
            var firstPerson = people.ToList().First();
            firstPerson.FirstName = "tes";
            myContainer.UpdatePerson(firstPerson);
            myContainer.SaveChanges();
        }
        public static void LogTrackedPerson(MyLocalContainer container)
        {
            //view the status of all items 
            foreach (var entityDescriptor in container.EntityTracker.Entities)
            {
                if (entityDescriptor.Entity is Models.Person castedPerson)
                {
                    Console.WriteLine($"{entityDescriptor.State} - {castedPerson.PersonId} " + $"{castedPerson.FirstName} {castedPerson.LastName}");
                }
            }
        }
    }
}
