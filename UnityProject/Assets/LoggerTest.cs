
using UnityEngine;

public class LoggerTest : MonoBehaviour
{
	Network.Communicator _debugProtocol = new Network.Communicator();

	void Start()
	{
		_debugProtocol.Setup();
		Debugging.Logger.Initialize();
	}

	private void OnApplicationQuit()
	{
		_debugProtocol.Dispose();
		Debugging.Logger.Finish();
	}

	void Update()
	{
		_debugProtocol.Send(new Network.LogMessage() {  Data = System.Text.Encoding.UTF8.GetBytes("Test") });
		Debugging.Logger.Log("Test");
	}
}
