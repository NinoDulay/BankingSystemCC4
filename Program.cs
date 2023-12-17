using System;
using System.Globalization;
using System.Collections.Generic;


public class Program{
    // Create empty linked lists for bank accounts and transactions
    static LinkedList<String[]> bank_accounts = new LinkedList<String[]>();
    static LinkedList<String[]> transactions = new LinkedList<String[]>();

    // Create an empty linkedlistnode to store the logged in user.
    static LinkedListNode<String[]> current_user = null;

    // Set the current logged in user
    private static void SetCurrentUser(string[] data){
        for (LinkedListNode<string[]> node = bank_accounts.First; node != null; node=node.Next){
            if (node.Value == data){
                current_user = node;
                break;
            }
        }
    }
    //    0        1            2       3         4         5        6
    // [bank_num, firstname, lastname, address, balance, username, password]
    // A function that checks if an account exists
    static bool IsAccountExisting(string username){
        foreach(String[] data in bank_accounts){
            if (data[5] == username || data[0] == username){
                return true;
            }
        }
        return false;
    }

    // Will return true if the username and password is valid, then set the current logged in user.
    static bool Login(string username, string password){ 
        int uname_index = 5;
        int pword_index = 6;

        foreach(String[] data in bank_accounts){
            if (data[uname_index] == username){
                if (data[pword_index] == password){
                    SetCurrentUser(data);
                    Console.WriteLine($"Login Success!");
                    return true;
                } 
            }
        }
        return false;
    }

    
    // Add a new user in the linkedlist 'bank_accounts' as a string of array
    static bool Register(string bank_num, string first, string last, string address, double deposit, string username, string password){
        if (IsAccountExisting(username)){
            return false;
        }
        else{
            String[] bank_acc = {bank_num, first, last, address, deposit.ToString(), username, password};
            bank_accounts.AddLast(bank_acc);
            return true;
        }
    }

    // Transfer fund through their bank account and add a transaction record
    static bool TransferFunds(string sender, string recipient, double transfer_amt){
        for (LinkedListNode<string[]> node = bank_accounts.First; node != null; node=node.Next){
            if (node.Value[0] == recipient){
                string prev_bal = node.Value[4];
                node.Value[4] = (Double.Parse(prev_bal) + transfer_amt).ToString();
                TransactionCreationReceive(recipient, prev_bal, sender, transfer_amt, Double.Parse(node.Value[4]));
                return true;
            }
        }

        return false;
    }

    // Add a transaction for withdrawal that requires previous balance and amount withdrawn
    static void TransactionCreationWithdraw(string prev_bal, double amt_withdrawn){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {current_user.Value[0], "Withdraw", datestr, prev_bal, amt_withdrawn.ToString(), current_user.Value[4]};
        transactions.AddFirst(trans);

        //    0        1            2       3         4         5        6
        // [bank_num, firstname, lastname, address, balance, username, password]
    }

    // Add a transaction for deposit that requires previous balance and amount withdrawn
    static void TransactionCreationDeposit(string prev_bal, double amt_deposited){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {current_user.Value[0], "Deposit", datestr, prev_bal, amt_deposited.ToString(), current_user.Value[4]};
        transactions.AddFirst(trans);
    }

    // Convert bank account number to their real name (similar to search account)
    static string NumToName(string bank_num){
        string name = "";
        foreach(String[] data in bank_accounts){
            if (data[0] == bank_num){
                name = $"{data[1]} {data[2]}";
            }
        }
        return name;

        //    0         1            2       3         4         5        6
        // [bank_num, firstname, lastname, address, balance, username, password]
    }


    // Add a transaction for send transfer that requires previous balance, recipient, and amount withdrawn
    static void TransactionCreationTransfer(string prev_bal, string recipient, double amt_transferred){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {current_user.Value[0], "Transfer", datestr, recipient, NumToName(recipient), prev_bal, amt_transferred.ToString(), current_user.Value[4]};
        transactions.AddFirst(trans);
    }

     // Add a transaction for receive transfer that requiresr recipient, previous balance, sender, and new balance of the recipient
    static void TransactionCreationReceive(string recipient, string prev_bal, string sender, double amt_transferred, double new_recipient_bal){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {recipient, "Received", datestr, sender, NumToName(sender), prev_bal, amt_transferred.ToString(), new_recipient_bal.ToString()};
        transactions.AddFirst(trans);
    }

