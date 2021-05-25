// class PlayerScores

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using Proyecto26;
/*
    <Summary>
    + SignUpUser(email: string, username: string, password: string) : UserClient

    + SignInUser(email: string, password: string) : UserClient

    + UpdateUser(email: string, password: string, userVersion: string) : int

    + DeleteUser(string email, string username, string password) : int

    + GetPlayer(name: string): PlayerClient

    + UpdatePlayer(name: string, score: int, coin: int, companion: string): int

    + GetAllRank() : Rank[]

    + getPlayerRank(name: String): Rank
*/

public class ClientUserAccess : MonoBehaviour
{
    public Text example;
    public Text userCompanion;
    public Text coin;

    public InputField emailText;
    public InputField usernameText;
    public InputField passwordText;
    public InputField newVersion;

    public InputField newScore;
    public InputField newCoin;
    public InputField newCompanion;

    public string realURL = "122.35.41.80:9090/sak/";
    public string localURL = "http://localhost:9090/sak/";
    //public string localURL = "http://a4e6b6a50da0.ngrok.io/sak/";
    public LoginData user = new LoginData();
    public UserData player = new UserData();

    public static ClientUserAccess CUAinstance = null;  

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (CUAinstance == null)
        {        
            //if not, set instance to this
            CUAinstance = this;
        }
        //If instance already exists and it's not this:
        else if (CUAinstance != this)
        {        
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
        }   
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void SignUpUserButton()
    {   
        example.text = "";
        SignUpUser(emailText.text, usernameText.text, passwordText.text);
    }
    public void SignInUserButton()
    {
        example.text = "";
        SignInUser(emailText.text, passwordText.text);
    }
    public void UpdateUserButton()
    {
        example.text = "";
        UpdateUser(emailText.text, passwordText.text, newVersion.text);
    }
    public void DeleteUserButton()
    {
        example.text = "";
        DeleteUser(emailText.text, usernameText.text, passwordText.text);
    }
    public void GetAllRankButton()
    {
        example.text = "";
        GetAllRank();
    }

    public void GetPlayerRankButton()
    {
        example.text = "";
        GetPlayerRank(usernameText.text);
    }
    public void UpdatePlayerButton()
    {
        example.text = "";
        try{
            updatePlayer(usernameText.text, int.Parse(newScore.text), int.Parse(newCoin.text), newCompanion.text);
        }
        catch{
            Debug.Log("can't convert string to integer.");
        }
    }

