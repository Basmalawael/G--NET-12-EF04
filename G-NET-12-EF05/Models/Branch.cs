using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_12_EF05.Models
{
    public class Branch
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber {  get; set; }

        //Navigation Property
        public Manager ManagersBR { get; set; }
        //fk
        public int ManagerId { get; set;}

        //Navigation 
        public ICollection<Account> AccountsBR { get; set; }
        


    }
}
