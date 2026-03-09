using UnityEngine;
using TMPro;

public class SubgameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI movesRemainingText;

    public void UpdateMovesRemaining(int movesRemaining)
    {
        movesRemainingText.text = $"Moves Remaining: {movesRemaining}";
    }
}
