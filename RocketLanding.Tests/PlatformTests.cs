using NUnit.Framework;
using System;

namespace RocketLanding.Tests
{
    [TestFixture]
    public class PlatformTests
    {
        Platform _platform;
        const int PLATFORM_POS_X = 5;
        const int PLATFORM_POS_Y = 5;
        const int PLATFORM_WIDTH = 10;
        const int PLATFORM_LENGTH = 10;

        [SetUp]
        public void Setup()
        {
            _platform = new Platform(PLATFORM_POS_X, PLATFORM_POS_Y, PLATFORM_WIDTH, PLATFORM_LENGTH);
        }


        /// <summary>If a rocket checks a valid available landing position on the platform, the returned value is "ok for landing"  </summary>
        [Test]
        public void CheckForLanding_WhenRocketPositionIsInPlatform_ReturnsOkForLanding()
        {
            var rocket=new Rocket();
            var rocketPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 0, 0);
            var result=_platform.CheckForLanding(rocket, rocketPosition.X,rocketPosition.Y);

            Assert.That(result, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If a rocket checks a landing position outside the platform, the returned value is "out of platform"  </summary>
        [Test]
        public void CheckForLanding_WhenRocketPositionIsOutOfPlatform_ReturnsOutOfPlatform()
        {
            var rocket = new Rocket();

            var outOfPlatformPosition= _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( -1, -1);

            var result = _platform.CheckForLanding(rocket, outOfPlatformPosition.X, outOfPlatformPosition.Y);

            Assert.That(result, Is.EqualTo(Constants.OUTOFPLATFORM).IgnoreCase);
        }

        /// <summary>If a rocket checks a landing position that was previously checked by another rocket, the returned value is "clash"  </summary>
        [Test]
        public void CheckForLanding_WhenCurrRocketChecksPrevRocketPosition_ReturnsClash()
        {
            Rocket prevRocket = new Rocket();
            Rocket currRocket = new Rocket();

            var rocketPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 1, 1);
            
            _platform.CheckForLanding(prevRocket, rocketPosition.X, rocketPosition.Y);
           var result=_platform.CheckForLanding(currRocket, rocketPosition.X, rocketPosition.Y);

            Assert.That(result, Is.EqualTo(Constants.CLASH).IgnoreCase);
        }

        /// <summary>If rocket A checks a landing position that was previously checked by rocket B but rocket B changed to another position far from the previous position , the returned value is "Ok For Landing"  </summary>
        [Test]
        public void CheckForLanding_WhenRocketChecksPositionThatAnotherRocketCheckedAndChanged_ReturnsOkForLanding()
        {
            var prevRocket = new Rocket();
            var currRocket = new Rocket();

            var prevRocketFirstPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 1, 1);
            var prevRocketSecondPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 3, 3);
            var currRocketPosition = prevRocketFirstPosition;

            _platform.CheckForLanding(prevRocket, prevRocketFirstPosition.X, prevRocketFirstPosition.Y);
            _platform.CheckForLanding(prevRocket, prevRocketSecondPosition.X, prevRocketSecondPosition.Y);
            var result = _platform.CheckForLanding(currRocket, currRocketPosition.X, currRocketPosition.Y);

