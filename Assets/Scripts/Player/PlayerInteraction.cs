using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public float raycastDistance = 5f;

    private GameObject lastHitGameObject;
    private void Start()
    {
       // OnWeaponChanged(LoadoutManager.instance.CurrentWeapon);
    }

    private void Update()
    {
        LoadoutManager.instance.OnWeaponChanged += OnWeaponChanged;

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

    private void OnWeaponChanged(GameObject gun)
    {
        ProjectileGun currentWeapon = gun.GetComponent<ProjectileGun>();
        raycastDistance = currentWeapon.shootForce;

        // ATTENZIONE: QUESTA RIGA DI DEBUG FA CROLLARE GLI FPS
        // Debug.Log("Arma attuale: " + currentWeapon.name + "\nGittata (shoot Force): " + currentWeapon.shootForce + " impostata come distanza di interazione");
    }
}
