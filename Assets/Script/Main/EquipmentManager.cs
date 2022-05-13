﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private static EquipmentManager instance;

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static EquipmentManager Instance
    {
        get
        {
            if (null == instance)
            {
                //게임 인스턴스가 없다면 하나 생성해서 넣어준다.
                instance = new EquipmentManager();
            }
            return instance;
        }
    }
    public equipment makeEquipment()
    {
        int rand1 = Random.Range(0, 5);
        int rand2 = Random.Range(0, 4);
        while (rand1 == rand2) rand2 = Random.Range(0, 4);
        string[] prefix = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        string[] equipName = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        int degradeMount = 0;
        int improveMount = 0;
        switch (rand1)
        {
            case (int)Enums.EquipmentStat.atk:
                improveMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.def:
                improveMount = Random.Range(1, 4);
                break;
            case (int)Enums.EquipmentStat.maxHp:
                improveMount = Random.Range(1, 5) * 5;
                break;
            case (int)Enums.EquipmentStat.cost:
                improveMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.act:
                improveMount = 1;
                break;
        }
        switch (rand2)
        {
            case (int)Enums.EquipmentStat.atk:
                degradeMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.def:
                degradeMount = Random.Range(1, 4);
                break;
            case (int)Enums.EquipmentStat.maxHp:
                degradeMount = Random.Range(2, 5) * 5;
                break;
            case (int)Enums.EquipmentStat.cost:
                degradeMount = 1;
                break;
            
        }
        int t = Random.Range(1, 101);
        if (t <= 45)
        {
            t = 0;
        }
        else if (t <= 90) t = 1;
        else t = 2;
        int randPrefix = Random.Range(0, 10);
        int randEquip = Random.Range(0, 10);
        List<int> l1 = new List<int>();
        l1.Add(rand1);
        List<int> l2 = new List<int>();
        l2.Add(improveMount);
        equipment e = new equipment(t, prefix[randPrefix]+" "+equipName[randEquip],l1,l2, rand2, degradeMount);
        return e;
    }
    public List<string> equipmentStrings(equipment e)
    {
        List<string> names= new List<string>();
        string equipName = e.equipName;
        names.Add(equipName);
        string s = "착용 조건: ";
        switch (e.type)
        {
            case 0:s += "전방";
                break;
            case 1:s += "후방";
                break;
            case 2:s += "전체";
                break;
        }
        names.Add(s);
        s = "증가 스탯:";
        for (int i = 0; i < e.improveStat.Count; i++)
        {
            if (i > 0) s += "\n";
            switch (e.improveStat[i])
            {
                case (int)Enums.EquipmentStat.atk:
                    s += " 공격력(" + e.improveMount[i] + ")";
                    break;
                case (int)Enums.EquipmentStat.def:
                    s += " 지구력(" + e.improveMount[i] + ")";
                    break;
                case (int)Enums.EquipmentStat.maxHp:
                    s += " 최대 HP(" + e.improveMount[i] + ")";
                    break;
                case (int)Enums.EquipmentStat.cost:
                    s += " 부여 코스트(" + e.improveMount[i] + ")";
                    break;
                case (int)Enums.EquipmentStat.act:
                    s += " 행동력(" + e.improveMount[i] + ")";
                    break;
            }
        }
        names.Add(s);
        s = "감소 스탯:";
        switch (e.degradeStat)
        {
            case (int)Enums.EquipmentStat.atk:
                s += " 공격력(" + e.degradeMount + ")";
                break;
            case (int)Enums.EquipmentStat.def:
                s += " 지구력(" + e.degradeMount + ")";
                break;
            case (int)Enums.EquipmentStat.maxHp:
                s += " 최대 HP(" + e.degradeMount + ")";
                break;
            case (int)Enums.EquipmentStat.cost:
                s += " 부여 코스트(" + e.degradeMount + ")";
                break;
          
        }
        names.Add(s);
        return names;
    }
}
