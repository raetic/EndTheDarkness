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
    public List<int> DMG=new List<int>();
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
    // Start is called before the first frame update
    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject&&!isDie)
                {
                    if (BM.character != hit.collider.GetComponent<Character>()&&
                        hit.collider.GetComponent<Character>().Act>0)
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
    public void onHit(int dmg)
    {
        DMG.Add(dmg);
        board.text += "\ndmg:" + dmg;
    }
    public void onDamage(int dmg)
    {

        BM.Setting();
       
        if (Armor > 0)
        {
            dmgStack++;
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
            die();
        }
    }
    void die()
    {
        Hp = 0;
        isDie = true;
        Color color = new Color(0.3f, 0.3f, 0.3f);
        GetComponent<SpriteRenderer>().color = color;
        Act = 0;
        board.text = "";
        Armor = 0;
       BM.diecount++;
        if (BM.diecount == 4)
            Time.timeScale = 0;
        for(int i = 0; i < BM.forward.Count; i++)
        {
            if (BM.forward[i] == gameObject.GetComponent<Character>())
            {
                
                BM.forward.RemoveAt(i);
            }
        }
    }
}
