using SimpleNotebook.BLL.Abstract;
using SimpleNotebook.DI;
using SimpleNotebook.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace SimpleNotebook.PL.Web.Models
{
    public class NoteVM
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Birth year")]
        [Remote("IsExpectedBirthYear", "Note", AreaReference.UseRoot, ErrorMessage = "Birth year is more than expected years")]
        public int BirthYear { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
        public static explicit operator NoteVM(NoteDTO dto)
        {
            return new NoteVM
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthYear = dto.BirthYear,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public static explicit operator NoteDTO(NoteVM model)
        {
            return new NoteDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthYear = model.BirthYear,
                PhoneNumber = model.PhoneNumber
            };
        }

        private static INoteBLL _noteBLO = Provider.NoteBLO;

        /// <summary>
        /// Adds Note to database
        /// </summary>
        /// <param name="noteVM">Note DTO</param>
        /// <returns>Is adding success</returns>
        public static bool Add(NoteVM noteVM)
        {
            return _noteBLO.Add((NoteDTO)noteVM);
        }

        /// <summary>
        /// Removes Note from database
        /// </summary>
        /// <param name="id">Nore Id in database</param>
        /// <returns>Is removing success</returns>
        public static bool Remove(Guid id)
        {
            return _noteBLO.Remove(id);
        }

        /// <summary>
        /// Gets Note from database
        /// </summary>
        /// <param name="id">Note id in database</param>
        /// <returns>NoteVM if exist and NULL otherwise</returns>
        public static NoteVM Get(Guid id)
        {
            return (NoteVM)_noteBLO.Get(id);
        }

        /// <summary>
        /// Get all Notes from database, sorted by names and birth year
        /// </summary>
        /// <returns>All NoteVMs or empty if none exist</returns>
        public static IEnumerable<NoteVM> GetAll()
        {
            return _noteBLO.GetAll().Select(dto => (NoteVM)dto).ToArray();
        }

        /// <summary>
		/// Search in database by: first name, last name and phone number
		/// </summary>
		/// <param name="query">Search query</param>
		/// <returns>Notes that first name, last name or phone number matchs query</returns>
        public static IEnumerable<NoteVM> Find(string query)
        {
            return _noteBLO.Find(query).Select(dto => (NoteVM)dto).ToArray();
        }

        /// <summary>
        /// Validated entered birth year
        /// </summary>
        /// <param name="birthYear">Validated value</param>
        /// <returns>Is value expected match with max human age property</returns>
        public static bool Check(int birthYear)
        {
            return NoteValidator.Check(birthYear, false);
        }
    }
}