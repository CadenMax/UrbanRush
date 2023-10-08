using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey = "a2c1dd5c17505bd4dc40169270ec3a698b8bbb2b958fd224334680cb1a504539";

    public void GetLeaderboard(){
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {
            for (int i = 0; i < names.Count; ++i) {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score){
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) => {
            GetLeaderboard();
        }));
    }
}