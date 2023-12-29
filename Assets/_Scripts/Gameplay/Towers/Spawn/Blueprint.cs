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

		// Private Variables.
		private bool _valid = false;

		private Transform _spawner;
		private Renderer[] _renderers;
	
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

    
		/**
	     * <summary>
	     * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
	     * </summary>
	     * <param name="other">The other Collider involved in this collision.</param>
	     */
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<TowerSpawner>() && other.GetComponent<TowerSpawner>().IsEmpty && !CompareTag("GoblArcX"))
			{
				_valid = true;
				_spawner = other.transform;
				ChangeMat();
			}
			
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
