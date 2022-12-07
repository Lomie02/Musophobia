using System.Collections;
using UnityEngine;

public class URLLinks : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/ehQfGYd4qs"); /// to be confirmed
    }

    public void OpenItch()
    {
        Application.OpenURL("https://thecatholicrats.itch.io/musophobia");
    }

    public void OpenItchTeam()
    {
        Application.OpenURL(" https://thecatholicrats.itch.io/");
    }

    public void OpenGameJolt()
    {
        Application.OpenURL("https://gamejolt.com/games/musophobia/767743");
    }

    public void OpenURL(string link)
        {
            Application.OpenURL(link);
        }
    
}

    


