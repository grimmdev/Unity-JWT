using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.JWT;

public class JWTTest : MonoBehaviour {

	string secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

	void Awake()
	{
		var payLoad = new Dictionary<string, object>()
		{
			{ "claim1", 0 },
			{ "claim2", "claim2-value" }
		};
		JWT.ENCODE (payLoad, secretKey, (token) => {
			Debug.Log(token);
			Debug.Log("Verified: " + JWT.VERIFY(token, secretKey));
			JWT.DECODE(token, secretKey, (results) => {
				Debug.Log(results);
			});
		});
	}
}