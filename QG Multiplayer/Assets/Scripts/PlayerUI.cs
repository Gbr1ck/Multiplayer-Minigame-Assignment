using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Player target;
    float characterControllerHeight = 0f;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup currentCanvasGroup;
    Vector3 targetPosition;

    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    void Awake(){
        currentCanvasGroup = this.GetComponent<CanvasGroup>();
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Update(){
        if (target == null){
            Destroy(this.gameObject);
            return;
        }
    }

    void LateUpdate(){
        if (targetRenderer != null){
            this.currentCanvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
        }

        if (targetTransform != null){
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            //this.transform.position = thisCamera.WorldToScreenPoint (targetPosition) + screenOffset;
        }
    }

    public void SetTarget(Player currentTarget){
        if (currentTarget == null){
            return;
        }
        target = currentTarget;
        if (playerNameText != null){
            playerNameText.text = target.photonView.Owner.NickName;
        }
        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponent<Renderer>();
        CharacterController characterController = currentTarget.GetComponent<CharacterController>();

        if (characterController != null){
            characterControllerHeight = characterController.height;
        }
    }
}
