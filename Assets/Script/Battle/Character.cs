﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Character : MonoBehaviour
{
    public int characterNo; 
    public int maxHp;
    public int Hp;
    public int Atk;
    public int Armor;
    public int Act;
    public int turnAtk;
    public TurnManager TM;
    public BattleManager BM;
    public TextMeshProUGUI hpT;
    public TextMeshProUGUI atkT;
    public TextMeshProUGUI armorT;
    public TextMeshProUGUI actT;
    public TextMeshProUGUI board;
    public bool[] passive;
    public List<DMGboard> DMGboards=new List<DMGboard>();
    public string enemyName;
    public struct ArmorBreak
    {
        public int dmg;
        public string name;

    }
    public List<ArmorBreak> armorBreak = new List<ArmorBreak>();
    
    public class DMGboard
    {      
       public int dmg;
       public string name;
       public int count;
        public void countup()
        {
            count++;
        }
        public void setDmg(int dmg)
        {
            this.dmg = dmg;
        }
        public void setName(string name)
        {
            this.name = name;
        }

    }
    public int cost;
    public bool isSet;
    public bool isTurnStart;
    public bool isTurnEnd;
    public int hitStack;
    public int dmgStack;
    public int NextTurnMinusAct;
    public bool isDie;
    public int nextarmor;
    public bool card8;
    public int card8point;
    public string Name;
    public int reflect;
    int[] Status=new int[20];
    public void Acting()
    {
     
        if (Status[0] > 0)
        {
            onDamage(Status[0], "중독");
        }
    }
    public void StatusAbnom(int status,int count)
    {
        Status[status] += count;
    }
    // Start is called before the first frame update
    private void Update()
    {       
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject&&!isDie && !BM.SelectMode&&!BM.EnemySelectMode)
                {
                    if (BM.character != hit.collider.GetComponent<Character>()  )
                    {
                        BM.CharacterSelect(hit.collider.gameObject); }
                    else
                        BM.CancleCharacter();
                }
            }              
        }
    }
    public void LateUpdate()
    {
        hpT.text = Hp + "/" + maxHp;
        atkT.text = "Atk : " + turnAtk;
        armorT.text = "Armor : " + Armor;
        actT.text = "Act : " + Act;
    }
    private void Awake()
    {
        Act = 1;
        Hp = maxHp;
    }
    public void onDynamicHit(int dmg,string enemyname)
    {      
        for (int i = 0; i < DMGboards.Count; i++)
        {
            if (DMGboards[i].name == enemyname)
            {
               
                DMGboards[i].setDmg(dmg);
                break;
            }
        }
        board.text = "";
        for (int i = 0; i < DMGboards.Count; i++)
        {
            string newstring = "<sprite name=" + DMGboards[i].name + "><sprite name=dmg>" + DMGboards[i].dmg;
            if (DMGboards[i].count > 0) newstring += " x" + (DMGboards[i].count + 1);
            newstring += "\n";
            board.text += newstring;
        }
    }
    public void BoardClear()
    {
        board.text = "";     
        string newstring = "";
        if (Status[0] != 0)
            {
                newstring = "<sprite name=poison>" + Status[0] + "\n";
            }
        board.text += newstring;
        
    }

    public void onHit(int dmg,string enemyname)
    {        
        bool isThere = false;       
        for(int i = 0; i < DMGboards.Count; i++)
        {         
            if (DMGboards[i].dmg == dmg && DMGboards[i].name == enemyname)
            {            
                isThere = true;
                DMGboards[i].countup();
                break;
            }
        }
        if (!isThere)
        {
            DMGboard newBoard=new DMGboard();
            newBoard.setDmg(dmg);
            newBoard.setName(enemyname);
            DMGboards.Add(newBoard);          
        }
        board.text = "";
        for(int i = 0; i < DMGboards.Count; i++)
        {
            string newstring = "<sprite name=" + DMGboards[i].name + "><sprite name=dmg>" + DMGboards[i].dmg;
            if (DMGboards[i].count > 0) newstring += " x" + (DMGboards[i].count+1);
            newstring += "\n";
            board.text += newstring;
        }
       
            string astring = "";
            if (Status[0] != 0)
            {
              astring = "<sprite name=poison>"+ Status[0]+"\n";
            }
            board.text += astring;
        
    }
    public void onDamage(int dmg,string enemyname)
    {
        BM.log.logContent.text += "\n"+Name + "(가)이 " + enemyname + "에게 " + dmg + "의 피해를 입었다!";
        BM.Setting();
        if (reflect > 0)
        {
            for(int i = 0; i < BM.Enemys.Length; i++)
            {
                if (BM.Enemys[i].GetComponent<Enemy>().Name == enemyname)
                {
                    BM.Enemys[i].GetComponent<Enemy>().onHit(reflect);
                    BM.log.logContent.text+="\n"+Name+"에게 데미지가 주어져서 " + enemyname + "에게 " + reflect + "의 데미지!";
                }
            }
        }
        dmgStack++;
        enemyName = enemyname;
        if (Armor > 0)
        {
            ArmorBreak newA = new ArmorBreak();
            if (Armor > dmg)
            {
                newA.dmg = dmg/2;
            }
            else
            {
                newA.dmg = Armor / 2;
            }
            newA.name = enemyname;
            armorBreak.Add(newA);
           
            Armor -= dmg;
            if (Armor < 0)
            {
                Hp += Armor;
                Armor = 0;
            }
        }
        else
        {
            hitStack++;
            
            Hp -= dmg;
        }
        if (Hp <= 0)
        {
            if (!isDie)
            {
                Hp = 0;
                die();
            }
        }
    }
    void die()
    {
        isDie = true;
        Hp = 0;
        hpT.text = Hp + "/" + maxHp;
        Color color = new Color(0.3f, 0.3f, 0.3f);
        GetComponent<Image>().color = color;
        Act = 0;
        board.text = "";
        Armor = 0;
        BM.diecount++;
        if (BM.diecount == 4)
        { Time.timeScale = 0;
            BM.Defetead();
        }
        for(int i = 0; i < BM.forward.Count; i++)
        {
            if (BM.forward[i] == gameObject.GetComponent<Character>())
            {
                
                BM.forward.RemoveAt(i);
            }
        }
    }
}
