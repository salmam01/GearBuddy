using System.Text.Json;
using GearBuddy.Models;

namespace GearBuddy.Services
{
    public class Pricing
    {
        private static readonly DrawCost _drawData;
        static Pricing() {
            _drawData = Pricing.GetDrawData();

            // Make sure beadPackages is high->low
            _drawData.beadPackages.Sort();
            _drawData.beadPackages.Reverse();
        }

        private static DrawCost GetDrawData()
        {   
            string json = File.ReadAllText("./Data/DrawCost.json");
            return JsonSerializer.Deserialize<DrawCost>(json);
        }

        public static int GetBeadQuantityFor(int draws)
        {
            return _drawData.beadsPerDraw * draws;
        }
        
        public static List<int> GetPackages(int beadQuantity)
        {
            
            int accCost = beadQuantity;
            List<int> packages = [];
            
            for (int i = 0; i < _drawData.beadPackages.Count; i++)
            {
                while(accCost >= _drawData.beadPackages[i]) {
                    accCost -= _drawData.beadPackages[i];
                    packages.Add(_drawData.beadPackages[i]);
                }
            }

            // check if there are any remaining beads
            // and add the cheapest package
            if (accCost > 0) {
                packages.Add(_drawData.beadPackages[_drawData.beadPackages.Count - 1]);
            }

            return packages;
        }

        public static double PackagesToPrice(List<int> packages, string currency)
        {
            double cost = 0;

            for (int i = 0; i < packages.Count; i++)
            {
                cost += _drawData.price[currency][packages[i].ToString()];
            }

            return cost;
        }

        public static string GetDrawPrice(int draws, string currency)
        {
            int beadQuantity = GetBeadQuantityFor(draws);
            List<int> packages = GetPackages(beadQuantity);
            double cost = PackagesToPrice(packages, currency);
            return FormatForCurrenct(cost, currency);
        }

        public static string GetDrawPrice(List<int> packages, string currency)
        {
            double cost = PackagesToPrice(packages, currency);
            return FormatForCurrenct(cost, currency);
        }
        
        public static string FormatForCurrenct(double cost, string currency)
        {
            return _drawData.currencyFormat[currency].Replace("{cost}", cost.ToString("0.00"));
        }

        public static string FormatPackages(List<int> packages)
        {
            return string.Join("\r\n",
                packages
                    .GroupBy(p => p)
                    .Select(g => g.Count() > 1 ? $"{g.Key} x {g.Count()}" : $"{g.Key} x 1")
            );
        }

    }
}
