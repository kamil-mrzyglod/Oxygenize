using System.Collections.Generic;

namespace Oxygenize.Test.TestClasses
{
    public class EfPocoTest
    {
        public string Id { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string HouseNumber { get; set; }

        public string FlatNumber { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public virtual IList<Company> Companies { get; set; }

    }

    public class Company
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string MobilePhone { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string AddressId { get; set; }
    }
}