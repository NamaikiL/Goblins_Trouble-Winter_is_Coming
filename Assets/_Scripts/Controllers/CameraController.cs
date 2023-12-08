using UnityEngine;

public class CameraController : MonoBehaviour
{

	#region Variables

	// Private Serialized field Variables.
	[SerializeField] private float _moveSpeed = 0.5f;
	[SerializeField] private float _screenMargin;
	
	[SerializeField] private Vector2 levelMarginX;
	[SerializeField] private Vector2 levelMarginZ;
	
	// Private Variables.
	private float _horizontal;
	private float _vertical;
	private float _screenWidth;
	private float _screenHeight;
	private float _horizontalMouse;
	private float _verticalMouse;
	
	private Vector3 _movement = Vector3.zero;
    
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    _horizontal = Input.GetAxis("Horizontal");
	    _vertical = Input.GetAxis("Vertical");

	    _screenWidth = Screen.width;
	    _screenHeight = Screen.height;

	    _horizontalMouse = Input.mousePosition.x;
	    _verticalMouse = Input.mousePosition.y;
	    
	    if (_horizontal != 0f || _vertical != 0f)
	    {
		    _movement.Set(_horizontal, 0f, _vertical);
	    }
	    else
	    {
		    #if UNITY_STANDALONE_WIN && !UNITY_EDITOR
		    if (_horizontalMouse < _screenMargin)
		    {
			    _movement.Set(-1f, 0f, 0f);
		    }
		    else if (_horizontalMouse > _screenWidth - _screenMargin)
		    {
			    _movement.Set(1f, 0f, 0f);
		    }
		    else
		    {
			    if (_verticalMouse < _screenMargin)
			    {
				    _movement.Set(0f, 0f, -1f);
			    }
			    else if (_verticalMouse > _screenHeight - _screenMargin)
			    {
				    _movement.Set(0f, 0f, 1f);
			    }
			    else
			    {
				    _movement.Set(0f,0f,0f);
			    }
		    }
		    #endif
	    }
	    
	    transform.Translate(_movement * _moveSpeed * Time.deltaTime);
	    
	    if (transform.position.x < levelMarginX.x)
	    {
		    Vector3 newPos = new Vector3(levelMarginX.x, transform.position.y, transform.position.z);
		    transform.position = newPos;
	    }
	    if (transform.position.x > levelMarginX.y)
	    {
		    Vector3 newPos = new Vector3(levelMarginX.y, transform.position.y, transform.position.z);
		    transform.position = newPos;
	    }
	    if (transform.position.z < levelMarginZ.x)
	    {
		    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, levelMarginZ.x);
		    transform.position = newPos;
	    }
	    if (transform.position.z > levelMarginZ.y)
	    {
		    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, levelMarginZ.y);
		    transform.position = newPos;
	    }
    }
	
	#endregion

}
