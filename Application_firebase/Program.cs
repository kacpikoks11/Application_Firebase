using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using System.Text.Json;

namespace Program
{
    class Userdata
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
   
    static class Prog
    {

        static FirebaseConfig ifc = new FirebaseConfig
        {
            AuthSecret = "nvVBqAC86XLnRt4xluWGMwW0sdnYrvDZEUNQY0TC",
            BasePath = "https://test-19bdf-default-rtdb.firebaseio.com/"
        };

        static FirebaseClient Client;



        static void Main()
        {
            string? Username = "", Password = "";
            bool logged = false;
            try
            {
                Client = new(ifc);
                Console.WriteLine("Connected");
            }
            catch
            {
                throw new Exception("No internet connection, Abort");
            }


            while (!logged)
            {
                Console.WriteLine("Menu:\n1.LogIn\n2.Register\n3.Get data\n4.Quit");
                char x = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (x)
                {

                    //Login
                    case '1':
                        //Getting data from user
                        Console.WriteLine("--Log In--");
                        Console.WriteLine("Username:");
                        Username = Console.ReadLine();
                        Console.WriteLine("Password:");
                        Password = Console.ReadLine();


                        //Checking for null
                        if (Username == null || Password == null)
                        {
                            Console.WriteLine("Password or username is null");
                            break;
                        }
                        //checking for empty string
                        if (Username == "" || Password == "")
                        {
                            Console.WriteLine("No password or username provided");
                            break;
                        }

                        //Getting rensponse
                        FirebaseResponse Response = Client.Get("Users/" + Username+ "/Password");

                        // if response is null it means there is no account like that
                        if (Response.ResultAs<string>() is null)
                        {
                            Console.WriteLine("No such username");
                            break;
                        }

                        //checking if password is equal -  if its logged right
                        // if so we are logged 
                        if (Password == Response.ResultAs<string>())
                        {
                            Console.WriteLine("Succesfully logged");
                            logged = true;
                        }
                        else
                            Console.WriteLine("Wrong password");
                        break;



                    //register  
                    case '2':
                        //Getting data from user
                        Console.WriteLine("--Register--");
                        Console.WriteLine("Username:");
                        Username = Console.ReadLine();
                        Console.WriteLine("Password:");
                        Password = Console.ReadLine();

                        //checking for password and username
                        if (Password is null || Password == "")
                            break;
                        if (Username is null || Username == "")
                            break;

                        //Checking if the username is taken
                        if (Client.Get("Users/" + Username+ "/Password").ResultAs<string>() is not null)
                        {
                            Console.WriteLine("this username is used");
                            break;
                        }

                        // setting data
                        Userdata SetData = new Userdata()
                        {
                            Username = Username,
                            Password = Password
                        };
                        Client.Set("Users/" + Username, SetData);

                        Console.WriteLine("Succesfully registered");
                        break;
                   

                        //getting data
                    case '3':

                        //variable to get data
                        var data = Client.Get("Users").ResultAs<Dictionary<string, Userdata>>();

                        foreach (var user in data)
                        {
                            Console.WriteLine(user.Value.Username);
                        }
                        break;


                    //quit
                    case '4':
                        return;

                    default:
                        Console.WriteLine("Wrong key");
                        break;
                }
            }
           
        
        
        }
    }
}