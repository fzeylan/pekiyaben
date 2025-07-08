using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Validation
{
    public class DateBeforeNow : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d;
            if (value.GetType() == typeof(int))
            {
                if (!DateTime.TryParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                  DateTimeStyles.None, out d))
                {
                    return false;
                }
            }
            else
            {
                d = Convert.ToDateTime(value);
            }

            return d <= DateTime.Now;

        }
    }

    public class DateAfterNow : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d;
            if (value.GetType() == typeof(int))
            {
                if (!DateTime.TryParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                  DateTimeStyles.None, out d))
                {
                    return false;
                }
            }
            else
            {
                d = Convert.ToDateTime(value);
            }

            return d <= DateTime.Now;
        }
    }
}
