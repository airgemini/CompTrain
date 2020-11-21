using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Account
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Token { get; set; }
    }
}
