using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;

namespace Zenject
{
    // See comment in ILateFixed.cs for usage
    public class LateFixedManager
    {
        List<UpdatableInfo> _updatables;

        public LateFixedManager(
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ILateFixed> updatables,
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ValuePair<Type, int>> priorities)
        {
            _updatables = new List<UpdatableInfo>();

            foreach (var renderable in updatables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = priorities
                    .Where(x => renderable.GetType().DerivesFromOrEqual(x.First))
                    .Select(x => x.Second).ToList();

                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _updatables.Add(
                    new UpdatableInfo(renderable, priority));
            }

            _updatables = _updatables.OrderBy(x => x.Priority).ToList();

#if UNITY_EDITOR
            foreach (var renderable in _updatables.Select(x => x.Updatable).GetDuplicates())
            {
                Assert.That(false, "Found duplicate IGuiRenderable with type '{0}'".Fmt(renderable.GetType()));
            }
#endif
        }

        public void OnLateFixed()
        {
            foreach (var renderable in _updatables)
            {
                try
                {
#if ZEN_INTERNAL_PROFILING
                    using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
#if UNITY_EDITOR
                    using (ProfileBlock.Start("{0}.GuiRender()", renderable.Updatable.GetType()))
#endif
                    {
                        renderable.Updatable.LateFixed();
                    }
                }
                catch (Exception e)
                {
                    throw Assert.CreateException(
                        e, "Error occurred while calling {0}.GuiRender", renderable.Updatable.GetType());
                }
            }
        }

        class UpdatableInfo
        {
            public ILateFixed Updatable;
            public int Priority;

            public UpdatableInfo(ILateFixed updatable, int priority)
            {
                Updatable = updatable;
                Priority = priority;
            }
        }
    }
}
