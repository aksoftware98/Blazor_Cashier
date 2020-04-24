namespace BlazorCashier.Shared
{
    public class ApiEndpoints
    {
        public static string Base = "http://localhost:44306";
        public static string Customers = $"{Base}/customers";
        public static string Vendors = $"{Base}/vendors";
        public static string Bills = $"{Base}/bills";
        public static string Stocks = $"{Base}/stocks";
        public static string Items = $"{Base}/items";
        public static string Sessions = $"{Base}/sessions";
        public static string Employees = $"{Base}/employees";
        public static string Invoices = $"{Base}/invoices";
        public static string Discounts = $"{Base}/discounts";
        public static string Organizations = $"{Base}/organizations";
        public static string Auth = $"{Base}/auth";
        public static string Login = $"{Auth}/login";
        public static string ChangePassword = $"{Auth}/changepassword";
    }
}
