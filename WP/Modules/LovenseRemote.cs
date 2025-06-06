using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using UnityEngine;
using WengaPort.Api;

namespace WengaPort.Modules
{
    public class LovenseRemote : MonoBehaviour
    {
        public static ArrayList toys = new ArrayList();
        static string findButton = null;
        public static QMNestedButton menu;
        static QMSingleButton addButtonUI;
        static KeyCode lockButton;
        static KeyCode holdButton;
        public void Start()
        {
            menu = new QMNestedButton(Buttons.UtilsMenu.ThisMenu, 5, 0, "Lovense", "Lovense Menu", null, null, null, Color.yellow);
            MenuText Text = new MenuText(menu, 800, 280, "Connected toys: 0 / 2");
            Text.SetFontSize(70);
            addButtonUI = new QMSingleButton(menu, 1, 1, "Add\nLovense", delegate () 
            {
                string token = getToken();//gets id from link in clipboard
                string[] idName = getIDandName(token);//name, id
                if (token == null || idName == null)
                {
                    Extensions.Logger.WengaLogger("Failed to connect to Lovense");
                }
                if (toys.Count == 2)
                {
                    Extensions.Logger.WengaLogger("To much toys connected (2 Max)");
                }
                else
                {
                    new Toy(idName[0], token, idName[1]);
                    //Text.SetText($"Connected toys: {toys.Count} / 2");
                }
            }, "Paste the full control key here", null, null);

            new QMSingleButton(menu, 4, 1, "Clear", () =>
            {
                foreach (var Button in Toy.Buttons)
                {
                    Destroy(Button);
                }
                foreach (Toy toy in toys)
                {
                    toy.setSpeed(0);
                }
                toys.Clear();
                //Text.SetText($"Connected toys: 0 / 2");
                Toy.x = 2;
            }, "Remove all Lovense toys");

            new QMSlider(Utils.QuickMenu.transform.Find(menu.getMenuName()), "Intensity", 250, -520, delegate (float value)
            {
                sliderspeed = value;
            }, 0, 100, 0, true);
        }
        static float sliderspeed = 0;
        public void Update()
        {
            float speed = 0;
            if (toys.Count == 0) return;
            else if (findButton != null) getButton();
            foreach (Toy toy in toys)
            {
                switch (toy.hand)
                {
                    case "none":
                        break;
                    case "left":
                        speed = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
                        break;
                    case "right":
                        speed = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
                        break;
                    case "either":
                        float left = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
                        float right = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
                        if (left > right) speed = left;
                        else speed = right;
                        break;
                    case "slider":
                        speed = sliderspeed / 50;
                        break;
                }
                toy.setSpeed(speed);
            }
        }

        public static void getButton()
        {
            //A-Z
            for (int i = 97; i <= 122; i++)
                if (Input.GetKey((KeyCode)i))
                {
                    setButton((KeyCode)i);
                    return;
                }

            //left vr controller buttons
            if (Input.GetKey(KeyCode.JoystickButton0)) setButton(KeyCode.JoystickButton0);
            else if (Input.GetKey(KeyCode.JoystickButton1)) setButton(KeyCode.JoystickButton1);
            else if (Input.GetKey(KeyCode.JoystickButton2)) setButton(KeyCode.JoystickButton2);
            else if (Input.GetKey(KeyCode.JoystickButton3)) setButton(KeyCode.JoystickButton3);
            else if (Input.GetKey(KeyCode.JoystickButton8)) setButton(KeyCode.JoystickButton8);
            else if (Input.GetKey(KeyCode.JoystickButton9)) setButton(KeyCode.JoystickButton9);

            //right vr controller buttons
            else if (Input.GetKey(KeyCode.Joystick1Button0)) setButton(KeyCode.Joystick1Button0);
            else if (Input.GetKey(KeyCode.Joystick1Button1)) setButton(KeyCode.Joystick1Button1);
            else if (Input.GetKey(KeyCode.Joystick1Button2)) setButton(KeyCode.Joystick1Button2);
            else if (Input.GetKey(KeyCode.Joystick1Button3)) setButton(KeyCode.Joystick1Button3);
            else if (Input.GetKey(KeyCode.Joystick1Button8)) setButton(KeyCode.Joystick1Button8);
            else if (Input.GetKey(KeyCode.Joystick1Button9)) setButton(KeyCode.Joystick1Button9);
        }

