using Assets.Script.Core.Weapon;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponEnum;

[Serializable]
public class Inventory
{
    private List<BaseWeapon> weapons;
    private Dictionary<EAmmoType, int> ammoPool;
    private Dictionary<EAmmoType, int> ammoCurrent;
    private int numOfWeaponSlot = 3;
    //void Start()
    //{
    //    weapons = SaveGame.Load<BaseWeapon[]>("WeaponConfig", new BaseWeapon[0], new SaveGameJsonSerializer());
    //}

    //void Update()
    //{

    //}

    public List<BaseWeapon> Weapons { get => weapons; set => weapons = value; }

    public Inventory()
    {
        weapons = new List<BaseWeapon>();
        ammoPool = new Dictionary<EAmmoType, int>();
        ammoCurrent = new Dictionary<EAmmoType, int>();
    }

    // Singleton pattern
    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Inventory();
            }
            return instance;
        }
    }

    public BaseWeapon getWeapon(int index)
    {
        // return clamp index of weapons
        //         return index < weapons.Length ? weapons[index] : null;
        return weapons[Mathf.Clamp(index, 0, weapons.Count - 1)];
    }

    public int addWeapon(BaseWeapon baseWeapon)
    {
        weapons.Add(baseWeapon);

        // return index of weapon just added
        return weapons.Count - 1;
        //return baseWeapon;
    }

    public void removeWeapon(int index)
    {
        weapons.RemoveAt(index);
    }

    // get length of weapons
    public int getWeaponLength()
    {
        return weapons.Count;
    }

    internal void setWeaponState(int index, EWeaponState equipping)
    {
        weapons[index].WeaponState = equipping;
    }
    public int getAmmoPool(EAmmoType ammoType)
    {
        return ammoPool[ammoType];
    }
    public void setAmmoPool(EAmmoType ammoType, int value)
    {
        ammoPool[ammoType] = value;
    }
    public int getAmmoCurrent(EAmmoType ammoType)
    {
        return ammoCurrent[ammoType];
    }
    public void setAmmoCurrent(EAmmoType ammoType, int value)
    {
        ammoCurrent[ammoType] = value;
    }

    public void addAmmoPool(EAmmoType ammoType, int value)
    {
        ammoPool[ammoType] += value;
    }

    public void addAmmoCurrent(EAmmoType ammoType, int value)
    {
        ammoCurrent[ammoType] += value;
    }

    public int getNumOfWeaponSlot()
    {
        return numOfWeaponSlot;
    }

    public void setNumOfWeaponSlot(int value)
    {
        numOfWeaponSlot = value;
    }
}
