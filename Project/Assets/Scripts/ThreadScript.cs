using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ThreadScript : MonoBehaviour
{
	private void Start()
	{
		if (ThreadScript._003C_003Ef__mg_0024cache0 == null)
		{
			ThreadScript._003C_003Ef__mg_0024cache0 = new ThreadStart(ThreadScript.DoWork);
		}
		this.t = new Thread(ThreadScript._003C_003Ef__mg_0024cache0);
		this.t.Start();
	}

	public static void DoWork()
	{
		for (;;)
		{
			Thread.Sleep(3000);
			System.Random random = new System.Random();
			object obj = ThreadScript.o;
			lock (obj)
			{
				ThreadScript.p.x = (float)random.NextDouble() * 10f;
				ThreadScript.p.y = (float)random.NextDouble() * 10f;
				Thread.Sleep(100);
				ThreadScript.p.z = (float)random.NextDouble() * 10f;
			}
			UnityEngine.Debug.Log(ThreadScript.p);
		}
	}

	private void Update()
	{
		object obj = ThreadScript.o;
		lock (obj)
		{
			ThreadScript.p.x = 800f;
			ThreadScript.p.y = 800f;
			ThreadScript.p.z = 800f;
		}
	}

	[ContextMenu("�����߳�")]
	private void StopThread()
	{
		this.t.Abort();
	}

	protected static Vector3 p = Vector3.zero;

	protected static float lastTime = 0f;

	protected static object o = new object();

	private Thread t;

	[CompilerGenerated]
	private static ThreadStart _003C_003Ef__mg_0024cache0;
}
