using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPassionProjectW2024n01605783.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        public string BlogHeading { get; set; }
        public string BlogContent { get; set; }
        public string BlogShortDescription { get; set; }

        public DateTime BlogPublishedDate { get; set; }
        public string BlogAuthor { get; set; }


        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
    }

    public class BlogDto
    {
        public int BlogId { get; set; }

        public string BlogHeading { get; set; }

        public string BlogContent { get; set; }

        public string BlogShortDescription { get; set; }

        public string BlogAuthor { get; set; }

        public DateTime BlogPublishedDate { get; set; }
        public int ExerciseId { get; set; }
    }
}