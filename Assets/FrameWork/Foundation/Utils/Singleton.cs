namespace Cr7Sund.Runtime.Util
{
    //PLAN replace with IOC
    // Why IOC
    //1 . use interface but not exposed the real class
    
    // Why not IOC
    // 1. Cache Performance

    public class Singleton<T> where T : new()
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        public static T _instance;
    }
}