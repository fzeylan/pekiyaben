using System;
using System.ComponentModel.DataAnnotations;

namespace Emoda.PekiYaBen.Entity.User
{
    public class AdminUserInfo
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(32)]
        public string Password { get; set; }
        public int Status { get; set; }
    }
}
