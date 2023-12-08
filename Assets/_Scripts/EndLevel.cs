using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{

	#region Variables

	private GameManager _gameManager;
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
	    if (other.gameObject.layer == 7)
	    {
		    _gameManager.RemoveLife(1);
		    Destroy(other.gameObject);
	    }
    }
	
	#endregion

}
