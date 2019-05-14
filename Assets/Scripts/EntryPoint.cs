using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EntryPoint : MonoBehaviour
{
	[SerializeField] RawImage rawImage = null;

	async void Start()
	{
		var cts = new CancellationTokenSource();
		var url = "https://www.google.co.jp/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png";
		
		try
		{
			var tex = await WebRequestUtil.LoadTexture(url, cts.Token);
			rawImage.texture = tex;
		}
		catch(Exception e)
		{
			Debug.LogError("Catch " + e.Message);
		}
	}
}
