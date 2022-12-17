using UnityEngine;


namespace GameLibrary
{
    public static class MathHelper
    {
        public static bool IsInQuadXZ(Vector3 quadCenterPosition, Vector2 quadSize, Vector3 position)
        {
            return !(position.x < quadCenterPosition.x - quadSize.x * 0.5f ||
                     position.x > quadCenterPosition.x + quadSize.x * 0.5f ||
                     position.z < quadCenterPosition.z - quadSize.y * 0.5f ||
                     position.z > quadCenterPosition.z + quadSize.y * 0.5f);
        }
        
        
        public static bool IsInQuadXZ(Vector3 quadCenterPosition, float quadRadius, Vector3 position)
        {
            return !(position.x < quadCenterPosition.x - quadRadius ||
                     position.x > quadCenterPosition.x + quadRadius ||
                     position.z < quadCenterPosition.z - quadRadius ||
                     position.z > quadCenterPosition.z + quadRadius);
        }
        
        
        public static Vector3 GetRandomPositionInQuadXZ(Vector3 quadCenterPosition, Vector2 quadSize)
        {
            var minPositionX = quadCenterPosition.x - quadSize.x * 0.5f;
            var maxPositionX = quadCenterPosition.x + quadSize.x * 0.5f;
            var minPositionZ = quadCenterPosition.z - quadSize.y * 0.5f;
            var maxPositionZ = quadCenterPosition.z + quadSize.y * 0.5f;
            
            return new Vector3(
                Random.Range(minPositionX, maxPositionX),
                0.0f,
                Random.Range(minPositionZ, maxPositionZ));
        }
    }
}