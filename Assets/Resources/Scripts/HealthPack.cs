public class HealthPack : Interactable
{
    protected override void Interact()
    {
        if (PlayerController.instance.playerHealth < 100)
        {
            PlayerController.instance.playerHealth += 25;
            UIManager.instance.UpdateHealth();
            gameObject.SetActive(false);
        }
    }
}