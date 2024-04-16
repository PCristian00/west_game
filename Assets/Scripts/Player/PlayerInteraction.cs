using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public float raycastDistance = 5f;

    private GameObject lastHitGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));


        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance))
        {
            //Interactable hitInteractable = hitInfo.collider.GetComponent<Interactable>();
            GameObject hitGameObject = hitInfo.collider.gameObject;
            if (hitGameObject != lastHitGameObject)
            {
                if (lastHitGameObject != null)
                {
                    foreach (var interactable in lastHitGameObject.GetComponents<IHover>())
                    {
                        interactable.HoverExit();
                    }
                }

                lastHitGameObject = hitGameObject;

                //hitInteractable.HoverEnter();
                foreach (var interactable in lastHitGameObject.GetComponents<IHover>())
                {
                    interactable.HoverEnter();
                }


            }

            //if(hitInteractable==null) return;

            if (Input.GetButtonDown("Fire1"))
            {
                //hitInteractable.Activate();
                foreach (var interactable in lastHitGameObject.GetComponents<IActivate>())
                {
                    interactable.Activate();
                }
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                //hitInteractable.Examine();
                foreach (var interactable in lastHitGameObject.GetComponents<IExamine>())
                {
                    interactable.Examine();
                }
            }
        }
        else
        {
            if (lastHitGameObject != null)
            {
                //lastHitGameObject.HoverExit();
                foreach (var interactable in lastHitGameObject.GetComponents<IHover>())
                {
                    interactable.HoverExit();
                }
                lastHitGameObject = null;
            }
        }
    }
}
