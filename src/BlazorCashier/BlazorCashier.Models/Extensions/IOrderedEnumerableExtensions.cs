using System.Collections.Generic;
using System.Linq;

namespace BlazorCashier.Models.Extensions
{
    public static class IOrderedEnumerableExtensions
    {
        public static Stack<T> ToStack<T>(this IOrderedEnumerable<T> elements)
        {
            Stack<T> stack = new Stack<T>();
            
            elements.ToList().ForEach(item =>
            {
                stack.Push(item);
            });

            return stack;
        }
    }
}
