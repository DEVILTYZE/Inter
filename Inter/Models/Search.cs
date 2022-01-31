using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Inter.Models
{
    public class Search
    {
        [HiddenInput]
        public List<Post> Posts { get; set; }
        
        [HiddenInput]
        public List<User> Users { get; set; }
    }
}