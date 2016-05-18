using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;


[CustomEditor(typeof(ThreeGlasses_Headset))]
public class ThreeGlasses_Headset_Editor : Editor {
	// Use this for initialization

	private Texture2D _logo;
	private Texture2D _logo1;
	private Texture2D _screenProjectionIcon;

	[MenuItem("3Glasses/AddDeviceControl")]
	private static void AddDevice()
    {
        if (Selection.activeGameObject.GetComponent<ThreeGlasses_Headset>() == null)
            Selection.activeGameObject.AddComponent<ThreeGlasses_Headset>();
    }


	[SerializeField]
	GameObject leftEye
	{
		get{
			try
			{
				return Selection.activeGameObject.GetComponent<ThreeGlasses_Headset>()._leftEye;
			}catch
			{
				return null;
			}
		}
		set{
			Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._leftEye = value;
		}
	}
	[SerializeField]
	GameObject rightEye
	{
		get{
			try
			{
				return Selection.activeGameObject.GetComponent<ThreeGlasses_Headset>()._rightEye;
			}catch
			{
				return null;
			}
		}
		set{
			Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._rightEye = value;
		}
	}

	[SerializeField]
	private bool _enableHeadTracking
	{
		get{
            try
            {
                return Selection.activeGameObject.GetComponent<ThreeGlasses_Headset>()._enableHeadTracking;
            }catch
            {
                return false;
            }
			}
		set{
			Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._enableHeadTracking = value;
		}
	}

	[SerializeField]
	private bool _enableScreenProjection
	{
		get{
			return Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._enableScreenProjection;
			}
		set{
			Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._enableScreenProjection = value;
			}
	}
	[SerializeField]
	private bool _enablePayment
	{
		get{
            return ThreeGlasses_Headset._enablePayment;
		}
		set{
            ThreeGlasses_Headset._enablePayment = value;
		}
	}
    [SerializeField]
    private bool _enableQuit
    {
        get
        {
            return ThreeGlasses_Headset._enableQuit;
        }
        set
        {
            ThreeGlasses_Headset._enableQuit = value;
        }
    }
	[SerializeField]
	private string _appKey
	{
		get{
            return ThreeGlasses_Headset._appKey;
		}
		set{
            ThreeGlasses_Headset._appKey = value;
		}
	}
    [SerializeField]
	private MonoBehaviour _payFailedMono
    {
        get
        {
            return ThreeGlasses_Headset._payFailedMono;
        }
        set
        {
			ThreeGlasses_Headset._payFailedMono = value;
        }
    }
	[SerializeField]
	private GameObject _payFailedObject
	{
		get
		{
			return ThreeGlasses_Headset._payFailedObject;
		}
		set
		{
			ThreeGlasses_Headset._payFailedObject = value;
		}
	}

    [SerializeField]
    private int _payFailedFunctionIndex
    {
        get
        {
            return ThreeGlasses_Headset._payFailedFunctionIndex;
        }
        set
        {
            ThreeGlasses_Headset._payFailedFunctionIndex = value;
        }
    }
    [SerializeField]
    private System.Reflection.MethodInfo _payFailedFunction
    {
        get
        {
            return ThreeGlasses_Headset._payFailedFunction;
        }
        set
        {
            ThreeGlasses_Headset._payFailedFunction = value;
        }
    }
	[SerializeField]
	private float _eyeDistance
	{
		get{
			return Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._eyeDistance;
		}
		set{
				Selection.activeGameObject.GetComponent<ThreeGlasses_Headset> ()._eyeDistance = value;
		}
	}

	private void DeleteHeadModle()
	{
		if(!_enableHeadTracking)
		{
			if (leftEye != null) 
			{
				DestroyImmediate(leftEye);
			}
			if (rightEye != null) 
			{
				DestroyImmediate(rightEye);
			}
			Selection.activeTransform.localEulerAngles = Vector3.zero;
		}
	}
	private void GenerateHeadModle()
	{
		if(_enableHeadTracking)
		{	
            if(Selection.activeGameObject.GetComponent<Camera>() != null)
			{
				Camera cam = Selection.activeGameObject.GetComponent<Camera>();
				cam.clearFlags = CameraClearFlags.Nothing;
				cam.cullingMask = 0;
				cam.rect = new Rect(0,0,1,1);
				cam.renderingPath = RenderingPath.Forward;
				cam.depth = 0;

			}else
			{
				Selection.activeGameObject.AddComponent<Camera>();
			}

			if (leftEye == null) 
			{
				leftEye = new GameObject();
				leftEye.transform.SetParent(Selection.activeTransform);
				leftEye.name = "leftEye";
				Camera cam = leftEye.AddComponent<Camera>();
				cam.fieldOfView = (float)ThreeGlassesInterfaces.GetFov();
				cam.rect = new Rect(0,0,0.5f,1);
				cam.renderingPath = RenderingPath.Forward;

				cam.depth = -1;
				
			}else
			{
				leftEye = Selection.activeGameObject.transform.FindChild ("leftEye").gameObject;
				leftEye.GetComponent<Camera>().transform.localPosition = new Vector3 (-_eyeDistance / 2f, 0, 0);
			}
			if (rightEye == null) 
			{
				rightEye = new GameObject();
				rightEye.transform.SetParent(Selection.activeTransform);
				rightEye.name = "rightEye";
				Camera cam = rightEye.AddComponent<Camera>();
				cam.fieldOfView = (float)ThreeGlassesInterfaces.GetFov();
				cam.rect = new Rect(0.5f,0,0.5f,1);
				cam.renderingPath = RenderingPath.Forward;
				cam.transform.localPosition = new Vector3 (_eyeDistance / 2f, 0, 0);
				cam.depth = -1;
				
			}else
			{
				rightEye = Selection.activeTransform.FindChild ("rightEye").gameObject;
				rightEye.GetComponent<Camera>().transform.localPosition = new Vector3 (_eyeDistance / 2f, 0, 0);
			}

		}
	}
	public void OnEnable()
	{
		if(EditorApplication.isPlaying)
			return;
		_logo = new Texture2D (256, 256);
		_screenProjectionIcon = new Texture2D (128, 128);
		_logo.LoadImage (System.IO.File.ReadAllBytes (@"Assets\\3Glasses\\Resources\\logo.png"));
		_screenProjectionIcon.LoadImage (System.IO.File.ReadAllBytes (@"Assets\\3Glasses\\Resources\\screenProjection.png"));

		_logo1 = new Texture2D (256, 256);
		_logo1.LoadImage (System.IO.File.ReadAllBytes (@"Assets\\3Glasses\\Resources\\logo1.png"));

	}

