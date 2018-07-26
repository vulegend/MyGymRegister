using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGymRegister
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string JMBG { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public byte Gender { get; set; }
        public string Email { get; set; }

        public User(string name,string surname,string jmbg,string phoneNumber,string address,string gender,string email)
        {
            Name = name;
            Surname = surname;
            JMBG = jmbg;
            Mobile = phoneNumber;
            Address = address;

            switch(gender)
            {
                case "Žensko":
                    Gender = (byte)0;
                    break;
                case "Muško":
                    Gender = (byte)1;
                    break;
                default:
                    Gender = 3;
                    break;
            }
            
            Email = email;
        }
    }
}
