using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        anim.SetBool("TextFadeIn", false);
        anim.SetBool("TextFadeOut", true);
    }
}
