using System;


[Serializable]
public class LoginData
{
    public long id;
	public String userName;
	public String email;
	public String password;  // Doesn't hold real password
	public String userVersion;
}
