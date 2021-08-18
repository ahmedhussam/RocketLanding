using System;
using System.Collections.Generic;

namespace RocketLanding
{
    public class Platform
    {    
         public Boundaries PlatformBoundaries { get; }
         Boundaries LandingAreaBoundaries { get; }
         Dictionary<Guid, Coordinates> RocketsCheckedPositions = new Dictionary<Guid, Coordinates>();
         Dictionary<Coordinates,int> PlaformReserverdPositions = new Dictionary<Coordinates, int>();

        public Platform(int posX,int posY,int width=10, int length = 10, int landingAreaWidth=100, int landingAreaLength = 100)
        {
            PlatformBoundaries = new Boundaries(posX, posY, width, length);
            LandingAreaBoundaries = new Boundaries(0, 0, landingAreaWidth, landingAreaLength);
            ValidatePlatformSize(width, length);
            ValidateLandingAreaSize(landingAreaWidth, landingAreaLength);
            ValidatePlatformInLandingArea();
        }

        private void ValidatePlatformSize(int width, int length)
        {
            var isValidSize = ValidateSize(width, length);
            if (!isValidSize)
                throw new ArgumentException("Plaform Width and Length must be at least 1");
        }

        private void ValidateLandingAreaSize(int width, int length)
        {
            var isValidSize = ValidateSize(width, length);
            if (!isValidSize)
                throw new ArgumentException("Landing Area Width and Length must be at least 1");
        }

        private bool ValidateSize(int width, int length)
        {
            return width >= 1 && length >= 1;
            
        }

        private void ValidatePlatformInLandingArea()
        {
            var isValidPlatformInLandingArea = PlatformBoundaries.Top >= LandingAreaBoundaries.Top &&
                  PlatformBoundaries.Bottom <= LandingAreaBoundaries.Bottom &&
                  PlatformBoundaries.Left >= LandingAreaBoundaries.Left &&
                  PlatformBoundaries.Right <= LandingAreaBoundaries.Right;

            if (!isValidPlatformInLandingArea)
                throw new PlatformOutOfLandingAreaBoundsException(Constants.PLATFORMOUTOFLANDINAREAEXCEPTION);
        }

        public string CheckForLanding(Rocket rocket,int posX,int posY)
        {
            var rocketPosition = new Coordinates(posX, posY);

            if (CheckOutOfPlatform(rocketPosition))
                return Constants.OUTOFPLATFORM;

            else if (CheckPositionClash(rocket.RocketId,rocketPosition))
                return Constants.CLASH;

            else
                RegisterRocketPosition(rocket.RocketId, rocketPosition);

            return Constants.OKFORLANDING;
        }

        private void RegisterRocketPosition(Guid rocketId, Coordinates rocketPosition)
        {
            if (IsPrevPositionSamePosition(rocketId,rocketPosition))
                return;

            RemovePreviousPostions(rocketId);
            AddNewPosition(rocketId,rocketPosition);
        }

        private bool IsPrevPositionSamePosition(Guid rocketId, Coordinates rocketPosition)
        {
            return RocketsCheckedPositions.ContainsKey(rocketId) && RocketsCheckedPositions[rocketId].Equals(rocketPosition);
        }

        private void AddNewPosition(Guid rocketId,Coordinates rocketPosition)
        {
            AddToRocketCheckedPosition(rocketId, rocketPosition);
            var allPositions = GetAllPositions(rocketPosition);
            AddPositionsToPlaformReserverdPositions(allPositions);
        }

        private List<Coordinates> GetAllPositions(Coordinates rocketPosition)
        {
            var allPositions= new List<Coordinates>() { rocketPosition };
            var lstSurroundingPostions = rocketPosition.GetSurroundingPositions();
            allPositions.AddRange(lstSurroundingPostions);
            return allPositions;
            
        }

        private void AddPositionsToPlaformReserverdPositions(List<Coordinates> positions)
        {
            positions.ForEach((p) => 
            {
                if (CheckOutOfPlatform(p))
                    return;

                if (!PlaformReserverdPositions.ContainsKey(p)) PlaformReserverdPositions.Add(p, 1); 
                else PlaformReserverdPositions[p] += 1; 
            });
        }

        private void AddToRocketCheckedPosition(Guid rocketId, Coordinates rocketPosition)
        {
            if (!RocketsCheckedPositions.ContainsKey(rocketId))
                RocketsCheckedPositions.Add(rocketId, rocketPosition);
        }

        private Coordinates RemovePreviousPostions(Guid rocketId)
        {
            if (!RocketsCheckedPositions.ContainsKey(rocketId))
                return null;

            var prevPosition = RocketsCheckedPositions[rocketId];

            var allPositions=GetAllPositions(prevPosition);

            RemoveFromRocketCheckedPositions(rocketId);
            RemoveFromRegistry(allPositions);

            return prevPosition;

        }

        private void RemoveFromRocketCheckedPositions(Guid rocketId)
        {
            RocketsCheckedPositions.Remove(rocketId);
        }

        private void RemoveFromRegistry(List<Coordinates> allPositions)
        {
            allPositions.ForEach((p) => { 
                
                if (PlaformReserverdPositions.ContainsKey(p)) {
                    PlaformReserverdPositions[p]--;
                    if(PlaformReserverdPositions[p]<=0) PlaformReserverdPositions.Remove(p);
                } 
            });
        }

       
       

        private bool CheckPositionClash(Guid rocketId, Coordinates rocketPosition)
        {
            if (IsPrevPositionSamePosition(rocketId, rocketPosition))
                return false;

            var prevPosition=RemovePreviousPostions(rocketId);

            var isValidPosition=PlaformReserverdPositions.ContainsKey(rocketPosition);
            if (!isValidPosition && prevPosition!=null)
                RegisterRocketPosition(rocketId, prevPosition);

            return isValidPosition;
        }

        private bool CheckOutOfPlatform(Coordinates rocketPosition)
        {
            return rocketPosition.X < PlatformBoundaries.Left || rocketPosition.X >= PlatformBoundaries.Right || rocketPosition.Y < PlatformBoundaries.Top || rocketPosition.Y >= PlatformBoundaries.Bottom;
        }
    }
}
