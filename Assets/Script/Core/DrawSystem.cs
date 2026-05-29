using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawSystem : MonoBehaviour
{

   public Image[] cards=new Image[4];
   private List<Sprite> _cardPool = new();
   private List<Sprite> _lastCards = new();
   
   
   private void Start()
   {
      DrawRandomCard(); 
   }

   private void DrawRandomCard()
   {

      for (int i = 0; i < 4; i++)
      {
         int r=Random.Range(0, _cardPool.Count);
         Sprite card =_cardPool[r];
         cards[i].sprite = card;
         
         _lastCards.Add(card);
         _cardPool.RemoveAt(r);
      }
      
   }
    
}


