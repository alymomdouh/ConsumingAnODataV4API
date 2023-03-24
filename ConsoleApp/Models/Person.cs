using Microsoft.OData.Client;
using System;

namespace ConsoleApp.Models
{
    [Key("PersonId")]
    public class Person
    {
        public int PersonId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int NumberOfRecordsOnWishList { get; set; }
        public decimal AmountOfCashToSpend { get; set; }
    }

}
