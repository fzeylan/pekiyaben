using PekiYaBen.API.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class APIResponse
    {
        public bool Succeed { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public APIResponse() { }

        public APIResponse(CommandResponse command)
        {
            this.Succeed = command.Succeed;
            this.Data = command.Data;
            this.Message = command.Message;
            this.Title = command.Title;
        }
    }
}
