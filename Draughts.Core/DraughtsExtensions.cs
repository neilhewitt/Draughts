using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public static class DraughtsExtensions
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> input)
        {
            int count = input.Count();
            List<T> inputAsList = new List<T>(input);

            int[] indices = Enumerable.Range(0, count).ToArray();

            for (int i = 0; i < count; ++i)
            {
                int position = _random.Next(i, count);
                yield return inputAsList[indices[position]];
                indices[position] = indices[i];
            }
        }

        public static T RandomFirstOrDefault<T>(this IEnumerable<T> input)
        {
            return input.Randomize().FirstOrDefault();
        }
    }
}
