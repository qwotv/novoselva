using System;
using System.Collections.Generic;

namespace AvaloniaApplication2.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Post { get; set; } = null!;
}
