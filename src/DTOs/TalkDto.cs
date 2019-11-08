using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.DTOs
{
    public class TalkDto
    {
        public int TalkId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(4000, MinimumLength= 20)]
        public string Abstract { get; set; }
        [Range(100,300)]
        public int Level { get; set; }

        public SpeakerDto Speaker { get; set; }

    }
}