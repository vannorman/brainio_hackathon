/* StreamConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 */
using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class SerialStreamComm : MonoBehaviour {




	/* The serial port where the Stream is connected. */
	[Tooltip("The serial port where the Stream is connected")]
	public string port = "COM4";
	/* The baudrate of the serial port. */
	[Tooltip("The baudrate of the serial port")]
	public int baudrate = 9600;

	private SerialPort stream;




	void Start(){

		Open ();





	}



	public void Open () {
		// Opens the serial port
		stream = new SerialPort(port, baudrate);
		stream.ReadTimeout = 50;
		stream.Open();
		//this.stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
	}

	public void WriteToStream(string message)
	{
		// Send the request
		stream.WriteLine(message);
		stream.BaseStream.Flush();

	}

	public string ReadFromStream(int timeout = 0)
	{
		stream.ReadTimeout = timeout;
		try
		{
			return stream.ReadLine();
		}
		catch (TimeoutException)
		{
			return null;
		}
	}


	public IEnumerator AsynchronousReadFromStream(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
	{
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);

		string dataString = null;

		do
		{
			// A single read attempt
			try
			{
				dataString = stream.ReadLine();
			}
			catch (TimeoutException)
			{
				dataString = null;
			}

			if (dataString != null)
			{
				callback(dataString);
				yield return null;
			} else
				yield return new WaitForSeconds(0.05f);

			nowTime = DateTime.Now;
			diff = nowTime - initialTime;

		} while (diff.Milliseconds < timeout);

		if (fail != null)
			fail();
		yield return null;
	}

	public void Close()
	{
		stream.Close();
	}

	float t = 0;
	void Update(){
		if (Input.GetKeyDown (KeyCode.P)) {
			Debug.Log ("send!");
			WriteToStream ("ECHO blah");
		
		}
		t -= Time.deltaTime;
		if (t < 0) {
			t = 0.1f;
			StartCoroutine
			(
				AsynchronousReadFromStream
				(   (string s) => Debug.Log(s),     // Callback
					null,
//					() => Debug.LogError("Error!"), // Error callback
					100f                          // Timeout (milliseconds)
				)
			);
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			stream.BaseStream.Flush ();
			stream.Write ("r");
		} else if (Input.GetKeyDown (KeyCode.G)) {
			stream.BaseStream.Flush ();
			stream.Write ("g");
		} else if (Input.GetKeyDown (KeyCode.B)) {
			stream.BaseStream.Flush ();
			stream.Write ("b");
		}
	}

}