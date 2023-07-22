using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Feedback
    {
        public Feedback()
        {
            DateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        //public int RatingId { get; set; }
        //public Rating Rating { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
