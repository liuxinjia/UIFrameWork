using UnityEngine;
using UnityEditor;
using Unity.Profiling.Editor;
using Unity.Profiling;
using System;

namespace Cr7Sund.MyCoroutine.Editor
{
    [Serializable]
    [ProfilerModuleMetadata("Coroutine Collection")]
    public class CoroutineProfilerModule : ProfilerModule
    {
        static readonly ProfilerCounterDescriptor[] k_ChartCounters = new ProfilerCounterDescriptor[]
        {
                new ProfilerCounterDescriptor("Creation Count", ProfilerCategory.Scripts),
                new ProfilerCounterDescriptor("Termination Count", ProfilerCategory.Scripts),
                new ProfilerCounterDescriptor("Execution Count", ProfilerCategory.Scripts),
                new ProfilerCounterDescriptor("Execution Times", ProfilerCategory.Scripts),
        };

        public CoroutineProfilerModule() : base(k_ChartCounters)
        {

        }
    }

    // public static class GameStats
    // {
    //     public static readonly ProfilerCategory TanksCategory = ProfilerCategory.Scripts;

    //     public const string TankTrailParticleCountName = "Tank Trail Particles";
    //     public static readonly ProfilerCounterValue<int> TankTrailParticleCount =
    //         new ProfilerCounterValue<int>(TanksCategory, TankTrailParticleCountName, ProfilerMarkerDataUnit.Count,
    //             ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);
    // }


}