	public override void OnInspectorGUI()
	{
		//
		GUILayout.BeginVertical ();
		{
			//draw logo
			GUILayout.BeginHorizontal ();
			{
				GUILayout.FlexibleSpace ();
				GUILayout.Label (_logo, GUILayout.Height (150),GUILayout.Width(250));
				GUILayout.FlexibleSpace ();
			}
			GUILayout.EndHorizontal ();	

			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
			//head traking
			if(_enableHeadTracking = EditorGUILayout.Toggle("Enable Head tracking", _enableHeadTracking))
			{
				GenerateHeadModle();

				//Property
				GUILayout.BeginHorizontal();
				GUILayout.Label("Eyes Distance: a");
				GUILayout.FlexibleSpace();
				_eyeDistance = EditorGUILayout.FloatField(_eyeDistance);
				GUILayout.EndHorizontal();

				//Other property
				Selection.activeTransform.position = EditorGUILayout.Vector3Field("Head Position", Selection.activeTransform.position);
				Selection.activeTransform.localEulerAngles= EditorGUILayout.Vector3Field("Headset Rotation", Selection.activeTransform.localEulerAngles);
				//
				GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
			}else
			{
				DeleteHeadModle();
			}
			if(_enableScreenProjection = EditorGUILayout.Toggle("Enabel Screen Projection", _enableScreenProjection))
			{
				GUILayout.BeginHorizontal ();
				{
					GUILayout.FlexibleSpace ();
					GUILayout.Label (_screenProjectionIcon, GUILayout.Height (128),GUILayout.Width(128));
					GUILayout.FlexibleSpace ();
				}
				GUILayout.EndHorizontal ();
				Vector4 res = ThreeGlassesInterfaces.GetScreenPosRes();
				EditorGUILayout.Vector2Field("Headset Position", new Vector2(res.x,res.y));
				EditorGUILayout.Vector2Field("Headset Resolution", new Vector2(res.z,res.w));
				GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
			}
			if(_enablePayment = EditorGUILayout.Toggle("Enable Payment", _enablePayment))
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("App Key: ");
				GUILayout.FlexibleSpace();
				_appKey = EditorGUILayout.TextField(_appKey);
				GUILayout.EndHorizontal();
                if(_enableQuit  = EditorGUILayout.Toggle("Enable Quit when Failed", _enableQuit))
                {

                }else
                {
					GameObject temp = (GameObject)EditorGUILayout.ObjectField("Failed Function", _payFailedObject, typeof(GameObject),true);

                    if (_payFailedObject == null ||temp != _payFailedObject)
                    {
                        _payFailedFunctionIndex = -1;
                        _payFailedObject = temp;
                    }
                    //
                    if (_payFailedObject != null)
                    {
                        MonoBehaviour[] coms = _payFailedObject.GetComponents<MonoBehaviour>();
                        List<string> methods = new List<string>();

                        foreach (MonoBehaviour com in coms)
                        {
                            System.Reflection.MethodInfo[] meds = com.GetType().GetMethods();
                            foreach(System.Reflection.MethodInfo me in meds)
                            {
                                methods.Add(me.Name);
                            }
                        }
                        _payFailedFunctionIndex = EditorGUILayout.Popup("functions", _payFailedFunctionIndex, methods.ToArray());
                        if (_payFailedFunctionIndex == -1)
                        {
                            _payFailedFunction = null;
                        }
                        else
                        {
                            foreach (MonoBehaviour com in coms)
                            {
                                System.Reflection.MethodInfo[] meds = com.GetType().GetMethods();
                                foreach (System.Reflection.MethodInfo me in meds)
                                {
                                    if (me.Name == methods[_payFailedFunctionIndex])
                                    {
										_payFailedMono = com;
                                        _payFailedFunction = me;
                                    }
                                }
                            }
                        }
                    }
                    if(GUILayout.Button("Test Payment"))
                    {
						if(_appKey != "" && _payFailedFunction != null && !ThreeGlassesInterfaces.PayApp(_appKey))
                        {
							_payFailedFunction.Invoke(_payFailedMono,null);
                        }
                    }
                }
                
			}
		}
		GUILayout.EndVertical ();
		//
		//
	}
   

	void OnSceneGUI()
	{
		Handles.BeginGUI ();
		GUI.Label (new Rect (Screen.width / 2 - 250 / 2, Screen.height - 100, 250, 150), _logo1);
		GenerateHeadModle ();
		Handles.EndGUI ();
		//To redraw inspector
		Repaint ();
	}
}
