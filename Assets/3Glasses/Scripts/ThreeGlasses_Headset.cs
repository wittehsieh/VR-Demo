using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ThreeGlasses_Headset : MonoBehaviour {
    [SerializeField]
	public bool _enableHeadTracking;


    [SerializeField]
	public bool _enableScreenProjection;

	[SerializeField]
	public static bool _enablePayment;

	[SerializeField]
    public static bool _enableQuit;

	[SerializeField]
	public static  string _appKey;

	[SerializeField]
	public static GameObject _payFailedObject;

	[SerializeField]
    public static MonoBehaviour _payFailedMono;

	[SerializeField]
    public static int _payFailedFunctionIndex = -1;

	[SerializeField]
    public static System.Reflection.MethodInfo _payFailedFunction;

	[SerializeField]
	public GameObject _leftEye;
	[SerializeField]
	public GameObject _rightEye;


	[SerializeField]
	public float _eyeDistance = 0.1f;


	private Material leftMat;
	private Material rightMat;
	private Mesh leftMesh = null;
	private Mesh rightMesh = null;


	void Awake()
	{

#if UNITY_EDITOR
#else

		if(_enablePayment && !ThreeGlassesInterfaces.PayApp(_appKey))
		{
			if(_enableQuit)
				Application.Quit(); 
			else
				_payFailedFunction.Invoke(_payFailedMono,null);
		}
		if(_enableScreenProjection)
		{
			ThreeGlassesInterfaces.SetPositionAndResolution();
		}
#endif
    }
	// Use this for initialization


	void Start () {
       
		leftMat = new Material(Shader.Find("Effect/DistortionMesh"));
		rightMat = new Material(Shader.Find("Effect/DistortionMesh"));
		leftMesh = ThreeGlassesInterfaces.GenerateMesh(true, false);
		rightMesh = ThreeGlassesInterfaces.GenerateMesh(false, false);
	}
	public void PayFailed()
	{
		Debug.Log("Pay failed!");
	}
	void Update()
	{
		if(_enableHeadTracking)
		{
			ThreeGlassesInterfaces.UpdateHeadPosRot (this.gameObject);
		}
	}
	void DistortEye (bool rightEye, RenderTexture src)
	{
		Material mat = rightEye ? rightMat : leftMat;

		mat.mainTexture = src;
		
		if(rightMesh == null || leftMesh == null)
			return;
		
		Mesh eyeMesh = rightEye ? rightMesh : leftMesh;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        float halfWidth = 0.5f * screenWidth;

        GL.Viewport(new Rect(rightEye ? halfWidth : 0f, 0f, halfWidth, screenHeight));
		
		GL.PushMatrix ();
		GL.LoadOrtho ();
		
		for(int i = 0; i < mat.passCount; i++)
		{
			mat.SetPass (i);
			if(eyeMesh != null) Graphics.DrawMeshNow (eyeMesh, Matrix4x4.identity);
		}
		
		GL.PopMatrix ();
	}
	void OnRenderImage (RenderTexture src, RenderTexture dest)
	{
		RenderTexture.active = dest;

		DistortEye (false, src);
		DistortEye (true, src);
	}

}
