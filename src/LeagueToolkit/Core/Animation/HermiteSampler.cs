using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation;

public struct HermiteSampler<T> : ICurveSampler<T>
{
    public HermiteSampler() { }

    public T Sample(float time) => throw new NotImplementedException();
}
