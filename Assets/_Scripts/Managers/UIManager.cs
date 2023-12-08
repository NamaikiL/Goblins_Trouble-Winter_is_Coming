using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	#region Variables

	[SerializeField] private TMP_Text money;
	[SerializeField] private TMP_Text life;
	
	[Header("Tower Manager")]
	[SerializeField] private Transform towerHolder;

	public static UIManager instance;
	
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    void Awake()
    {
	    if (instance)
		    Destroy(gameObject);
	    else
		    instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	#endregion

	#region Gameplay

	public void TowerInstantiate(GameObject tower)
	{
		Instantiate(tower, Vector3.zero, Quaternion.identity, towerHolder);
	}

	public void UpdateMoneyText(int quantity)
	{
		money.text = quantity.ToString();
	}

	public void UpdateLifeText(int quantity)
	{
		life.text = quantity.ToString();
	}

	#endregion

}
