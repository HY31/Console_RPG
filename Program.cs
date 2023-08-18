namespace ConsoleRPG
{
    internal class Program
    {
        private static Player player;
        static void Main(string[] args)
        {

            Console.WriteLine("콘솔 RPG의 세계에 오신 것을 환영합니다.");
            Console.WriteLine("Enter키를 눌러 게임에 접속할 수 있습니다.");
            Console.ReadLine();


            //메서드만 갖다 놓기
            PlayerData();
            ViliageScene();
        }

        static void ViliageScene()
        {
            Console.WriteLine("어서 오십시오! {0}!", player.Name);
            Console.WriteLine("현재 위치 : 마을");
            Console.WriteLine("");
            Console.WriteLine("마을에선 던전으로 들어가기 전에 장비를 확인할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태창");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동의 번호를 입력해주세요. \n >>");

            int input = CheckInput(1, 3);
            switch(input)
            {
                case 1: Console.WriteLine("상태창으로 이동합니다");
                    DisplayPlayerInfo();
                    break;
                case 2: Console.WriteLine("인벤토리로 이동합니다.");
                    DisplayInventory();
                    break;
                case 3: Console.WriteLine("상점으로 이동합니다.");
                    break;
            }
        }

        static int CheckInput(int min, int max)
        {
            while(true)
            {
                string input = Console.ReadLine();

                bool Check = int.TryParse(input, out int checkVal);
                if(Check)
                {
                    if(checkVal >= min && checkVal <= max) return checkVal;
                }
                Console.WriteLine("잘못된 입력입니다. 선택지의 번호를 입력해주세요.");
            }
        }

        static void DisplayPlayerInfo()
        {
            Console.WriteLine("");
            Console.WriteLine("플레이어의 정보를 확인합니다.");
            Console.WriteLine("");
            Console.WriteLine("이름 : {0}", player.Name);
            Console.WriteLine("");
            Console.WriteLine("직업 : {0}", player.Job);
            Console.WriteLine("");
            Console.WriteLine("레벨 : {0}", player.Level);
            Console.WriteLine("체력 : {0}", player.HP);
            Console.WriteLine("공격력 : {0}", player.StatATK);
            Console.WriteLine("방어력 : {0}", player.StatDEF);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("보유 골드 : {0}", player.Gold);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            int input = CheckInput(0, 0);
            switch(input)
            {
                case 0: GameStartScene();
                    break;
            }
        }

        static void DisplayInventory()
        {
            Console.WriteLine("");
            Console.WriteLine("{0}의 인벤토리",player.Name);
            Console.WriteLine("보유중인 아이템들을 관리할 수 있습니다.");

        }

        static void PlayerData()
        {
            player = new Player("김전사", "전사", 10, 10, 15, 500, 1000);
        }

        public class Player
        {
            public string Name { get; }
            public string Job { get; }
            public int Level { get; }
            public int StatDEF { get; }
            public int StatATK { get; }
            public int Gold { get; }
            public int HP { get; }

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

        public class Item
        {
            public string Sword { get; }
        }
    }

    // 글자 색깔 바꾸는 코드
   // Console.ForegroundColor = ConsoleColor.Red;
}