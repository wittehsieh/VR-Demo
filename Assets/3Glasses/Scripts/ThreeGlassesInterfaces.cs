using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class ThreeGlassesInterfaces  
{
	public const string vrLib = "SZVRPlugin";

	[DllImport(vrLib,CallingConvention= CallingConvention.Cdecl)]
	private static extern bool SZVR_GetData(float[] input, float[] output);

	[DllImport(vrLib,CallingConvention= CallingConvention.Cdecl)]
	private static extern bool SZVR_PayApp(string appKey);

	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
	
	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
	
	[DllImport("user32.dll")]
	static extern IntPtr GetActiveWindow();


	private const uint SWP_SHOWWINDOW = 0x0040;
	private const int GWL_STYLE = -16;
	private const int WS_BORDER = 1;


	public struct QuaternionL
	{
		public Double x;
		public Double y;
		public Double z;
		public Double w;
	}

	public struct DistMeshVert
	{
		public float screenPosNDC_x;
		public float screenPosNDC_y;
		public float timewarpLerp;
		public float shade;
		public float uv_u;
		public float uv_v;
		public float uvR_u;
		public float uvR_v;
		public float uvG_u;
		public float uvG_v;
		public float uvB_u;
		public float uvB_v;
	};

    public static void IntChair()
    {
        float[] inputs = new float[2];
		inputs [0] = 5;
        inputs [1] = 0;
        float[] result = new float[1];
        SZVR_GetData(inputs, result);
    }

    public static void ChairToZero()
    {
        float[] inputs = new float[2];
        inputs[0] = 5;
        inputs[1] = 1;
        float[] result = new float[1];
        SZVR_GetData(inputs, result);
    }

    public static void ChairToMiddle()
    {
        float[] inputs = new float[2];
        inputs[0] = 5;
        inputs[1] = 2;
        float[] result = new float[1];
        SZVR_GetData(inputs, result);
    }
    public static void ChairCenterMove(Vector3 center, Vector3 directionY, int time = 10)
    {
        float[] inputs = new float[9];
        inputs[0] = 5;
        inputs[1] = 3;
        inputs[2] = center.x;
        inputs[3] = center.y;
        inputs[4] = center.z;
        inputs[5] = directionY.x;
        inputs[6] = directionY.y;
        inputs[7] = directionY.z;
        inputs[8] = time;
        float[] result = new float[1];
        SZVR_GetData(inputs, result);
    }

	public static Vector4 GetScreenPosRes()
	{
		float[] inputs = new float[1];
		inputs [0] = 0;
		float[] result = new float[4];
		Vector4 res = new Vector4 ();
		if(SZVR_GetData (inputs, result))
		{
			res.x = result [0];
			res.y = result [1];
			res.z = result [2];
			res.w = result [3];
			return res;
		}else
		{
			return res;
		}
	}

    public static void SetPositionAndResolution()
	{
		float[] inputs = new float[1];
		inputs [0] = 0;
		float[] result = new float[4];
		if(SZVR_GetData (inputs, result))
		{
			Screen.SetResolution ((int)result[2], (int)result[3], false);
            SetWindowLong(GetActiveWindow(), GWL_STYLE, WS_BORDER);
            SetWindowPos(GetActiveWindow(), 0, (int)result[0], (int)result[1], (int)result[2], (int)result[3], SWP_SHOWWINDOW);
		}

	}

	public static bool PayApp(string appKey)
	{
		return SZVR_PayApp (appKey);
	}

	public static void GetCameraOrientation(ref Quaternion rotation)
	{
		float[] inputs = new float[1];
		inputs [0] = 1;
		float[] result = new float[4];
		if(SZVR_GetData (inputs, result))
		{
			rotation.x = -result[0];
			rotation.y = -result[1];
			rotation.z = result[2];
			rotation.w = result[3];
		}
	}
	public static void GetCameraPosition(ref Vector3 position)
	{
		float[] inputs = new float[1];
		inputs [0] = 6;
		float[] result = new float[3];
		if(SZVR_GetData (inputs, result))
		{
			position.x = result[0];
			position.y = result[1];
			position.z = result[2];
		}
	}
	public static void GetWandPosAndRot(int index , ref Vector3 position, ref Quaternion orotation)
	{
		float[] inputs = new float[2];
		inputs [0] = 7;
		inputs [1] = index;
		float[] result = new float[7];
		if(SZVR_GetData (inputs, result))
		{
			position.x = result[0];
			position.y = result[1];
			position.z = result[2];
			orotation.x = result[3];
			orotation.y = result[4];
			orotation.z = result[5];
			orotation.w = result[6];
		}
	}
	public static float GetFov()
	{
		/*
		float[] inputs = new float[1];
		inputs [0] = 4;
		float[] result = new float[1];
		if(SZVR_GetData (inputs, result))
			return result[0];
		else return 0;
		*/
		return 110f;
	}
	public static Mesh GenerateMesh(bool right, bool flipY)
	{
		Mesh mesh = new Mesh();
		
		int numVerts = 0;
		int numIndicies = 0;

		float[] inputs = new float[2];
		inputs [0] = 2;
		inputs [1] = right ? 0 : 1;
		float[] result = new float[2];
		if(!SZVR_GetData (inputs, result))
		{
			return mesh;
		}

		numVerts = (int)result[0];
		numIndicies = (int)result[1];

		//DistMeshVert[] meshVerts = new DistMeshVert[numVerts];
		int[] triIndices = new int[numIndicies];

		float[] inputs1 = new float[5];
		inputs1 [0] = 3;
		inputs1 [1] = (float)numVerts;
		inputs1 [2] = (float)numIndicies;
		inputs1 [3] = right ? 0 : 1;
		inputs1 [4] = flipY ? 0 : 1;

		float[] result1 = new float[numVerts*12 + numIndicies];
		if(!SZVR_GetData (inputs1, result1))
		{
			return mesh;
		}
		int num = 0;
		Vector3[] positions = new Vector3[numVerts];
		Vector2[] uv   = new Vector2[numVerts];
		Vector2[] uvR  = new Vector2[numVerts];
		Vector4[] uvGB = new Vector4[numVerts];
		
		for(int i = 0; i < numVerts; i++)
		{
			positions[i].x = result1[num++];
			positions[i].y = result1[num++];
			num++;
			positions[i].z = result1[num++];
			uv[i].x        = result1[num++];
			uv[i].y        = result1[num++];
			uvR[i].x       = result1[num++];
			uvR[i].y       = result1[num++];
			uvGB[i].x      = result1[num++];
			uvGB[i].y      = result1[num++];
			uvGB[i].z      = result1[num++];
			uvGB[i].w      = result1[num++];
		}
		for(int i = 0; i < numIndicies; i++)
		{
			triIndices[i] = (int)result1[num++];
		}
		mesh.vertices  = positions;
		
		mesh.uv        = uv;
		
		mesh.uv2       = uvR;
		
		mesh.tangents   = uvGB;
		
		mesh.triangles = triIndices;

		
		return mesh;
	}


	public static void UpdateHeadPosRot(GameObject _target)
	{

		Quaternion orientation = new Quaternion();


		ThreeGlassesInterfaces.GetCameraOrientation(ref orientation);

		_target.transform.localRotation = orientation;
	}


	private static Quaternion QNormarize(Quaternion r, int i = 1)
	{
		float sum = Mathf.Sqrt(r.x * r.x + r.y * r.y + r.z * r.z + r.w * r.w);
		if(sum < 0.01f * i)
			sum =0;
		return sum == 0f? r: new Quaternion(r.x/sum,r.y/sum,r.z/sum,r.w/sum);
	}
	private static int fact(int n)   
	{
		if (n == 1||n == 0)
			return 1;
		else
			return n * fact(n - 1);
	}
	private static float singedPow(float a, float n)
	{
		if (n % 2 == 0)
						return a == 0 ? 0 :(float)( Math.Abs (a) * Math.Pow (a, n - 1));
				else
						return a == 0 ? 0 : (float)(Math.Pow (a, n));
	}

	private void RotateTrasform(Transform tran, Vector3 angles)
	{
		if(tran == null)
			return;
		tran.Rotate (angles,Space.Self);
	}
}
