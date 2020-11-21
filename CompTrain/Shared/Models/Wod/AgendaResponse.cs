using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Wod
{
    public class AgendaResponse
    {
        public DateTime Date { get; set; }
        public Data.Wod Wod { get; set; }

        public bool IsRest { get; set; }

    }
}
