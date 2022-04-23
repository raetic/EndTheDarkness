﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterPassive : MonoBehaviour
{
    BattleManager BM;
    CardManager CM;
    ActManager AM;
    int myNo;
    int[] myPassvie = new int[4];
    Character myCharacter;
    public int ghost = 49;//큐 전용 변수
    bool isKing;//큐 전용 변수
    [SerializeField] TextMeshProUGUI ghostText;//큐 전용 변수
    [SerializeField] Sprite upQ;//큐 전용 변수
    int turnGhost;//큐 전용 변수
   public GameObject DMGtext;


    bool isAct1;//포르테 전용 변수


    int ReplyCardDrow;//리플리 전용 변수
  public bool ReplyAttackDone;//리플리 전용 변수
    bool ReplyIsUp;//리플리 전용 변수
    int ReplyAttackCount;


    


    private void Start()
    {
        myNo = GetComponent<Character>().characterNo;
        myPassvie = GetComponent<Character>().passive;
        myCharacter = GetComponent<Character>();
        BM = myCharacter.BM;
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        AM = GameObject.Find("ActManager").GetComponent<ActManager>();
    }


    public void MyHit(Enemy e,int dmg)
    {
        if (myCharacter.isDie) return;
        GameObject dmgText = Instantiate(DMGtext,gameObject.transform);

        dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        dmgText.GetComponent<DMGtext>().GetType(0, dmg);



        if (myCharacter.reflect > 0)
        {
            
        }
     
        
    }


    IEnumerator MyHitCor(Enemy e)
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;

        if (myCharacter.reflect > 0)
        {

            transform.localScale *= 1.2f;
            if (!e.isDie)
            {
                BM.OnAttack(myCharacter.reflect, e, myCharacter, 1);
                BM.log.logContent.text += "" + e.Name + "에게 반사데미지" + myCharacter.reflect + "이 주어집니다.";
                BM.curMessage.text += "" + e.Name + "에게 반사데미지" + myCharacter.reflect + "이 주어집니다.";
            }

            yield return new WaitForSeconds(0.7f);
            transform.localScale /= 1.2f;
            BM.curMessage.text = "";
            yield return new WaitForSeconds(0.3f);
        }




        BM.otherCor = false;
        yield return null;
    }


    public void MyAttack()
    {
        if (myCharacter.isDie) return;
        if (myNo == 2 && myPassvie[3] > 0 && !ReplyIsUp)
        {

            ReplyAttackCount++;
            if (ReplyAttackCount > 20)
            {
                AM.MakeAct(8, 0, null, myCharacter, null, myPassvie[3]);
            }
        }
       
    }
    public void Sparky4()
    {
        if (myCharacter.isDie) return;
        myCharacter.RealAtkUp(2);
    }
    public void SpecialDrow(int drow)
    {
        if (myCharacter.isDie) return;
        if (myNo == 1 && myPassvie[2] > 0 && !isKing)
        {
            AM.MakeAct(3, drow, null, myCharacter, null, myPassvie[2]); //흑백       
        }
        if (myNo == 4 && myPassvie[1] > 0 && !isAct1)
        {
            AM.MakeAct(14, 0, null, myCharacter, null, myPassvie[1]); //미라클 드로우

        }
       
    }
    public void Q3(int drow)
    {
        if (myCharacter.isDie) return;
        int d = 0;
        while (d != drow)
        {
       
            d++;
            GameObject newCard = CM.field[CM.field.Count -  d];
            if (newCard.GetComponent<BlackWhite>() == null)
            {
                newCard.AddComponent<BlackWhite>();
                newCard.GetComponent<BlackWhite>().birth();
            }
            else
            {
                newCard.GetComponent<BlackWhite>().PlusStack();
            }
        }
    }
    public void Porte2()
    {
        if (myCharacter.isDie) return;
        isAct1 = true;
        BM.costUp(1);
        myCharacter.ActUp(1);
    }

    public void TurnStart()
    {
        if (myCharacter.isDie) return;
        if (myNo == 1 && isKing)
        {
            myCharacter.ActUp(myPassvie[0]);
        }
        if (myNo == 1 && myPassvie[1] > 0)
        {
            turnGhost = 0;
        }
        if (myNo == 3 && myPassvie[1] > 0)
        {
            AM.MakeAct(10, 0, null, myCharacter, null, myPassvie[1]); //선봉의 호령
       
        }
        if (myNo == 3 && myPassvie[3] > 0)
        {
            if (BM.back.Count == 1 && BM.back[0] == myCharacter) {
                AM.MakeAct(12, 0, null, myCharacter, null, myPassvie[3]);
           
            }
            if (BM.forward.Count == 1 && BM.forward[0] == myCharacter) {
                AM.MakeAct(12, 1, null, myCharacter, null, myPassvie[3]);
               
            }
                    
        }
        if (myNo == 4 && myPassvie[1] > 0)
        {
            isAct1 = false;
        }
        if (myNo == 4 && myPassvie[2] > 0)
        {
            BM.porte3count = myCharacter.passive[2];
            BM.porte3();
        }
        if (myNo == 4 && myPassvie[3] > 0)
        {
            AM.MakeAct(16, 0, null, myCharacter, null, myPassvie[3]);
        }
        if (myNo == 2 && myPassvie[0] > 0) ReplyCardDrow = 0;
    } //개편완


    public void Vangara2(int type)
    {
        if (myCharacter.isDie) return;
        if (type ==0)
        {
            BM.costUp(1);          
        }
        else
        {
            myCharacter.getArmor(3);          
        }
    }
    public void Vangara4()
    {
        if (myCharacter.isDie) return;
        myCharacter.getArmor(7);
        myCharacter.ActUp(1);
    }
    public void Porte4()
    {
        if (myCharacter.isDie) return;
        if (CM.Deck.Count > CM.Grave.Count)
        {
            
            CM.DeckToGrave(CM.Deck[Random.Range(0, CM.Deck.Count)]);
        }
        else if (CM.Deck.Count < CM.Grave.Count)
        {
            CM.GraveToDeck(CM.Grave[Random.Range(0, CM.Grave.Count)]);
        }

    }
  

    public void TurnEndTimeCount() //개편완
    {

        if (myCharacter.isDie) return;
        if (myNo== 1 && myPassvie[0] > 0&&!isKing&&ghost>50)
        {
            AM.MakeAct(1, 0, null, myCharacter, null, myPassvie[0]);
            //백옥의 왕         
        }
        if (myNo == 4 && myPassvie[0] > 0&& CM.field.Count > 3)
        {
            AM.MakeAct(13, 0, null, myCharacter, null, myPassvie[0]); //창조의 잠재력
        }
        
        
    }
  
    public void Q1()
    {
        if (myCharacter.isDie) return;
        myCharacter.maxHp = 100;//진화 후 체력은?
        myCharacter.hpT.text = myCharacter.Hp + "/" + myCharacter.maxHp;
            CM.PlusCard(13);
            CM.PlusCard(13);
            CM.PlusCard(14);
            CM.PlusCard(14);
        if (!isKing)
        {
            myCharacter.myImage.sprite = upQ; 
            
            isKing = true;
            myCharacter.Atk += 2;
            myCharacter.AtkUp(2);
        }
        BM.startCost++;
    }
    public void Porte1()
    {
        if (myCharacter.isDie) return;
        BM.TurnCardCount += 2;
    }


    
    
    public void EnemyHit(Enemy e)
    {
        if (myCharacter.isDie) return;
        if (myNo == 2 && myPassvie[1] > 0)
        { 
           
                AM.MakeAct(6, myCharacter.turnAtk, e, myCharacter, null, myPassvie[1]);
                    
        }
        if (myNo == 3 && myPassvie[0] > 0)
        {
            AM.MakeAct(9, 0, null, myCharacter, null, myPassvie[0]);
        }  
       
       
    }
    public void EnemyHitByReply(Enemy e)
    {
        if (myCharacter.isDie) return;
        if (myNo == 3 && myPassvie[0] > 0)
        {
            AM.MakeAct(9, 0, null, myCharacter, null, myPassvie[0]);
        }


    }


    public void Sparky2(int dmg,Enemy e)
    {
        if (myCharacter.isDie) return;
        e.onHit(dmg, myCharacter.curNo,true);   
    }


   

    public void TeamHit(int no)
    {
        if (myCharacter.isDie) return;
        if (myNo == 3 && myPassvie[0] > 0)
        {
            
               AM.MakeAct(9,1,null,myCharacter,null, myPassvie[0]);
            
        }
      
    }
    public void Vangara1()
    {
        if (myCharacter.isDie) return;
        myCharacter.getArmor(1);
    }
   
    public void MyArmorHit(int armor,Enemy e)
    {
        if (myCharacter.isDie) return;
        if (myNo == 3 && myPassvie[2] > 0)
        {
            AM.MakeAct(11, armor, e, myCharacter, null, myPassvie[2]);
        }
      
       
    }
    public void Vangara3(int dmg,Enemy E)
    {
        if (myCharacter.isDie) return;
        if (!E.isDie)
        {
            BM.OnAttack(dmg, E, myCharacter, 1);
        }
   }
  
    public void CardUse()
    {
        if (myCharacter.isDie) return;
        if (myNo == 2 && myPassvie[0] > 0)
        {
            ReplyCardDrow++;
            if (ReplyCardDrow % 3 == 0)
            {
               AM.MakeAct(5,1,null,myCharacter,null,myPassvie[0]);
            }
        }    
    }
    public void Sparky1()
    {
        if (myCharacter.isDie) return;
        myCharacter.ActUp(1);
    }
    

    public void myAct()
    {
        if (myCharacter.isDie) return;

        if (myNo == 2 && myPassvie[2]>0&&myCharacter.Act == 0&& CM.field.Count > 0)
        {
            AM.MakeAct(7, 1, null, myCharacter, null, myPassvie[2]);
        }
      
    }
    public void Sparky3()
    {
        if (myCharacter.isDie) return;
        if (CM.field.Count > 0)
        {
            int rand = Random.Range(0, CM.field.Count);
            CM.field[rand].GetComponent<Card>().cardcost = 0;
            CM.field[rand].GetComponent<Card>().costT.text = "" + 0;     
        }
    }
  
    public void ActMinus(int m,Enemy e)
    {
        if (myCharacter.isDie) return;
        BM.log.logContent.text += "\n" + myCharacter.Name + "이(가) " +e.Name + "에게 " +m + "의 행동력 감소를 받았다.";
        myCharacter.onMinusAct(m);
        GameObject dmgText = Instantiate(DMGtext, gameObject.transform);

        dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        dmgText.GetComponent<DMGtext>().GetType(1, m);


        if (myNo == 1 && myPassvie[3] > 0 && CM.Grave.Count > 0)
        {
            AM.MakeAct(4, 1, null, myCharacter, null, myPassvie[3]);
        }
        
       
    }
    public void Q4()
    {
        if (myCharacter.isDie) return;
        if (CM.Grave.Count > 0)
        {
            int rand = Random.Range(0, CM.Grave.Count);

            GameObject newCard = CM.Grave[rand];
            if (newCard.GetComponent<BlackWhite>() == null)
            {
                newCard.AddComponent<BlackWhite>();
                newCard.GetComponent<BlackWhite>().birth();
            }
            else
            {
                newCard.GetComponent<BlackWhite>().PlusStack();
            }
            CM.GraveToField(newCard);


            
        }
    }
  
    public void GhostRevive(int ghostplus)
    {
        if (myCharacter.isDie) return;
        ghost += ghostplus;        
        ghostText.text = ghost+"";
        int x = turnGhost + ghostplus;
        int q1Count = 0;
        if (myPassvie[1] > 0&&!isKing)
        {
            while (turnGhost != x)
            {
                turnGhost++;
                if (turnGhost % 4 == 0)
                {
                    q1Count++;
                }
            }
            AM.MakeAct(2, q1Count, null, myCharacter, null, myPassvie[1]);            
        }
    }

    public void Q2(int g)
    {
        if (myCharacter.isDie) return;
        myCharacter.ActUp(g);
    }

 
   
}
