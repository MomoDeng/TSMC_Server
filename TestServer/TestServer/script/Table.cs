using System;

public class Table
{
	public Table()
	{

	}

	public static string GetMsgRespose(string msg) {
		string s = string.Empty;
		
		if (msg == GetMsgByNo(MsgNo.dog))
		{
			return GetRseposeMsgByNo(MsgNo.dog);
		}
		else if (msg == GetMsgByNo(MsgNo.cat))
		{
			return GetRseposeMsgByNo(MsgNo.cat);
		}
		else if (msg == GetMsgByNo(MsgNo.god))
		{
			return GetRseposeMsgByNo(MsgNo.god);
		}
		else {
			s = "No correspond Msg";
		}

        return s;
	}

	public static string GetMsgByNo(MsgNo no){
		string s = string.Empty;
		switch (no) 
		{
			case MsgNo.dog:
				s = "dog";
				break;
			case MsgNo.cat:
				s = "cat";
				break;
			case MsgNo.god:
				s = "god";
				break;

		}

		return s;
	}

	public static string GetRseposeMsgByNo(MsgNo no) {
		string msg = string.Empty;
		switch (no)
		{
			case MsgNo.dog:
				msg = "Wan wan";
				break;
			case MsgNo.cat:
				msg = "meow meow";
				break;
			case MsgNo.god:
				msg = "skr skr";
				break;

		}

		return msg;
	}

}

public enum MsgNo { 
	dog,
	cat,
	god
}
