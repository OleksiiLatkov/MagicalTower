using UnityEngine;

namespace MagicalTower.Enemies.Movement
{
    /// <summary>Walks straight toward the target on the ground plane. Stateless, so it can be shared.</summary>
    [System.Serializable]
    public class StraightMovement : IEnemyMovement
    {
        public void Tick(Transform self, Vector3 target, float speed, float dt)
        {
            Vector3 pos = self.position;
            Vector3 flatTarget = new Vector3(target.x, pos.y, target.z);
            Vector3 dir = flatTarget - pos;

            if (dir.sqrMagnitude <= 0.0001f) 
                return;

            dir.Normalize();
            self.position = pos + dir * (speed * dt);
            self.rotation = Quaternion.LookRotation(dir);
        }
    }
}