    public LoginData SignUpUser(string email, string username, string password)
    {
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        LoginData returnValue = new LoginData();

        RestClient.Post<LoginData>(localURL + "signupuser", userData).Then(
            response =>
            {
                if (response.email.Equals("error")){
                    // When Email is already in DB or user doesn't give any email
                    Debug.Log("Invalid email!");
                    example.text = "Invalid email!"; 
                    returnValue = null;
                }
                else if (response.userName.Equals("error")){
                    // When userName is already in DB
                    Debug.Log("Please use another Nickname!");
                    example.text = "Please use another Nickname!"; 
                    returnValue = response;
                }
                else{
                    // Sign up succeed
                    Debug.Log("Hi " + response.userName + ". Sign up completed!");
                    example.text = "Hi " + response.userName + ". Sign up completed!";
                    returnValue = null;
                    Debug.Log(returnValue.userName);
                }
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
        // If signup fails, then return null. If signup succeed, then return the new signup userclient object.
        return returnValue;
    }

    public LoginData SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
    /*
#if UNITY_EDITOR
        SceneManager.LoadScene("Menu");
        return null;
#endif
    */

        LoginData returnValue = new LoginData();

        RestClient.Post<LoginData>(localURL + "signinuser", userData).Then(
            response =>
            {
                if (response == null){
                    Debug.Log("Please check Email again..");
                    example.text = "Please check Email again..";

                    returnValue = null;
                }
                else if (response.password.Equals("error")){
                    Debug.Log("Please check password again..");
                    example.text = "Please check password again..";
                    returnValue = null;
                }
                else{
                    Debug.Log("Hi " + response.userName);
                    example.text = "Hi " + response.userName;
                    
                    user.id = response.id;
                    user.userName = response.userName;
                    user.email = response.email;
                    user.password = response.password;
                    user.userVersion = response.userVersion;

                    UserData p = GetPlayer(user.userName);
                    player.knight = p.knight;
                    player.coin = p.coin;

                    SceneManager.LoadScene("Menu");

                    returnValue = response;
                }
            }).Catch(error =>
        {
            // If the request fails
            Debug.Log(error);
        });
        // If signin fails, then return null. If signin succeed, then return userclient object.
        return returnValue;
    }

    //change password
    public int UpdateUser(string email, string password, string userVersion)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"userVersion\":\"" + userVersion + "\"}";

        int returnValue = 0;

        RestClient.Post<Flag>(localURL + "/update/logindata", userData).Then(
            response =>
            {
                if (response.flag == 1){
                    returnValue = 1;
                    Debug.Log("Update user's data succeed!");
                }
                else {
                    Debug.Log("Update fails");
                }
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
        // If succeed, return 1, else return 0
        return returnValue;
    }

    //delete ID
    /*
    * Important Task : Password check needed.
    */
    public int DeleteUser(string email, string username, string password)
    {
        // JSON body only needs email & username. The rest of the data don't matter.
        string userData = "{\"userName\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password +
         "\",\"userVersion\":\"" + "1.0" + "\"}";

        int returnValue = 0;

        RestClient.Post<Flag>(localURL + "delete", userData).Then(
            response =>
            {
                if (response.flag == 1){
                    Debug.Log("Account successfully deleted..Good Bye!");
                    returnValue = 1;
                }
                else{
                    Debug.Log("Deletion failed");
                }
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });
        return returnValue;
    }

    public UserData GetPlayer (string name){

        string userData = "{\"userName\":\"" + name + "\",\"rank\":" + 123 + ",\"score\":" + 123 + "}";

        UserData returnValue = new UserData();

        RestClient.Post<UserData>(localURL + "get/user", userData).Then(
            response =>
            {
                returnValue = response;
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;

    }

    public int updatePlayer(string name, int score, int coin, string knight){
        string playerdata = "{\"userName\":\"" + name + "\",\"score\":" + score + ",\"coin\":" + coin +
         ",\"knight\":\"" + knight + "\"}";

        int returnValue = 0;

        RestClient.Post<Flag>(localURL + "update/user", playerdata).Then(
            response =>
            {
                if (response.flag == 1){
                    // update succeed
                    example.text = "Updated player data succeed";
                    returnValue = 1;
                }
                else{
                    example.text = "flag not working";
                }
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;
        
    }


    public NameRank[] GetAllRank()
    {
        //List<PlayerClient> returnValue = new List<PlayerClient>();
        // Rank[] returnValue;
        RestClient.GetArray<NameRank>(localURL + "sorted").Then(
            response =>
            {
                foreach (NameRank r in response) {
                    example.text = example.text + r.userName + ": Ranking->" + r.rank + ", Score->"+ r.score + '\n';
                }
                Debug.Log(response[0].userName);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return null;
    }

    public NameRank GetPlayerRank(string name){

        string playerdata = "{\"userName\":\"" + name + "\",\"score\":" + 123 + ",\"coin\":" + 123 +
         ",\"knight\":\"" + "Dummy" + "\"}";

        NameRank returnValue = new NameRank();

        RestClient.Post<NameRank>(localURL + "getrankscore", playerdata).Then(
            response =>
            {
                returnValue = response;
                example.text = response.userName + "'s rank: " + response.rank;
                Debug.Log(response.rank);
            }).Catch(error =>
            {
                // If the request fails
                Debug.Log(error);
            });

        return returnValue;

    }
}