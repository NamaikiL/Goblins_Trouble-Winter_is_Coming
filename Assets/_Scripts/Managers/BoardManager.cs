using _Scripts.Gameplay.Towers;
using _Scripts.Gameplay.Towers.Types;
using _Scripts.Managers;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    #region Variables

    // Tower selection Variables.
    private GameObject _selectedTower;
    
    // Managers Variables.
    private UIManager _uiManager;
    
    #endregion

    #region Builtin Methods
    
    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    void Start()
    {
        _uiManager = UIManager.Instance;
    }

    
    /**
     * <summary>
     * Update is called once per frame.
     * </summary>
     */
    void Update()
    {
        CheckTower();
    }
    
    #endregion

    #region Custom Methods

    /**
     * <summary>
     * Function that check when a player click on a specific tower and show its stats.
     * </summary>
     */
    private void CheckTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100f, 1 << 8))
        {
            _selectedTower = hit.collider.gameObject;
            
            _uiManager.UpdateTowerCard(_selectedTower, true);
        }

        if (!Physics.Raycast(ray, out hit, 100f, 1 << 8) 
            && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            _uiManager.UpdateTowerCard(null, false);
        }
    }
    
    #endregion

}
