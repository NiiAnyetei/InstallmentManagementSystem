using Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Data
{
    public class UserRefreshToken
    {
        [Key]
        public Guid Id { get; set; } = SequentialGuidGenerator.Instance.Create();

        [Required]
        public string UserName { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
