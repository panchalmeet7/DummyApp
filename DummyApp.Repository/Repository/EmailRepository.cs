using DummyApp.Entities.Data;
using DummyApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Repository.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DummyAppContext _dummyAppContext;
        public EmailRepository(DummyAppContext dummyAppContext)
        {
            _dummyAppContext = dummyAppContext;
        }
    }
}
