using CustomerOrderManagement.Domain.Common;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace CustomerOrderManagement.Domain.Entities
{
    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
