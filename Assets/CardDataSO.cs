using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card Game/Card Data")]
public class CardDataSOS : ScriptableObject
{
    public int CardID;
    public string CardName;
    public Sprite CardSprite;
    public bool IsOpen;
}