    // Just print all transaction from the 'transactions' linkedlist
    static void ShowTransaction(){
        Console.WriteLine($"Bank Account: {current_user.Value[0]}");
        Console.WriteLine("");
        foreach(String[] data in transactions){
            if (data[0] == current_user.Value[0]){
                Console.WriteLine($"---------------------");
                Console.WriteLine($"{data[1]} - {data[2]}");
                Console.WriteLine($"---------------------");
                
                if(data[1] == "Withdraw"){
                    Console.WriteLine($"Prev Balance: {data[3]}");
                    Console.WriteLine($"Withdraw Amt: {data[4]}");
                    Console.WriteLine($"Current Balance: {data[5]}");
                } else if(data[1] == "Deposit"){
                    Console.WriteLine($"Prev Balance: {data[3]}");
                    Console.WriteLine($"Deposit Amt: {data[4]}");
                    Console.WriteLine($"Current Balance: {data[5]}");
                } else if(data[1] == "Transfer"){
                    Console.WriteLine($"Recipient Account: {data[3]}");
                    Console.WriteLine($"Recipient Name: {data[4]}");
                    Console.WriteLine($"Prev Balance: {data[5]}");
                    Console.WriteLine($"Transfer Amt: {data[6]}");
                    Console.WriteLine($"Current Balance: {data[7]}");
                } else if(data[1] == "Received"){
                    Console.WriteLine($"Sender Account: {data[3]}");
                    Console.WriteLine($"Sender Name: {data[4]}");
                    Console.WriteLine($"Prev Balance: {data[5]}");
                    Console.WriteLine($"Transfer Amt: {data[6]}");
                    Console.WriteLine($"Current Balance: {data[7]}");
                }
                
                Console.WriteLine($"---------------------");
                Console.WriteLine($"");
            }
        }
    }

    // Just print all the registered accounts for testing purposes
    static void SeeRegisteredAccounts(){
        foreach(String[] data in bank_accounts){
            Console.WriteLine($"-------------------------");
            Console.WriteLine($"ACC NUM: {data[0]}");
            Console.WriteLine($"-------------------------");
            Console.WriteLine($"First: {data[1]}");
            Console.WriteLine($"Last: {data[2]}");
            Console.WriteLine($"Address: {data[3]}");
            Console.WriteLine($"Balance: {data[4]}");
            Console.WriteLine($"Username: {data[5]}");
            Console.WriteLine($"Password: {data[6]}");
            Console.WriteLine();
        }
    }


