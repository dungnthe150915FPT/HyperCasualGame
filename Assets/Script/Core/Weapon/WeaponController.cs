using Assets.Script.Core.Library;
using Assets.Script.Core.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponEnum;

public class WeaponController : MonoBehaviour
{

    private BaseWeapon weaponStat;
    public BaseWeapon WeaponStat
    {
        get { return weaponStat; }
        set { weaponStat = value; }
    }

    private float timeJustFire = 0f;

    public delegate void TaskCallBack(Component sender, object result);
    public void setupWeapon(GameObject weaponHand)
    {
        bullet = Resources.Load<GameObject>(CONST.PREFAB_BULLET_PATH);
        muzzle = weaponHand.transform.Find(CONST.OBJECT_MUZZLE_EXTRACTOR).gameObject;
        shell = weaponHand.transform.Find(CONST.OBJECT_SHELL_EXTRACTOR).gameObject;

    }
    private GameObject bullet;
    private GameObject muzzle;
    private GameObject shell;
    public bool OnFire(TaskCallBack taskCallBack)
    {
        bool success = false;
        if ((timeJustFire > 0.1 || timeJustFire == 0) && weaponStat.AmmoCurrent > 0)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = muzzle.transform.position;
            Vector2 direction = muzzle.transform.position - shell.transform.position;
            newBullet.GetComponent<BulletController>().GetComponent<Rigidbody2D>().AddForce(direction * weaponStat.BulletSpeed);
            Destroy(newBullet, 2f);
            timeJustFire = 0f;
            setAmmo(1);
            callBackFireUpdateUI(taskCallBack);
            success = true;
        }
        timeJustFire += Time.fixedDeltaTime;
        return success;
    }
    private void setAmmo(int ammo)
    {
        weaponStat.AmmoCurrent -= ammo;
    }
    private void callBackFireUpdateUI(TaskCallBack taskCallBack)
    {
        if (taskCallBack != null) taskCallBack(this, weaponStat.AmmoCurrent);
    }
    internal void OnFireStop()
    {
        timeJustFire = 0f;
    }

    internal void OnReload(TaskCallBack taskCallBack, int ammoPool)
    {
        if (weaponStat.AmmoCurrent < weaponStat.AmmoMax)
        {
            int ammoClamp = Mathf.Clamp(ammoPool, 0, weaponStat.AmmoMax);
            int ammoNeed = weaponStat.AmmoMax - weaponStat.AmmoCurrent;
            int ammoReload = ammoClamp > ammoNeed ? ammoNeed : ammoClamp;
            weaponStat.AmmoCurrent += ammoReload;
            if (taskCallBack != null) taskCallBack(this, ammoNeed);
        }
    }
}
