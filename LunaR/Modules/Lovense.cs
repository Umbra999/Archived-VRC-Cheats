using LunaR.Wrappers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;

namespace LunaR.Modules
{
    internal class Lovense : MonoBehaviour
    {
        public Lovense(IntPtr ptr) : base(ptr)
        {
        }

        private static readonly List<Toy> toys = new();

        public static Toy CreateToy(string URL, bool Local)
        {
            string token = GetAuthToken(URL);
            string[] idName = getIDandName(token);
            if (token == null || idName == null)
            {
                Extensions.Logger.LogError("Failed to connect to Lovense");
                return null;
            }
            else return new Toy(idName[0], token, idName[1], Local);
        }

        public static void DisconnectToy(Toy Toy)
        {
            Toy.SendVibration(0);
            toys.Remove(Toy);
        }

        private static int HandleLocalGesture(bool Friends)
        {
            int speed = 0;

            foreach (VRC.Player Player in Utils.PlayerManager.GetAllPlayers())
            {
                if (Friends && !Player.IsFriend()) continue;
                float Distance = Vector3.Distance(Utils.CurrentUser.transform.position, Player.transform.position);
                if (Distance < 1.5f)
                {
                    HandGestureController.Gesture Gesture = Player.GetVRCPlayer().field_Private_AnimatorControllerManager_0.field_Private_VRC_AnimationController_0.field_Private_HandGestureController_0.field_Private_Gesture_2;
                    if (Gesture == HandGestureController.Gesture.Open || Gesture == HandGestureController.Gesture.None) Gesture = Player.GetVRCPlayer().field_Private_AnimatorControllerManager_0.field_Private_VRC_AnimationController_0.field_Private_HandGestureController_0.field_Private_Gesture_0;
                    switch (Gesture)
                    {
                        case HandGestureController.Gesture.Fist:
                            speed = 20;
                            break;

                        case HandGestureController.Gesture.RockNRoll:
                            speed = 18;
                            break;

                        case HandGestureController.Gesture.Peace:
                            speed = 15;
                            break;

                        case HandGestureController.Gesture.Point:
                            speed = 12;
                            break;

                        case HandGestureController.Gesture.ThumbsUp:
                            speed = 10;
                            break;
                    }
                    break;
                }
            }
            return speed;
        }

        private static int HandleRemoteGesture()
        {
            int speed = 0;

            HandGestureController.Gesture Gesture = Utils.CurrentUser.field_Private_AnimatorControllerManager_0.field_Private_VRC_AnimationController_0.field_Private_HandGestureController_0.field_Private_Gesture_2;
            if (Gesture == HandGestureController.Gesture.Open || Gesture == HandGestureController.Gesture.None) Gesture = Utils.CurrentUser.field_Private_AnimatorControllerManager_0.field_Private_VRC_AnimationController_0.field_Private_HandGestureController_0.field_Private_Gesture_0;
            switch (Gesture)
            {
                case HandGestureController.Gesture.Fist:
                    speed = 20;
                    break;

                case HandGestureController.Gesture.RockNRoll:
                    speed = 18;
                    break;

                case HandGestureController.Gesture.Peace:
                    speed = 15;
                    break;

                case HandGestureController.Gesture.Point:
                    speed = 12;
                    break;

                case HandGestureController.Gesture.ThumbsUp:
                    speed = 10;
                    break;
            }
            return speed;
        }

        private static int HandleLocalTouch(bool Friends)
        {
            List<float> LocalPositions = new();

            foreach (VRC.Player Player in Utils.PlayerManager.GetAllPlayers())
            {
                if (Friends && !Player.IsFriend()) continue;
                try
                {
                    Animator Animator = Player.GetVRCPlayer().field_Internal_Animator_0;

                    GameObject LeftHand = Animator.GetBoneTransform(HumanBodyBones.LeftHand).gameObject;
                    GameObject RightHand = Animator.GetBoneTransform(HumanBodyBones.RightHand).gameObject;
                    GameObject Head = Animator.GetBoneTransform(HumanBodyBones.Head).gameObject;
                    GameObject Hips = Animator.GetBoneTransform(HumanBodyBones.Hips).gameObject;

                    GameObject TargetHips = Utils.CurrentUser.field_Internal_Animator_0.GetBoneTransform(HumanBodyBones.Hips).gameObject;

                    float LeftHandFloat = Vector3.Distance(LeftHand.transform.position, TargetHips.transform.position);
                    float RightHandFloat = Vector3.Distance(RightHand.transform.position, TargetHips.transform.position);
                    float HeadFloat = Vector3.Distance(Head.transform.position, TargetHips.transform.position);
                    float HipsFloat = Vector3.Distance(Hips.transform.position, TargetHips.transform.position);

                    if (LeftHandFloat <= 0.2f) LocalPositions.Add(LeftHandFloat);
                    if (RightHandFloat <= 0.2f) LocalPositions.Add(RightHandFloat);
                    if (HeadFloat <= 0.2f) LocalPositions.Add(HeadFloat);
                    if (HipsFloat <= 0.2f) LocalPositions.Add(HipsFloat);
                }
                catch { }
            }

            if (LocalPositions.Count > 0)
            {
                int Transfer = Convert.ToInt32(LocalPositions.Min() * 100);
                return HandleInt(20 - Transfer);
            }

            return 0;
        }

