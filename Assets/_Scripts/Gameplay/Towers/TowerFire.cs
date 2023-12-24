using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TowerFire : TowerFeatures
{

	#region Variables

	[Header("Fire Properties")]
	[SerializeField] private float damages;
	[SerializeField] private float range;

	private float _currentDistance;
	private float _shortestDistance;

	private Transform _targetEnemy;
	
	private Collider[] _enemiesInRange;
	
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        StartCoroutine(Detection());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Fire();
    }
	
	#endregion

	#region Custom Methods

	IEnumerator Detection()
	{
		yield return new WaitForSeconds(.1f);

		_enemiesInRange = Physics.OverlapSphere(transform.position, range);
		//_enemiesInRange = Physics.OverlapSphere(transform.position, range, LayerMask.NameToLayer("Enemy"));

		Transform _currentEnemy = null;
		
		foreach (Collider enemy in _enemiesInRange)
		{
			if (enemy.GetComponent<NavMeshAgent>())
			{
				_currentDistance = enemy.GetComponent<Enemy>().GetPathRemainingDistance();
			
				if (_currentDistance < _shortestDistance || _shortestDistance <= 0f)
				{
					_shortestDistance = _currentDistance;
					_currentEnemy = enemy.transform;
				}
			}
			yield return new WaitForEndOfFrame();
		}

		_targetEnemy = _currentEnemy;
		_currentDistance = 0f;
		_shortestDistance = 0f;

		StartCoroutine(Detection());
	}
	
	
	private void Fire()
	{
		if (!_targetEnemy) return;
		if (_turretHead)
		{
			//Vector3 targetDirection = _targetEnemy.position - transform.position;
			_turretHead.transform.LookAt(_targetEnemy);
		}
	}

	#endregion

}
