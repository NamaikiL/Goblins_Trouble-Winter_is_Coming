using _Scripts.Managers;
using UnityEngine;

public class EndLevel : MonoBehaviour
{

	#region Variables

	private GameManager _gameManager;
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    void Start()
    {
        _gameManager = GameManager.Instance;
    }
    
    
    /**
     * <summary>
     * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
     * </summary>
     * <param name="other">The other Collider involved in this collision.</param>
     */
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
