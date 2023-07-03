using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    private BaseMagic baseMagic;
    public BaseMagic BaseMagic
    {
        set { baseMagic = value; }
        get { return baseMagic; }
    }
}
