using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerDownHandler
{
    public Transform cameraTransform;
    public AudioController audioController;
    public AudioClip pressSound;

    public void OnPointerDown(PointerEventData eventData)
    {
        audioController.PlaySound(pressSound, cameraTransform, 1.0f);
    }
}
