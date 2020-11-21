using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Account
{
    public class EmailConfirmationRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }

    }
}
