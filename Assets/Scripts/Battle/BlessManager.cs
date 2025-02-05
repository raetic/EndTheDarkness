﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class BlessManager : SingletonMonobehaviour<MonoBehaviour>
{
    [SerializeField] BattleManager BM;
    GameData GD;
    bool[] bless=new bool[21];
    private void Start()
    {
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(path3))
        {
            string gameData = File.ReadAllText(path3);
            GD = JsonConvert.DeserializeObject<GameData>(gameData);
        }
      
            bless = GD.blessbool;
        if (bless[2])
        {
            
            int rand = Random.Range(BM.line, BM.ChD.size);
            BM.characters[rand].bless[2] = true;
            BM.characters[rand].atk += 2;
        }
        if (bless[3]&&GD.bless3count>0)
        {
            BM.CardCount += 3;
            BM.TurnCardCount +=3;
            GD.bless3count--;
            if (GD.bless3count == 0) GD.blessbool[3] = false;
        }
        if (bless[4])
        {
            BM.GD.blessbool[4] = true;
        }
        if (bless[7])
        {
            BM.GD.blessbool[7] = true;
        }
		if (bless[12])
		{
			BM.GD.blessbool[12] = true;
		}
		if (bless[17])
        {
            for(int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].DefUp(1);
                BM.characters[i].speed += 0.2f;
                BM.characters[i].speed= Mathf.Round(BM.characters[i].speed*10)*0.1f;
            }
        }
        
    }
}
