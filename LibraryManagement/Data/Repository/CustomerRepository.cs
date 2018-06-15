﻿using LibraryManagement.Data.Interfaces;
using LibraryManagement.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Data.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(LibraryDBContext context) : base(context)
        {
        }

        public override void Delete(Customer entity)
        {
            _context.Books.Where(b => b.Borrower == entity).ToList()
                .ForEach(a =>
                {
                    a.Borrower = null;
                    a.BorrowerId = 0;
                });

            base.Delete(entity);
        }
    }
}
