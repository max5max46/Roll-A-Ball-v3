using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTrigger : MonoBehaviour
{
    private int BlockCount;
    public TextMeshProUGUI DestroyedCountText;
    public PlayerController Player;

// Start is called before the first frame update
void Start()
{
    BlockCount = 0;
    SetDestroyedCountText();
}

// Update is called once per frame
void Update()
{
    transform.position = Player.transform.position;
}
void SetDestroyedCountText()
{
    DestroyedCountText.text = "Blocks Destoryed: " + BlockCount.ToString() + "/15";
    if (BlockCount >= 15)
    {
        Player.IsAllBlocksGone = true;
        Player.SetCountText();
    }
}
private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("Breakable") && Player.CoolDown >= 0.1)
    {
        other.gameObject.SetActive(false);
        BlockCount = BlockCount + 1;

        SetDestroyedCountText();
    }
}
}
