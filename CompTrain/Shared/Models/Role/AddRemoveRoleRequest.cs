using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Role
{
    public class AddRemoveRoleRequest
    {
        public string RoleId { get; set; }
        public string UserId { get; set; } 
        public bool Add { get; set; }
    }
}
