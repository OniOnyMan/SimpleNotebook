using SimpleNotebook.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebook.BLL.Abstract
{
    public static class NoteValidator
    {
        /// <summary>
        /// Expected max age of human
        /// </summary>
        public static int MaxAge
        {
            get { return _maxAge; }
            set
            {
                if (value > 0)
                    _maxAge = value;
                else throw new ArgumentException("Age can't have negative value");
            }
        }

        /// <summary>
        /// Incapsuleted field of expected max age
        /// </summary>
        private static int _maxAge = 100;

        static NoteValidator()
        {
            try
            {
                MaxAge = int.Parse(ConfigurationManager.AppSettings["BLODefaultMaxAge"]);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Incorrect configuration file. \"BLODefaultMaxAge\" key is invalid", e);
            }
        } 

        /// <summary>
        /// Validated Note fields
        /// </summary>
        /// <param name="note">Validated note</param>
        /// <param name="isIdCorrectable">Is id property allowed to fix</param>
        /// <returns>Summarized result of validate</returns>
        public static bool Check(NoteDTO note, bool isIdCorrectable = false)
        {
            if (isIdCorrectable && note.Id == Guid.Empty)
                note.Id = Guid.NewGuid();

            return Check(note.BirthYear) && Check(note.Id)
                && Check(nameof(note.FirstName), note.FirstName)
                && Check(nameof(note.LastName), note.LastName)
                && Check(nameof(note.PhoneNumber), note.PhoneNumber);
        }

        /// <summary>
        /// Validated string property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue">Validated value</param>
        /// <returns>Is value valid for application</returns>
        public static bool Check(string propertyName, string propertyValue)
        {
            if (string.IsNullOrWhiteSpace(propertyValue))
                throw new ArgumentException(string.Format("{0} is empty", propertyName));
            else return true;
        }

        /// <summary>
        /// Validated birth year property
        /// </summary>
        /// <param name="birthYear">Validated value</param>
        /// <param name="isExceptionsThrowAble">Is exceptions throwed able</param>
        /// <returns>Is value expected match with max human age property</returns>
        public static bool Check(int birthYear, bool isExceptionsThrowAble = true)
        {
            var nowYear = DateTime.Now.Year;
            if (birthYear > nowYear)
                if (isExceptionsThrowAble)
                    throw new ArgumentOutOfRangeException("Birth year is in future");
                else return false;
            else if (birthYear < nowYear - MaxAge)
                if (isExceptionsThrowAble)
                    throw new ArgumentOutOfRangeException(
                        string.Format("Birth is more than expected {0} years", MaxAge));
                else return false;
            else return true;
        }

        /// <summary>
        /// Validated id property
        /// </summary>
        /// <param name="id">Validated value</param>
        /// <returns>Is value valid for application</returns>
        public static bool Check(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id is empty");
            else return true;
        }
    }
}
