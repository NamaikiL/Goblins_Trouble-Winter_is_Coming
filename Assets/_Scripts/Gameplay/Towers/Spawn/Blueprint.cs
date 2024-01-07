using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Gameplay.Towers.Spawn
{
	public class Blueprint : MonoBehaviour
	{

		#region Variables

		[Header("Blueprint parameters")]
		[SerializeField] private Material valid;
		[SerializeField] private Material invalid;
		[SerializeField] private GameObject tower;
		
		[Header("Audio")]
		[SerializeField] private AudioSource button2;
		[SerializeField] private AudioSource upgradeSell;

		// Spawn Variables.
		private bool _valid;
		private Transform _spawner;
		private Renderer[] _renderers;
		
		// Tower Information Variables.
		private TowerFeatures _towerFeatures;
		
		// Managers Variables.
		private GameManager _gameManager;
	
		#endregion

		#region Builtin Methods

		/**
	     * <summary>
	     * Start is called before the first frame update.
	     * </summary>
	     */
		void Start()
		{
			_renderers = GetComponentsInChildren<Renderer>();
			_towerFeatures = tower.GetComponent<TowerFeatures>();
			_gameManager = GameManager.Instance;
		}

    
		/**
	     * <summary>
	     * Update is called once per frame.
	     * </summary>
	     */
		void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f, 1 << 6))
			{
				transform.position = hit.point;
			}
			// If it's valid and player have enough money.
			if (Input.GetMouseButtonDown(0) 
			    && _valid 
			    && _gameManager.CurrentMoney >= _towerFeatures.Levels[_towerFeatures.CurrentLevel].cost)
			{
				button2.Play();
				upgradeSell.Play();
				GameObject towerSpawn = Instantiate(tower, _spawner.position, Quaternion.identity, transform.parent);
				towerSpawn.name = tower.name;
				towerSpawn.GetComponent<TowerFeatures>().SpawnerInformation = _spawner;
				_spawner.GetComponent<TowerSpawner>().FillIt();
				Destroy(gameObject, upgradeSell.clip.length);
			}
			if (Input.GetMouseButtonDown(1))
			{
				Destroy(gameObject);
			}
		}

    
		/**
	     * <summary>
	     * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
	     * </summary>
	     * <param name="other">The other Collider involved in this collision.</param>
	     */
		private void OnTriggerEnter(Collider other)
		{
			// If it's not a GoblArcX, act normally.
			if (other.GetComponent<TowerSpawner>() && other.GetComponent<TowerSpawner>().IsEmpty && !CompareTag("GoblArcX"))
			{
				_valid = true;
				_spawner = other.transform;
				ChangeMat();
			}
			
			// If it's a GoblArcX, need to be in a high ground.
			if (other.GetComponent<TowerSpawner>() && other.GetComponent<TowerSpawner>().IsEmpty && CompareTag("GoblArcX") && other.CompareTag("High"))
			{
				_valid = true;
				_spawner = other.transform;
				ChangeMat();
			}
		}

    
		/**
	     * <summary>
	     * OnTriggerExit is called when the Collider other has stopped touching the trigger.
	     * </summary>
	     * <param name="other">The other Collider involved in this collision.</param>
	     */
		private void OnTriggerExit(Collider other)
		{
			if (other.GetComponent<TowerSpawner>())
			{
				_valid = false;
				_spawner = null;
				ChangeMat();
			}
		}

		#endregion

		#region Custom Methods

		/**
		 * <summary>
		 * Function that change the mat based on if it's on a spawn position or not.
		 * </summary>
		 */
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
}
