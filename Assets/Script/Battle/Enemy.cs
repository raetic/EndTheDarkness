﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public int Hp;
    public int maxHp;
    public int Armor;
    public int Atk;
    public TurnManager TM;
    public BattleManager BM;
    [SerializeField] TextMeshProUGUI HpT;
    public TextMeshProUGUI Board;
    [SerializeField] TextMeshProUGUI ArmorT;
    public int hitStack;
    public int dmgStack;
    public int nextTurnArmor;
    public bool isDie;
    public bool noDie;
    public bool power;
    public bool immortal;
    public string Name;
    int RecoverHp;
    public bool Shadow;
    int EndTurnDmg;
    EnemyInfo ei;
    public Slider hpSlider;
    public Image myImage;
    ActManager AM;
    public int myNo;
    public bool goingShadow;
    
    public bool CanShadow()
    {
        bool can = false;
        for(int i = 0; i < BM.Enemys.Length; i++)
        {
            if (BM.Enemys[i] == gameObject)
                continue;
            if (!BM.Enemys[i].GetComponent<Enemy>().isDie && !BM.Enemys[i].GetComponent<Enemy>().goingShadow)
            {
                can = true;
            }
        }
        return can;
    }
    private void Awake()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        Hp = maxHp;
        ei = GameObject.Find("SelectEnemyInformation").GetComponent<EnemyInfo>();
        AM = GameObject.Find("ActManager").GetComponent<ActManager>();
        
    }        
    public void onClickEvent()
{
    if (isDie) return;
    if (BM.SelectMode) return;
        if (Shadow) return;
    if (BM.EnemySelectMode)
    {
       
        BM.EnemySelect(gameObject);
    
    }
   
       

    
}
    public void onEnterEvnent()
    {

        if (isDie) return;
            ei.setThis(this);
    }
    public void onExitEvent()
    {
        if (isDie) {
            ei.g.SetActive(false);
            return; }
        ei.setNull();
    }
    public void EnemyStartTurn()
    {
        if (Shadow&&!isDie)
        {
            Shadow = false;
            myImage.color = new Color(1, 1, 1, 1);
        }
        power = false;
        immortal = false;
    }
    public void EnemyEndTurn()
    {             
        AM.EarlyAct();
    }
    void TurnStart()
    {
        TM.PlayerTurnStart();
    }
    float curalpha;
    public void Hit()
    {
        curalpha = myImage.color.a;
        Color color = new Color(1, 0, 0,curalpha);
        myImage.color = color;
    }
    public void HitEnd()
    {
        Color color = new Color(1, 1, 1, curalpha);
        if (isDie)
        {
          color = new Color(0.3f, 0.3f, 0.3f);
        }
        myImage.color = color;
    }
    
    public void onHit(int dmg,int no,bool reply)
    {
        GameObject Dmg = Instantiate(BM.DmgPrefebs, transform);
        Dmg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Dmg.GetComponent<DMGtext>().GetType(0, dmg);
        BM.characters[no].myPassive.MyAttack();
        for (int i = 0; i < BM.CD.size; i++)
        {
            if (!reply)
            {
                BM.characters[i].myPassive.EnemyHit(this);
            }
            else
            {
                BM.characters[i].myPassive.EnemyHitByReply(this);
            }
        }
      
        if (!power)
        {        
            
            if (Armor > 0)
            {
                Armor -= dmg;
                if (Armor < 0)
                {
                    Hp += Armor;
                    Armor = 0;
                }
            }
            else
            {
                if (!power)
                {
                    hitStack++;
                    Hp -= dmg;
                    if (Hp > maxHp)
                        Hp = maxHp;
                }
            }
            if (Hp <= 0)
            {
                if (immortal)
                {
                    Hp = 1;
                }
                else
                {
                    isDie = true;
                    bool V = true;
                    GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < e.Length; i++)
                    {
                        if (!e[i].GetComponent<Enemy>().isDie)
                        {
                            V = false;
                        }
                    }
                    die();
                    if (V&&!BM.isV) BM.Victory();
                    Hp = 0;
                    Color color = new Color(0.3f, 0.3f, 0.3f);
                    myImage.color = color;
                    hpSlider.transform.Find("Fill Area").gameObject.SetActive(false);
                }
            }
        }
        hpSlider.value= Hp / (float)maxHp;
       
    }

    public void GetArmorStat(int arm)
    {
        Armor += arm;
    }


    public void GetArmor(int arm,string enemyname)
    {
        
        nextTurnArmor += arm;
        string newstring = "<sprite name="+enemyname+"><sprite name=armor>" + arm + "\n";
        Board.text += newstring;
    }
    List<int> HpI = new List<int>();
    List<string> HpS = new List<string>();


    public void GetHp(int amount)
    {



        Hp += amount;
        if (Hp > maxHp) Hp = maxHp;
        hpSlider.value = Hp / (float)maxHp;

    }



    public void GetDynamicHp(int amount,string enemyname)
    {
        
        string newstring = Board.text;
        bool isThere = false;
        for(int i = 0; i < HpS.Count; i++)
        {
            if (HpS[i] == enemyname)
            {
                isThere = true;
                int curR = HpI[i];
                if (amount > 0)
                {
                    newstring = newstring.Replace("<sprite name=" + enemyname + "><sprite name=recover>" + curR + "\n"
                    , "<sprite name=" + enemyname + "><sprite name=recover>" + amount + "\n");
                    HpS.RemoveAt(i);
                    HpI.RemoveAt(i);
                    HpS.Add(enemyname);
                    HpI.Add(amount);
                }
                else
                {
                    newstring = newstring.Replace("<sprite name=" + enemyname + "><sprite name=recover>" + curR + "\n"
                  , "");
                    HpS.RemoveAt(i);
                    HpI.RemoveAt(i);
                }
                RecoverHp -= curR;
                RecoverHp += amount;            
                break;
            }
        }
        if (!isThere && amount !=0)
        {
            RecoverHp += amount;
            HpS.Add(enemyname);
            HpI.Add(amount);
            newstring = "<sprite name=" + enemyname + "><sprite name=recover>" + amount + "\n";
        }
        Board.text = newstring;
    }


    public void onShadow()
    {
        Shadow = true;      
    }


    public void onEnemyHit(int dmg, string Name)
    {
        EndTurnDmg += dmg;
        
        string newstring = "<sprite name=" + Name+ "><sprite name=dmg>" + dmg + "\n";
        Board.text += newstring;
    }


    public void HpUp()
    {
        HpI.Clear();
        HpS.Clear();
        Hp -= EndTurnDmg;
        EndTurnDmg = 0;
        Hp += RecoverHp;
        RecoverHp = 0;
        if (Hp >= maxHp)
            Hp = maxHp;
        hpSlider.value = Hp / (float)maxHp;
    }
    public void die()
    {
        Hp = 0;
        
    }
}

