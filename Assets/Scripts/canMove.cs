using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class canMove : MonoBehaviour {

	SerialPort stream = new SerialPort("/dev/cu.usbmodem1421",9600);
	int buttonState = 0;



	// Use this for initialization
	void Start () {
		stream.Open ();	
	}
	
	// Update is called once per frame
	void Update () {
		string value = stream.ReadLine ();
		buttonState = int.Parse (value);
		Debug.Log ("connected;" + buttonState);
	}


}
