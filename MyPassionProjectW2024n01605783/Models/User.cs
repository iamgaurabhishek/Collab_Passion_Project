using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPassionProjectW2024n01605783.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserDescription { get; set; }
        public string UserAge { get; set; }
        public string UserWeight { get; set; }



    }
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserDescription { get; set; }
        public string UserAge { get; set; }
        public string UserWeight { get; set; }

    }
}