            Assert.That(result, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If rocket A checks a landing position that falls in the surrounding area of rocket B position, returns "clash" </summary>
        [Test]
        public void CheckForLanding_WhenCurrRocketChecksPrevRocketSurroundingPosition_ReturnsClash()
        {
            var prevRocket = new Rocket();
            var currRocket = new Rocket();

            var prevRocketPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance(1, 1);
            var currRocketPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance(2, 1);

            _platform.CheckForLanding(prevRocket, prevRocketPosition.X, prevRocketPosition.Y);
            var result = _platform.CheckForLanding(currRocket, currRocketPosition.X, currRocketPosition.Y);

            Assert.That(result, Is.EqualTo(Constants.CLASH).IgnoreCase);
        }

        /// <summary>If multiple rockets check positions that are nonoverlapping and valid with the surrouding area constraint, returns "ok for landing"</summary>
        [Test]
        public void CheckForLanding_WhenMultipleRocketsCheckingNonOverlappingPositions_ReturnsOkForLanding()
        {
            var r1 = new Rocket();
            var r2 = new Rocket();
            var r3 = new Rocket();
            
            var r1Position = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 0, 0);
            var r2Position = _platform.PlatformBoundaries.TopRightCorner.GetPositionByDistance( 0, 0);
            var r3Position = _platform.PlatformBoundaries.BottomLeftCorner.GetPositionByDistance( 0, 0);

            var res1=_platform.CheckForLanding(r1, r1Position.X, r1Position.Y);
            var res2 =_platform.CheckForLanding(r2, r2Position.X, r2Position.Y);
            var res3=_platform.CheckForLanding(r3, r3Position.X, r3Position.Y);
            

            Assert.That(res1, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
            Assert.That(res2, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
            Assert.That(res3, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If multiple rockets check positions that are nonoverlapping and valid with the surrouding area constraint, returns "ok for landing"</summary>
        [Test]
        public void CheckForLanding_WhenSameRocketChecksSamePositionTwice_ReturnsOkForLanding()
        {
            var rocket = new Rocket();
            
            var position= _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance(0, 0);

            _platform.CheckForLanding(rocket, position.X, position.Y);
            var result=_platform.CheckForLanding(rocket, position.X, position.Y);

            Assert.That(result,Is.EqualTo(Constants.OKFORLANDING));
        }

        /// <summary>If Rocket A first position shares boundaries with Rocket B position, then Rocket A changes position that falls in the shared boundaries, returns "clash"</summary>
        [Test]
        public void CheckForLanding_WhenARocketTriesToChangePositionInSharedBoundariesWithAnotherRocket_ThrowsPlatformOutOfBoundException()
        {
            var r1 = new Rocket();
            var r2 = new Rocket();

            var r1Position = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 1, 1);
            var r2Position = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 3, 1);
            var r1SecondPosition = _platform.PlatformBoundaries.TopLeftCorner.GetPositionByDistance( 2, 1);

            _platform.CheckForLanding(r1, r1Position.X, r1Position.Y);
            _platform.CheckForLanding(r2, r2Position.X, r2Position.Y);
            var result=_platform.CheckForLanding(r1, r1SecondPosition.X, r1SecondPosition.Y);

            Assert.That(result, Is.EqualTo(Constants.CLASH).IgnoreCase);
        }

        /// <summary>If Rocket checks Top Left Corner position of the platform, returns "ok for landing"</summary>
        [Test]
        public void CheckForLanding_WhenARocketChecksPlatformTopLeftCorner_ReturnsOkForLanding()
        {
            var rocket = new Rocket();
            var result=_platform.CheckForLanding(rocket, _platform.PlatformBoundaries.TopLeftCorner.X, _platform.PlatformBoundaries.TopLeftCorner.Y);
            Assert.That(result, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If Rocket checks Bottom Left Corner position of the platform, returns "ok for landing"</summary>
        [Test]
        public void CheckForLanding_WhenARocketChecksPlatformBottomLeftCorner_ReturnsOkForLanding()
        {
            var rocket = new Rocket();
            var result = _platform.CheckForLanding(rocket, _platform.PlatformBoundaries.BottomLeftCorner.X, _platform.PlatformBoundaries.BottomLeftCorner.Y);
            Assert.That(result, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If Rocket checks Top Right Corner position of the platform, returns "ok for landing"</summary>
        [Test]
        public void CheckForLanding_WhenARocketChecksPlatformTopRightCorner_ReturnsOkForLanding()
        {
            var rocket = new Rocket();
            var result = _platform.CheckForLanding(rocket, _platform.PlatformBoundaries.TopRightCorner.X, _platform.PlatformBoundaries.TopRightCorner.Y);
            Assert.That(result, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If Rocket checks Bottom Right Corner position of the platform, returns "ok for landing"</summary>
        [Test]
        public void CheckForLanding_WhenARocketChecksPlatformBottomRightCorner_ReturnsOkForLanding()
        {
            var rocket = new Rocket();
            var result = _platform.CheckForLanding(rocket, _platform.PlatformBoundaries.BottomRightCorner.X, _platform.PlatformBoundaries.BottomRightCorner.Y);
            Assert.That(result, Is.EqualTo(Constants.OKFORLANDING).IgnoreCase);
        }

        /// <summary>If platform outside the landing area bounds, throws "platform out of bound" exception</summary>
        [Test]
        public void PlatformConstructor_WhenPlatformOutOfLandingAreaBounds_ThrowsPlatformOutOfBoundException()
        {
            var rocket = new Rocket();

            int platformWidth = 100;
            int platformLength = 100;
            int landingAreaWidth = platformWidth - 1;
            int landingAreaLength = platformLength - 1;
            Assert.That(() => new Platform(0, 0, platformWidth, platformLength, landingAreaWidth, landingAreaLength), Throws.Exception.TypeOf<PlatformOutOfLandingAreaBoundsException>());
        }

        /// <summary>If platform is created with negative dimensions, throws Argument Exception</summary>
        [Test]
        public void PlatformConstructor_WhenCreatingNegativeWidthLength_ThrowsArgumentException()
        {
            var platformWidth = -1;
            var platformLength = -1;
            Assert.That(()=>new Platform(0,0, platformWidth, platformLength), Throws.Exception.TypeOf<ArgumentException>());
        }

        /// <summary>If landing is created with negative dimensions, throws Argument Exception</summary>
        [Test]
        public void PlatformConstructor_WhenCreatingLandingAreaWithNegativeWidthLength_ThrowsArgumentException()
        {
            var platformWidth = 1;
            var platformLength = 1;
            var landingAreaWidth = -1;
            var landingAreaLength = -1;

            Assert.That(() => new Platform(0, 0, platformWidth, platformLength, landingAreaWidth, landingAreaLength), Throws.Exception.TypeOf<ArgumentException>());
        }

        /// <summary>If platform is created with negative position coordinates, throws Argument Exception</summary>
        [Test]
        public void PlatformConstructor_WhenCreatingPlatformWithNegativePositionXY_ThrowsArgumentException()
        {
            var platformX = -1;
            var platformY = -1;

            Assert.That(() => new Platform(platformX, platformY), Throws.Exception.TypeOf<PlatformOutOfLandingAreaBoundsException>());
        }

       
    }
}