using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SectionTrigger : MonoBehaviour
{
    public GameObject roadSection;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger")){
            Instantiate(roadSection, new Vector3(3.405854f, 1.868671f, 190.8f), Quaternion.identity);
        }
    }
}

