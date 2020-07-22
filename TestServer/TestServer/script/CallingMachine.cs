using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class CallingMachine
{
    private Queue<Callee> _callees;


	public CallingMachine()
	{
		_callees = new Queue<Callee>();
	}

	public void AddNewClient(Callee newCallee) {
		_callees.Enqueue(newCallee);
	}

	public Callee GetCalleeFromQueue() {
		return _callees.Dequeue();
	}

	public bool HaveClientWaiting() {
		return (_callees.Count != 0 ? true : false );
	}

}
