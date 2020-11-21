using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Account
{
    public class ChangePasswordResponse
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
