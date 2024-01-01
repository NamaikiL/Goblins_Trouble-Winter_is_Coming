using UnityEngine;

namespace _Scripts.Gameplay.Towers.Spawn
{
	public class TowerSpawner : MonoBehaviour
	{

		#region Variables

		// Lock Variables.
		private bool _isEmpty = true;
	
		#endregion

		#region Properties

		public bool IsEmpty => _isEmpty;
    
		#endregion

		#region Custom Methods

		/**
		 * <summary>
		 * Function that make the spawner unusable.
		 * </summary>
		 */
		public void FillIt()
		{
			_isEmpty = !_isEmpty;
			gameObject.SetActive(_isEmpty);
		}

		#endregion

	}
}
