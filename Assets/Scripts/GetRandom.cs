using Random = UnityEngine.Random;
namespace Randomaze
{
    public class GetRandom
    {
        public static bool GetChooise(int percent)
        {
            int range = Random.Range(0, 101);
            if (percent > range)
            {
                return true;
            }
            else return false;
        }
      
    }
}