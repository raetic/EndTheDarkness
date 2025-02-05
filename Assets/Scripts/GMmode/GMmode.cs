﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
public class GMmode : MonoBehaviour
{
    public CardData CD;
    public CharacterData ChD;
    public GameData GD = new GameData();
    [SerializeField] GameObject CardView;
    [SerializeField] GameObject CharacterView;
	int[] CardCount;
    [SerializeField]TextMeshProUGUI[] passiveCounts;

    [SerializeField] GameObject GetEquipmentCanvas;
    [SerializeField] TextMeshProUGUI[] equipStrings;
    [SerializeField] TMP_InputField blessSelect;
	[SerializeField] SO_CardList cardList;
	// TO DO : 캐릭터 카드 셋팅
    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        string path2 = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        if (File.Exists(path))
        {
            string cardData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CardData>(cardData);
        }
        if (File.Exists(path2))
        {
            string characterData = File.ReadAllText(path2);
            ChD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        }
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(path3))
        {
            string gameData = File.ReadAllText(path3);
            GD = JsonConvert.DeserializeObject<GameData>(gameData);
        }
		CardCount = new int[cardList.cardDetails.Count];
        for (int i = 0; i < CD.cardDetails.Count; i++)
        {
            CardCount[CD.cardDetails[i].no]++;
        }
    }

public void GoLobby()
    {   if(blessSelect.text!="")
        GD.blessSelect = int.Parse(blessSelect.text);
        Debug.Log(blessSelect);
        string characterData = JsonConvert.SerializeObject(ChD);
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        File.WriteAllText(path, characterData);
        SaveCard();
        string cardData = JsonConvert.SerializeObject(CD);
        path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        File.WriteAllText(path, cardData);
        string gameData = JsonConvert.SerializeObject(GD);
        path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, gameData);
        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
        //SceneManager.LoadScene("Scene2_Lobby");
    }  
    public void GetIgnum()
    {
        GD.Ignum += 10000;
    }
    public void GetTribute()
    {
        GD.tribute += 10000;
    }
    public void GetAct()
    {
        GD.isNight = false;
        GD.isAct = false;
        GD.isActInDay = false;
    }
    void SaveCard()
    {
		CD.lastId = 0;
		CD.SetSO(cardList);
		CD.cardDetails.Clear();
        for(int i = 1; i < cardList.cardDetails.Count; i++)
        {
            int count = 0;
            while (count < CardCount[i])
            {
				++count;
				CD.AddDefaultCard(i);
			}
        }
    }
    public void OpenCardView()
    {
        for (int i = 0; i < CardInfo.Instance.cd.Length; i++)
        {
            CardCount[i] = 0;
        }
		for (int i = 0; i < CD.cardDetails.Count; i++)
		{
			CardCount[CD.cardDetails[i].no]++;
		}
		for (int i = 0; i < CardInfo.Instance.cd.Length-1; i++)
        {
            CardView.transform.GetChild(i).gameObject.SetActive(true);
            CardView.transform.GetChild(i).GetComponent<SetCardInGM>().set(i+1,this,CardCount[i+1]);
        }
    }
    public void OpenPassiveView()
    {
        for(int i = 0; i < ChD.size; i++)
        {
            CharacterView.transform.GetChild(i).gameObject.SetActive(true);
            CharacterView.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name;
            for (int j = 0; j < 4; j++)
            {
                CharacterView.transform.GetChild(i).GetChild(1 + j).GetComponent<TextMeshProUGUI>().text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[j];
                CharacterView.transform.GetChild(i).GetChild(1 + j).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].passive[j]+"";
            }
        }
    }
    public void CardChange(int no,int size)
    {
        CardCount[no] = size;
    }
    public void ChangePassivePlus(int k)
    {
        ChD.characterDatas[k / 4].passive[k % 4]++;
        if (ChD.characterDatas[k / 4].passive[k % 4] < 0) ChD.characterDatas[k / 4].passive[k % 4] = 0;
        passiveCounts[k].text = ChD.characterDatas[k / 4].passive[k % 4]+"";
    }
    public void ChangePassiveMinus(int k)
    {
        ChD.characterDatas[k / 4].passive[k % 4]--;
        if (ChD.characterDatas[k / 4].passive[k % 4] < 0) ChD.characterDatas[k / 4].passive[k % 4] = 0;
        passiveCounts[k].text = ChD.characterDatas[k / 4].passive[k % 4] + "";
    }
    public void GetRandomEquipment()
    {
        //canvasOn = true;
        equipment curEquip;
        GetEquipmentCanvas.SetActive(true);
        curEquip = EquipmentManager.Instance.makeEquipment();
        GetEquipmentCanvas.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[curEquip.equipNum];
        List<string> sList = EquipmentManager.Instance.equipmentStrings(curEquip);
        equipStrings[0].text = sList[0];
        equipStrings[1].text = sList[1] + '\n' + sList[2] + '\n' + sList[3];
        GD.EquipmentList.Add(curEquip);
    }
    public void GetSpecialEquipment(int i)
    {
        CharacterView.SetActive(false);
        GetEquipmentCanvas.SetActive(true);
        equipment curEquip=EquipmentManager.Instance.makeSpecialEquipment(ChD.characterDatas[i].code);     
        GetEquipmentCanvas.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[curEquip.equipNum];
        List<string> sList = EquipmentManager.Instance.equipmentStrings(curEquip);
     
        equipStrings[0].text = sList[0];
        if (curEquip.special == 0)
            equipStrings[1].text = sList[1] + '\n' + sList[2] + '\n' + sList[3];
        else equipStrings[1].text = sList[1];
        GD.EquipmentList.Add(curEquip);
    }
    public void SetBattleByNum(int battle)
    {
        GD.victory = battle;
    }
}

