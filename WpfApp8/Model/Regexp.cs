using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp8.Model
{
    public static class Regexp
    {

        public static bool IsProductNameValid(string productname)
        {
            string pattern = @"\d";
            if (string.IsNullOrEmpty(productname) || productname.Length > 20)
            {
                return false;
            }
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(productname);
            if (matches.Count > 0)
            {
                return false;
            }
            return true;


        }
        public static bool IsDescriptionValid(string description)
        {
            if (string.IsNullOrEmpty(description) || description.Length > 20)
                return false;
            return true;


        }

        public static bool IsPriceValid(double? price)
        {
            if(price == null) { return false; }
            try
            {
                Convert.ToDouble(price);
            }
            catch (Exception ex)
            {
                return false;
            }
            if(price > 1000000000)
            {
                return false;
            }
            return true;


        }
    }
}
