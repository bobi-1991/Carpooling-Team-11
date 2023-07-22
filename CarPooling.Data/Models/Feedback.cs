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

        public Feedback(int authorId, int recipientId, string comment, int rating)
            : base()
        {
            this.AuthorId = authorId;
            this.RecipientId = recipientId;
            this.Comment = comment;
            this.Rating = rating;
        }
        public int Rating { get; set; }
        public string Comment { get; set; }

        // Foreign keys with navigation properties
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int RecipientId { get; set; }
        public User Recipient { get; set; }

    }
}
