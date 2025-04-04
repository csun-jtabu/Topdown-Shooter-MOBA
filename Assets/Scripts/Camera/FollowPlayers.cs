using System.Numerics;
using UnityEngine;

public class FollowPlayers : MonoBehaviour
{
    [SerializeField]
    private float _followSpeed = 2f; // this is the speed the camera will follow
    //[SerializeField]
    //public Transform singleTargetPlayer; // this is what we'll be tracking
    //public Transform targetMultiplayer1; // this is what we'll be tracking
    //public Transform targetMultiplayer2; // this is also what we'll be tracking

    [SerializeField] public bool multiplayer; // boolean stating whether it is singleplayer or multiplayer.

    public Camera m_OrthographicCamera;


    // Source: https://discussions.unity.com/t/2d-camera-to-follow-two-players/100593/3
    void SetCameraSize() {
        // multiplying by 0.5, because the ortographicSize is actually half the height
        float ortographicSizeWidth = Mathf.Abs(GameObject.FindGameObjectsWithTag("MultiPlayerOne")[0].transform.position.x - GameObject.FindGameObjectsWithTag("MultiPlayerTwo")[0].transform.position.x) * 0.5f;
        float ortographicSizeHeight = Mathf.Abs(GameObject.FindGameObjectsWithTag("MultiPlayerOne")[0].transform.position.y - GameObject.FindGameObjectsWithTag("MultiPlayerTwo")[0].transform.position.y) * 0.5f;

        // computing the size
        m_OrthographicCamera.orthographicSize = Mathf.Max(ortographicSizeHeight, ortographicSizeWidth);
    }



    // Update is called once per frame
    void Update()
    {
        if (multiplayer) {
            // calculate the midpoint between the two players
            //UnityEngine.Vector3 midPoint = (targetMultiplayer1.transform.position + targetMultiplayer2.transform.position) / 2f;
            UnityEngine.Vector3 midPoint = (GameObject.FindGameObjectsWithTag("MultiPlayerOne")[0].transform.position + GameObject.FindGameObjectsWithTag("MultiPlayerTwo")[0].transform.position) / 2f;

            // basically gets the player's position
            UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(midPoint.x, midPoint.y, -10);
            
            // basically moves the camera to the player
            transform.position = newPosition;

            SetCameraSize();
        } else {
            // basically gets the player's position
            //UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(singleTargetPlayer.position.x, singleTargetPlayer.position.y, -10);
            UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(GameObject.FindGameObjectsWithTag("SinglePlayer")[0].transform.position.x, GameObject.FindGameObjectsWithTag("SinglePlayer")[0].transform.position.y, -10);

            // basically moves the camera to the player
            transform.position = newPosition;
        }
    }
}
