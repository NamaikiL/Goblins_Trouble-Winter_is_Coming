using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{

	#region Variables

	private bool _isEmpty = true;
	
    #endregion

    #region Properties

	public bool IsEmpty => _isEmpty;
    
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	#endregion

	#region Custom Methods

	public void FillIt()
	{
		_isEmpty = false;
	}

	#endregion

}
