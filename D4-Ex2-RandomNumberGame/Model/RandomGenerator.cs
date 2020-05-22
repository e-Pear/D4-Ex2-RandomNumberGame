using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Model
{
    class RandomGenerator
    {
        private Random random;

        public RandomGenerator()
        {
            random = new Random();
        }

        public List<int> DrawNumbersWithoutRepetition(int amountOfDrawnNumbers, int minDrawValue, int maxDrawValue)
        {
            List<int> drawResults = new List<int>();
            List<int> drawPool = new List<int>();
            
            int drawnPos;

            if (maxDrawValue - minDrawValue + 1 < amountOfDrawnNumbers) return drawResults;

            for (int i = minDrawValue; i <= maxDrawValue; i++)
            {
                drawPool.Add(i);
            }

            for (int j = 0; j < amountOfDrawnNumbers; j++)
            {
                drawnPos = random.Next(minDrawValue, (maxDrawValue + 1) - j);
                drawResults.Add(drawPool[drawnPos - 1]);
                drawPool.RemoveAt(drawnPos - 1);
            }

            return drawResults;
        }
    }
}
