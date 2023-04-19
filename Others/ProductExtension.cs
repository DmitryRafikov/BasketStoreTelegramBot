using BasketStoreTelegramBot.Features.ProductInformation;
using System;

namespace BasketStoreTelegramBot.Others
{
    class CostCalculator
    {
        public static int CalculateCost(int minCost, ISizeContainer sizes)
        { 
            if(sizes.Diameter.HasValue)
                return (int)CalculateCircleArea(sizes.Height.Value, sizes.Diameter.Value)*minCost;
            return (int)CalculateSquareArea(sizes.Height.Value, sizes.Width.Value, sizes.Length.Value)*minCost;
        }
        private static double CalculateSquareArea(int heigth, int width, int length)
        {
            return heigth * width * length;
        }
        private static double CalculateCircleArea(int height, int diameter) 
        {
            double radius = diameter / 2;
            return Math.PI* radius* (radius+height);
        }
    }
}
