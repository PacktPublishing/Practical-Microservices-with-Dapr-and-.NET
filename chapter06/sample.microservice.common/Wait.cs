using System;

namespace sample.microservice.common
{
    public class Wait
    {
        public static void Random(int minWait = 3000, int maxWait = 10000)
        {
            int wait = new System.Random().Next(minWait,maxWait);
            System.Threading.Thread.Sleep(wait);
        }
    }
}
