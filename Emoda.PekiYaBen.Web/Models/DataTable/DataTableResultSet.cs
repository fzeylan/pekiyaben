using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Emoda.PekiYaBen.Web.Models.DataTable
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DataTableResultSet<T> where T:new()
    {
        /// <summary>Array of records. Each element of the array is itself an array of columns</summary>
        //public List<List<string>> data = new List<List<string>>();

        public IEnumerable<T> data { get; set; }

        /// <summary>value of draw parameter sent by client</summary>
        public int draw;

        /// <summary>filtered record count</summary>
        public int recordsFiltered;

        /// <summary>total record count in resultset</summary>
        public int recordsTotal;

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DataTableResultError<T> : DataTableResultSet<T> where T:new()
    {
        public string error;
    }
}