using System;

namespace SimpleNotebook.Entities
{
    public class NoteDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public string PhoneNumber { get; set; }
    }
}
