using UnityEngine;
using UnityEngine.Pool;

namespace Minigame.TowerBuilder
{
    public class BulletPooling : MonoBehaviour
    {
        [Header("Pool Config")]
        [SerializeField] private BulletProjectile bulletPrefab;
        [SerializeField] private int defaultCapacity = 15;
        [SerializeField] private int maxCapacity = 30;
        [SerializeField] private bool collectionCheck = true;

        [Header("Attack Config")]
        [SerializeField] private Vector3 firePoint = new Vector3(1f, 0f, 0f);
        [SerializeField] private int bulletCount = 3;
        [SerializeField] private float bulletVelocity;
        [SerializeField] private float spreadAngle = 15f;
        [SerializeField] private float initialAttackDelay; // so the wolf wouldn't start attack as soon as scene is played
        [SerializeField] private float cooldownBetweenShots;
        private float nextTimeToShoot;


        private IObjectPool<BulletProjectile> bulletPool;

        private void Awake()
        {
            bulletPool = new ObjectPool<BulletProjectile>(CreateBulletInstance, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxCapacity);
            nextTimeToShoot = Time.time + initialAttackDelay;
        }

        private void Update()
        {
            Attack();
        }

        private void Attack()
        {
            if (Time.time < nextTimeToShoot) return;

            if (bulletPool != null)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    BulletProjectile bullet = bulletPool.Get();
                    if (bullet == null) return;

                    float angleOffset = 0f;
                    if (bulletCount > 1)
                    {
                        angleOffset = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, i / (bulletCount - 1f));
                    }

                    Vector3 firePos = transform.position + transform.TransformDirection(firePoint);
                    Quaternion spreadRotation = transform.rotation * Quaternion.Euler(0f, 0f, angleOffset);

                    bullet.transform.SetPositionAndRotation(firePos, spreadRotation);
                    Vector2 direction = spreadRotation * Vector2.right;


                    bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletVelocity, ForceMode2D.Impulse);

                    bullet.Deactivate();
                }

                nextTimeToShoot = Time.time + cooldownBetweenShots;
            }
            Debug.Log("atac");
        }

        private BulletProjectile CreateBulletInstance()
        {
            BulletProjectile bulletInstance = Instantiate(bulletPrefab);
            bulletInstance.BulletPool = bulletPool;
            return bulletInstance;
        }

        private void OnGetFromPool(BulletProjectile pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(BulletProjectile pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(BulletProjectile pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 firePosition = transform.position + transform.TransformDirection(firePoint);
            Gizmos.DrawWireSphere(firePosition, 0.2f);

            for (int i = 0; i < bulletCount; i++)
            {
                float angleOffset = 0f;
                if (bulletCount > 1)
                {
                    angleOffset = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, i / (bulletCount - 1f));
                }

                Quaternion spreadRotation = transform.rotation * Quaternion.Euler(0f, 0f, angleOffset);
                Vector3 direction = spreadRotation * Vector2.right;

                Gizmos.color = i == bulletCount / 2 ? Color.yellow : Color.red;
                Gizmos.DrawLine(firePosition, firePosition + direction * 2f);
            }
        }
    }
}

