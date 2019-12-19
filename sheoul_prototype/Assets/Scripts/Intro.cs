using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    private const float MAX_INTERACTION_DISTANCE = 3.0f;

    [SerializeField]private GameObject introTorch;
    [SerializeField]private GameObject playerTorch;

    [SerializeField]private CanvasManager canvasManager;

    private Transform cameraTransform;
    private string currentString;

    public bool introFinished;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        canvasManager.HideInventoryPanel();
        currentString = "";
        introFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!introFinished)
        {
            CheckForIntroTorch();

            if (Input.GetKeyDown(KeyCode.E) && currentString == "IntroTorch")
            {
                Destroy(introTorch);
                playerTorch.SetActive(true);
                canvasManager.ShowInventoryPanel();
                canvasManager.HideInteractionPanel();
                introFinished = true;
            }
        }
    }

    private void CheckForIntroTorch()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
                            out RaycastHit hitInfo,
                            MAX_INTERACTION_DISTANCE))
        {
            string newString =
                hitInfo.collider.gameObject.name;

            if (newString != null && newString != currentString)
                currentString = newString;

            if (currentString == "IntroTorch") canvasManager.ShowInteractionPanel("Press 'E' to pick up torch");
            else canvasManager.HideInteractionPanel();
        }
    }
}
