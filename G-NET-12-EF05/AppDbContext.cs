using G_NET_12_EF05.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_12_EF05
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        optionsBuilder.UseSqlServer("Server=.; Database=BankDb; Trusted_Connection=True; TrustServerCertificate=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Table_Account
            modelBuilder.Entity<Account>()
                        .HasKey(A => A.AccountNumber); //Pk 

            //RS 

            modelBuilder.Entity<Account>()
                        .HasOne(A => A.BranchACC)
                        .WithMany(B => B.AccountsBR)
                        .HasForeignKey(A => A.BranchCode);
            #endregion

            #region Table_Manager

            modelBuilder.Entity<Manager>(en =>
            {
                en.HasKey(M => M.Id);
            });

            //Add 

            modelBuilder.Entity<Manager>().HasData
                (

                new Manager { Id = 1, FullName = "Ahmed Ali", Email = "ahmed@bank.com", PhoneNumber = "010112345", HireDate = DateTime.Now },
                new Manager { Id = 2, FullName = "Yasmine Ahmed", Email = "yasmine@bank.com", PhoneNumber = "0122333444", HireDate = new DateTime(2024, 2, 1) },
                new Manager { Id = 3, FullName = "OmerTaha", Email = "Omer@bank.com", PhoneNumber = "0156789820", HireDate = DateTime.Now }
                );

            #endregion

            #region Table_Branch 
            modelBuilder.Entity<Branch>()
                        .HasKey(B => B.Code);  //PK 

            //Rs 

            modelBuilder.Entity<Branch>()
                        .HasOne(B => B.ManagersBR)
                        .WithOne(M => M.BranchsMA)
                        .HasForeignKey<Branch> (B => B.ManagerId);

            //Add
            modelBuilder.Entity<Branch>().HasData
                (

                new Branch { Code = 101, Name = "Main Branch", Address = "Cairo", PhoneNumber = "19000", ManagerId = 1 },
                new Branch { Code = 102, Name = "Giza Branch", Address = "Giza", PhoneNumber = "16222", ManagerId = 2 },
                new Branch { Code = 103, Name = "Alex Branch", Address = "Alex", PhoneNumber = "18000", ManagerId = 3 }



                );

            #endregion

            #region Table_Customer
            modelBuilder.Entity<Customer>()
                        .HasKey(C => C.Id); //PK
            //Rs
         
                     

            #endregion

            #region Table_Transaction 
            modelBuilder.Entity<Transaction>()
                        .HasKey(T => T.TransactionNumber);

            //RS
            modelBuilder.Entity<Transaction>()
                        .HasOne(T => T.AccountTr) // 1
                        .WithMany(A => A.TransactionsAC) // M
                        .HasForeignKey(T => T.AccountAccountNumber); //FK 

            #endregion

            #region Table_CustomerAccount 

            modelBuilder.Entity<CustomerAccount>(en =>
            {
                en.HasKey(CA => new { CA.AccountAccountNumber, CA.CustomerId });

                en.HasOne(CA => CA.CustomerCA)
                .WithMany(C => C.CustomerAccountC)
                .HasForeignKey(CA => CA.CustomerId);

                en.HasOne(CA => CA.AccountCA)
                .WithMany(A => A.CustomerAccountA)
                .HasForeignKey(CA => CA.AccountAccountNumber);


                  
            });

            #endregion



        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
