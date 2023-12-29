using System.Collections;
using UnityEngine;

namespace _Scripts.Gameplay.Towers.Types
{
    public class TowerGoblArcX : TowerFire
    {

        #region Variables

        [Header("GoblArcX Parameters")] 
        [SerializeField] private float fireForce;
        
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
            base.Fire();

            if (Target && towerFireLevelStats != null && CurrentLevel < towerFireLevelStats.Count)
            {
                if (!Target.GetComponent<Enemy>().NeedFire
                    || (Target.GetComponent<Enemy>().NeedFire && towerFireLevelStats[CurrentLevel].isFire))
                {
                    if (shooting == null)
                    {
                        shooting = GoblArcXFire();
                        StartCoroutine(shooting);
                    }
                }
            }
        }


        /**
         * <summary>
         * Coroutine for the fire of the GoblingGun.
         * </summary>
         */
        private IEnumerator GoblArcXFire()
        {
            foreach (GameObject shootingPoint in shootingPoints)
            {
                Vector3 turretHeadRotation = TurretHead.transform.rotation.eulerAngles;
                
                // Instantiate the bullet.
                GameObject bulletSpawn = Instantiate(
                    towerFireLevelStats[CurrentLevel].bullet, 
                    shootingPoint.transform.position, 
                    Quaternion.Euler(90f, turretHeadRotation.y, turretHeadRotation.z)
                );
                
                // Configure the bullet.
                bulletSpawn.GetComponent<BulletController>().ConfigureBullet(
                    towerFireLevelStats[CurrentLevel].damages, 
                    0f, 
                    towerFireLevelStats[CurrentLevel].isFire, 
                    BulletController.BulletType.BaseTowerFire
                );
                
                // Apply a force to the velocity of the bullet.
                bulletSpawn.GetComponent<Rigidbody>().AddForce(TurretHead.transform.forward * fireForce, ForceMode.Impulse);
            }

            // Wait the fire-rate of the tower.
            yield return new WaitForSeconds(towerFireLevelStats[CurrentLevel].firerate);
            shooting = null;
        }

        #endregion

    }
}
