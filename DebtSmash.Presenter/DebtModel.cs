using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DebtSmash.Presenter
{
    public class Debt
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public List<String> comments { get; set; }
        public int timesBurned { get; set; }
    }
}
