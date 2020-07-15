using System;

public class IdHandler
{
	bool[] _IsIdUsed = new Boolean[10000];   // 裝哪些 ID 可用
	int _cur, _next;


	public IdHandler()
	{
		_cur = 0;
		_next = 1;
	}

	public int GetNewId() {
		_cur = _next;
		_next++;
		return _cur;	
	}


}
