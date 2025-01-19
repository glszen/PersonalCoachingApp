using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.User.Dtos
{
    public class LoginUserDto
    {
        public string Email { get; set; }

        [DataType(DataType.Password)] //Hidden password.
        public string Password { get; set; }
    }
}
