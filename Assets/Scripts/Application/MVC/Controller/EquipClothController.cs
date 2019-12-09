using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EquipClothController : Controller
{
    public override void Execute(object data)
    {
        UIShop shop = GetView<UIShop>();
        BuyClothArgs e = data as BuyClothArgs;
        GameModel gm = GetModel<GameModel>();
        gm.TakeOnCloth = e.id;
        //更新信息
        shop.UpdateUI();
      
     

    }
}