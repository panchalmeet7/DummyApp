﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Entities.ViewModels
{
    public class MailSendViewModel
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
    }
}