    // Will activate once the user has logged in
    static void LoggedInSystem(){
        string user_choice;

        while (true){
            // Main Menu
            Console.WriteLine($"Good day, {current_user.Value[1]}!");
            Console.WriteLine("What do you want to do:");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Deposit");
            Console.WriteLine("4. Transfer Funds");
            Console.WriteLine("5. See Transactions");
            Console.WriteLine("6. Logout");
            Console.WriteLine("");
            Console.Write(">>> ");
            user_choice = Console.ReadLine(); // User input

            // Check Balance
            if (user_choice == "1"){
                Console.WriteLine("");
                Console.WriteLine($"Current Balance: Php {current_user.Value[4]}");
                Console.WriteLine("");
            } 
            // Withdraw
            else if (user_choice == "2"){
                string withdraw_amt;
                double withdraw_converted;
                Console.WriteLine("");

                Console.Write($"How much do you want to withdraw: ");
                withdraw_amt = Console.ReadLine();

                // Check if withdrawal amount is valid (not a string, or less than the balance)
                if(!Double.TryParse(withdraw_amt, out withdraw_converted)){
                    Console.WriteLine("Please enter a valid amount.");
                } else{
                    if (withdraw_converted < 0 || withdraw_converted > Double.Parse(current_user.Value[4])){
                        Console.WriteLine("Invalid Value, please try again.");
                    } else {
                        Console.WriteLine($"Successfully withdrawn Php {withdraw_amt}");
                        string prev_bal = current_user.Value[4];
                        current_user.Value[4] =  (Double.Parse(current_user.Value[4]) - withdraw_converted).ToString();
                        TransactionCreationWithdraw(prev_bal, withdraw_converted);
                    } 
                }
                Console.WriteLine("");
            } 
            // Deposit
            else if (user_choice == "3"){
                string deposit_amt;
                double deposit_converted;
                Console.WriteLine("");
                Console.Write($"How much do you want to deposit: ");

                deposit_amt = Console.ReadLine(); // User input

                // Check if deposit amount is valid (not a string, or less than the balance)
                if(!Double.TryParse(deposit_amt, out deposit_converted)){
                    Console.Write("Please enter a valid amount.");
                } else{
                    if (deposit_converted <= 0){
                        Console.WriteLine("Invalid Value, please try again.");
                    } else {
                        Console.WriteLine($"Successfully deposited Php {deposit_converted}");
                        string prev_bal = current_user.Value[4];
                        current_user.Value[4] =  (Double.Parse(current_user.Value[4]) + deposit_converted).ToString();
                        TransactionCreationDeposit(prev_bal, deposit_converted);
                    } 
                }
                Console.WriteLine("");

            } 
            // Transfer funds through bank account number
            else if (user_choice == "4"){
                double transfer_amt_converted;

                Console.Write("Enter recipient's number: ");
                string recipient = Console.ReadLine(); // User Input

                // Check if recipient is existing
                if(!IsAccountExisting(recipient)){
                    Console.WriteLine("Account does not exist. Going back to menu.");
                    continue;
                }

                Console.Write("Enter transfer amount: ");
                string transfer_amt = Console.ReadLine(); // User Input

                // Check if transfer amount is valid (not a string, or less than the balance)
                if(!Double.TryParse(transfer_amt, out transfer_amt_converted)){
                    Console.WriteLine("Please enter a valid amount. Going back to menu.");
                } else{
                    if (transfer_amt_converted < 0 || transfer_amt_converted > Double.Parse(current_user.Value[4])){
                        Console.WriteLine("Invalid Value, please try again.");
                    } else {
                        Console.WriteLine($"Successfully transfered Php {transfer_amt_converted}");
                        Console.WriteLine($"Recipient: {recipient}");
                        string prev_bal = current_user.Value[4];
                        current_user.Value[4] =  (Double.Parse(current_user.Value[4]) - transfer_amt_converted).ToString();
                        TransactionCreationTransfer(prev_bal, recipient, transfer_amt_converted);
                        TransferFunds(current_user.Value[0], recipient, transfer_amt_converted);
                        Console.WriteLine($"Recipient Name: {NumToName(recipient)}\n\n");
                    }
                }
                Console.WriteLine("");

            } 
            // Show transaction
            else if (user_choice == "5"){
                Console.WriteLine("");
                ShowTransaction();
                Console.WriteLine("");
            // Log out then remove saved user
            } else if(user_choice == "6"){
                Console.WriteLine("Thank you for using our system!");
                Console.WriteLine("");
                current_user = null;
                break;
            }
        }
        
    }

