using System.Collections.Generic;

namespace RocketLanding
{
    public class Coordinates
    {
        public Coordinates(int x,int y){
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get;}

        public List<Coordinates> GetSurroundingPositions()
        {
            return new List<Coordinates>()
            {
                 GetTopLeft(),
                 GetTopMiddle(),
                 GetTopRight(),
                 GetMiddleLeft(),
                 GetMiddleRight(),
                 GetBottomLeft(),
                 GetBottomMiddle(),
                 GetBottomRight()
            };
        }

        public Coordinates GetPositionByDistance(int distX, int distY)
        {
            return new Coordinates(X + distX, Y + distY);
        }

        public Coordinates GetTopLeft()
        {
            return new Coordinates(X - 1, Y - 1);
        }

        public Coordinates GetTopMiddle()
        {
            return new Coordinates(X, Y - 1);
        }

        public Coordinates GetTopRight()
        {
            return new Coordinates(X + 1, Y - 1);
        }

        public Coordinates GetMiddleLeft()
        {
            return new Coordinates(X - 1, Y);
        }

        public Coordinates GetMiddleRight()
        {
            return new Coordinates(X + 1, Y);
        }

        public Coordinates GetBottomLeft()
        {
            return new Coordinates(X - 1, Y + 1);
        }

        public Coordinates GetBottomMiddle()
        {
            return new Coordinates(X, Y + 1);
        }

        public Coordinates GetBottomRight()
        {
            return new Coordinates(X + 1, Y + 1);
        }


        public override int GetHashCode()
        {
            return $"{X},{Y}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinates))
                return false;

            var coords = obj as Coordinates;
            return X== coords.X && Y== coords.Y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }


    }
}
