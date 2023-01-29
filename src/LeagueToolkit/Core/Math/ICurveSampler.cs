using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Math
{
    public interface ICurveSampler<T>
    {
        public T Sample(float time);
    }
}
