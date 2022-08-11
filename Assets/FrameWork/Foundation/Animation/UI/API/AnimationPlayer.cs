namespace Cr7Sund.Animation.UI
{
    public class AnimationPlayer : IUpdatable
    {
        public IAnimation self;

        public ITransitionAnimation Animation { get;  set; }
        public bool IsPlaying { get; private set; }
        public bool IsFinished { get; private set; }
        public float Time { get; private set; }

        public void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void Play(){
            IsPlaying =  true;
        }
    }
}