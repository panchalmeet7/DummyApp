using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DummyApp.Entities.ViewModels; //latest c#10 feature, We don’t need to put our code in indented namespace code blocks anymore:

public class ImageUploadViewModel
{
    public int ImageID { get; set; }

    public IFormFile? ImagePath { get; set; }
}

