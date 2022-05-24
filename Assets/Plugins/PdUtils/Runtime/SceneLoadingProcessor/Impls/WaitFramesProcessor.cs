using System;
using UniRx;

namespace PdUtils.SceneLoadingProcessor.Impls
{
    public class WaitFramesProcessor : Process
    {
        private readonly int _frames;

        public WaitFramesProcessor(int frames)
        {
            _frames = frames;
        }

        public override void Do(Action onComplete)
        {
            Observable.IntervalFrame(_frames, FrameCountType.EndOfFrame)
                .Subscribe(l => onComplete());
        }
    }
}