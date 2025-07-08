using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emoda.PekiYaBen.Entity.PersonalStrategy
{
    public class PersonalStrategy
    {
        public LifeArea[] _LifeAreas { get; set; }

        public string _Vision { get; set; }

        public string _Mission { get; set; }

    }

    public class LifeArea
    {
        public string _Area { get; set; }

        public int _Order { get; set; }

        public string _Description { get; set; }

        public int _Point { get; set; }
    }
}
