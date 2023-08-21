using static ConsoleRPG.Program;

namespace ConsoleRPG
{
    internal class Program
    {
        private static Player player;
        private static Inventory inventory;
        static void Main(string[] args)
        {

            Console.WriteLine("콘솔 RPG의 세계에 오신 것을 환영합니다.");
            Console.WriteLine("Enter키를 눌러 게임에 접속할 수 있습니다.");
            Console.ReadLine();

            inventory = new Inventory(10); //최대 아이템 10개

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
            Console.WriteLine($"어서 오십시오! {player.Name}!");
            Console.WriteLine("현재 위치 : 마을");
            Console.WriteLine("");
            Console.WriteLine("마을에선 던전으로 들어가기 전에 장비를 확인할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태창");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
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
                    Console.WriteLine("상점으로 이동합니다.");
                    break;
                case 0: Console.WriteLine("게임을 종료합니다.");
                    break;
            }
        }

        static void DisplayInventory()  // 인벤토리 창
        {
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
            ItemsInInventory(inventory); // 아이템 목록 값 할당

            Console.WriteLine("[장비 관리창]");
            Console.WriteLine("장비를 관리할 수 있습니다.");
            Console.WriteLine("");
            inventory.ItemList(); // 아이템 목록 호출
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");
            Choices();
            int input = CheckInput(0, 3);
            switch (input)
            {
                case 0:
                    Console.Clear();
                    DisplayInventory();
                    break;
            }
        }

        static void DisplayPlayerInfo()  // 플레이어 상태창
        {
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

        

        static void Equipped() // 장비 장착 관리 - 아직 안씀
        {
            
        }

        static void PlayerData()
        {
            player = new Player("김전사", "전사", 10, 10, 15, 500, 1000);
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
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsEquipped { get; set; }

            public Item(string name, string description)
            {
                Name = name;
                Description = description;
            }
        }

        public class Inventory
        {
            private Item[] items;
            
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

            public void ItemList()  //아이템 목록 출력 함수
            {
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        Console.WriteLine($"{i + 1}.{items[i].Name}   |  {items[i].Description}");
                    }
                }
            }

        }
        static void ItemsInInventory(Inventory inventory)  // 인벤토리에 있는 아이템
        {
            Item sword = new Item("낡은 단검", "낡아서 이가 나간 단검이다.  |  공격력 + 1");
            Item bow = new Item("낡은 활", "금방이라도 시위가 끊어질 것 같은 활.  | 공격력 + 1");
            Item shield = new Item("나무 방패", "나무로 만든 못 미더운 방패.  |  방어력 + 1");

            inventory.AddItem(sword);
            inventory.AddItem(bow);
            inventory.AddItem(shield);
        }


        static void Choices()
        {
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동의 번호를 입력해주세요.");
            Console.Write(">> ");
        }

        
    }

    // 글자 색깔 바꾸는 코드
   // Console.ForegroundColor = ConsoleColor.Red;
}