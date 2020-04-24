using System.Linq.Expressions;

namespace BlazorCashier.Models.Identity
{
    public static class Roles
    {
        public const string Owner = nameof(Owner);
        public const string Sales = nameof(Sales);
        public const string Supervisor = nameof(Supervisor);
    }
}
