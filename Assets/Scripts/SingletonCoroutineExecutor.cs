using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCoroutineExecutor : MonoBehaviour
{
    public static SingletonCoroutineExecutor Instance
	{
		get
		{
			if(_instance == null)
			{
				var go = new GameObject("SingletonCoroutineExecutor");
				_instance = go.AddComponent<SingletonCoroutineExecutor>();
			}
			return _instance;
		}
	}

	static SingletonCoroutineExecutor _instance;

	public Coroutine Execute(IEnumerator coroutine)
	{
		return StartCoroutine(coroutine);
	}

	public Coroutine Execute(IEnumerator coroutine, Action onEnd)
	{
		return StartCoroutine(WorkCoroutine(coroutine, onEnd));
	}

	IEnumerator WorkCoroutine(IEnumerator coroutine, Action onEnd)
	{
		yield return coroutine;
		onEnd?.Invoke();
	}
}
