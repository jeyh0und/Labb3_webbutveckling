using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButalandIntErasmus.Shared.DTOs;

public record PersonDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public PersonDto(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
