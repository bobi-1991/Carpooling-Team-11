using CarPooling.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Feedback:Entity
    {

        public Feedback()
        {
        }

        public Feedback(string authorId, string recipientId, string comment, int rating)
            : base()
        {
            this.AuthorId = authorId;
            this.RecipientId = recipientId;
            this.Comment = comment;
            this.Rating = rating;
        }
        public int Rating { get; set; }
        public string Comment { get; set; }

        // Foreign key for Author
        public string AuthorId { get; set; }

        // Navigation property for Author
        public User Author { get; set; }

        // Foreign key for Recipient
        public string RecipientId { get; set; }

        // Navigation property for Recipient
        public User Recipient { get; set; }

    }
}
