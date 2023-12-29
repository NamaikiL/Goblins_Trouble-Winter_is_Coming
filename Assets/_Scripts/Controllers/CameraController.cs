using UnityEngine;

namespace _Scripts.Controllers
{
	public class CameraController : MonoBehaviour
	{

		#region Variables

		[Header("Camera Movements Parameters")]
		[SerializeField] private float moveSpeed = 0.5f;
		[SerializeField] private float screenMargin;
	
		[Header("Camera Clamp Parameters")]
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

		#region Builtin Methods

		/**
	     * <summary>
	     * Update is called once per frame.
	     * </summary>
	     */
		void Update()
		{
			CameraInputs();
			MoveCamera();
			ClampCamera();
		}
	
		#endregion

		#region Camera Methods

		/**
		 * <summary>
		 * Function that take care about camera values.
		 * </summary>
		 */
		private void CameraInputs()
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
				CameraMouseControls();
			}
		}


		/**
		 * <summary>
		 * Function that control the camera with mouse position.
		 * </summary>
		 */
		private void CameraMouseControls()
		{
		#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
		    if (_horizontalMouse < screenMargin)
		    {
			    _movement.Set(-1f, 0f, 0f);
		    }
		    else if (_horizontalMouse > _screenWidth - screenMargin)
		    {
			    _movement.Set(1f, 0f, 0f);
		    }
		    else
		    {
			    if (_verticalMouse < screenMargin)
			    {
				    _movement.Set(0f, 0f, -1f);
			    }
			    else if (_verticalMouse > _screenHeight - screenMargin)
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


		/**
		 * <summary>
		 * Function that move the camera based on values calculated on other function.
		 * </summary>
		 */
		private void MoveCamera()
		{
			transform.Translate(_movement * (moveSpeed * Time.deltaTime));
		}


		/**
		 * <summary>
		 * Function that Clamp the camera.
		 * </summary>
		 */
		private void ClampCamera()
		{
			Transform currentTransform = transform;
			Vector3 currentPosition = currentTransform.position;
			Vector3 newPosition = currentPosition;

			if (currentPosition.x < levelMarginX.x)
			{
				newPosition.x = levelMarginX.x;
			}
			if (currentPosition.x > levelMarginX.y)
			{
				newPosition.x = levelMarginX.y;
			}
			if (currentPosition.z < levelMarginZ.x)
			{
				newPosition.z = levelMarginZ.x;
			}
			if (currentPosition.z > levelMarginZ.y)
			{
				newPosition.z = levelMarginZ.y;
			}

			currentTransform.position = newPosition;
		}

		#endregion

	}
}
