using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IT.Ext
{
    public static class xTask
    {
        /// <summary>
        /// ConfigureAwait(false)
        /// </summary>
        public static ConfiguredTaskAwaitable CA(this Task task) => task.ConfigureAwait(false);

        /// <summary>
        /// ConfigureAwait(false)
        /// </summary>
        public static ConfiguredTaskAwaitable<TResult> CA<TResult>(this Task<TResult> task) => task.ConfigureAwait(false);
    }
}