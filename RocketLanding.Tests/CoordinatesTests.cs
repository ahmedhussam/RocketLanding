using NUnit.Framework;


namespace RocketLanding.Tests
{
    [TestFixture]
    public class CoordinatesTests
    {
        Coordinates _coordinates;
        const int _DefaultPosX = 5;
        const int _DefaultPosY = 5;

        [SetUp]
        public void SetUp()
        {
            _coordinates = new Coordinates(_DefaultPosX, _DefaultPosY);
        }


        /// <summary>When Coordinates Object ToString() is called, returns the coordinates in this format as "x,y" -> "1,1"</summary>
        [Test]
        public void ToString_WhenCalled_ReturnsFormattedCoords()
        {
            var result = _coordinates.ToString();

            Assert.That(result, Is.EqualTo($"{_DefaultPosX},{_DefaultPosY}"));
        }

        [Test]
        public void Equals_WhenComparingCoordinatesObjWithADifferentObjType_ReturnsFalse()
        {
            
            object obj = new object();

            var result = _coordinates.Equals(obj);

            Assert.That(result, Is.False);
        }

        [Test]
        public void GetSurroundingPositions_WhenCalled_ReturnsListOfCoordinates()
        {
            var result=_coordinates.GetSurroundingPositions();
            Assert.That(result, Has.Count.EqualTo(8));
        }


        [Test]
        public void GetPositionByDistance_WhenCalled_ReturnsNewPositionFromTheCurrPositionByTheDistance()
        {
            var result=_coordinates.GetPositionByDistance(1,1);

            Assert.That(result, Is.EqualTo(new Coordinates(_DefaultPosX + 1, _DefaultPosY + 1)));
        }

        [Test]
        [TestCase(1,1,0,0)]
        [TestCase(2, 2, 1, 1)]
        public void GetPositionByDistance_WhenCalled_ReturnsTopLeftPositionFromTheCurrPosition(int inX,int inY,int outX,int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetTopLeft();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 1, 0)]
        [TestCase(2, 2, 2, 1)]
        public void GetPositionByDistance_WhenCalled_ReturnsTopMiddlePositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetTopMiddle();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 2, 0)]
        [TestCase(2, 2, 3, 1)]
        public void GetPositionByDistance_WhenCalled_ReturnsTopRightPositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetTopRight();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 0, 1)]
        [TestCase(2, 2, 1, 2)]
        public void GetPositionByDistance_WhenCalled_ReturnsMiddleLeftPositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetMiddleLeft();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 2, 1)]
        [TestCase(2, 2, 3, 2)]
        public void GetPositionByDistance_WhenCalled_ReturnsMiddleRightPositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetMiddleRight();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 0, 2)]
        [TestCase(2, 2, 1, 3)]
        public void GetPositionByDistance_WhenCalled_ReturnsBottomLeftPositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetBottomLeft();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 1, 2)]
        [TestCase(2, 2, 2, 3)]
        public void GetPositionByDistance_WhenCalled_ReturnsBottomMiddlePositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetBottomMiddle();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }

        [Test]
        [TestCase(1, 1, 2, 2)]
        [TestCase(2, 2, 3, 3)]
        public void GetPositionByDistance_WhenCalled_ReturnsBottomRightPositionFromTheCurrPosition(int inX, int inY, int outX, int outY)
        {
            var coordinates = new Coordinates(inX, inY);
            var result = coordinates.GetBottomRight();
            Assert.That(result, Is.EqualTo(new Coordinates(outX, outY)));
        }
    }
}
