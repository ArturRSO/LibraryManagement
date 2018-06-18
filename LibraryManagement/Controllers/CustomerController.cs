using LibraryManagement.Data.Interfaces;
using LibraryManagement.Data.Model;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;

        public CustomerController(ICustomerRepository customerRepository, IBookRepository bookRepository)
        {
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
        }

        [Route("Customer")]
        public IActionResult List()
        {
            if (!_customerRepository.Any()) return View("Empty");

            List<CustomerViewModel> customerVM = new List<CustomerViewModel>();

            var customers = _customerRepository.GetAll();

            foreach (var customer in customers)
            {
                customerVM.Add(new CustomerViewModel
                {
                    Customer = customer,
                    BookCount = _bookRepository.Find(x => x.BorrowerId == customer.CustomerId).Count()
                });
            }

            return View(customerVM);
        }

        public IActionResult Update(int id)
        {
            Customer customer = _customerRepository.GetById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            _customerRepository.Update(customer);

            return RedirectToAction("List");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            _customerRepository.Create(customer);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            var customer = _customerRepository.GetById(id);

            _customerRepository.Delete(customer);

            return RedirectToAction("List");
        }
    }
}
