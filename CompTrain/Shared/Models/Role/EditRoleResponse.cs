using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Role
{
    public class EditRoleResponse
    {
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
