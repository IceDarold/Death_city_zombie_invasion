using System;
using System.Collections.Generic;
using System.Linq;
using DataCenter;
using UnityEngine;

namespace inApps
{
    public class InApps : Singleton<InApps>
    {
        private static readonly string[] _tags = new[]
        {
            "GOLD_18000",   //1
            "GOLD_31000",   //2
            "GOLD_72000",   //3
            "DIAMONDS_500",   //4
            "DIAMONDS_1050",   //5
            "DIAMONDS_2400",   //6
            "DNA_10800",   //7
            "DNA_32400",   //8
            "DNA_90000",   //9
            "VIP",   //10
            "GOLD_DNA_X2",    //11
            "STARTER_PACK",   //12
            "DAILY_BOX",   //13
            "DAILY_GOLD",   //14
            "DAILY_DNA",   //15
            "ZOMBIE_KILLER_BAG",   //16
            "HARVESTER_PACKAGE",   //17
            "DEATH_MESSENGER",   //18
            "TERMINATOR_PACKAGE",   //19
            "SPECIAL_EQUIPMENT",   //20
            "BATTLE_PACK",   //21
            "RESURRECTION_PACKAGE",   //22
            "M1911_PISTOL",   //23
            "FAMAS_RIFLE",   //24
            "M4_RIFLE",   //25
            "UMP_MICRO_PUNCH",   //26
            "AK47_RIFLE",   //27
            "MP5_MINIATURE_PUNCH",   //28
            "BREN_RIFLE",   //29
            "SUPER_DISCOUNT",   //30
            "GOLD_160000",   //31
            "GOLD_450000",   //32
            "DIAMONDS_5400",   //33
            "DIAMONDS_15000",   //34
            "DNA_387900",   //35
            "DEATH_MESSENGER",   //36
            "MP5",   //37
            "BREN_PACKAGE",   //38
            "SPAS12_PACKAGE",   //39
            "M92F_PACKAGE",   //40
            "M32_PACKAGE",   //41
            "DAILY_BOX",//42
            "CONQUEST_PACKAGE",   //43 
            "HALLOWEEN_GIFT",   //44
        };

        public void Fetch()
        {
            
        }

        public void Purchase(int id, Action onComplete)
        {

        }

        public string GetPrice(int id)
        {
            var price = GetValuePrice(id);
            var yan = GetCurrency();
            return price + yan;
        }

        public int GetValuePrice(int id)
        {
            var strings = "".Split(' ');
            if (string.IsNullOrEmpty(strings[0])) return -1;
            return int.Parse(strings[0]);
        }
        
        public string GetCurrency()
        {
            return Singleton<GlobalData>.Instance.GetCurrentLanguage() == LanguageEnum.English ? " YAN" : " ЯН"; 
        }

