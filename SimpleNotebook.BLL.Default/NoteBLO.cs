using SimpleNotebook.BLL.Abstract;
using SimpleNotebook.DAL.Abstract;
using SimpleNotebook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebook.BLL.Default
{
    public class NoteBLO : INoteBLL
    {
        /// <summary>
        /// DAL instance
        /// </summary>
        private INoteDAL _noteDAO;

        /// <summary>
        /// On execute provide execute DAL instance
        /// </summary>
        /// <param name="noteDAO">DAL instance</param>
        public NoteBLO(INoteDAL noteDAO)
        {
            _noteDAO = noteDAO ?? throw new ArgumentNullException("NoteDAO is null");
        }

        public bool Add(NoteDTO noteDTO)
        {
            try
            {
                NoteValidator.Check(noteDTO, true);
                return _noteDAO.Add(noteDTO);
            }
            catch (Exception e)
            {
                //TO DO: log here
                return false;
            }
        }

        public IEnumerable<NoteDTO> Find(string query)
        {
            try
            {
                query = query.ToLower();
                NoteValidator.Check(nameof(query), query);
                return _noteDAO.GetAll().Where(x => x.LastName.ToLower().Contains(query)
                                                 || x.FirstName.ToLower().Contains(query)
                                                 || x.PhoneNumber.Contains(query));
            }
            catch (Exception e)
            {
                //TO DO: log here
                return new NoteDTO[0];
            }
        }

        public NoteDTO Get(Guid id)
        {
            try
            {
                NoteValidator.Check(id);
                return _noteDAO.Get(id);
            }
            catch (Exception e)
            {
                //TO DO: log here
                return new NoteDTO();
            }
        }

        public IEnumerable<NoteDTO> GetAll()
        {
            try
            {
                return _noteDAO.GetAll();
            }
            catch (Exception e)
            {
                //TO DO: log here
                return null;
            }
        }

        public bool Remove(Guid id)
        {
            try
            {
                NoteValidator.Check(id);
                return _noteDAO.Remove(id);
            }
            catch (Exception e)
            {
                //TO DO: log here
                return false;
            }
        }
    }
}
