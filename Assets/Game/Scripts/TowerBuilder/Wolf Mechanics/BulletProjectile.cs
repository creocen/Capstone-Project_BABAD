using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

namespace Minigame.TowerBuilder
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2f;
        public bool hitOther = false;

        private IObjectPool<BulletProjectile> bulletPool;

        public IObjectPool<BulletProjectile> BulletPool { set => bulletPool = value; }

       

        public void Deactivate()
        {
            StartCoroutine(DeactivateRoutine(lifeTime));
        }

      

        IEnumerator DeactivateRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            Rigidbody2D body = GetComponent<Rigidbody2D>();
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;

            bulletPool?.Release(this);
        }
    }
}
