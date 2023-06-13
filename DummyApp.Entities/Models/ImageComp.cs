using System;
using System.Collections.Generic;

namespace DummyApp.Entities.Models;

public partial class ImageComp
{
    public int ImageId { get; set; }

    public string ImagePath { get; set; } = null!;
}
