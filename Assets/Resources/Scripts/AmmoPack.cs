public class AmmoPack : Interactable
{
    protected override void Interact()
    {
        GunController.instance.totalAmmo += 5;
        UIManager.instance.UpdateAmmo();
        gameObject.SetActive(false);
    }
}