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

    private float timeJustFire = 0f;

    public delegate void TaskCallBack(Component sender, object result);
    private bool _isOneFire = false;
    private int maxBulletFirstTime = 1;
    private int currentBulletFirstTime = 0;


    public bool IsOneFire
    {
        get { return _isOneFire; }
        set { _isOneFire = value; }
    }
    public void setupWeapon(GameObject weaponHand)
    {
        bullet = Instantiate(Resources.Load<GameObject>(CONST.PREFAB_BULLET_PATH));
        muzzle = weaponHand.transform.Find(CONST.OBJECT_MUZZLE_EXTRACTOR).gameObject;
        shell = weaponHand.transform.Find(CONST.OBJECT_SHELL_EXTRACTOR).gameObject;
    }
    private GameObject bullet;
    private GameObject muzzle;
    private GameObject shell;
    public void OnFire(TaskCallBack taskCallBack, GameObject weaponHand)
    {
        //if ((timeJustFire > 0f && timeJustFire <= weaponStat.FireRate
        //    && currentBulletFirstTime <= maxBulletFirstTime))
        //{

        //    currentBulletFirstTime++;
        //    if (currentBulletFirstTime <= maxBulletFirstTime)
        //    {
        //        GameObject newBullet = Instantiate(bullet);
        //        newBullet.transform.position = muzzle.transform.position;
        //        Vector2 direction = muzzle.transform.position - shell.transform.position;
        //        newBullet.GetComponent<BulletController>().GetComponent<Rigidbody2D>().AddForce(direction * weaponStat.BulletSpeed);
        //        Destroy(newBullet, 2f);
        //        timeJustFire = 0f;
        //        // Call back to update UI
        //        if (taskCallBack != null)
        //            taskCallBack(this, "update UI");
        //        timeJustFire += Time.fixedDeltaTime;
        //        return;
        //    }
        //}

     
        if (timeJustFire > 0.1 || timeJustFire == 0)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = muzzle.transform.position;
            Vector2 direction = muzzle.transform.position - shell.transform.position;
            newBullet.GetComponent<BulletController>().GetComponent<Rigidbody2D>().AddForce(direction * weaponStat.BulletSpeed);
            Destroy(newBullet, 2f);
            timeJustFire = 0f;
            // Call back to update UI
            if (taskCallBack != null)
                taskCallBack(this, "update UI");
        }



        timeJustFire += Time.fixedDeltaTime;
        Debug.Log("OnFire: " + timeJustFire);
    }

    internal void OnFireStop()
    {
        currentBulletFirstTime = 0;
        timeJustFire = 0;
    }
}
