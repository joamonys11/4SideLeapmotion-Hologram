using UnityEngine;
using Leap.Unity;


  /// <summary>
  /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
  /// allows rotation, translation, and scale of the object (RTS).
  /// </summary>
  public class LeapRTS1 : MonoBehaviour {

    public enum RotationMethod {
      None,
      Single,
      Full
    }

	[SerializeField]
	private PinchDetector _pinchDetectorA;
	[SerializeField]
	private PinchDetector _pinchDetectorB;
	private PinchDetector PinchL;
	private PinchDetector PinchR;
	private GameObject PinchLeft;
	private GameObject PinchRight;

    [SerializeField]	
    private RotationMethod _oneHandedRotationMethod;
	//public string Itemname;
    [SerializeField]
    private RotationMethod _twoHandedRotationMethod;

    [SerializeField]
    private bool _allowScale = true;

    [Header("GUI Options")]
    [SerializeField]
    private KeyCode _toggleGuiState = KeyCode.None;

    [SerializeField]
    private bool _showGUI = true;
	[SerializeField]
    private Transform _anchor;

    private float _defaultNearClip;

	private Vector3 startpos;

	void Awake()
	{

		PinchLeft = GameObject.FindGameObjectWithTag ("pinchleft");
		PinchRight = GameObject.FindGameObjectWithTag ("pinchright");
		PinchL = PinchLeft.GetComponent<PinchDetector> ();
		PinchR = PinchRight.GetComponent<PinchDetector> ();
		_pinchDetectorA = PinchL;
		_pinchDetectorB = PinchR;
			
	}

    void Start() {
//      if (_pinchDetectorA == null || _pinchDetectorB == null) {
//        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
//        enabled = false;
//      }
//		startpos = transform.position;


      GameObject pinchControl = new GameObject("RTS Anchor");
      _anchor = pinchControl.transform;
      _anchor.transform.parent = transform.parent;
      transform.parent = _anchor;

//		_anchor = transform	;
    }

    void Update() {
      if (Input.GetKeyDown(_toggleGuiState)) {
        _showGUI = !_showGUI;
      }


		float distanceL = Vector3.Distance (transform.position, PinchLeft.transform.position);	
		float distanceR = Vector3.Distance (transform.position, PinchRight.transform.position);	

//		print ("Distance  L:" + distanceL);
//		print ("Distance  R:" + distanceR);

		if (distanceL < 0.3 || distanceR < 0.3) {
			bool didUpdate = false;
			if (_pinchDetectorA != null)
				didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
			if (_pinchDetectorB != null)
				didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

			if (didUpdate) {
				transform.SetParent (null, true);
			}




			if (_pinchDetectorA != null && _pinchDetectorA.IsPinching &&
			    _pinchDetectorB != null && _pinchDetectorB.IsPinching) {
				transformDoubleAnchor ();
			} else if (_pinchDetectorA != null && _pinchDetectorA.IsPinching) {
				transformSingleAnchor (_pinchDetectorA);
			} else if (_pinchDetectorB != null && _pinchDetectorB.IsPinching) {
				transformSingleAnchor (_pinchDetectorB);
			}





			if (didUpdate) {
				transform.SetParent (_anchor, true);
			}


		}


    }

    void OnGUI() {
      if (_showGUI) {
        GUILayout.Label("One Handed Settings");
        doRotationMethodGUI(ref _oneHandedRotationMethod);
        GUILayout.Label("Two Handed Settings");
        doRotationMethodGUI(ref _twoHandedRotationMethod);
        _allowScale = GUILayout.Toggle(_allowScale, "Allow Two Handed Scale");
      }
    }

    private void doRotationMethodGUI(ref RotationMethod rotationMethod) {
      GUILayout.BeginHorizontal();

      GUI.color = rotationMethod == RotationMethod.None ? Color.green : Color.white;
      if (GUILayout.Button("No Rotation")) {
        rotationMethod = RotationMethod.None;
      }

      GUI.color = rotationMethod == RotationMethod.Single ? Color.green : Color.white;
      if (GUILayout.Button("Single Axis")) {
        rotationMethod = RotationMethod.Single;
      }

      GUI.color = rotationMethod == RotationMethod.Full ? Color.green : Color.white;
      if (GUILayout.Button("Full Rotation")) {
        rotationMethod = RotationMethod.Full;
      }

      GUI.color = Color.white;

      GUILayout.EndHorizontal();
    }

    private void transformDoubleAnchor() {
      _anchor.position = (_pinchDetectorA.Position + _pinchDetectorB.Position) / 2.0f;

      switch (_twoHandedRotationMethod) {
        case RotationMethod.None:
          break;
        case RotationMethod.Single:
          Vector3 p = _pinchDetectorA.Position;
          p.y = _anchor.position.y;
          _anchor.LookAt(p);
          break;
        case RotationMethod.Full:
          Quaternion pp = Quaternion.Lerp(_pinchDetectorA.Rotation, _pinchDetectorB.Rotation, 0.5f);
          Vector3 u = pp * Vector3.up;
          _anchor.LookAt(_pinchDetectorA.Position, u);
          break;
      }

      if (_allowScale) {
        _anchor.localScale = Vector3.one * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
      }
    }

    private void transformSingleAnchor(PinchDetector singlePinch) {
      _anchor.position = singlePinch.Position;

      switch (_oneHandedRotationMethod) {
        case RotationMethod.None:
          break;
        case RotationMethod.Single:
          Vector3 p = singlePinch.Rotation * Vector3.right;
          p.y = _anchor.position.y;
          _anchor.LookAt(p);
          break;
        case RotationMethod.Full:
          _anchor.rotation = singlePinch.Rotation;
          break;
      }

      _anchor.localScale = Vector3.one;
    }




  }

