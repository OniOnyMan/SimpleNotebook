using System;
using System.Collections.Generic;
using SimpleNotebook.DAL.Abstract;
using SimpleNotebook.Entities;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Configuration;

namespace SimpleNotebook.DAL.Xml
{
    public class NoteDAO : INoteDAL
    {
        /// <summary>
        /// Storage file location on disk
        /// </summary>
        public string StorageFilePath
        {
            get { return _storageFilePath; }
            private set
            {
                if (string.IsNullOrWhiteSpace(_storageFilePath))
                    _storageFilePath = value;
            }
        }

        /// <summary>
        /// Return true if storage file exist
        /// </summary>
        public bool IsStorageFileExist { get { return File.Exists(StorageFilePath); } }

        /// <summary>
        /// Shallow copy of data
        /// </summary>
        private List<NoteDTO> _ramStorage;

        /// <summary>
        /// Incapsuleted field for storage file location on disk
        /// </summary>
        private string _storageFilePath;

        /// <summary>
        /// Note comparer for sorting
        /// </summary>
        private NoteComparer _noteComparer = new NoteComparer();

        /// <summary>
        /// On execute read storage file if exist or create on setted file path
        /// </summary>
        public NoteDAO()
        {
            try
            {
                StorageFilePath = ConfigurationManager.AppSettings["DAOXmlFilePath"];
                LoadStorage();
            }
            catch (FileNotFoundException)
            {
                CreateStorage();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Incorrect configuration file. \"DAOXmlFilePath\" key not found", e);
            }
        }

        public bool Add(NoteDTO noteDTO)
        {
            if (noteDTO is null)
                throw new ArgumentNullException(nameof(noteDTO));
            if (_ramStorage.Exists(x => x.Id == noteDTO.Id))
                throw new ArgumentException(string.Format("Id \"{0}\" is already used", noteDTO.Id));
            _ramStorage.Add(noteDTO);
            SaveStorage();
            return true;
        }

        public NoteDTO Get(Guid id)
        {
            if (_ramStorage.Exists(x => x.Id == id))
            {
                var note = _ramStorage.Find(x => x.Id == id);
                return new NoteDTO
                {
                    Id = note.Id,
                    FirstName = note.FirstName,
                    LastName = note.LastName,
                    BirthYear = note.BirthYear,
                    PhoneNumber = note.PhoneNumber
                };
            }
            else return null;
        }

        public IEnumerable<NoteDTO> GetAll()
        {
            var result = _ramStorage.ToArray(); //TO DO: find method that return deep copy
            Array.Sort(result, _noteComparer);
            return result;
        }

        public bool Remove(Guid id)
        {
            if (_ramStorage.Exists(x => x.Id == id)) {
                _ramStorage.RemoveAll(x => x.Id == id);
                SaveStorage();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Read data from storage file if exist
        /// </summary>
	    /// <exception cref="FileNotFoundException"></exception>
        private void LoadStorage()
        {
            if (IsStorageFileExist)
            {
                using (var reader = new StreamReader(StorageFilePath))
                {
                    var serializer = new XmlSerializer(typeof(List<NoteDTO>));
                    _ramStorage = (List<NoteDTO>)serializer.Deserialize(reader);
                }
            }
            else throw new FileNotFoundException(
                string.Format("File with name \"{0}\" doesn't exist", StorageFilePath));
        }

        /// <summary>
        /// Create storage file
        /// </summary>
        private void CreateStorage()
        {
            _ramStorage = new List<NoteDTO>();
            using (File.Create(StorageFilePath)) { }
            SaveStorage();
        }

        /// <summary>
        /// Overwrite storage file
        /// </summary>
        private void SaveStorage()
        {
            if (IsStorageFileExist)
                File.Delete(StorageFilePath);
            using (var writer = new StreamWriter(StorageFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<NoteDTO>));
                serializer.Serialize(writer, _ramStorage);
            }
        }
    }
}
