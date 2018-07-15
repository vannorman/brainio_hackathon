using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class RGB : MonoBehaviour {

	SerialPort stream = new SerialPort("/dev/cu.usbmodem1421",9600);

	// Use this for initialization
	void Start () {
		stream.Open ();
	}

//	public string ReadFromArduino(int timeout = 0)
//	{
//		stream.ReadTimeout = timeout;
//		try
//		{
//			return stream.ReadLine();
//		}
//		catch (TimeoutException)
//		{
//			return null;
//		}
//	}

	float t = 0;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			stream.Write ("r");
		} else if (Input.GetKeyDown (KeyCode.G)) {
			stream.Write ("g");
		} else if (Input.GetKeyDown (KeyCode.B)) {
			stream.Write ("b");
		}
		t -= Time.deltaTime;
		if (t < 0) {
			t = 1;
			Debug.Log ("stream.ReadLine():" + stream.ReadLine ());
		}
	}
}
