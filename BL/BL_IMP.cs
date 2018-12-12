using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EZsearch3.DAL;
using EZsearch3.Models;

namespace EZsearch3.BL
{
    public class BL_IMP
    {
        /// <summary>
        /// The function saves the search details into tbl_history on ezsearchDB, using DAL layer.
        /// </summary>
        /// <param name="search">A search entity. Contains the value and the date will be enter to the DB.</param>
        /// <returns>True- if added successfully, else- False.</returns>
        public static bool AddSearchEntity(SearchEnt search)
        {
            try
            {
                //if value is already in DB updates the date only
                if (DAL_DB.GetHistoryList().Any(s => s.Value == search.Value))
                    return DAL_DB.UpdateSearchDate(search); 
                return DAL_DB.AddSearchEntity(search);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Upadates the date of value is already in DB, calling DAL layer.
        /// </summary>
        /// <param name="search">A search entity. Contains current date.</param>
        /// <returns>True- if updated successfully, else- False.</returns>
        public static bool UpdateSearchDate(SearchEnt search)
        {
            try
            {
                return DAL_DB.UpdateSearchDate(search);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Returns list of all the previous searches, order by date, calling DAL layer.
        /// </summary>
        /// <returns>List of search entities</returns>
        public static IEnumerable<SearchEnt> GetHistoryList()
        {
            try
            {
                return DAL_DB.GetHistoryList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }


    }
}