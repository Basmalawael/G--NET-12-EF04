using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_12_EF05.Models
{
    public class CustomerAccount
    { 
        public int CustomerId { get; set; } //FK Table Cust
        public int AccountAccountNumber { get; set; } //FK Table Account 

        public string OwnershipType { get; set; }
        public bool AccountStatus { get; set; }
        public DateTime OwnershipStartDate { get; set; }

        //Navigation

        public Customer CustomerCA { get;set; }
        public Account  AccountCA { get; set; }

       



    }
}
