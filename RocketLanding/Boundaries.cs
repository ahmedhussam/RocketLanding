namespace RocketLanding
{
    public class Boundaries
    {
        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Bottom { get; }

        private Coordinates _TopLeftCorner;
        public Coordinates TopLeftCorner { get { if (_TopLeftCorner == null) _TopLeftCorner = new Coordinates(Left,Top);return _TopLeftCorner; } }
        
        private Coordinates _TopRightCorner;
        public Coordinates TopRightCorner { get { if (_TopRightCorner == null) _TopRightCorner = new Coordinates(Right-1, Top); return _TopRightCorner; } }

        private Coordinates _BottomRightCorner;
        public Coordinates BottomRightCorner { get { if (_BottomRightCorner == null) _BottomRightCorner = new Coordinates(Right - 1, Bottom-1); return _BottomRightCorner; } }

        private Coordinates _BottomLeftCorner;
        public Coordinates BottomLeftCorner { get { if (_BottomLeftCorner == null) _BottomLeftCorner = new Coordinates(Left, Bottom - 1); return _BottomLeftCorner; } }


        public Boundaries(int posX,int posY,int width,int length)
        {
            Left = posX;
            Right = posX + width;
            Top = posY;
            Bottom = posY + length;

        }
    }
}
