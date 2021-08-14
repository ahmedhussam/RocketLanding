using System;


namespace RocketLanding
{
    public class PlatformOutOfLandingAreaBoundsException : Exception
    {
        public PlatformOutOfLandingAreaBoundsException(string message) : base(message)
        {

        }
    }
}
