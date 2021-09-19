using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IT.Ext
{
    public static class xValueTask
    {
        /// <summary>
        /// ConfigureAwait(false)
        /// </summary>
        public static ConfiguredValueTaskAwaitable CA(this ValueTask task) => task.ConfigureAwait(false);

        /// <summary>
        /// ConfigureAwait(false)
        /// </summary>
        public static ConfiguredValueTaskAwaitable<TResult> CA<TResult>(this ValueTask<TResult> task) => task.ConfigureAwait(false);
    }
}