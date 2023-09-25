using System;
using System.Threading;

namespace SocketIOClient.Extensions
{
    internal static class CancellationTokenSourceExtensions
    {
        public static void TryDispose(this CancellationTokenSource cts)
        {
            try
            {
                cts?.Dispose();
            }
            catch
            {
                //Ignore
            }
        }

        public static void TryCancel(this CancellationTokenSource cts)
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.TryCancel();
            }
        }
    }
}
