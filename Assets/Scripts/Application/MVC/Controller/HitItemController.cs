using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HitItemController : Controller
{
    public override void Execute(object data)
    {
        ItemArgs e = data as ItemArgs;
        PlayerMove player = GetView<PlayerMove>();
        GameModel gm = GetModel<GameModel>();
        UIBoard ui = GetView<UIBoard>();
        switch (e.kind)
        {
            case ItemKind.ItemMagnet:
                player.HitMagnet();
                ui.HitMagnet();
                gm.Magnet -= e.hitCount;
                break;
            case ItemKind.ItemMultiply:
                player.HitMultiply();
                ui.HitMultiply();
                gm.Multiply -= e.hitCount;
                break;
            case ItemKind.ItemInvincible:
                player.HitInvincible();
                ui.HitInvincible();
                gm.Invincible -= e.hitCount;
                break;
            default:
                break;
        }
        ui.UpdateUI();
    }
}