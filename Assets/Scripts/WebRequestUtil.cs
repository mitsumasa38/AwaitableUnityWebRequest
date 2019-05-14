using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public static class WebRequestUtil
{
	public static async Task<Texture> LoadTexture(string url, CancellationToken ct)
	{
		return await ConvertCoroutineToTask<Texture>(tcs => LoadTextureCore(url, tcs, ct));
	}

	static IEnumerator LoadTextureCore(string url, TaskCompletionSource<Texture> tcs, CancellationToken ct)
	{
		using(var req = new UnityWebRequest(url, "GET"))
		{
			req.downloadHandler = new DownloadHandlerTexture();
			req.disposeDownloadHandlerOnDispose = true;
			req.disposeUploadHandlerOnDispose = true;

			req.SendWebRequest();
			
			while(!req.isDone)
			{
				if(ct.IsCancellationRequested)
				{
					Debug.Log("Canceled");
					tcs.TrySetCanceled(ct);
					yield break;
				}

				yield return null;
			}

			if(ct.IsCancellationRequested)
			{
				Debug.Log("Canceled");
				tcs.TrySetCanceled(ct);
				yield break;
			}

			if(req.isNetworkError)
			{
				tcs.TrySetException(new Exception("NetworkError"));
				yield break;
			}
			
			if(req.isHttpError)
			{
				tcs.TrySetException(new Exception("HttpError"));
				yield break;
			}

			tcs.TrySetResult((req.downloadHandler as DownloadHandlerTexture).texture);
		}
	}

	static Task<T> ConvertCoroutineToTask<T>(Func<TaskCompletionSource<T>, IEnumerator> creation)
    {
        var tcs = new TaskCompletionSource<T>();

        var coroutine = creation(tcs);

        SingletonCoroutineExecutor.Instance.Execute(coroutine);

        return tcs.Task;
    }
}
