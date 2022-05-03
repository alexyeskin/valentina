using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplaying : MonoBehaviour
{
    public GameObject floatingTextPrefub;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void showFloatingDamageText(int damage) {
        if (floatingTextPrefub) {
            if (floatingTextPrefub.TryGetComponent(out FloatingText floatingText)) {
                floatingText.text = damage.ToString();
            }
            Instantiate(floatingTextPrefub, transform.position, Quaternion.identity, transform);
        }
    }
}
