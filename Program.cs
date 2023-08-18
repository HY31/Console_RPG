namespace ConsoleRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //메서드만 갖다 놓기
            GameStartScene();
        }


        static void GameStartScene()
        {
            Console.WriteLine("콘솔 RPG의 세계에 오신 것을 환영합니다.");
            Console.WriteLine("Enter키를 눌러 게임에 접속할 수 있습니다.");
            Console.ReadLine();

            Console.WriteLine("현재 위치 : 마을");
            Console.WriteLine("");
            Console.WriteLine("던전으로 들어가기 전에 장비를 확인할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태창");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동의 번호를 입력해주세요. \n >>");

            int input = CheckInput(1, 2);
            switch(input)
            {
                case 1: Console.WriteLine("상태창으로 이동합니다");
                    break;
                case 2: Console.WriteLine("인벤토리로 이동합니다.");
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


        
    }

    // 글자 색깔 바꾸는 코드
   // Console.ForegroundColor = ConsoleColor.Red;
}