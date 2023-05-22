using DummyApp.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Repository.Interface
{
    public interface IAccountRepository
    {
        public void AddUser(RegistrationViewModel model);
        public bool Validation_Input_UserEmail_Twice(RegistrationViewModel model);
    }
}
