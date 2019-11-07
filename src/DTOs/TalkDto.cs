namespace CoreCodeCamp.DTOs
{
    public class TalkDto
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int Level { get; set; }

        public SpeakerDto Speaker {get;set;}

    }
}