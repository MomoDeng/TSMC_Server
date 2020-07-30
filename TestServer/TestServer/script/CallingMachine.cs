using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class CallingMachine
{
    private Queue<TcpClient> _clients;
	//private Queue<Callee> _callees;

	public CallingMachine()
	{
		_clients = new Queue<TcpClient>();
	}

	public void AddNewClient(TcpClient newClient) {
		_clients.Enqueue(newClient);
	}

	public TcpClient GetClientFromQueue() {
		return _clients.Dequeue();
	}

	public bool HaveClientWaiting() {
		return (_clients.Count != 0 ? true : false );
	}


	//public void AddNewCallee(Callee callee) {
	//	_callees.Enqueue(callee);
	//}

	//public Callee GetCalleeFromQueue() {
	//	return _callees.Dequeue();
	//}


}
