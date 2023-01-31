using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation;

public interface IInterpolator<T>
{
    public T InterpolateCatmull(float time, float tau20, float tau31, T p0, T p1, T p2, T p3);
}
