using Assets.Script.Core.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponEnum;

public class WeaponController : MonoBehaviour
{
    private BaseWeapon weaponStat;

    public BaseWeapon WeaponStat { get; set; }

    //public EWeaponState GetState() => weaponStat.WeaponState;

    public delegate void TaskCallBack(Component sender, object result);

    public void OnFire(TaskCallBack taskCallBack, GameObject weaponHand)
    {


        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet"));
        bullet.transform.position = weaponHand.transform.position;

        float x = Camera.main.pixelWidth / 1;
        float y = Camera.main.pixelHeight / 2;
        Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
        //Vector3 direction = center - transform.position;
        Vector3 direction = Vector3.right;
        // increase mass of bullet

        bullet.GetComponent<BulletController>().GetComponent<Rigidbody2D>().AddForce(direction * 1000);

        Destroy(bullet, 30f);


        Debug.Log("I have started new Task.");
        Debug.Log("ammo: " + WeaponStat.AmmoCurrent.ToString());
        if (taskCallBack != null)
            taskCallBack(this, "assas");
    }

}
