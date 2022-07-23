using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

// ReSharper disable once InconsistentNaming
namespace PG.CastleBuilder.DOTS
{
    public class FollowPointerSystem : SystemBase
    {
        // OnUpdate runs on the main thread.
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            // Schedule job to rotate around up vector
            Entities
                .WithName("FollowPointerSystem")
                .ForEach((ref Translation translation, in FollowPointerComponent followPointer, in PointerWorldPositionComponent pointerPos) =>
                {
                    var pos = pointerPos.PointerPosition;
                    pos.x -= followPointer.PositionBuffer.x;
                    pos.y = 0;
                    pos.z -= followPointer.PositionBuffer.y;
                    translation.Value = pos;
                })
                .ScheduleParallel();
        }
    }
}

