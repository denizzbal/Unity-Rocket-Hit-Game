using System.Collections;
using UnityEngine;

public class ExplosionTime : MonoBehaviour
{
    IEnumerator TimeDisable()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(TimeDisable());
    }


}
