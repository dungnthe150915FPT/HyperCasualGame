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

    private float timeJustFire;

    public delegate void TaskCallBack(Component sender, object result);
    public void setupWeapon(GameObject weaponHand)
    {
        muzzle = weaponHand.transform.Find(CONST.OBJECT_MUZZLE_EXTRACTOR).gameObject;
        shell = weaponHand.transform.Find(CONST.OBJECT_SHELL_EXTRACTOR).gameObject;
        setupBullet();
    }

    private void setupBullet()
    {
        switch (weaponStat.AmmoType)
        {
            case EAmmoType.Circle:
                bullet = Resources.Load<GameObject>(CONST.PREFAB_CIRCLE_BULLET_PATH);
                break;
            case EAmmoType.Round:
                bullet = Resources.Load<GameObject>(CONST.PREFAB_ROUND_BULLET_PATH);
                break;
            case EAmmoType.Sharp:
                bullet = Resources.Load<GameObject>(CONST.PREFAB_SHARP_BULLET_PATH);
                break;
        }
        bullet.GetComponent<BulletController>().bulletDamage = weaponStat.AttackDamage;

        switch (weaponStat.WeaponType)
        {
            case EWeaponType.Rifle:
                audioClipFire = Resources.Load<AudioClip>(CONST.SOUND_RIFLE_FIRE_PATH);
                audioReload = Resources.Load<AudioClip>(CONST.SOUND_NORMAL_RELOAD_PATH);
                break;
            case EWeaponType.Shotgun:
                audioClipFire = Resources.Load<AudioClip>(CONST.SOUND_BIG_SHOT_PATH);
                audioReload = Resources.Load<AudioClip>(CONST.SOUND_NORMAL_RELOAD_PATH);
                break;
            case EWeaponType.Sniper:
                audioClipFire = Resources.Load<AudioClip>(CONST.SOUND_BIG_SHOT_PATH);
                audioReload = Resources.Load<AudioClip>(CONST.SOUNG_SNIPER_RELOAD_PATH);
                break;
            case EWeaponType.Pistol:
                audioClipFire = Resources.Load<AudioClip>(CONST.SOUND_RIFLE_FIRE_PATH);
                audioReload = Resources.Load<AudioClip>(CONST.SOUND_NORMAL_RELOAD_PATH);
                break;
            case EWeaponType.SMG:
                audioClipFire = Resources.Load<AudioClip>(CONST.SOUND_RIFLE_FIRE_PATH);
                audioReload = Resources.Load<AudioClip>(CONST.SOUND_NORMAL_RELOAD_PATH);
                break;
        }
    }

    private GameObject bullet;
    private GameObject muzzle;
    private GameObject shell;
    private AudioClip audioClipFire;
    private AudioClip audioReload;
    public bool OnFire(TaskCallBack taskCallBack)
    {
        bool success = false;
        if ((timeJustFire > 0.1 || timeJustFire == 0) && weaponStat.AmmoCurrent > 0)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = muzzle.transform.position;
            Vector2 direction = muzzle.transform.position - shell.transform.position;
            var angle = Vector2.SignedAngle(Vector2.right, direction.normalized) - 90f;
            newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            newBullet.GetComponent<BulletController>().GetComponent<Rigidbody2D>().AddForce(direction * weaponStat.BulletSpeed);
            Destroy(newBullet, 10f);
            timeJustFire = 0f;
            setAmmo(1);
            AudioSource.PlayClipAtPoint(audioClipFire, transform.position);
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
            // play audio reload, faster 2x time
            AudioSource.PlayClipAtPoint(audioReload, transform.position);
            int ammoClamp = Mathf.Clamp(ammoPool, 0, weaponStat.AmmoMax);
            int ammoNeed = weaponStat.AmmoMax - weaponStat.AmmoCurrent;
            int ammoReload = ammoClamp > ammoNeed ? ammoNeed : ammoClamp;
            weaponStat.AmmoCurrent += ammoReload;
            if (taskCallBack != null) taskCallBack(this, ammoNeed);
        }
    }
}
