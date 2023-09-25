using System;
using System.Threading.Tasks;

namespace SocketIOClient.Extensions
{
    internal static class EventHandlerExtensions
    {
        public static void TryInvoke<T>(this EventHandler<T> handler, object sender, T args)
        {
            handler?.Invoke(sender, args);
        }

        public static void TryInvoke(this EventHandler handler, object sender, EventArgs args)
        {
            handler?.Invoke(sender, args);
        }
        
        public static void TryInvoke<T>(this Action<T> action, T arg1)
        {
            action?.Invoke(arg1);
        }
        
        public static async Task TryInvokeAsync<T>(this Func<T, Task> func, T arg1)
        {
            if (func is null)
            {
                return;
            }
            
            var task = func(arg1);

            if(task is not null)
            {
                await task;
            }
        }
    }
}
