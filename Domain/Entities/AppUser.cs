using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