        private static int HandleRemoteTrigger()
        {
            float left = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
            float right = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
            float speed = left > right ? left : right;
            return HandleInt((int)speed);
        }

        private static int HandleRemoteTouch(bool Friends)
        {
            List<float> LocalPositions = new();

            foreach (VRC.Player Player in Utils.PlayerManager.GetAllPlayers())
            {
                if (Friends && !Player.IsFriend()) continue;
                try
                {
                    Animator Animator = Utils.CurrentUser.field_Internal_Animator_0;

                    GameObject LeftHand = Animator.GetBoneTransform(HumanBodyBones.LeftHand).gameObject;
                    GameObject RightHand = Animator.GetBoneTransform(HumanBodyBones.RightHand).gameObject;
                    GameObject Head = Animator.GetBoneTransform(HumanBodyBones.Head).gameObject;
                    GameObject Hips = Animator.GetBoneTransform(HumanBodyBones.Hips).gameObject;

                    GameObject TargetHips = Player.GetVRCPlayer().field_Internal_Animator_0.GetBoneTransform(HumanBodyBones.Hips).gameObject;

                    float LeftHandFloat = Vector3.Distance(LeftHand.transform.position, TargetHips.transform.position);
                    float RightHandFloat = Vector3.Distance(RightHand.transform.position, TargetHips.transform.position);
                    float HeadFloat = Vector3.Distance(Head.transform.position, TargetHips.transform.position);
                    float HipsFloat = Vector3.Distance(Hips.transform.position, TargetHips.transform.position);

                    if (LeftHandFloat <= 0.2f) LocalPositions.Add(LeftHandFloat);
                    if (RightHandFloat <= 0.2f) LocalPositions.Add(RightHandFloat);
                    if (HeadFloat <= 0.2f) LocalPositions.Add(HeadFloat);
                    if (HipsFloat <= 0.2f) LocalPositions.Add(HipsFloat);
                }
                catch { }
            }

            if (LocalPositions.Count > 0)
            {
                int Transfer = Convert.ToInt32(LocalPositions.Min() * 100);
                return HandleInt(20 - Transfer);
            }

            return 0;
        }

        public void Update()
        {
            if (toys.Count == 0) return;
            int speed = 0;
            foreach (Toy toy in toys)
            {
                if (toy.IsSelf)
                {
                    if (toy.TouchControl) speed = HandleLocalTouch(toy.FriendsOnly);
                    else if (toy.GestureContol) speed = HandleLocalGesture(toy.FriendsOnly);
                    else if (toy.TriggerControl) { }
                }
                else
                {
                    if (toy.TouchControl) speed = HandleRemoteTouch(toy.FriendsOnly);
                    else if (toy.TriggerControl) speed = HandleRemoteTrigger();
                    else if (toy.GestureContol) speed = HandleRemoteGesture();
                }
                toy.SendVibration(speed);
            }
        }

        private static int HandleInt(int speed)
        {
            if (speed < 10) return 0;
            if (speed < 12) return 10;
            if (speed < 15) return 12;
            if (speed < 18) return 15;
            if (speed < 20) return 18;
            return 20;
        }

        private static string[] getIDandName(string token)
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
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            int start = result.IndexOf("JSON.parse('") + 12;
            int end = result.IndexOf("')");
            if (end == -1) return null;
            JObject json = JObject.Parse(result.Substring(start, end - start));
            if (json.Count == 0) return null;
            else
            {
                string id = (string)json.First.First["id"];
                string name = (string)json.First.First["name"];
                name = char.ToUpper(name[0]) + name.Substring(1);
                return new string[] { name, id };
            }
        }

        public static string GetAuthToken(string url)
        {
            if (!url.StartsWith("https://c.lovense.com/c/")) return null;
            HttpWebResponse resp = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                resp = (HttpWebResponse)req.GetResponse();
                url = resp.Headers["Location"];
            }
            catch { return null; }
            finally
            {
                if (resp != null) resp.Close();
            }
            int pos = url.LastIndexOf("/") + 1;
            return url.Substring(pos, url.Length - pos);
        }

        public class Toy
        {
            private int lastspeed = 0;
            public readonly string token;
            public readonly string id;
            public readonly bool IsSelf;
            public bool FriendsOnly;

            public bool TouchControl;
            public bool TriggerControl;
            public bool GestureContol;

            public Toy(string name, string Auth, string LovenseID, bool OwnLovense)
            {
                token = Auth;
                id = LovenseID;
                IsSelf = OwnLovense;
                toys.Add(this);
                Extensions.Logger.Log($"Connected to {name}", Extensions.Logger.LogsType.Info);
            }

            public void SendVibration(int speed)
            {
                if (speed != lastspeed)
                {
                    lastspeed = speed;

                    SendToAPI(lastspeed);
                }
            }

            private void SendToAPI(int speed)
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
}