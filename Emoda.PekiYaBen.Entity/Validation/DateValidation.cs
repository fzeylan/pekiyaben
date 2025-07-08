using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Emoda.PekiYaBen.Entity.Validation
{
    public class DateBeforeThanNow : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d;
            if (value.GetType() == typeof(int)) { 
              if(!DateTime.TryParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
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

    public class DateAfterThanNow : ValidationAttribute
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
