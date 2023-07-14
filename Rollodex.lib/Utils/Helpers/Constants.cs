
namespace Rolodex.Lib.Utils.Helpers
{
    public class Constants
    {
       public List<string> PermissionList = new List<string>
       {
           "Edit",
           "View",
           "Export",
           "Others"
       };

       public List<string> ModuleList = new List<string>
       {
           "TeamManager",
           "GenerateReport",
           "AddClients",
           "ApiIntegrations",
           "Logs"
       };

        public static string SuperAdmin = "Super Admin";
        public static string ClientAdmin = "Client Admin";
        public static string TeamMember = "Team Member";

        public static string SuccessfulStatus = "Success";
        public static string FailedStatus = "Failed";

        public static string Invitation = "Invitation";
        public static string Login = "Login";
        public static string Search = "Search";

        //Report Type
        public static string Auto = "Auto";
        public static string Manual = "Manual";

        //API Ping status
        public static string Online = "Online";
        public static string Offline = "Offline";

        public static string SucessfulStatus = "Sucessful";

        //Period
        public static string Weekly = "weekly";
        public static string monthly = "monthly";
        public static string yearly = "yearly";
        public static string today = "today";
        public static string all = "all";

        //formatted period
        //Period
        public static string ThisWeek = "This week";
        public static string Thismonth = "monthly";
        public static string Overview = "yearly";

        //Period
        public static string UnSucessfulSearchRequest = "UnSuccesful Search Request";
        public static string SuccessfulSearchRequest = "Succesful Search Request";
        public static string LifeTimeRequest = "Life Time Search Request";


        //identifiers
        public static string primary = "Primary";
        public static string Secondary = "Secondary";

        //category
        public static string Identity = "Identity";
        public static string Entity = "Entity";
        public static string Category = "Category";

        //Metric Settings
        public static string AveargeNumberOfAPiCalls= "Average number of API calls daily, weekly and monthly";
        public static string TotalNumberOfApiCalls = "Total number of API calls daily, weekly and monthly";
        public static string Top3highestAPicallsForTeams = "Top 3 highest API calls for teams daily, weekly and monthly";
        public static string Top3highestAPicallsForMembers = "Top 3 highest API calls for members daily, weekly and monthly";
        public static string TotalNumberOfconvertedtaxpayers = "Total number of converted taxpayers";
        public static string TotalNumberOfUpgradedTaxpayers = "Total number of upgraded taxpayers";
    }
}
