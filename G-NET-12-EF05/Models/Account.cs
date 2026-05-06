using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_12_EF05.Models
{
    public class Account
    {   
        public int AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public string AccountType { get; set; }
        public DateTime OpeingDate { get; set; }

        //Navigation 
        public Branch? BranchACC { get; set; }
        public int  BranchCode { get; set; } //FK 

        //Nav
        public ICollection<Transaction> TransactionsAC { get; set; }

        public ICollection<CustomerAccount> CustomerAccountA { get; set; }

    }
}
