
// SUMMARY 

// The Sweet and Salty Console Application prints numbers to the console screen starting at a number specified by the user and stopping at a number specified by the user.  

// The range of the two numbers is limited to 1000.  

// The quantity of numbers printed per line is also decided by the user with a maximum of 30. 

// The application will print “Sweet” instead of any number that is a multiple of 3. 

// The application will print “Salty” instead of any number that is a multiple of 5. 

// The application will print “Sweet’nSalty” instead of any number that is a multiple of both 3 and 5. 

// The application prints a summary after completing the printout. 


// using variables to store user decisions
int firstNum = askForNum("one"); 
int secondNum = askForNum("two"); 

// ensure first num is less that second num
while(firstNum > secondNum){
    Console.WriteLine("Please ensure firstNum is less that second num!"); 
    firstNum = askForNum("one"); 
    secondNum = askForNum("two"); 
}

// Using math abs to find the absolute difference between two numbers; 
// Re prompting user to choose a number if difference is bigger than 1000
if(Math.Abs(firstNum - secondNum) > 1000){
    Console.WriteLine("Please choose a number that has a difference less than or equal to 1000");
    firstNum = askForNum("one");
    secondNum = askForNum("two");  
} else if(firstNum > secondNum) {
    Console.WriteLine("Please ensure numbers are in the correct order. num1 < num2.");
} else {
    int amountOfNumsToPrint = askForNumOfPrintLines(); 
    printSweetSalty(firstNum, secondNum, amountOfNumsToPrint); 


}


// method used for checking user input and verifying that user inputs a number
static int askForNum(string numberStr){
    Console.WriteLine("Please enter number " + numberStr + "."); 
    string? numStr = Console.ReadLine(); 
    int num = int.MaxValue; 
    bool containsNum = int.TryParse(numStr, out num); 

    while(!containsNum ){
        Console.WriteLine("Please enter a number."); 
        numStr = Console.ReadLine(); 
        containsNum = int.TryParse(numStr, out num); 
    }
    return num; 
    
   
}

// method used for asking and verifying that user inputs a number between 5 and 30 
static int askForNumOfPrintLines(){
    Console.WriteLine("Please enter the quantity of numbers to print between 5 (inclusive) and 30 (inclusive)."); 
    string? amountStr = Console.ReadLine(); 
    int amount = -1; 
    bool containsAmount = int.TryParse(amountStr, out amount); 
    while(!containsAmount || amount < 5 || amount > 30){
        Console.WriteLine("Please enter the quantity of numbers to print between 5 (inclusive) and 30 (inclusive)."); 
        amountStr = Console.ReadLine(); 
        containsAmount = int.TryParse(amountStr, out amount); 
    }

    return amount; 
}

// method used for printing sweet, salty or sweetandsalty
// list is used to store sweet,salty etc.. until amount to print is reached
// prints the amount of sweet salty and sweetSalty at the end
static void printSweetSalty(int numOne, int numtwo, int amountOnLine){
    int countSalty = 0;
    int countSweet = 0; 
    int countSweetAndSalty = 0; 

    List<int> salty = new List<int>();
    List<int> sweet = new List<int>();
    List<int> sweetAndSalty = new List<int>(); 

    int counterGroup = 1; 
    for(int i = numOne; i <= numtwo; i++){
        if(i % 3 == 0 && i % 5 == 0){
           sweetAndSalty.Add(i); 
           countSweetAndSalty++; 
        } else if( i % 3 == 0){
            sweet.Add(i); 
            countSweet++; 
        } else if( i % 5 == 0){
            salty.Add(i); 
            countSalty++; 
        }

        if(salty.Count + sweet.Count + sweetAndSalty.Count == amountOnLine){

            Console.WriteLine("------------------ group" + counterGroup + "------------------");
            printListOneLine("Salty: ", salty); 
            printListOneLine("Sweet: " , sweet); 
            printListOneLine("SweetAndSalty: " , sweetAndSalty); 
            Console.WriteLine("------------------ group" + counterGroup + "------------------");
            Console.WriteLine(); 
            counterGroup++; 
            salty = new List<int>();
            sweet = new List<int>();
            sweetAndSalty = new List<int>(); 

        }
    }
    Console.WriteLine("Total sweet: "+ countSweet); 
    Console.WriteLine("Total salty: " + countSalty); 
    Console.WriteLine("Total sweet and salty : " + countSweetAndSalty); 
    

}


// method used for printing a list of sweet, salty or sweet and salty on one line 
static void printListOneLine(string prefix, List<int> list){
    Console.Write(prefix + " "); 
    foreach(int num in list){
        Console.Write(num); 
        Console.Write(" "); 
    }
    Console.WriteLine(); 
}