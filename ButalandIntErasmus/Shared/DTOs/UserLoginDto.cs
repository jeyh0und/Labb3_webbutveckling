using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButalandIntErasmus.Shared.DTOs;

public class UserLoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password can not be empty!")]
    public string Password { get; set; }
}
