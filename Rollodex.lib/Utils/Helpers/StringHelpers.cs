using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rolodex.Lib.Utils.Helpers
{
    public class StringHelpers
    {
        public static bool IsStringEqual(string value1, string value2)
        {
            return value1.ToLower().Trim() == value2.ToLower().Trim();
        }

        public static bool IsBase64String(string base64String)
        {

            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                var result = Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception)
            {
                // Handle the exception
                return false;
            }


        }

     public   static bool ValidateEmail(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, pattern);
        }

       public static bool ValidateEmailList(List<string> emailList)
        {
            foreach (string email in emailList)
            {
                if (!ValidateEmail(email))
                {
                    return false;
                }
            }
            return true;
        }


        public static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string password = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }

        public static string getHourActivated(int Hour)
        {
            if (Hour >= 0 && Hour < 12)
            {
                return $"{Hour} AM";
            }

            var result = Hour % 12;
            return $"{result} PM";
        }




        public static string getCUrrentDayInAMonth()
        {

            DateTime currentDate = DateTime.Now;
            int year = currentDate.Year;
            int month = currentDate.Month;
            int day = currentDate.Day;

            int daysInMonth = DateTime.DaysInMonth(year, month);

            int daysPassed = day - 1;  // Subtract 1 to get the number of days passed

            return daysPassed.ToString();
        }


        public static string FileSize(string fullPath)
        {
            double fileSize = new FileInfo(fullPath).Length / 1000;
            return fileSize.ToString();
        }


        public static string ToTitleCase(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }
    }



    public class GenericHelper
    {
        private static Expression<Func<T, string>> GetExpression<T>(string property)
        {
            var userdata = Expression.Parameter(typeof(T), "x");
            var menuProperty = Expression.PropertyOrField(userdata, property);
            var lambda = Expression.Lambda<Func<T, string>>(menuProperty, userdata);

            return lambda;
        }

        public static PropertyInfo? GetProperty<T>(string field)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertyInfo = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(field,
                StringComparison.InvariantCultureIgnoreCase));
            return propertyInfo;
        }


        public static object GetPropertyValue<T>(string propertyName, object instanceObject)
        {
            try
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var propertyInfo = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyName,
                    StringComparison.InvariantCultureIgnoreCase));

                var result = propertyInfo.GetValue(instanceObject, null);
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public static List<T> SortData<T>(List<T> data, string sortField, string sortOrder)
        {
            try
            {
                string SortField = sortField.ToLower();
                string SortOrder = sortOrder.ToLower();


                if (string.IsNullOrEmpty(sortField))
                {
                    SortField = "Id";
                    SortOrder = "asc";
                }

                if (string.IsNullOrEmpty(sortOrder))
                {
                    SortOrder = "asc";
                }

                SortField = StringHelpers.ToTitleCase(SortField);

                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo propertyInfo = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(SortField, StringComparison.InvariantCultureIgnoreCase));



                if (SortOrder == "asc")
                {
                    data = data.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                else
                {
                    data = data.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
                }
                return data;
            }
            catch (Exception)
            {

                return data;
            }


        }
    }

}
