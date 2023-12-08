using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

	#region Variables

	[SerializeField] private float _moveSpeed; 
	
	private Transform _destination;
	private NavMeshAgent _agent;
    
    #endregion

    #region Properties

    public Transform Destination
    {
	    get { return _destination; }
	    set { _destination = value; }
    }
    
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update.
    void Start()
    {
	    _agent = GetComponent<NavMeshAgent>();
	    _agent.speed = _moveSpeed;
    }

    // Update is called once per frame.
    void Update()
    {
	    _agent.SetDestination(_destination.position);
    }
	
	#endregion

	#region Custom Methods

	public float GetPathRemainingDistance()
	{
		if (_agent.pathPending ||
		    _agent.pathStatus == NavMeshPathStatus.PathInvalid ||
		    _agent.path.corners.Length == 0)
			return -1f;

		float distance = 0.0f;
		for (int i = 0; i < _agent.path.corners.Length - 1; ++i)
		{
			distance += Vector3.Distance(_agent.path.corners[i], _agent.path.corners[i + 1]);
		}

		return distance;
	}

	#endregion

}