        public static void setButton(KeyCode button)
        {
            if (findButton.Equals("lockButton"))
            {
                lockButton = button;
            }
            else if (findButton.Equals("holdButton"))
            {
                holdButton = button;
            }
            findButton = null;
        }

        static string[] getIDandName(string token)
        {
            if (token == null) return null;
            var url = "https://c.lovense.com/app/ws2/play/" + token;
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Headers["authority"] = "c.lovense.com";
            httpRequest.Headers["sec-ch-ua"] = "\"Google Chrome\";v=\"87\", \" Not; A Brand\";v=\"99\", \"Chromium\";v=\"87\"";
            httpRequest.Headers["sec-ch-ua-mobile"] = "?0";
            httpRequest.Headers["upgrade-insecure-requests"] = "1";
            httpRequest.Headers["sec-fetch-site"] = "same-origin";
            httpRequest.Headers["sec-fetch-mode"] = "navigate";
            httpRequest.Headers["sec-fetch-user"] = "?1";
            httpRequest.Headers["sec-fetch-dest"] = "document";
            httpRequest.Headers["accept-language"] = "en-US,en;q=0.9";
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            httpRequest.Referer = "https://c.lovense.com/app/ws/play/" + token;
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                int start = result.IndexOf("JSON.parse('") + 12;
                int end = result.IndexOf("')");
                if (end == -1) return null;
                JObject json = JObject.Parse(result.Substring(start, end - start));
                if (json.Count == 0)
                {
                    return null;
                }
                else
                {
                    string id = (string)json.First.First["id"];
                    string name = (string)json.First.First["name"];
                    name = char.ToUpper(name[0]) + name.Substring(1);//make first letter uppercase
                    return new string[] { name, id };
                }
            }
        }

        public static string getToken()
        {
            string url = Clipboard.GetText();
            if (!url.Contains("https://c.lovense.com/c/")) return null;
            HttpWebResponse resp = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                resp = (HttpWebResponse)req.GetResponse();
                url = resp.Headers["Location"];
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (resp != null) resp.Close();
            }
            int pos = url.LastIndexOf("/") + 1;
            return url.Substring(pos, url.Length - pos);
        }
        public LovenseRemote(IntPtr ptr) : base(ptr) { }
    }

    public class Toy
    {
        public static List<GameObject> Buttons = new List<GameObject>();
        public string hand = "none";
        public static int x = 2;
        private QMSingleButton button;
        private float lastSpeed;
        private string name;
        private string token;
        private string id;

        public Toy(string name, string token, string id)
        {
            this.token = token;
            this.id = id;
            this.name = name;
            button = new QMSingleButton(LovenseRemote.menu, x++, 1, $"[{name}] \nNo\nHand", delegate () {
                changeHand();
            }, "Change Hand for Trigger Control", null, null);
            Buttons.Add(button.getGameObject());
            LovenseRemote.toys.Add(this);
        }

        public void setSpeed(float speed)
        {
            speed = (int)(speed * 10);
            if (speed != lastSpeed)
            {
                lastSpeed = speed;
                new Thread(() =>
                {
                    send((int)speed);
                })
                { IsBackground = true }.Start();
            }
        }

        public void changeHand()
        {
            switch (hand)
            {
                case "none":
                    hand = "left";
                    button.setButtonText($"[{name}]\nLeft Trigger");
                    break;
                case "left":
                    hand = "right";
                    button.setButtonText($"[{name}]\nRight Trigger");
                    break;
                case "right":
                    hand = "either";
                    button.setButtonText($"[{name}]\nBoth Trigger");
                    break;
                case "either":
                    hand = "slider";
                    button.setButtonText($"[{name}]\nSlider");
                    break;
                case "slider":
                    hand = "none";
                    button.setButtonText($"[{name}]\nNone");
                    break;
            }
        }

        private void send(int speed)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create("https://c.lovense.com/app/ws/command/" + token);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write("order=%7B%22cate%22%3A%22id%22%2C%22id%22%3A%7B%22" + id + "%22%3A%7B%22v%22%3A" + speed + "%2C%22p%22%3A-1%2C%22r%22%3A-1%7D%7D%7D");
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
        }
    }
}