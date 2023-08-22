using System;
using static ConsoleRPG.Program;
using static System.Reflection.Metadata.BlobBuilder;

namespace ConsoleRPG
{
    internal class Program
    {
        private static Player player;
        private static Inventory inventory;
        private static Item item;
        private static Store store;
        private static StoreItem storeItem;
        static void Main(string[] args)
        {
            Title();
            
            Console.WriteLine("콘솔 RPG의 세계에 오신 것을 환영합니다.");
            Console.WriteLine("Enter키를 눌러 게임에 접속할 수 있습니다.");
            Console.ReadLine();
            Console.Clear();

            inventory = new Inventory(10); //최대 아이템 10개
            store = new Store(10); // 최대 상점 아이템 개수

            //메서드만 갖다 놓기
            PlayerData();
            ViliageScene();

        }
        static int CheckInput(int min, int max) // 선택지 함수
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool Check = int.TryParse(input, out int checkVal);
                if (Check)
                {
                    if (checkVal >= min && checkVal <= max) return checkVal;
                }
                Console.WriteLine("잘못된 입력입니다. 선택지의 번호를 입력해주세요.");
            }
        }

        static void ViliageScene()  // 마을 씬
        {
            Title();
            Console.WriteLine($"어서 오십시오! {player.Name}!");
            Console.WriteLine("현재 위치 : 마을");
            Console.WriteLine("");
            Console.WriteLine("마을에선 던전으로 들어가기 전에 장비를 확인할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태창 보기");
            Console.WriteLine("2. 인벤토리 보기");
            Console.WriteLine("3. 상점 가기");
            Console.WriteLine("0. 게임 종료");
            Choices();

            int input = CheckInput(0, 3); // 선택지 함수. 선택지마다 쓸 것
            switch(input)
            {
                case 1: Console.Clear(); 
                    DisplayPlayerInfo();
                    break;
                case 2: Console.Clear(); 
                    DisplayInventory();
                    break;
                case 3: Console.Clear();
                    StoreScene();
                    break;
                case 0: Console.WriteLine("게임을 종료합니다.");
                    break;
            }
        }

        static void DisplayInventory()  // 인벤토리 창
        {
            Title();
            ItemsInInventory(inventory); // 아이템 목록 값 할당

            Console.WriteLine($"[{player.Name}의 인벤토리]");
            Console.WriteLine("보유중인 아이템들을 관리할 수 있습니다.");
            Console.WriteLine("");
            inventory.ItemList();  // 아이템 목록 호출
            Console.WriteLine("");
            Console.WriteLine("1. 장비 관리");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");

            Choices();
            int input = CheckInput(0, 1);
            switch (input)
            {
                case 0:
                    Console.Clear();
                    ViliageScene();
                    break;
                case 1:
                    Console.Clear();
                    DisplayEquipScene();
                    break;
            }
        }
        static void DisplayEquipScene() // 장비 관리창
        {
            Title();
            ItemsInInventory(inventory); // 아이템 목록 값 할당

            Console.WriteLine("[장비 관리창]");
            Console.WriteLine("장비를 관리할 수 있습니다.");
            Console.WriteLine("");
            inventory.ItemList(); // 아이템 목록 호출
         
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");
            Choices();
            int input = CheckInput(0, inventory.GetItemsLength());
            
            switch (input)
            {
                case 0:
                    Console.Clear();
                    DisplayInventory();
                    break;
                default :
                    Console.Clear();
                    item = inventory.GetItem(input - 1);
                    if (item.IsEquipped == false)
                    {
                        item.IsEquipped = true;
                        player.StatATK += item.UpATK;
                        player.StatDEF += item.UpDEF;
                    }
                    else
                    {
                        item.IsEquipped = false;
                        player.StatATK -= item.UpATK;
                        player.StatDEF -= item.UpDEF;
                    }
                    DisplayEquipScene();
                    break;
            }
        }

        static void DisplayPlayerInfo()  // 플레이어 상태창
        {
            Title();
            Console.WriteLine("[상태창]");
            Console.WriteLine("플레이어의 정보를 확인합니다.");
            Console.WriteLine("");
            Console.WriteLine($"이름 : {player.Name}");
            Console.WriteLine("");
            Console.WriteLine($"직업 : {player.Job}");
            Console.WriteLine("");
            Console.WriteLine($"레벨 : {player.Level}");
            Console.WriteLine($"체력 : {player.HP}");
            Console.WriteLine($"공격력 : {player.StatATK}");
            Console.WriteLine($"방어력 : {player.StatDEF}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"보유 골드 : {player.Gold}G");
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");

            Choices();
            int input = CheckInput(0, 0);
            switch(input)
            {
                case 0:
                    Console.Clear();
                    ViliageScene();
                    break;
            }
        }

        static void PlayerData()
        {
            player = new Player("김전사", "전사", 10, 10, 15, 50000, 1000);
        }

        public class Player // 플레이어 상태 틀
        {
            public string Name { get; set; }
            public string Job { get; set; }
            public int Level { get; set; }
            public int StatDEF { get; set; }
            public int StatATK { get; set; }
            public int Gold { get; set; }
            public int HP { get; set; }

            public Player(string name, string job, int level, int statDEF, int statATK, int gold, int hp)
            {
                Name = name;
                Job = job;
                Level = level;  
                StatDEF = statDEF;
                StatATK = statATK;
                HP = hp;
                Gold = gold;
            }
        }

        //인벤토리 관련 모든 것
        public class Item
        {
            public string Name { get; }
            public string Description { get; }
            public bool IsEquipped { get; set; }
            public int UpATK { get; set; }
            public int UpDEF { get; set; }
            public Item(string name, string description, int upATK, int upDEF)
            {
                Name = name;
                Description = description;
                UpATK = upATK;
                UpDEF = upDEF;
                IsEquipped = false;
            }
        }

        public class Inventory
        {
            private Item[] items;  // 외부에서 맘대로 못건드리게 private으로 해놓음
            
            public Inventory(int size)
            {
                items  = new Item[size];
            }

            public void AddItem(Item item) // 아이템 추가 기능
            {
                for (int i=0; i<items.Length; i++)
                {
                    if (items[i] == null)
                    {
                        bool isDuplicate = false;

                        foreach (Item existingItem in items)
                        {
                            if (existingItem != null && existingItem.Name == item.Name)
                            {
                                isDuplicate = true;
                                break;
                            }
                        }

                        if(!isDuplicate)
                        {
                            items[i] = item;
                            break;
                        }
                        
                    }
                }
            }

            public Item GetItem(int itemIndex) //외부에서 접근할 수 있게 만들었다. 
            {
                if(itemIndex < items.Length)
                {
                    return items[itemIndex];
                }
                return null;
            }

            public int GetItemsLength()
            {
                return items.Length;
            }

            public void ItemList()  //아이템 목록 출력 함수
            {
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        string itemText = $"{i + 1}.";
                        if (items[i].IsEquipped)
                        {
                            itemText += "[E] ";
                        }
                        itemText += $"{items[i].Name}  |  {items[i].Description}";
                        Console.WriteLine(itemText);
                    }
                }
            }

        }
        static void ItemsInInventory(Inventory inventory)  // 인벤토리에 있는 아이템
        {
            Item sword = new Item("낡은 단검", "낡아서 이가 나간 단검이다.  |  공격력 + 3", 3, 0);
            Item bow = new Item("낡은 활", "금방이라도 시위가 끊어질 것 같은 활.  | 공격력 + 4", 4, 0);
            Item shield = new Item("나무 방패", "나무로 만든 못 미더운 방패.  |  방어력 + 3", 0, 3);

            inventory.AddItem(sword);
            inventory.AddItem(bow);
            inventory.AddItem(shield);
        }

        // 상점 관련
        static void StoreScene()
        {
            Title();
            ItemInStore(store);
            Console.WriteLine("[상점]");
            Console.WriteLine("");
            Console.WriteLine("어서 오시게! 쓸만한 물건이 많으니 천천히 둘러보게나!");
            Console.WriteLine("판매중인 아이템들의 번호를 눌러 구매할 수 있다네!");
            Console.WriteLine("");
            store.StoreItemList();
            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");
            Console.WriteLine("무엇을 할텐가?");
            Console.Write(">>");
            int input = CheckInput(0, store.GetStoreItemsLength());
            switch(input)
            {
                case 0: Console.Clear();
                    ViliageScene();
                    break;
                default: Console.Clear();
                    store.SellItem(input - 1, inventory);
                    StoreScene();
                    break;
            }
        }

        public class StoreItem : Item  // 판매하려면 Item에 들어가야되므로 상속
        {
            public bool IsSoldOut { get; set; }
            public string PriceTxt { get; set; }
            public int Price { get; set; }
            public StoreItem(string name, string description, int upATK, int upDEF, int price, string priceTxt)
                : base(name, description, upATK, upDEF)
            {
                IsSoldOut = false;
                Price = price;
                PriceTxt = priceTxt;
            }
        }
        static void ItemInStore(Store store)
        {
            StoreItem spear = new StoreItem("튼튼한 창", "좋은 목재와 쇠로 만든 창. 튼튼하다!  |  공격력 + 10", 10, 0, 1500, "  |  가격 : 1500 G");
            StoreItem ironShield = new StoreItem("강철 방패", "강철로 이루어진 믿음직한 방패.  |  방어력 + 12", 0, 12, 1700, "  |  가격 : 1700 G");
            StoreItem leatherBoots = new StoreItem("가죽 장화", "가죽으로 만들어진 장화.  |  방어력 + 7  |  가격 : 1000 G", 0, 7, 1000, "  |  가격 : 1000 G");
            StoreItem clothgloves = new StoreItem("천 장갑", "부드럽지만 얇은 천 장갑.  |  방어력 + 4  |  가격 : 500 G", 0, 4, 500, "  |  가격 : 500G");

            store.AddStoreItem(spear);
            store.AddStoreItem(ironShield);
            store.AddStoreItem(leatherBoots);
            store.AddStoreItem(clothgloves);
        }

        public class Store
        {
            private StoreItem[] storeItems;

            public Store(int storeSize)
            {
                storeItems = new StoreItem[storeSize];
            }
            public void AddStoreItem(StoreItem storeItem) // 아이템 추가 기능
            {
                for (int i = 0; i < storeItems.Length; i++)
                {
                    if (storeItems[i] == null)
                    {
                        bool isDuplicate = false;

                        foreach (StoreItem existingItem in storeItems)
                        {
                            if (existingItem != null && existingItem.Name == storeItem.Name)
                            {
                                isDuplicate = true;
                                break;
                            }
                        }

                        if (!isDuplicate)
                        {
                            storeItems[i] = storeItem;
                            break;
                        }

                    }
                }
            }
            public StoreItem GetStoreItem(int Index) //외부에서 접근할 수 있게 만들었다. 
            {
                if (Index < storeItems.Length)
                {
                    return storeItems[Index];
                }
                return null;
            }
            public void StoreItemList()  // 상점 아이템 목록 출력 함수
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("[판매중인 아이템 목록]");
                Console.WriteLine("");
                for (int i = 0; i < storeItems.Length; i++)
                {
                    if (storeItems[i] != null)
                    {
                        string storeItemText = $"{i + 1}.";
                        if (storeItems[i].IsSoldOut)
                        {
                            storeItemText += "[품절!] ";
                        }
                        storeItemText += $"{storeItems[i].Name}  |  {storeItems[i].Description}";
                        Console.WriteLine(storeItemText);
                    }
                }
            }

            public void SellItem(int storeItemIndex, Inventory inventory)
            {
                string[] sellDialogues = {
                    "훌륭한 장비지! 잘 쓰게나!",
                    "그거 참 괜찮은 물건일세! 잘 선택 했구만!",
                    "내가 만들었지만 참 걸작이란 말이지! 잘 써주게!"};
                string[] cantSell = {
                    "자네 골드가 부족하구만! 골드를 벌어서 다시 오게나!",
                    "우리 가게는 외상이 안된다네, 젊은 친구!",
                    "골드가 부족하군! 던전에 다녀오는게 어떻겠나!"};
                string[] soldOutDialogues = {
                    "그건 이미 팔린 상품일세!",
                    "그건 다 나갔어! 다른 상품을 보게나!",
                    "그 상품은 품절일세! 다음에 다시 찾아주겠나!" };

                StoreItem storeItem = GetStoreItem(storeItemIndex);
                if(storeItem != null)
                {
                    if(player.Gold >= storeItem.Price && storeItem.IsSoldOut == false)  // 판매 성공일 때
                    {
                        Item convertedItem = storeItem; // 부모 클래스로 형변환
                        inventory.AddItem(convertedItem); // 판매 처리 및 인벤토리 추가 로직
                        storeItem.IsSoldOut = true;
                        player.Gold -= storeItem.Price;

                        Console.SetCursorPosition(0,6);
                        Console.ForegroundColor = ConsoleColor.Green;   //  대사 색깔 변경
                        string sellDialogue = NpcDialogues(sellDialogues);   // 랜덤 대사
                        Console.WriteLine(sellDialogue);  
                        Console.ResetColor();  // 대사 색깔 리셋
                        Console.SetCursorPosition(0, 0);
                    }
                    else if(player.Gold < storeItem.Price)  // 구매 불가
                    {
                        Console.SetCursorPosition(0, 6);
                        Console.ForegroundColor = ConsoleColor.Red;
                        string cantSellDialogue = NpcDialogues(cantSell);
                        Console.WriteLine(cantSellDialogue);
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 0);
                    }
                    else if(storeItem.IsSoldOut == true) // 품절일 때
                    {
                        Console.SetCursorPosition(0, 6);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        string soldOutDialogue = NpcDialogues(soldOutDialogues);
                        Console.WriteLine(soldOutDialogue);
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 0);
                    }
                }
            }
            public int GetStoreItemsLength()
            {
                return storeItems.Length;
            }
        }

        static string NpcDialogues(string[] dialogues)  // 대사를 랜덤하게 출력하는 메서드
        {
            Random random = new Random();
            int randomIndex = random.Next(dialogues.Length);

            return dialogues[randomIndex];
        }

        static void Choices()
        {
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동의 번호를 입력해주세요.");
            Console.Write(">> ");
        }

        static void Title()
        {
            string text = "☜§    Console RPG    §☞";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text);
        }

        
    }

    // 글자 색깔 바꾸는 코드
   // Console.ForegroundColor = ConsoleColor.Red;
}