using System;
using static ConsoleRPG.Program;
using static System.Reflection.Metadata.BlobBuilder;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleRPG
{
    internal class Program
    {
        private static Player player;
        private static Inventory inventory;
        private static Item item;
        private static Store store;
        private static Dungeon dungeon;
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
            GameManager.LoadGame();
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
            Console.WriteLine($"어서 오십시오! {player.Name}님!");
            Console.WriteLine("현재 위치 : 마을");
            Console.WriteLine("");
            Console.WriteLine("마을에선 던전으로 들어가기 전에 장비를 확인할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태창 보기");
            Console.WriteLine("2. 인벤토리 보기");
            Console.WriteLine("3. 상점으로");
            Console.WriteLine("4. 던전 입구로");
            Console.WriteLine("5. 여관으로");
            Console.WriteLine("");
            Console.WriteLine("0. 게임 종료");
            Choices();

            int input = CheckInput(0, 5); // 선택지 함수. 선택지마다 쓸 것
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
                case 4: Console.Clear();
                    ToDungeonEntrance();
                    break;
                case 5: Console.Clear();
                    MotelScene();
                    break;
                case 0: Console.WriteLine("게임을 저장하고 종료합니다.");
                    GameManager.SaveGame(player);
                    break;
            }
        }

        static void MotelScene()
        {
            Title();
            Console.WriteLine("[여관]");
            Console.WriteLine("");
            Console.WriteLine("여관 주인 : 여관에 오신 걸 환영합니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 식사하기(체력 50 회복) | 100 G");
            Console.WriteLine("2. 잠자기(체력 완전 회복) | 200 G");
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("무엇을 하시겠어요?");
            Console.Write(">>");
            int input = CheckInput(0, 2); // 선택지 함수. 선택지마다 쓸 것
            switch (input)
            {
                case 1:Console.Clear();
                    Console.SetCursorPosition(0, 10);
                    Console.WriteLine("");
                    if(player.HP < player.MaxHP)
                    {
                        Console.WriteLine("여관 주인 : 주문하신 음식 나왔습니다~ 맛있게 드세요~");
                        Console.WriteLine("");
                        Console.WriteLine("100골드를 지불합니다.");
                        player.Gold -= 100;
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("맛있다! 체력이 50 회복됩니다.");
                        Console.ResetColor();
                        player.HP += 50;
                    }
                    else
                    {
                        Console.WriteLine("더 이상 식사할 필요는 없을 것 같다.");
                    }
                    Thread.Sleep(2000);
                    Console.Clear();
                    MotelScene();
                    break;
                case 2: Console.Clear();
                    break;
                case 0: Console.Clear();
                    ViliageScene();
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
            Console.WriteLine($"체력 : {player.HP} / {player.MaxHP}");
            Console.WriteLine($"공격력 : {player.StatATK}");
            Console.WriteLine($"방어력 : {player.StatDEF}");
            Console.WriteLine($"전투력 : {player.Power}");
            Console.WriteLine("");
            Console.WriteLine($"경험치 : {player.Exp} / {player.ExpNeeded}");
            Console.WriteLine("");
            Console.WriteLine($"보유 골드 : {player.Gold}G");
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");
            Console.Write(">>");

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
            player = new Player("김전사", "전사", 10, 10, 15, 1000, 100, 100, 0);
        }

        public class Player // 플레이어 상태 틀
        {
            public string Name { get; set; }
            public string Job { get; set; }
            public int Level { get; set; }

            private int statATK;
            private int statDEF;
            public int StatDEF { get { return statDEF; } set { statDEF = value; UpdatePower(); } }
            public int StatATK { get { return statATK; } set { statATK = value; UpdatePower(); } }
            public int Gold { get; set; }

            private int hp;
            public int HP
            {
                get { return hp; }
                set
                {
                    hp = Math.Min(value, MaxHP);
                }
            }

            public int MaxHP { get; set; }

            private int power;
            public int Power { get { return power; } private set { power = value; } }
            private void UpdatePower()
            {
                Power = StatATK + StatDEF;
            }
            public int Exp { get; set; } // 추가: 경험치
            public int ExpNeeded { get; set; } // 추가: 다음 레벨까지 필요한 경험치
            public Player(string name, string job, int level, int statDEF, int statATK, int gold, int hp, int maxHp, int exp)
            {
                Name = name;
                Job = job;
                Level = level;  
                StatDEF = statDEF;
                StatATK = statATK;
                MaxHP = maxHp;
                HP = hp;
                Gold = gold;
                Exp = exp; // 추가: 경험치
                ExpNeeded = CalculateExpNeeded();
            }
            public void GetExp(int amount)  // 경험치 얻는 메서드
            {
                Exp += amount;
                if (Exp >= ExpNeeded)
                {
                    LevelUp();
                }
            }

            private void LevelUp()  // 레벨 업
            {
                Level++;
                Exp -= ExpNeeded;
                ExpNeeded = CalculateExpNeeded();
                StatATK += 5;
                StatDEF += 5;
                Console.SetCursorPosition(10, 10);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"축하합니다! 레벨 업! 현재 레벨: {Level}");
                Console.ResetColor();
            }

            private int CalculateExpNeeded() // 레벨 업에 필요한 경험치
            {
                return Level * 2; 
            }
        }

        //인벤토리 관련 모든 것
        public class Item
        {
            public string Name { get; }
            public string StatTxt { get; set; }
            public string Description { get; }
            public bool IsEquipped { get; set; }
            public int UpATK { get; set; }
            public int UpDEF { get; set; }
            public Item(string name, string statTxt,  string description,  int upATK, int upDEF)
            {
                Name = name;
                StatTxt = statTxt;
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
                        string itemText = $"{i + 1}. ";
                        if (items[i].IsEquipped)
                        {
                            itemText += "[E] ";
                        }
                        itemText += $"{items[i].Name,-7} | {items[i].StatTxt,-7} | {items[i].Description,-20}";
                        Console.WriteLine(itemText);
                    }
                }
            }
        }
        static void ItemsInInventory(Inventory inventory)  // 인벤토리에 있는 아이템
        {
            Item sword = new Item("낡은 단검", "공격력 + 3", "낡아서 이가 나간 단검이다.", 3, 0);
            Item bow = new Item("낡은 활", "공격력 + 4", "금방이라도 시위가 끊어질 것 같은 활.",  4, 0);
            Item shield = new Item("나무 방패", "방어력 + 3", "나무로 만든 못 미더운 방패.",   0, 3);

            inventory.AddItem(sword);
            inventory.AddItem(bow);
            inventory.AddItem(shield);
        }

        // 상점 관련
        static void StoreScene()
        {
            Title();
            ItemInStore(store);
            Console.WriteLine("[브룬의 상점]");
            Console.WriteLine("");
            Console.WriteLine($"현재 보유 골드 : {player.Gold} G");
            Console.WriteLine("");
            Console.WriteLine("브룬 : 어서 오시게! 쓸만한 물건이 많으니 천천히 둘러보게나!");
            Console.WriteLine("브룬 : 판매중인 아이템들의 번호를 눌러 구매할 수 있다네!");
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
            public int Price { get; set; }
            public string PriceTxt { get; set; }
            public StoreItem(string name,string statTxt, string description, int upATK, int upDEF, int price, string priceTxt)
                : base(name, statTxt, description, upATK, upDEF)
            {
                IsSoldOut = false;
                Price = price;
                PriceTxt = priceTxt;
            }
        }
        static void ItemInStore(Store store)
        {
            StoreItem spear = new StoreItem("튼튼한 창", "공격력 + 10", "좋은 목재와 쇠로 만든 창. 튼튼하다!", 10, 0, 1500, "가격 : 1500 G");
            StoreItem mace = new StoreItem("청동 메이스", "공격력 15", "청동으로 만든 철퇴.", 14, 0, 2000, "가격 : 2000 G");
            StoreItem claymore = new StoreItem("클레이모어", "공격력 + 20", "양날의 대검. 상당히 무겁다.", 20, 0, 2500, "가격 : 2500 G");
            StoreItem ironShield = new StoreItem("강철 방패", "방어력 + 12", "강철로 이루어진 믿음직한 방패.",   0, 12, 1700, "가격 : 1700 G");
            StoreItem leatherBoots = new StoreItem("가죽 장화", "방어력 + 7", "가죽으로 만들어진 장화.", 0, 7, 1000,"가격 : 1000 G");
            StoreItem clothgloves = new StoreItem("천 장갑", "방어력 + 4", "부드럽지만 얇은 천 장갑.", 0, 4, 500, "가격 : 500G");

            store.AddStoreItem(spear);
            store.AddStoreItem(mace);
            store.AddStoreItem(claymore);
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
                        storeItemText += $"{storeItems[i].Name,-7}  |  {storeItems[i].StatTxt,-7}  |  {storeItems[i].Description,-20}  |  {storeItems[i].PriceTxt,-7}";
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

                        Console.SetCursorPosition(0 , 12);
                        Console.ForegroundColor = ConsoleColor.Green;   //  대사 색깔 변경
                        string sellDialogue = NpcDialogues(sellDialogues);   // 랜덤 대사
                        Console.WriteLine(sellDialogue);  
                        Console.ResetColor();  // 대사 색깔 리셋
                        Console.SetCursorPosition(0, 0);
                    }
                    else if(player.Gold < storeItem.Price && storeItem.IsSoldOut == false)  // 구매 불가
                    {
                        Console.SetCursorPosition(0, 12);
                        Console.ForegroundColor = ConsoleColor.Red;
                        string cantSellDialogue = NpcDialogues(cantSell);
                        Console.WriteLine(cantSellDialogue);
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 0);
                    }
                    else if(storeItem.IsSoldOut == true) // 품절일 때
                    {
                        Console.SetCursorPosition(0, 12);
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

        static void ToDungeonEntrance()
        {
            Console.Clear();
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("던전 입구로 향합니다.");
            Thread.Sleep(1000);
            DisplayDungeonEntrance();
        }
        static void DisplayDungeonEntrance()
        {
            Console.Clear();
            Title();
            Console.WriteLine("[던전 입구]");
            Console.WriteLine("경비병 : 이 곳은 던전 입구입니다.");
            Console.WriteLine("");
            Console.WriteLine($"현재 내 전투력 : {player.Power}");
            Console.WriteLine("");
            DungeonSelect(player);
        }

        public class DungeonDifficulty
        {
            public string Name { get; set; }

            public DungeonDifficulty(string name)
            {
                Name = name;
            }
        }

        public class Dungeon
        {
            public string Name { get; set; }
            public DungeonDifficulty Difficulty { get; set; }
            public int RequiredPower { get; set; } // 필요한 전투력

            public Dungeon(string name, DungeonDifficulty difficulty, int requiredPower)
            {
                Name = name;
                Difficulty = difficulty;
                RequiredPower = requiredPower;
            }

            public bool CanEnter(Player player)
            {
                return player.Power >= RequiredPower; // 플레이어 전투력이 필요 전투력보다 높으면 입장 가능
            }
        }

        static void DungeonSelect(Player player)
        {
            DungeonDifficulty easy = new DungeonDifficulty("쉬움");
            DungeonDifficulty normal = new DungeonDifficulty("보통");
            DungeonDifficulty hard = new DungeonDifficulty("어려움");
            DungeonDifficulty veryHard = new DungeonDifficulty("매우 어려움");

            Dungeon firstDungeon = new Dungeon("고블린의 소굴", easy, 20);
            Dungeon secondDungeon = new Dungeon("트롤 동굴", normal, 40);
            Dungeon thirdDungeon = new Dungeon("골렘의 사원", hard, 60);
            Dungeon lastDungeon = new Dungeon("용의 둥지", veryHard, 100);

            Console.WriteLine("경비병 : 어떤 던전에 도전하시겠습니까?");
            Console.WriteLine("어떤 던전에 입장하시겠습니까?");
            Console.WriteLine("1. 고블린의 소굴  |  필요 전투력 : 20");
            Console.WriteLine("2. 트롤 동굴      |  필요 전투력 : 60");
            Console.WriteLine("3. 골렘의 사원    |  필요 전투력 : 80");
            Console.WriteLine("4. 용의 둥지      |  필요 전투력 : 120");
            Console.WriteLine("");
            Console.WriteLine("0. 마을로 돌아가기");
            Console.Write(">>");

            int input = CheckInput(0, 4);

            Dungeon selectedDungeon = null;

            switch (input)
            {
                case 1:
                    selectedDungeon = firstDungeon;
                    break;
                case 2:
                    selectedDungeon = secondDungeon;
                    break;
                case 3:
                    selectedDungeon = thirdDungeon;
                    break;
                case 4:
                    selectedDungeon = lastDungeon;
                    break;
                case 0:Console.Clear();
                    ViliageScene();
                    break;
            }

            if (selectedDungeon != null && selectedDungeon.CanEnter(player) && player.HP > 50)
            {
                Random random = new Random();
                int randomNumber = random.Next(100); // 0부터 99까지의 난수 생성
                Console.Clear();
                Console.WriteLine($"{selectedDungeon.Name}에 입장합니다!");
                Console.Clear();
                DungeonAnimation();

                if (randomNumber < 80 && selectedDungeon == firstDungeon) // 80퍼 확률
                {
                    Console.SetCursorPosition(0, 10);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("축하합니다! 던전을 클리어 하셨습니다!");
                    Console.ResetColor();
                    Console.WriteLine("");
                    Console.WriteLine("[결과 정산]");
                    Console.WriteLine("");
                    Console.WriteLine("골드 + 500G", player.Gold += 500);
                    Console.WriteLine("체력 - 20", player.HP -= 20);
                    Console.WriteLine("경험치 + 10");
                    player.GetExp(10);
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("마을로 돌아갑니다.");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    Console.Clear();
                    ViliageScene();
                }
                else if (randomNumber < 75 && selectedDungeon == secondDungeon) // 75퍼 확률
                {
                    Console.SetCursorPosition(0, 10);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("축하합니다! 던전을 클리어 하셨습니다!");
                    Console.ResetColor();
                    Console.WriteLine("");
                    Console.WriteLine("[결과 정산]");
                    Console.WriteLine("");
                    Console.WriteLine("골드 + 1500G", player.Gold += 1500);
                    Console.WriteLine("체력 - 25", player.HP -= 25);
                    Console.WriteLine("경험치 + 20");
                    player.GetExp(20);
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("마을로 돌아갑니다.");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    Console.Clear();
                    ViliageScene();
                }
                else if (randomNumber < 70 && selectedDungeon == thirdDungeon) // 70퍼 확률
                {
                    Console.SetCursorPosition(0, 10);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("축하합니다! 던전을 클리어 하셨습니다!");
                    Console.ResetColor();
                    Console.WriteLine("");
                    Console.WriteLine("[결과 정산]");
                    Console.WriteLine("");
                    Console.WriteLine("골드 + 2000G", player.Gold += 2000);
                    Console.WriteLine("체력 - 30", player.HP -= 30);
                    Console.WriteLine("경험치 + 40");
                    player.GetExp(40);
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("마을로 돌아갑니다.");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    Console.Clear();
                    ViliageScene();
                }
                else if (randomNumber < 60 && selectedDungeon == lastDungeon) // 60퍼 확률
                {
                    Console.SetCursorPosition(0, 10);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("축하합니다! 던전을 클리어 하셨습니다!");
                    Console.ResetColor();
                    Console.WriteLine("");
                    Console.WriteLine("[결과 정산]");
                    Console.WriteLine("");
                    Console.WriteLine("골드 + 3000G", player.Gold += 3000);
                    Console.WriteLine("체력 - 40", player.HP -= 40);
                    Console.WriteLine("경험치 + 80");
                    player.GetExp(80);
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("마을로 돌아갑니다.");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    Console.Clear();
                    ViliageScene();
                }
                else
                {
                    Console.SetCursorPosition(0, 10);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("던전 클리어에 실패했습니다...");
                    Console.ResetColor();
                    Console.WriteLine("");
                    Console.WriteLine("[결과 정산]");
                    Console.WriteLine("체력 - 80", player.HP -= 80);
                    if(player.HP <=0)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(10, 10);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("사망하였습니다.");
                        Console.ResetColor();
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("");
                    }
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("마을로 돌아갑니다...");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    Console.Clear();
                    ViliageScene();

                }
            }
            else if(selectedDungeon != null && selectedDungeon.CanEnter(player) && player.HP < 50)
            {
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("경비병 : 모험가님? 체력을 회복하고 들어가시는게 어떨까요?");
                Console.ResetColor();
                Thread.Sleep(2000);
                DisplayDungeonEntrance(); 
            }
            else if(selectedDungeon != null)
            {
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("경비병 : 그 던전은 모험가 님에겐 너무 위험하군요.");
                Console.ResetColor();
                Thread.Sleep(2000);
                DisplayDungeonEntrance();
            }
        }
        static void DungeonAnimation()  //던전 진행 애니메이션
        {
            for (int i = 0; i < 2; i++)    
            {
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("던전 진행 중...");
                Thread.Sleep(500);
                Console.Clear();
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("던전 진행 중..");
                Thread.Sleep(500);
                Console.Clear();
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("던전 진행 중.");
                Thread.Sleep(500);
                Console.Clear();
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("던전 진행 중");
                Thread.Sleep(500);
                Console.Clear();
            }
        }

        public class GameManager
        {
            public static void SaveGame(Player player)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Player));
                using (TextWriter writer = new StreamWriter("savegame.xml"))
                {
                    serializer.Serialize(writer, player);
                }
                Console.WriteLine("게임이 성공적으로 저장되었습니다.");
            }

            public static Player LoadGame()
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Player));
                using (TextReader reader = new StreamReader("savegame.xml"))
                {
                    return (Player)serializer.Deserialize(reader);
                }
            }
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
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop+2);  // 가운데 정렬하는 방법
            Console.WriteLine(text);
            Console.WriteLine("");
            Console.WriteLine("");
        } 
    }
}