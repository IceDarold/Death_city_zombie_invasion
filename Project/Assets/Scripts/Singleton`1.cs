using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (Singleton<T>.applicationIsQuitting)
			{
				return (T)((object)null);
			}
			object @lock = Singleton<T>._lock;
			T instance;
			lock (@lock)
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T), true));
					if (Singleton<T>._instance == null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(typeof(T).ToString())) as GameObject;
						Singleton<T>._instance = gameObject.GetComponent<T>();
						(Singleton<T>._instance as Singleton<T>).Init();
						gameObject.name = "(singleton) " + typeof(T).ToString();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				instance = Singleton<T>._instance;
			}
			return instance;
		}
	}

	public void OnDestroy()
	{
		if (!Singleton<T>.ClearMode)
		{
			Singleton<T>.applicationIsQuitting = true;
		}
		Singleton<T>.ClearMode = false;
	}

	public virtual void Init()
	{
	}

	public static void DestorySingleton()
	{
		object @lock = Singleton<T>._lock;
		lock (@lock)
		{
			if (Singleton<T>._instance != null)
			{
				Singleton<T>.ClearMode = true;
				UnityEngine.Object.Destroy(Singleton<T>._instance.gameObject);
				Singleton<T>._instance = (T)((object)null);
			}
		}
	}

	private static T _instance;

	private static object _lock = new object();

	private static bool applicationIsQuitting = false;

	private static bool ClearMode = false;
}
