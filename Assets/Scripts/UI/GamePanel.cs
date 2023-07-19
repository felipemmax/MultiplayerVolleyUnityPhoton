using TMPro;
using UnityEngine;
namespace Volley.UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        public void UpdateScore(string leftScore, string rightScore)
        {
            scoreText.text = leftScore + " x " + rightScore;
        }

        public void SetWinner(string team)
        {
            scoreText.text = "Time " + team + " venceu o jogo";
        }
    }
}