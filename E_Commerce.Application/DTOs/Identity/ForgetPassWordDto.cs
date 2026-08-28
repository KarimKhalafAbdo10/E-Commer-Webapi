using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Identity
{
    public class ForgetPassWordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        // Optional parameter: "web" (default) or "mobile"
        public string ClientType { get; set; } = "web";
    }
}
