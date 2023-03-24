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
    }
}
