using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entites.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstName, string lastName, string street, 
        string city, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string FirstName{set;get;}
        public string LastName{set;get;}
        public string Street{set;get;}
        public string City{set;get;}
        public string State{set;get;}
        public string ZipCode{set;get;}        
    }
}