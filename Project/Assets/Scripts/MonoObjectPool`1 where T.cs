using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoObjectPool<T> where T : MonoBehaviour
{
	public MonoObjectPool(T t)
	{
		this.templete = t;
		this.templete.gameObject.SetActive(false);
	}

	private T create(string _name)
	{
		T result = UnityEngine.Object.Instantiate<T>(this.templete);
		result.transform.parent = this.templete.transform.parent;
		result.transform.localScale = Vector3.one;
		result.transform.localPosition = Vector3.zero;
		result.name = _name;
		return result;
	}

	private T Find(string name)
	{
		if (this.inUse.ContainsKey(name))
		{
			return this.inUse[name];
		}
		return (T)((object)null);
	}

	public T Get(string name)
	{
		T t = this.Find(name);
		if (t != null)
		{
			return t;
		}
		if (this.m_Objects.Count > 0)
		{
			t = this.m_Objects.Pop();
		}
		else
		{
			t = this.create(name);
		}
		this.inUse.Add(name, t);
		t.gameObject.SetActive(true);
		return t;
	}

	public void ReStore(T t)
	{
		t.gameObject.SetActive(false);
		this.m_Objects.Push(t);
	}

	public void ReStore(string name)
	{
		T t = this.Find(name);
		if (t != null)
		{
			t.gameObject.SetActive(false);
			this.m_Objects.Push(t);
			this.inUse.Remove(name);
		}
	}

	~MonoObjectPool()
	{
	}

	private Stack<T> m_Objects = new Stack<T>();

	private Dictionary<string, T> inUse = new Dictionary<string, T>();

	private T templete;
}
