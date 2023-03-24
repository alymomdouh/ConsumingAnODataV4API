using ConsoleApp.Models;
using Microsoft.OData.Client;
using System;

namespace ConsoleApp
{
    public class MyLocalContainer : DataServiceContext
    {
        public DataServiceQuery<Person> People { get; }

        public MyLocalContainer(Uri serviceRoot) : base(serviceRoot)
        {
            People = base.CreateQuery<Person>("People");
        }


        // operation for person 
        public void AddPerson(Person person)
        {
            base.AddObject("People", person);
        }
        public void UpdatePerson(Person person)
        {
            base.UpdateObject("People", person);
        }
        public void DeletePerson(Person person)
        {
            base.DeleteObject("People", person);
        }
    }
}
