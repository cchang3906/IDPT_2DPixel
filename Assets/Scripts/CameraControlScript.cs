using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 mousePosition;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float shakeDuration;
    public bool testShake;
    private Vector3 playerPosition;
    private Vector3 clampedCameraPosition;
    private GameObject Player;
    private Vector3 goalPosition;
    private float cameraWidth;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        goalPosition = GameObject.FindGameObjectWithTag("Goal").transform.position;
        cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = Player.transform.position;
    }
    public void HandleCameraRotation(Vector2 lookInput)
    {
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane));
        Vector3 cameraPosition = new Vector3((playerPosition.x + mousePosition.x) / 2, (playerPosition.y + mousePosition.y) / 2, -10);
        float clampedX = Mathf.Clamp(cameraPosition.x, playerPosition.x - 5, playerPosition.x + 5);
        float clampedY = Mathf.Clamp(cameraPosition.y, playerPosition.y - 3, playerPosition.y + 3);
        clampedCameraPosition = new Vector3(clampedX, clampedY, -10);
        if (goalPosition.x >= playerPosition.x && clampedCameraPosition.x + (cameraWidth / 2) - 1.5f >= goalPosition.x)
        {
            clampedCameraPosition = new Vector3(goalPosition.x - (cameraWidth / 2) + 1.5f, clampedCameraPosition.y, clampedCameraPosition.z);
            //clampedCameraPosition = new Vector3(goalPosition.x, clampedY, -10);
        }
        else if (goalPosition.x <= playerPosition.x && clampedCameraPosition.x - (cameraWidth / 2) + 1.5f <= goalPosition.x)
        {
            clampedCameraPosition = new Vector3(goalPosition.x + (cameraWidth / 2) - 1.5f, clampedCameraPosition.y, clampedCameraPosition.z);
            //clampedCameraPosition = new Vector3(goalPosition.x, clampedY, -10);
        }
        else
        {
            clampedCameraPosition = new Vector3(clampedX, clampedY, -10);
        }
        transform.position = clampedCameraPosition;

        //Vector3 cameraPosition = new Vector3((transform.position.x + mousePosition.x) / 2, (transform.position.y + mousePosition.y) / 2, -10);
        //float clampedX = Mathf.Clamp(cameraPosition.x, transform.position.x - 5, transform.position.x + 5);
        //float clampedY = Mathf.Clamp(cameraPosition.y, transform.position.y - 3, transform.position.y + 3);
        //clampedCameraPosition = new Vector3(clampedX, clampedY, -10);
        //mainCamera.transform.position = clampedCameraPosition;
    }
    void OnDrawGizmos()
    {
        //if (clampedCameraPosition.x + (cameraWidth / 2) - 1.5f >= goalPosition.x)
        //{
        //    Gizmos.DrawCube(new Vector3(clampedCameraPosition.x + (cameraWidth / 2) - 2, clampedCameraPosition.y, clampedCameraPosition.z), new Vector3(1, 1, 1));
        //}
        //else
        //{
        //    Gizmos.DrawSphere(new Vector3(clampedCameraPosition.x + (cameraWidth / 2) - 2, clampedCameraPosition.y, clampedCameraPosition.z), 1);
        //}
    }

    public IEnumerator Shake()
    {
        float timer = 0f;
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float strength = curve.Evaluate(timer / shakeDuration);
            transform.position = clampedCameraPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
    }
}
