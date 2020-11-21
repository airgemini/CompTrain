using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Models.Wod
{
    public class ShowResponse
    {
        public DateTime Date { get; set; }
        public bool IsRest { get; set; }

        public bool IsSelected { get; set; }

        public Data.Wod Wod { get; set; }
    }
}
