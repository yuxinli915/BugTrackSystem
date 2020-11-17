using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class SystemItem
    {
        public int Id { get; set; }
        [Required, MaxLength(30, ErrorMessage = "Your title is too long.")]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}