using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

namespace Network_Photon.BasicTutorial
{
    public class Tester : MonoBehaviour
    {

        [MyBox.ButtonMethod]
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        [MyBox.ButtonMethod]
        public void ConnectUsingSettings()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }


        [MyBox.ButtonMethod]
        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        }

        [MyBox.ButtonMethod]
        public void LoadLevel()
        {
            PhotonNetwork.LoadLevel("Room for 1");
        }

        [MyBox.ButtonMethod]
        public void IsMaster()
        {
            Debug.Log($"Is Master {PhotonNetwork.IsMasterClient}");
        }

        [MyBox.ButtonMethod]
        public void IsConnected()
        {
            Debug.Log($"Is Connected {PhotonNetwork.IsConnected}");
        }

        [MyBox.ButtonMethod]
        public void ConnectOnPremise()
        {
            // OnPremise ��� �� SetAuth�� ���� UserId�� �ʱ�ȭ�ؾ� JoinRoom�� �� �ִ�.
            PhotonNetwork.ConnectToMaster("127.0.0.1", 5055, "");
        }

    }
}