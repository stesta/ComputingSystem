using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing.Core;

public static class Clocks
{
    public static IEnumerable<int> Clock()
    {
        while (true)
        {
            yield return 0;
            yield return 1;
        }
    }

    public static IEnumerable<int> SlowClock()
    {
        while (true)
        {
            yield return 0;
            yield return 0;
            yield return 1;
        }
    }
}