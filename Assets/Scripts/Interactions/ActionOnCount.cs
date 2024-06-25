using UnityEngine;

public class ActionOnCount : MonoBehaviour
{
    public CountOnActivate countOnActivate;
    public int countLimit = 5;
    private bool accept = true;

    // Update is called once per frame
    void Update()
    {
        if (countOnActivate)
        {
            if (countOnActivate.GetCount() >= countLimit && accept)
            {
                accept = false;
                Action();
            }
        }
        else Debug.Log("nessun contatore collegato");
    }

    // Azione di test: Ingrandisce oggetto a cui è collegato
    private void Action()
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale += Vector3.one;
    }
}
