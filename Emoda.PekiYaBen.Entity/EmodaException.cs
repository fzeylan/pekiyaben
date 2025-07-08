using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emoda.PekiYaBen.Entity
{
    public class EmodaException: Exception
    {
        public EmodaException(string message, Exception innerEx = null) :
            base("PekiYaBen : " + message, innerEx)
        {

        }
    }
}
