using System.Collections;
using UnityEngine;

namespace _Scripts.Gameplay.Towers.Types
{
    public class TowerGoblinator : TowerFire
    {

        #region Variables

        [Header("Goblinator Parameters")] 
        [SerializeField] [Range(0f, 90f)] private float angle;
        [SerializeField] private float firePower;
        
        // Private variables.
        private IEnumerator shooting;

        #endregion

        #region Builtin Methods

        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        protected override void Start()
        {
            base.Start();
        }

        
        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        protected override void Update()
        {
            base.Update();
            if(!IsStun) Fire();
        }
	
        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function that calculate the direction of the tower and its fire.
         * </summary>
         */
        protected override void Fire()
        {
            if (!Target) return;

            if (Target && towerFireLevelStats != null && CurrentLevel < towerFireLevelStats.Count)
            {
                if (!Target.GetComponent<Enemy>().NeedFire
                    || (Target.GetComponent<Enemy>().NeedFire && towerFireLevelStats[CurrentLevel].isFire))
                {
                    if (TurretHead)
                    {
                        Vector3 goblinatorPos = transform.position;
                        Vector3 targetPosition = Target.position;

                        Vector3 direction = new Vector3(
                            targetPosition.x - goblinatorPos.x,
                            0f,
                            targetPosition.z - goblinatorPos.z
                        );

                        TurretHead.transform.rotation = Quaternion.LookRotation(direction);
                    }

                    if (shooting == null)
                    {
                        shooting = GoblinatorFire();
                        StartCoroutine(shooting);
                    }
                }
            }
        }


        /**
         * <summary>
         * Coroutine that calculate the fire trajectory of the bullet and fire it.
         * </summary>
         */
        private IEnumerator GoblinatorFire()
        {
            foreach(GameObject shootingPoint in shootingPoints)
            {
                Quaternion shootingPointRotation = shootingPoint.transform.rotation;
                Vector3 turretHeadRotation = TurretHead.transform.rotation.eulerAngles;
                
                GameObject bulletSpawn = Instantiate(
                    towerFireLevelStats[CurrentLevel].bullet,
                    shootingPoint.transform.position, 
                    Quaternion.Euler(angle, turretHeadRotation.y, turretHeadRotation.z)
                    );
                
                // Configure the bullet.
                bulletSpawn.GetComponent<BulletController>().ConfigureBullet(
                    towerFireLevelStats[CurrentLevel].damages, 
                    2.5f, 
                    towerFireLevelStats[CurrentLevel].isFire, 
                    BulletController.BulletType.Goblinator
                    );
                
                // Apply it's cannonball velocity to it.
                bulletSpawn.GetComponent<Rigidbody>().velocity = CalculateCannonBall(shootingPoint, angle, firePower);
            }

            yield return new WaitForSeconds(towerFireLevelStats[CurrentLevel].firerate);
            shooting = null;
        }


        /**
         * <summary>
         * Function to calculate the parabolic trajectory of the bullet.
         * </summary>
         * <param name="idShootPoint">The id of the cannon.</param>
         * <param name="angleCanon">The angle of the cannonball.</param>
         * <param name="firePowerCanon">the fire power of the cannonball.</param>
         */
        private Vector3 CalculateCannonBall(GameObject idShootPoint, float angleCanon, float firePowerCanon)
        {
            Vector3 targetPos = Target.position;
            Vector3 shootingPointPos = idShootPoint.transform.position;

            // Calculate the direction of the enemy from the tower.
            Vector3 directionToEnemy = (targetPos - shootingPointPos).normalized;

            // Calculate the vertical distance between the target and the tower.
            float verticalDistance = shootingPointPos.y - targetPos.y;

            // Calculate the fire power.
            float initialVelocity = Mathf.Sqrt((firePowerCanon * firePowerCanon) - 2 * Physics.gravity.y * verticalDistance);

            // Launching vector.
            Vector3 launchVelocity = new Vector3(directionToEnemy.x, Mathf.Tan(Mathf.Deg2Rad * angleCanon) * initialVelocity,
                directionToEnemy.z);
            launchVelocity = launchVelocity.normalized * initialVelocity;

            return launchVelocity;
        }

        #endregion

    }
}
