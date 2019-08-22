using SimpleNotebook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebook.BLL.Abstract
{
    public interface INoteBLL
    {
        /// <summary>
        /// Adds Note to database
        /// </summary>
        /// <param name="noteDTO">Note DTO</param>
        /// <returns>Is adding success</returns>
        /// <exception cref="ArgumentNullException"></exception>
        bool Add(NoteDTO noteDTO);

        /// <summary>
        /// Removes Note from database
        /// </summary>
        /// <param name="id">Nore Id in database</param>
        /// <returns>Is removing success</returns>
        bool Remove(Guid id);

        /// <summary>
        /// Gets Note from database
        /// </summary>
        /// <param name="id">Note id in database</param>
        /// <returns>NoteDTO if exist and NULL otherwise</returns>
        NoteDTO Get(Guid id);

        /// <summary>
        /// Get all Notes from database, sorted by names and birth year
        /// </summary>
        /// <returns>All NoteDTOs or empty if none exist</returns>
        IEnumerable<NoteDTO> GetAll();

        /// <summary>
		/// Search in database by: first name, last name and phone number
		/// </summary>
		/// <param name="query">Search query</param>
		/// <returns>Notes that first name, last name or phone number matchs query</returns>
	    IEnumerable<NoteDTO> Find(string query);
    }
}
