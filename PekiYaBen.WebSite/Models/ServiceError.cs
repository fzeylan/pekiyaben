using System;
using System.Collections.Generic;

namespace PekiYaBen.WebSite.Models
{
    public class ServiceException : Exception
    {
        public List<ServiceError> Errors { get; set; }

        public ServiceException(string source, List<ServiceError> errors)
        {
            Source = source;
            Errors = errors;
        }
    }

    public class ServiceError
    {
        public ServiceError(string description = "", string code = "")
        {
            Description = description;
            Code = code;
        }

        public string Code { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Code) ? Description : $"[{Code}]: {Description}";
        }
    }
}
