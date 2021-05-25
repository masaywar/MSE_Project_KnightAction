using System;


[Serializable]
public class LoginData
{
    public long id;
	public String userName;
	public String email;
	public String password;  // Doesn't hold real password
	public String userVersion;

	public LoginData Clone()
	{
		LoginData clone = new LoginData();

		clone.id = id;
		clone.userName = userName;
		clone.email = email;
		clone.password = null;
		clone.userVersion = userVersion;

		return clone;
	}

	public void ShallowClone(LoginData clone)
	{
		clone.id = id;
		clone.userName = userName;
		clone.email = email;
		clone.password = null;
		clone.userVersion = userVersion;
	}
}
