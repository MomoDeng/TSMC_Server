using System;

public class Table
{
	public Table()
	{

	}

	public static string GetMsgByNo(MsgNo no)
	{
		string msg = string.Empty;
		switch (no)
		{
			case MsgNo.dog:
				msg = "ask dog";
				break;
			case MsgNo.cat:
				msg = "ask cat";
				break;
			case MsgNo.god:
				msg = "ask god";
				break;

		}

		return msg;
	}

	public static string GetRseposeMsgByNo(MsgNo no)
	{
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

public enum MsgNo
{
	dog,
	cat,
	god
}
