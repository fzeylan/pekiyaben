using System.Collections.Generic;

namespace PekiYaBen.WebSite.Models
{
    public partial class ApiResponse
    {
        public bool Result
        {
            get
            {
                return this.Errors.Count == 0;
            }
        }
        public object Data { get; set; }
        public List<ServiceError> Errors { get; set; } = new List<ServiceError>();

        public ApiResponse() { }

        public ApiResponse(string message)
        {
            this.Message = message;
        }

        public ApiResponse(object data)
        {
            this.Data = data;
        }

        public ApiResponse(List<ServiceError> errors)
        {
            this.Errors = errors;
        }

        public ApiResponse(ServiceError error)
        {
            this.Errors.Add(error);
        }

        public ApiResponse(object data, ServiceError error)
        {
            this.Data = data;
            this.Errors.Add(error);
        }

        public ApiResponse(object data, string message, ServiceError error)
        {
            this.Data = data;
            this.Message = message;
            this.Errors.Add(error);
        }

        public ApiResponse(object data, List<ServiceError> errors)
        {
            this.Data = data;
            this.Errors = errors;
        }
        public ApiResponse(object data, string message, List<ServiceError> errors)
        {
            this.Data = data;
            this.Message = message;
            this.Errors = errors;
        }
    }
    public partial class ApiResponse
    {
        public string Message { get; set; }
        public string RedirectUrl { get; set; }

        //public int Draw { get; set; }
        //public int RecordsTotal { get; set; }
        //public int RecordsFiltered { get { return RecordsTotal; } }

    }
}
