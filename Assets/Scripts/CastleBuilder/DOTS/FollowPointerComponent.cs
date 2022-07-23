using Unity.Entities;
using Unity.Mathematics;

// ReSharper disable once InconsistentNaming
namespace PG.CastleBuilder.DOTS
{
    [GenerateAuthoringComponent]
    public struct FollowPointerComponent : IComponentData
    {
        public float2 PositionBuffer;
    }
}
