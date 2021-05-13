using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmailPassword
{
    private String email;
	
	private String password;

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

}