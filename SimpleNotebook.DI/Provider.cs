using System;
using System.Configuration;
using SimpleNotebook.BLL.Default;
using SimpleNotebook.BLL.Abstract;
using SimpleNotebook.DAL.Abstract;
using SimpleNotebook.DAL.Xml;

namespace SimpleNotebook.DI
{
    public class Provider
    {
        /// <summary>
        /// DAL instance
        /// </summary>
        public static INoteDAL NoteDAO { get; private set; }

        /// <summary>
        /// BLL instance
        /// </summary>
        public static INoteBLL NoteBLO { get; private set; }


        static Provider()
        {
            string dal;
            try
            {
                dal = ConfigurationManager.AppSettings["DAL"];
            }
            catch (Exception e)
            {
                throw new ApplicationException("Incorrect configuration file. \"DAL key\" not found", e);
            }

            string bll;
            try
            {
                bll = ConfigurationManager.AppSettings["BLL"];
            }
            catch (Exception e)
            {
                throw new ApplicationException("Incorrect configuration file. \"BLL\" key not found", e);
            }

            SetCurrentDAL(dal);
            SetCurrentBLL(bll);
        }

        private static void SetCurrentDAL(string configValue)
        {
            switch (configValue.ToLower())
            {
                case "xml":
                    NoteDAO = new NoteDAO();
                    break;
                default:
                    throw new ApplicationException(
                        string.Format("Incorrect configuration file. Unexpected \"DAL\" key value: {0}", configValue));
            }
        }

        private static void SetCurrentBLL(string configValue)
        {
            switch (configValue.ToLower())
            {
                case "default":
                    NoteBLO = new NoteBLO(NoteDAO);
                    break;
                default:
                    throw new ApplicationException(
                        string.Format("Incorrect configuration file. Unexpected \"BLL\" key value: {0}", configValue));
            }
        }

    }
}