        private readonly Dictionary<string, Action> products = new() {
            ["GOLD_18000"] = () => { ShopItem("GOLD_18000"); },
            ["GOLD_31000"] = () => { ShopItem("GOLD_31000");  },
            ["GOLD_72000"] = () => { ShopItem("GOLD_72000"); },
            ["DIAMONDS_500"] = () => { ShopItem("DIAMONDS_500"); },
            ["DIAMONDS_1050"] = () => { ShopItem("DIAMONDS_1050"); },
            ["DIAMONDS_2400"] = () => { ShopItem("DIAMONDS_2400"); },
            ["DNA_10800"] = () => { ShopItem("DNA_10800"); },
            ["DNA_32400"] = () => { ShopItem("DNA_32400"); },
            ["DNA_90000"] = () => { ShopItem("DNA_90000"); },
            ["VIP"] = () =>
            {
                ShopItem("VIP");
                //(Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["GOLD_DNA_X2"] = () => { ShopItem("GOLD_DNA_X2"); },
            ["STARTER_PACK"] = () => { ShopItem("STARTER_PACK"); },
            ["DAILY_BOX"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPage<DailyLimitedPage>(PageName.DailyLimitedPage);
                (Singleton<UiManager>.Instance.GetPage(PageName.DailyLimitedPage) as DailyLimitedPage)?.Product(0);
            },
            ["DAILY_GOLD"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPage<DailyLimitedPage>(PageName.DailyLimitedPage);
                (Singleton<UiManager>.Instance.GetPage(PageName.DailyLimitedPage) as DailyLimitedPage)?.Product(1);
            },
            ["DAILY_DNA"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPage<DailyLimitedPage>(PageName.DailyLimitedPage);
                //(Singleton<UiManager>.Instance.GetPage(PageName.DailyLimitedPage) as DailyLimitedPage)?.Show();
                (Singleton<UiManager>.Instance.GetPage(PageName.DailyLimitedPage) as DailyLimitedPage)?.Product(2);
            },
            // ["DAILY_BOX"] = () => { ShopItem("DAILY_BOX"); },
            // ["DAILY_GOLD"] = () => { ShopItem("DAILY_GOLD"); },
            // ["DAILY_DNA"] = () => { ShopItem("DAILY_DNA"); },
            ["ZOMBIE_KILLER_BAG"] = () => { ShopItem("ZOMBIE_KILLER_BAG"); },
            ["HARVESTER_PACKAGE"] = () => { ShopItem("HARVESTER_PACKAGE"); },
            ["DEATH_MESSENGER"] = () => { ShopItem("DEATH_MESSENGER"); },
            ["TERMINATOR_PACKAGE"] = () => { ShopItem("TERMINATOR_PACKAGE");},
            ["SPECIAL_EQUIPMENT"] = () => { ShopItem("SPECIAL_EQUIPMENT"); },
            ["BATTLE_PACK"] = () => { ShopItem("BATTLE_PACK"); },
            ["RESURRECTION_PACKAGE"] = () => { ShopItem("RESURRECTION_PACKAGE"); },
            ["M1911_PISTOL"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("M1911_PISTOL").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["FAMAS_RIFLE"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("FAMAS_RIFLE").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["M4_RIFLE"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("M4_RIFLE").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["UMP_MICRO_PUNCH"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("UMP_MICRO_PUNCH").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["AK47_RIFLE"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("AK47_RIFLE").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["MP5_MINIATURE_PUNCH"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("MP5_MINIATURE_PUNCH").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["BREN_RIFLE"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("BREN_RIFLE").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["SUPER_DISCOUNT"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPushGift(GetData("SUPER_DISCOUNT").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["GOLD_160000"] = () => { ShopItem("GOLD_160000"); },
            ["GOLD_450000"] = () => { ShopItem("GOLD_450000"); },
            ["DIAMONDS_5400"] = () => { ShopItem("DIAMONDS_5400"); },
            ["DIAMONDS_15000"] = () => { ShopItem("DIAMONDS_15000"); },
            ["DNA_387900"] = () => { ShopItem("DNA_387900"); },
            ["DEATH_MESSENGER"] = () => { ShopItem("DEATH_MESSENGER"); },
            ["MP5"] = () =>
            {
                //Singleton<UiManager>.Instance.ShowPage<RecommendGiftPage>(PageName.RecommendGiftPage);
                Singleton<UiManager>.Instance.ShowPushGift(GetData("MP5").ID);
                (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.OnclickBuySucess();
            },
            ["BREN_PACKAGE"] = () => { ShopItem("BREN_PACKAGE"); },
            ["SPAS12_PACKAGE"] = () => { ShopItem("SPAS12_PACKAGE"); },
            ["M92F_PACKAGE"] = () => { ShopItem("M92F_PACKAGE"); },
            ["M32_PACKAGE"] = () => { ShopItem("M32_PACKAGE"); },
            ["DAILY_BOX"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPage<DailyLimitedPage>(PageName.DailyLimitedPage);
                (Singleton<UiManager>.Instance.GetPage(PageName.DailyLimitedPage) as DailyLimitedPage)?.Product(0);
            },
            ["CONQUEST_PACKAGE"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPage<EachLevelGiftPage>(PageName.EachLevelGiftPage);
                (Singleton<UiManager>.Instance.GetPage(PageName.EachLevelGiftPage) as EachLevelGiftPage)?.Product();
            },
            ["HALLOWEEN_GIFT"] = () =>
            {
                Singleton<UiManager>.Instance.ShowPage<HallowmasGiftPage>(PageName.HallowmasGiftPage);
                (Singleton<UiManager>.Instance.GetPage(PageName.HallowmasGiftPage) as HallowmasGiftPage)?.Product();
            },
            //["HALLOWEEN_GIFT"] = () => { ShopItem("HALLOWEEN_GIFT"); },
        };

        public void AddAction(string id, Action action)
        {
            products.TryAdd(id, action);
        }

        public static void ShopItem(string id, bool openShop = true)
        {
            var data = GetData(id);
            if (data == null)
            {
                Debug.Log("DATA IS NULL");
                return;
            }
            
            if (openShop)
            {
                if (Singleton<UiManager>.Instance.CurrentPage.Name != PageName.StorePage)
                {
                    (Singleton<UiManager>.Instance.GetPage(PageName.RecommendGiftPage) as RecommendGiftPage)?.Close();
                    Singleton<UiManager>.Instance.CurrentPage.Hide();
                    Singleton<UiManager>.Instance.ShowStorePage(0);
                }
                else
                {
                    (Singleton<UiManager>.Instance.GetPage(PageName.StorePage) as StorePage)?.Refresh();
                }
            }
            (Singleton<UiManager>.Instance.GetPage(PageName.StorePage) as StorePage)?.Product(data);
        }

        private static StoreData GetData(string id)
        {
            return StoreDataManager.StoreDatas.FirstOrDefault(d => d.ChargePoint - 1 == Array.IndexOf(_tags, id));
        }
        
    }
}
