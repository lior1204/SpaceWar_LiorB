using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Cube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(10, 3).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InCirc);
        transform.DORotate(new Vector3(180, 270,360), 4, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        transform.DOScale(transform.localScale*2, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBounce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
