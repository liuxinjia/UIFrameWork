namespace Cr7Sund.Transition.UI
{
    public interface ITransition
    {
        float Duration { get; }
        void Resume(float time);
        void Play(bool enter);
        void Reset();
        void Stop();


    }
}