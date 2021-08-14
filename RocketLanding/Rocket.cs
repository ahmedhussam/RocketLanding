using System;

namespace RocketLanding
{
    public class Rocket
    {
        public Guid RocketId { get;}
        public Rocket()
        {
            RocketId = Guid.NewGuid();
        }

    }
}
