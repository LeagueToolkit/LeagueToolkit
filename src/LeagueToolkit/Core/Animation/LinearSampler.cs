using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation;

public sealed class LinearSampler<T> : ICurveSampler<T>
{
    public T Sample(float time) => throw new NotImplementedException();
}
