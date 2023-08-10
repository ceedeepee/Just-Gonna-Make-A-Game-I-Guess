// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Photon.Pun;
// using TMPro;
// using Unity.Mathematics;
// using Unity.VisualScripting;
// using UnityEngine;
//
// public class NFTLoader : MonoBehaviourPun
// {
//     private WebStuff.Avatars avatars;
//     public WebStuff WebStuff;
//
//     public MisfitSwappah MisfitSwappah;
//
//     // Start is called before the first frame update
//     private bool localOne, localTwo;
//
//     void Start()
//     {
//         //WebStuff.avatarRunning = true;
//         //StartCoroutine(WebStuff.tryGetAvatars("D63bhHo634eXSj4Jq3xgu2fjB5XKc8DFHzDY9iZk7fv1"));
//         //StartCoroutine(localAvatarScript());
//         if (!photonView.IsMine)
//         {
//             StartCoroutine(RequestAvatar());
//         }
//     }
//
//     IEnumerator RequestAvatar()
//     {
//         yield return new WaitForSeconds(1);
//
//         photonView.RPC("NFTAvatarState", RpcTarget.Others);
//     }
//
//     public void getAvatars(string wallet)
//     {
//         WebStuff.avatarRunning = true;
//         StartCoroutine(WebStuff.tryGetAvatars(wallet));
//         StartCoroutine(localAvatarScript());
//     }
//
//     public int count;
//
//
//
//
//     public void nextAvatar()
//     {
//         count++;
//         if (count >= (avatars.monkes.Length + avatars.misfits.Length))
//         {
//             count = 0;
//         }
//
//         foreach (var gObjs in monkeObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         foreach (var gObjs in misfitObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         if (count == 0)
//         {
//             MisfitSwappah._BasicBehaviour.GetAnim.avatar = MisfitSwappah.Misfit;
//             misfitObjects[0].SetActive(true);
//             List<string> mizfits = new List<string>();
//             mizfits.Add(misfitObjects[0].name);
//             string[] str = mizfits.ToArray();
//             current = str;
//             photonView.RPC("setMizFit", RpcTarget.Others, str);
//             name.text = "Create your own!";
//             setJetpackParent(misfitJetpack);
//         }
//
//         if (avatars.monkes.Length > 0)
//         {
//             if (count < avatars.monkes.Length + 1 && count > 0)
//             {
//                 MisfitSwappah._BasicBehaviour.GetAnim.avatar = MisfitSwappah.Monke;
//                 name.text = avatars.monkes[count - 1].name;
//
//                 List<string> monkeBiz = new List<string>();
//                 foreach (var attribute in avatars.monkes[count - 1].attributes)
//                 {
//                     foreach (var gObjs in monkeObjects)
//                     {
//                         if (gObjs.name == attribute.value)
//                         {
//                             gObjs.SetActive(true);
//                             monkeBiz.Add(attribute.value);
//                         }
//                     }
//                 }
//
//                 string[] str = monkeBiz.ToArray();
//                 current = str;
//                 photonView.RPC("setMonke", RpcTarget.Others, str);
//                 isMonke = true;
//                 setJetpackParent(monkeJetpack);
//                 return;
//             }
//         }
//
//         if (avatars.misfits.Length > 0 && count >= avatars.monkes.Length + 1)
//         {
//             if ((count - avatars.monkes.Length + 1) < avatars.misfits.Length + 1)
//             {
//                 MisfitSwappah._BasicBehaviour.GetAnim.avatar = MisfitSwappah.Misfit;
//                 name.text = avatars.misfits[count - 1 - avatars.monkes.Length].name;
//                 List<string> mizfits = new List<string>();
//                 foreach (var attribute in avatars.misfits[count - 1 - avatars.monkes.Length].attributes)
//                 {
//                     foreach (var gObjs in misfitObjects)
//                     {
//                         if (gObjs.name == attribute.value)
//                         {
//                             if (attribute.trait_type != "Pose")
//                             {
//                                 gObjs.SetActive(true);
//                                 mizfits.Add(attribute.value);
//                             }
//                         }
//                     }
//                 }
//
//                 string[] str = mizfits.ToArray();
//                 current = str;
//                 photonView.RPC("setMizFit", RpcTarget.Others, str);
//                 setJetpackParent(misfitJetpack);
//                 isMonke = false;
//                 return;
//             }
//         }
//     }
//
//     public string[] current;
//     public bool isMonke;
//     public Transform jetpack, misfitJetpack, monkeJetpack, pengoJetpack, doggoJetpack;
//
//     public void previousAvatar()
//     {
//         count--;
//         if (count < 0)
//         {
//             count = (avatars.monkes.Length + avatars.misfits.Length) - 1;
//         }
//
//         foreach (var gObjs in monkeObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         foreach (var gObjs in misfitObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         if (count == 0)
//         {
//             MisfitSwappah._BasicBehaviour.GetAnim.avatar = MisfitSwappah.Misfit;
//             misfitObjects[0].SetActive(true);
//             List<string> mizfits = new List<string>();
//             mizfits.Add(misfitObjects[0].name);
//             string[] str = mizfits.ToArray();
//             current = str;
//             photonView.RPC("setMizFit", RpcTarget.Others, str);
//             name.text = "Create your own!";
//             setJetpackParent(misfitJetpack);
//         }
//
//         if (avatars.monkes.Length > 0)
//         {
//             if (count < avatars.monkes.Length + 1 && count > 0)
//             {
//                 MisfitSwappah._BasicBehaviour.GetAnim.avatar = MisfitSwappah.Monke;
//                 name.text = avatars.monkes[count - 1].name;
//                 List<string> monkeBiz = new List<string>();
//                 foreach (var attribute in avatars.monkes[count - 1].attributes)
//                 {
//                     foreach (var gObjs in monkeObjects)
//                     {
//                         if (gObjs.name == attribute.value)
//                         {
//                             gObjs.SetActive(true);
//                             monkeBiz.Add(attribute.value);
//                         }
//                     }
//                 }
//
//                 string[] str = monkeBiz.ToArray();
//                 current = str;
//                 photonView.RPC("setMonke", RpcTarget.Others, str);
//                 isMonke = true;
//                 setJetpackParent(monkeJetpack);
//                 return;
//             }
//         }
//
//         if (avatars.misfits.Length > 0 && count >= avatars.monkes.Length + 1)
//         {
//             if ((count - avatars.monkes.Length + 1) < avatars.misfits.Length + 1)
//             {
//                 MisfitSwappah._BasicBehaviour.GetAnim.avatar = MisfitSwappah.Misfit;
//                 name.text = avatars.misfits[count - 1 - avatars.monkes.Length].name;
//                 List<string> mizfits = new List<string>();
//                 foreach (var attribute in avatars.misfits[count - 1 - avatars.monkes.Length].attributes)
//                 {
//                     foreach (var gObjs in misfitObjects)
//                     {
//                         if (gObjs.name == attribute.value)
//                         {
//                             if (attribute.trait_type != "Pose")
//                             {
//                                 gObjs.SetActive(true);
//                                 mizfits.Add(attribute.value);
//                             }
//                         }
//                     }
//                 }
//
//                 string[] str = mizfits.ToArray();
//                 current = str;
//                 photonView.RPC("setMizFit", RpcTarget.Others, str);
//                 setJetpackParent(misfitJetpack);
//                 isMonke = false;
//                 return;
//             }
//         }
//     }
//
//     [PunRPC]
//     void NFTAvatarState()
//     {
//         if (isMonke)
//         {
//             photonView.RPC("setMonke", RpcTarget.Others, current);
//             setJetpackParent(monkeJetpack);
//         }
//         else
//         {
//             photonView.RPC("SendCurrentState", RpcTarget.All);
//             setJetpackParent(misfitJetpack);
//         }
//     }
//
//     void SetActiveFalseAll()
//     {
//         foreach (var gObjs in doggoObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         foreach (var gObjs in misfitObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         foreach (var gObjs in pengoObjects)
//         {
//             gObjs.SetActive(false);
//         }
//
//         foreach (var gObjs in monkeObjects)
//         {
//             gObjs.SetActive(false);
//         }
//     }
//
//
// }