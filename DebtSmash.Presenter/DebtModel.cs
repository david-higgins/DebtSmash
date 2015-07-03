using System;
using System.Collections.Generic;
using System.Text;

namespace DebtSmash.Presenter
{
    public class Debt
    {
        public String name { get; set; }
        public String description { get; set; }
        public List<String> comments { get; set; }
        public int timesBurned { get; set; }
    }
}
