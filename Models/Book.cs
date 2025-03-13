namespace BookstoreAPI.Models
{
    public class Book
    {
        public string Title { get; set; } // varchar(1023), NOT NULL
        public string? Creator { get; set; } // varchar(127), kann NULL sein
        public DateTime Issued { get; set; } // date, NOT NULL
        public ulong Downloads { get; set; } // bigint unsigned, NOT NULL
        public string Url { get; set; } // varchar(127), NOT NULL
        public string Language { get; set; } // varchar(8), NOT NULL
        public ulong? SubjectId { get; set; } // bigint unsigned, kann NULL sein
    }

}
