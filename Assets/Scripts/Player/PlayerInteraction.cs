using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public float raycastDistance = 5f;

    private GameObject lastHitGameObject;

    // Utilizzate per cambiare raycastDistance in base a gittata arma (OTTIMIZZARE)
    [SerializeField] private ProjectileGun currentWeapon;
    [SerializeField] private ProjectileGun prevWeapon;

    void Update()
    {
        ChangeRaycastDistance();

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));


        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance))
        {
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

                foreach (var interactable in lastHitGameObject.GetComponents<IHover>())
                {
                    interactable.HoverEnter();
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                foreach (var interactable in lastHitGameObject.GetComponents<IActivate>())
                {
                    interactable.Activate();
                }
            }
            else if (Input.GetButtonDown("Fire2"))
            {
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
                foreach (var interactable in lastHitGameObject.GetComponents<IHover>())
                {
                    interactable.HoverExit();
                }
                lastHitGameObject = null;
            }
        }
    }

    private void ChangeRaycastDistance()
    {
        // METTERE IN FUNZIONE CHE SI AGGIORNA SOLO A CAMBIO ARMA??? [VEDI FORSE CAMBIO STATI ESCAPE ROOM]
        // FUNZIONANTE MA POCO OTTIMIZZATO
        // FORSE INTERAGIRE CON LoadoutManager
        if (GameObject.FindGameObjectWithTag("Gun").TryGetComponent<ProjectileGun>(out currentWeapon))
        {

            if (currentWeapon != prevWeapon)
            {
                Debug.Log("Arma attuale: " + currentWeapon.name + "\nGittata (shoot Force): " + currentWeapon.shootForce + " impostata come distanza di interazione");
                raycastDistance = currentWeapon.shootForce;
                prevWeapon = currentWeapon;
            }
        }
    }
}
