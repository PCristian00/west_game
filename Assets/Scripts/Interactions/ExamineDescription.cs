using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineDescription : MonoBehaviour, IExamine
{

    [SerializeField] private string title;
    [SerializeField] private string description;

    public void Examine()
    {
        DescriptionManager.Instance.ShowDescription(title, description);
    }
}
