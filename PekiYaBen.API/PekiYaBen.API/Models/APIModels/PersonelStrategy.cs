using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class PersonalStrategy
    {
        public PersonalValue[] Values { get; set; }

        public string Vision { get; set; }

        public string Mission { get; set; }
    }

    public class PersonalValue
    {
        public string Title { get; set; }

        public int Order { get; set; }

        public string Comments { get; set; }
    }
}
