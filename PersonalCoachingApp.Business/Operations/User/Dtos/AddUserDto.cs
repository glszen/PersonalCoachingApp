﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.User.Dtos
{
    public class AddUserDto
    {
        public string Email { get; set; }

        [DataType(DataType.Password)] //Hidden password.
        public string Password { get; set; }

        public string FirstName { get; set; }   

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
