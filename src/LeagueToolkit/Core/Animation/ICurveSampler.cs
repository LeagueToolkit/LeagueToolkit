using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation
{
    public interface ICurveSampler<T>
    {
        public T Sample(float time);
    }
}
