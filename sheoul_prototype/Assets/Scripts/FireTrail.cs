using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    private const float MAX_INTERACTION_DISTANCE = 3.0f;




    public bool isButton;
    private bool isLit;
    private bool isInList;


    private List<FireTrail> flamesToLightUp;

    private Collider flameTrigger;
    private GameObject fire;



    public CanvasManager canvasManager;
    public Transform cameraTransform;
    private FireTrail currentFire;



    private void Start()
    {
        flameTrigger = GetComponent<Collider>();
        if (!isButton) fire = GetComponent<ParticleSystem>()?.gameObject;

        if (isButton) flamesToLightUp = new List<FireTrail>();
        isLit = false;
        isInList = false;
    }

    private void Update()
    {
        CheckForFire();
        CheckForInteraction();
    }

    ///////////////////////////////////////////////////////////////////////////

    private void CheckForFire()
    {
        FireTrail newFire = null;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
                            out RaycastHit hitInfo,
                            MAX_INTERACTION_DISTANCE))
        {
            newFire = hitInfo.collider.GetComponent<FireTrail>();

            if (newFire != null && newFire != currentFire)
                SetCurrentInteractive(newFire);
            else if (newFire == null)
                ClearCurrentFire();
        }
        else
            ClearCurrentFire();
    }

    private void CheckForInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentFire != null)
        {
            if (currentFire.isButton)
                foreach (FireTrail fire in flamesToLightUp)
                {
                    fire.LighFlame();
                }
        }
    }

    private void SetCurrentInteractive(FireTrail newFire)
    {
        currentFire = newFire;

        if (!currentFire.isLit && currentFire.isButton)
        {
            canvasManager.ShowInteractionPanel("Press 'E' to light");
            Debug.Log("DID THIS");
        }

    }
    private void ClearCurrentFire()
    {
        if (currentFire != null)
        {
            Debug.Log("AND THIS");
            currentFire = null;
            canvasManager.HideInteractionPanel();

        }
    }

    ///////////////////////////////////////////////////////////////////////////

    private void LighFlame()
    {
        fire.SetActive(true);
        isLit = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        bool colIsFire = (col.GetComponent<FireTrail>() == null) ? false : true;

        if (colIsFire)
        {
            if (!isButton) GetComponentInParent<FireTrail>().flamesToLightUp.Add(col.GetComponent<FireTrail>());

            col.GetComponent<FireTrail>().isInList = true;
        }
    }
}
