using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using DummyApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Repository.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAccountRepository _accountRepository;
        private readonly DummyAppContext _dummyAppContext;

        public AccountRepository(DummyAppContext dummyAppContext)
        {
            _dummyAppContext = dummyAppContext;
        }

        /// <summary>
        /// Adding Users Data into DB
        /// </summary>
        /// <param name="model"></param>
        public void AddUser(RegistrationViewModel model)
        {
            var newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                Password = model.Password,
            };
            _dummyAppContext.SaveChanges();
        }
    }
}
