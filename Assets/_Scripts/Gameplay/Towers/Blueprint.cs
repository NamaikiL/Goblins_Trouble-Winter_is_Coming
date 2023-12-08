using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{

	#region Variables

	[SerializeField] private Material valid;
	[SerializeField] private Material invalid;
	[SerializeField] private GameObject tower;

	private bool _valid = false;

	private Transform _spawner;
	private Renderer[] _renderers;
	
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update
    void Start()
    {
	    _renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, 100f, 1 << 6))
	    {
		    transform.position = hit.point;
	    }

	    if (Input.GetMouseButton(0) && _valid)
	    {
		    Destroy(gameObject);
		    Instantiate(tower, _spawner.position, Quaternion.identity, transform.parent);
		    _spawner.GetComponent<TowerSpawner>().FillIt();
	    }
	    if (Input.GetMouseButton(1))
	    {
		    Destroy(gameObject);
	    }
    }

    private void OnTriggerEnter(Collider other)
    {
	    if (other.GetComponent<TowerSpawner>() && other.GetComponent<TowerSpawner>().IsEmpty)
	    {
		    _valid = true;
		    _spawner = other.transform;
		    ChangeMat();
	    }
    }

    private void OnTriggerExit(Collider other)
    {
	    if (other.GetComponent<TowerSpawner>())
	    {
		    _valid = false;
		    _spawner = null;
		    Debug.Log("Peux pas poser");
		    ChangeMat();
	    }
    }

    #endregion

    #region Custom Methods

    private void ChangeMat()
    {
	    if (_valid)
	    {
		    foreach (Renderer rend in _renderers)
		    {
			    rend.material = valid;
		    }
	    }
	    else
	    {
		    foreach (Renderer rend in _renderers)
		    {
			    rend.material = invalid;
		    }
	    }
    }

    #endregion

}
