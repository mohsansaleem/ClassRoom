using Unity.Entities;
using Unity.Mathematics;

// ReSharper disable once InconsistentNaming
namespace PG.CastleBuilder.DOTS
{
    [GenerateAuthoringComponent]
    public struct PointerWorldPositionComponent : IComponentData
    {
        public float3 PointerPosition;
    }
}
