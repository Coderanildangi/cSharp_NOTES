using System.Text.Json.Serialization;

namespace Note_Server.Models
{
    public class Note
    {
        // OD must have a public setter for EF Core to work properly.
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tag { get; set; } = "None";

        //Setters and Getters for time stamps.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // This property is ignored during serialization.
        [JsonIgnore]
        public bool IsConfidential => Tag.Equals("Confidential", StringComparison.OrdinalIgnoreCase);

        // Create a safe , redacted version of the note.
        public Note RedactedCopy()
        {
            return new Note
            {
                // Use the existing Id, but copy other properties.
                Id = Id, 
                Title = $"LOCKED: {Title}",
                Content = "Content is confidential and requires unlock.",
                Tag = Tag,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }
    }
}
