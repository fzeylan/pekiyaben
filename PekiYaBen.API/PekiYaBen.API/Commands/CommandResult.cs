using Microsoft.Extensions.Configuration;
using PekiYaBen.API.Models.Entities;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using PekiYaBen.API.Helpers;

namespace PekiYaBen.API.Commands
{
    public class CommandResponse
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public object Data { get; set; }
    }
}
