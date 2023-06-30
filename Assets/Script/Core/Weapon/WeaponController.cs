using Assets.Script.Core.Library;
using Assets.Script.Core.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponEnum;

public class WeaponController : MonoBehaviour
{
    // Singleton pattern
    //private static WeaponController instance;
    //public static WeaponController Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = new WeaponController();
    //        }
    //        return instance;
    //    }
    //}


    private BaseWeapon weaponStat;
    // getter and setter for weaponStat
    public BaseWeapon WeaponStat
    {
        get { return weaponStat; }
        set { weaponStat = value; }
    }
    //public EWeaponState GetState() => weaponStat.WeaponState;
    public delegate void TaskCallBack(Component sender, object result);
    public void OnFire(TaskCallBack taskCallBack, GameObject weaponHand)
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>(CONST.PREFAB_BULLET_PATH));
        GameObject muzzle = weaponHand.transform.Find(CONST.OBJECT_MUZZLE_EXTRACTOR).gameObject;
        GameObject shell = weaponHand.transform.Find(CONST.OBJECT_SHELL_EXTRACTOR).gameObject;
        bullet.transform.position = muzzle.transform.position;































        // get direction from shell extractor to muzzle extractor
        Vector2 direction = muzzle.transform.position - shell.transform.position;

        bullet.GetComponent<BulletController>().GetComponent<Rigidbody2D>().AddForce(direction * weaponStat.BulletSpeed);


        Destroy(bullet, 30f);

        if (taskCallBack != null)
            taskCallBack(this, "assas");
    }
}
