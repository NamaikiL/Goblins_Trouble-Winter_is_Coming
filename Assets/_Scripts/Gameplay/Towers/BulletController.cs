using _Scripts.Gameplay.Enemies;
using UnityEngine;

namespace _Scripts.Gameplay.Towers
{
    public class BulletController : MonoBehaviour
    {
        public enum BulletType
        {
            BaseTowerFire,
            Goblinator
        }
    
        #region Variables

        // Bullet Stats Variables.
        private float _damage;
        private float _rangeOfDamage;
        private bool _isBulletFire;

        private BulletType _bulletType;
        
        // Goblinator Variables.
        private float _height;
        private bool _tPose;
    
        #endregion

        #region Builtin Methods

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            if(_bulletType == BulletType.Goblinator && !_tPose) GoblinatorBulletVeritcalPos();
        }

        
        /**
         * <summary>
         * OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
         * </summary>
         * <param name="other">The Collision data associated with this collision event.</param>
         */
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && _bulletType == BulletType.BaseTowerFire)
            {
                other.gameObject.GetComponent<Enemy>().EnemyDamage(_damage);
                Destroy(gameObject);
            }
        
            if (_bulletType == BulletType.Goblinator)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
                    other.gameObject.layer == LayerMask.NameToLayer("Road"))
                {
                    Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _rangeOfDamage, 1 << 7);

                    if (enemiesInRange.Length != 0)
                    {
                        foreach (Collider enemy in enemiesInRange)
                        {
                            enemy.GetComponent<Enemy>().EnemyDamage(_damage);
                        }
                    }
                    Destroy(gameObject);
                }
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                other.gameObject.layer == LayerMask.NameToLayer("Road"))
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function that configure the bullet parameters.
         * </summary>
         */
        public void ConfigureBullet(float damage, float rangeDamage, bool isFire, BulletType type)
        {
            _bulletType = type;
            _damage = damage;
            _isBulletFire = isFire;
        
            if(_bulletType == BulletType.Goblinator) _rangeOfDamage = rangeDamage;
        }


        /**
         * <summary>
         * Function that change the direction of the goblin when he's at its peak.
         * </summary>
         */
        private void GoblinatorBulletVeritcalPos()
        {
            if (transform.position.y >= _height) _height = transform.position.y;
            else
            {
                Vector3 rotation = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(90f, rotation.y, rotation.z);
                _tPose = true;
            }
        }
    
        #endregion

    }
}
