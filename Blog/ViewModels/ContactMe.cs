using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.ViewModels
{
    public class ContactMe
    {
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 5)]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 5)]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 5)]
        public string Message { get; set; }
    }
}
