﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Model
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
    }

}
