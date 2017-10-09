using EXP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EXP.Core.Interface
{
    public interface IValidationBookRepository
    {
        /// <summary>
        /// Get list validation books for page
        /// </summary>
        /// <returns></returns>
        List<ValidationBook> ListValidationBooksForPage(TableList listParam);
        /// <summary>
        /// Get validation books count
        /// </summary>
        /// <returns></returns>
        int GetValidationBooksCount(TableList listParam);
        /// <summary>
        /// Get Validation Book By Id
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        ValidationBook GetValidationBookById(int BookID);
        /// <summary>
        /// Create Validation Book
        /// </summary>
        /// <param name="ValidationBook"></param>
        /// <returns></returns>
        bool CreateValidationBook(ValidationBook ValidationBook);
        /// <summary>
        /// Update Validation Book
        /// </summary>
        /// <param name="ValidationBook"></param>
        /// <returns></returns>
        bool UpdateValidationBook(ValidationBook ValidationBook);
        /// <summary>
        /// Delete Validation Book
        /// </summary>
        /// <param name="ValidationBookID"></param>
        /// <returns></returns>
        void DeleteValidationBook(int ValidationBookID);
        

    }
}
