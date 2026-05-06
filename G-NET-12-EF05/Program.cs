using G_NET_12_EF05.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Xml.Schema;

namespace G_NET_12_EF05
{
    internal class Program
    {  
        static void Main(string[] args) 
        { 
            #region Bank : 

            using AppDbContext db = new AppDbContext();

            bool exit = false; 
            while (!exit)
            { 
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("National Bank - Management");
                Console.WriteLine("==============================================");
                Console.WriteLine("  1) Add a new Customer ");
                Console.WriteLine("  2) Open a new Account for a Customer");
                Console.WriteLine("  3) Update Account Status (Active / Closed)");
                Console.WriteLine("  4) Remove an Account from a Customer");
                Console.WriteLine("  5) List all Customers (with accounts)");
                Console.WriteLine("  0) Exit");
                Console.WriteLine("----------------------------------------------");
                Console.Write    ("  Enter choice: ");

                string input = Console.ReadLine();
                if (int.TryParse(input , out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            try
                            {
                                Console.WriteLine("\n--- Add New Customer ---");

                                Console.Write("FullName     :  ");
                                string name = Console.ReadLine();

                                Console.Write("National ID   :  ");
                                string NationalId = Console.ReadLine();

                                Console.Write("Date of Birth  : (dd-mm-yyyy) ");
                                DateTime dob = DateTime.Parse(Console.ReadLine());

                                Console.Write("Email          : ");
                                string email = Console.ReadLine();

                                Console.Write("Phone          : ");
                                string phone = Console.ReadLine();

                                Console.Write("Address        : ");
                                string address = Console.ReadLine();

                                Console.WriteLine("Customer Type  : ");
                                Console.WriteLine("   1) Individual");
                                Console.WriteLine("   2) Business");
                                Console.Write("Choice: ");

                                string typech = Console.ReadLine();

                                string Type = (typech == "1") ? "Individual" : "Business";

                                Customer customer = new Customer
                                {
                                    FullName = name,
                                    NationalId = NationalId,
                                    DateOfBirth = dob,
                                    Email = email,
                                    PhoneNumber = phone,
                                    Address = address,
                                    CustomerType = Type

                                };

                                db.Add(customer);
                                db.SaveChanges();



                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\nCustomer created successfully. CustomerId = {customer.Id}");
                                Console.ResetColor();
                            }
                            catch (Exception ex)
                            {
                                // --- الـ catch بنطبع فيها "إيه اللي باظ" ---
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"\nError: Invalid data! {ex.Message}");
                                Console.ResetColor();
                            }


                            break;


                        case 2:
                            Console.WriteLine("\n--- Open New Account ---");

                            Console.WriteLine("Account Number   : ");
                            int Account = int .Parse(Console.ReadLine());

                            Console.WriteLine("Account Type     : ");
                            Console.WriteLine(" 1) Savings\n 2) Current\n 3) Business");
                            Console.Write("Choice: ");

                            string acctype  = (Console.ReadLine() =="1") ? "Savings" : "Current";

                            Console.Write("Branch Code    : ");
                            int branchid = int.Parse(Console.ReadLine());

                            Console.Write("Customer Id    : ");
                            int custid = int.Parse(Console.ReadLine());

                            var branch = db.Branches.Find(branchid);
                            var cust = db.Customers.Find(custid);

                            if (branch == null ||  cust == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Branch or Customer does not exist!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Account acc = new Account
                                {
                                    AccountNumber = Account,
                                    AccountType = acctype ,
                                    BranchCode = branchid ,
                                    OpeingDate = DateTime.Now
                                };
                                db.Accounts.Add(acc);

                                CustomerAccount customerAccount = new CustomerAccount
                                {
                                    AccountAccountNumber = Account ,
                                    CustomerId = custid,
                                    OwnershipType = "Primary",
                                    AccountStatus = true,
                                    OwnershipStartDate = DateTime.Now


                                };
                                db.Add(customerAccount);
                                db.SaveChanges();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\nAccount '{Account}' created and linked to customer {custid}.");
                                Console.ResetColor();

                            }

                            break;

                        case 3:
                            Console.WriteLine("\n--- Update Account Status ---");

                            Console.WriteLine("Account Number   : ");
                            int upacc = int.Parse(Console.ReadLine());

                            Console.WriteLine("Customer Id      : ");
                            int Custid = int.Parse(Console.ReadLine());

                            var link = db.CustomerAccounts
                                         .FirstOrDefault(CA => CA.AccountAccountNumber == upacc && CA.CustomerId == Custid);

                            if (link == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Ownership record not found for this Customer and Account!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine("New Status       : ");
                                Console.WriteLine(" 1) Active\n  2) Closed");
                                Console.Write("Choice: ");
                                string statusChoice = Console.ReadLine();

                                link.AccountStatus = (statusChoice == "1");
                                db.SaveChanges();

                                Console.ForegroundColor = ConsoleColor.Green;

                                string statusText = link.AccountStatus ? "Active" : "Closed";
                                Console.WriteLine($"Status updated to {statusText}.");
                                Console.ResetColor();

                            }

                            break;
                        case 4:
                            Console.WriteLine("\n--- Remove Account From Customer ---");

                            Console.WriteLine("Account Number  : ");
                            int Reacc = int .Parse(Console.ReadLine());

                            Console.WriteLine("Customer Id     : ");
                            int CID = int.Parse(Console.ReadLine());


                            var Cacc = db.CustomerAccounts
                                        .FirstOrDefault(A =>A.AccountAccountNumber==Reacc && A.CustomerId == CID);

                            if (Cacc == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: This customer is not linked to this account.");
                                Console.ResetColor();
                            }
                            else
                            {
                                db.CustomerAccounts.Remove(Cacc);
                                db.SaveChanges();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nOwnership link deleted.");
                                Console.ResetColor();

                                bool hasOtherOwners = db.CustomerAccounts.Any(Ca => Ca.AccountAccountNumber == Reacc);
                                if (!hasOtherOwners)
                                {
                                    var accountToDelete = db.Accounts.Find(Reacc);

                                    if (accountToDelete != null)
                                    {
                                        db.Accounts.Remove(accountToDelete);
                                        db.SaveChanges();
                                        Console.WriteLine($"That was the last owner - account '{Reacc}' was also removed.");
                                    }
                                }




                            }

                                break;

                        case 5:
                            Console.WriteLine("\n--- All Customers ---");

                            var allCustomer = db.Customers.Include(C => C.CustomerAccountC)
                                                          .ThenInclude(Ca => Ca.AccountCA).ToList();
                            if ( !allCustomer.Any())
                            {
                                Console.WriteLine("No customers found in the system.");

                            }

                            else
                            {
                                foreach(var c in allCustomer)
                                {
                                    Console.WriteLine("--------------------------------------------------");
                                    Console.WriteLine($"Customer : {c.FullName} (ID: {c.Id})");
                                    Console.WriteLine($"Type    : {c.CustomerType} | Email: {c.Email}");

                                    if (c.CustomerAccountC != null && c.CustomerAccountC.Any())
                                    {
                                        Console.WriteLine("  Accounts:");
                                        foreach (var ca in c.CustomerAccountC)
                                        {
                                            // ca.Account هنا هو الحساب اللي جيناه بـ ThenInclude
                                            string status = ca.AccountStatus ? "Active" : "Closed";
                                            Console.WriteLine($"   - Acc No: {ca.AccountAccountNumber} | Bal: {ca.AccountCA.CurrentBalance:C} | Status: {status} ({ca.OwnershipType})");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("  (No accounts linked yet)");
                                    }
                                }
                            }
                                break;
                        case 0:
                            Console.WriteLine("Exiting... Goodbye!");
                            exit = true;
                            continue;

                        default:
                            Console.WriteLine("Invalid choice! Please select 0 to 5");
                            break; }


                }

                else
                {
                    Console.WriteLine("Invalid input! Please enter a number.");
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();

                #endregion
            }
        }
    }
}