    public static void Main(string[] args){
        // Register two accounts for testing
        Register("1230120313012", "Nino", "Dulay", "106 Ricabo St. Zamora, Meycauayan, Bulacan", 15000, "HoaxSnowden", "Password123");
        Register("1230120313052", "Loy", "Bayhon", "Northville 3, Bayugo, Meycauayan, Bulacan", 12000, "Loyloy", "Password3!@#");

        Console.WriteLine("Welcome to Group 2 Banking System!");
        while(true){
            // Menu
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.WriteLine();
            Console.Write(">>> ");

            string user_choice = Console.ReadLine(); // User Input

            // Login
            if (user_choice == "1"){
                string username;
                string password = "";

                while(true){
                    Console.WriteLine("\n");
                    Console.Write("Username: ");
                    username = Console.ReadLine();


                    // Make password show **** instead of the text for added security
                    Console.Write("Password: ");
                    ConsoleKey key;
                        do
                        {
                            var keyInfo = Console.ReadKey(intercept: true);
                            key = keyInfo.Key;

                            if (key == ConsoleKey.Backspace && password.Length > 0)
                            {
                                Console.Write("\b \b");
                                password = password[0..^1];
                            }
                            else if (!char.IsControl(keyInfo.KeyChar))
                            {
                                Console.Write("*");
                                password += keyInfo.KeyChar;
                            }
                        } while (key != ConsoleKey.Enter);

                    
                    Console.WriteLine();
                    // Check if account exists
                    if(IsAccountExisting(username)){
                        // If account exists, then login and check for password
                        if(Login(username, password)){
                            Console.WriteLine("");
                            Console.WriteLine("");
                            // Go to the main program
                            LoggedInSystem();
                            break;
                        } else {
                            Console.WriteLine("Password is not correct.");
                        }
                    } else{
                        Console.WriteLine("Username does not exist in the database.");
                    }
                    
                }
            // Register
            } else if(user_choice == "2"){
                // Initial Register Information Definition
                string firstname;
                string lastname;
                string address;
                string initial_deposit;
                double initial_deposit_converted;
                string username;
                string password = "";
                string confirm_password = "";

                while(true){
                    Console.Write("First Name: ");
                    firstname = Console.ReadLine();

                    Console.Write("Last Name: ");
                    lastname = Console.ReadLine();

                    Console.Write("Address: ");
                    address = Console.ReadLine();

                    while(true){
                        Console.Write("Initial Deposit: ");
                        initial_deposit = Console.ReadLine();
                        // Check if initial deposit is valid (not a string or negative)
                        if(!Double.TryParse(initial_deposit, out initial_deposit_converted)){
                            Console.Write("Please enter a valid amount.");
                        } else{
                            if (initial_deposit_converted >= 500){
                                break;
                            } else {
                                Console.WriteLine("Please make sure that the amount is higher than or equal to Php 500.");
                            }
                        }
                    }
                    
                    Console.Write("Username: ");
                    username = Console.ReadLine();

                    // Ask for password
                    while(true){
                        
                        // Make password show **** instead of the text for added security
                        Console.Write("Password: ");
                        ConsoleKey key;
                        do
                        {
                            var keyInfo = Console.ReadKey(intercept: true);
                            key = keyInfo.Key;

                            if (key == ConsoleKey.Backspace && password.Length > 0)
                            {
                                Console.Write("\b \b");
                                password = password[0..^1];
                            }
                            else if (!char.IsControl(keyInfo.KeyChar))
                            {
                                Console.Write("*");
                                password += keyInfo.KeyChar;
                            }
                        } while (key != ConsoleKey.Enter);


                        // Make password show **** instead of the text for added security
                        Console.Write("\nConfirm Password: ");
                        ConsoleKey key2;
                        do
                        {
                            var keyInfo = Console.ReadKey(intercept: true);
                            key2 = keyInfo.Key;

                            if (key2 == ConsoleKey.Backspace && confirm_password.Length > 0)
                            {
                                Console.Write("\b \b");
                                confirm_password = confirm_password[0..^1];
                            }
                            else if (!char.IsControl(keyInfo.KeyChar))
                            {
                                Console.Write("*");
                                confirm_password += keyInfo.KeyChar;
                            }
                        } while (key2 != ConsoleKey.Enter);

                        // If passwords are not the same, then repeat login
                        if(password != confirm_password){
                            Console.WriteLine("\nPlease Try Again.");
                        }else{
                            break;
                        }
                        
                    }

                    // Confirm if the info are correct
                    Console.WriteLine();
                    Console.WriteLine("\nYour provided information.");
                    Console.WriteLine($"Name: {firstname} {lastname}");
                    Console.WriteLine($"Address: {address}");
                    Console.WriteLine($"Initial Deposit: Php {initial_deposit} ");
                    Console.WriteLine($"Username: {username}");
                    Console.WriteLine($"Password: {password}");
                    
                    Console.Write("\nIs your Information correct (Y/N)?");
                    string yesorno = Console.ReadLine();
                    // If correct, then register. User date as the bank account number
                    if(yesorno.ToUpper() == "Y"){
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if(Register(date, firstname, lastname, address, initial_deposit_converted, username, password)){
                            Console.WriteLine($"\n\nYour Bank Account Number: {date}");
                            Console.WriteLine("Registration Successful.\n\n");
                            break;
                        } else {
                            Console.WriteLine($"Username already exists in the database.");
                            continue;
                        }
                    } else if(yesorno.ToUpper() == "N"){
                        continue;
                    }
                }
            // Exit program
            } else if(user_choice == "3"){
                Console.Write("See you!");
                break;
            }

        }
    }
}