using Random = UnityEngine.Random;
namespace Randomize
{
    public static class GetRandom
    {
        public static bool GetChoice(int percent)
        {
            int range = Random.Range(0, 101);
            if (percent > range)
            {
                return true;
            }
            else return false;
        }
        
        public static float Choose(float[] probs)
        {

            float total = 0;
            foreach (float elem in probs)
            {
                total += elem;
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }
        public static float ChooseException(float[] currenProbability, int type, int[,] exceptionArray)
        {
            float total = 0;
            float[] probability = new float[currenProbability.Length];
            

            for (int i = 0; i < currenProbability.Length; i++)
            {
                if (exceptionArray[type, i] == 0)
                {
                    probability[i] = currenProbability[i];
                }
            }
            
            
            foreach (float elem in probability)
            {
                total += elem;
            }
            
            float randomPoint = Random.value * total;

            for (int i = 0; i < probability.Length; i++)
            {
                if (randomPoint < probability[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probability[i];
                }
            }
            return probability.Length - 1;
        }

        public static bool GetBool()
        {
            int range = Random.Range(0, 101);
            if (50 > range)
            {
                return true;
            }
            else return false;
        }
        
        public static int GetRandomException(int min, int max, int exception)
        {
            int random = 0;
            for (int i = 0; i < max * 10; i++)
            {
                random = Random.Range(min, max);
                if(random != exception) break;
            }

            return random;
        }
    }
}