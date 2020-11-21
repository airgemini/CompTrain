using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompTrain.Server.Services
{
    public class EmailSenderOption
    {
        public string From { get; set; }
        public bool IsAuthenticate { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string Host { get; set; }

        public bool SSL { get; set; }

        public int Port { get; set; }

        public bool IsBodyHTML { get; set; } = true;
        public string Bcc { get; set; }
    }
}
