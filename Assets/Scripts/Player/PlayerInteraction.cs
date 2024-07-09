using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public float raycastDistance = 5f;

    private GameObject lastHitGameObject;

    public bool hasWeapons = true;

    [Header("Keybinds")]
    public KeyCode activateKey = KeyCode.C;
    public KeyCode examineKey = KeyCode.V;

    private void SetCursorLocked(bool locked)
    {
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void Update()
    {
        if (hasWeapons)
        {
            LoadoutManager.instance.OnWeaponChanged += OnWeaponChanged;
        }

        SetCursorLocked(GameManager.instance.IsInputActive);

        if (!GameManager.instance.IsInputActive) return;

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

            if (Input.GetKeyDown(activateKey))
            {
                foreach (var interactable in lastHitGameObject.GetComponents<IActivate>())
                {
                    interactable.Activate();
                }
            }
            else if (Input.GetKeyDown(examineKey))
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
    }
}
