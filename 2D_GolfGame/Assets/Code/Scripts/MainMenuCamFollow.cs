using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset; //offset for the camera
    [SerializeField] private float damping; //how fast camera will catch up to player

    public Transform target; //public if camera wants to target many things
    private Vector3 vel = Vector3.zero;

    private void FixedUpdate() {
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping); //smoothdamp
    }
}
