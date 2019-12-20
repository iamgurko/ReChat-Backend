using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReChat.API.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="En Az 4 En Fazla 8 Haneli Şifre Girmeniz Gerekmektedir!")]
        public string Password { get; set; }

    }
}
