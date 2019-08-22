using SimpleNotebook.Entities;
using System.Collections.Generic;

namespace SimpleNotebook.DAL.Abstract
{
    public class NoteComparer : IComparer<NoteDTO>
    {
        public int Compare(NoteDTO first, NoteDTO second)
        {
            var lastNameResult = first.LastName.CompareTo(second.LastName);

            return lastNameResult == 0 ? first.BirthYear.CompareTo(second.BirthYear) : lastNameResult;
        }
    }
}
