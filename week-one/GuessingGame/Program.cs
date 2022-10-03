/* 
Guessing Game 
Given a random pos integer, we'll prompt user to guess the number
1) let users know the distance between their num and random num
2) higher vs lower 
3) limit guesses 
4) notify if the user got it right
5) users can also restart the game
6) keep track of hiscore 
*/

Random dice = new Random();
int rndNum = dice.Next(0,10);

int? userGuess = null;


static void isClose(int target, int? guess) {
    if (guess > target) {
        if ( guess - target <= 2){
            Console.WriteLine("hot");
        }
    } else if (target > guess){
        if ( target - guess <= 2){
            Console.WriteLine("hot");
        }
    }
}

while(userGuess != rndNum){
    try {
        Console.WriteLine("Please guess a number between 1 and 10.");
        string input = Console.ReadLine();
        userGuess = int.Parse(input);
        isClose(rndNum, userGuess);

    } catch (Exception e){
        Console.WriteLine("Please make sure to type a number!");
    }
}

