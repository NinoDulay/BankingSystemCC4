using System;
using System.Globalization;
using System.Collections.Generic;


public class Program{
    // Create empty linked lists for bank accounts and transactions
    static LinkedList<String[]> bank_accounts = new LinkedList<String[]>();
    static LinkedList<String[]> transactions = new LinkedList<String[]>();

    // Create an empty linkedlistnode to store the logged in user.
    static LinkedListNode<String[]> current_user = null;

    // Check for username and password in the bank_accounts.
    private static void SetCurrentUser(string[] data){
        for (LinkedListNode<string[]> node = bank_accounts.First; node != null; node=node.Next){
            if (node.Value == data){
                current_user = node;
                break;
            }
        }
    }

    static bool IsAccountExisting(string username){
        int uname_index = 5;
        foreach(String[] data in bank_accounts){
            if (data[uname_index] == username || data[0] == username){
                return true;
            }
        }
        return false;
    }

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

    
    static bool Register(string bank_num, string first, string last, string address, double deposit, string username, string password){
        String[] bank_acc = {bank_num, first, last, address, deposit.ToString(), username, password};

        bank_accounts.AddLast(bank_acc);
        return true;
    }

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

    static void TransactionCreationWithdraw(string prev_bal, double amt_withdrawn){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {current_user.Value[0], "Withdraw", datestr, prev_bal, amt_withdrawn.ToString(), current_user.Value[4]};
        transactions.AddLast(trans);
    }

    static void TransactionCreationDeposit(string prev_bal, double amt_deposited){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {current_user.Value[0], "Deposit", datestr, prev_bal, amt_deposited.ToString(), current_user.Value[4]};
        transactions.AddLast(trans);
    }

    static string NumToName(string bank_num){
        string name = "";
        foreach(String[] data in bank_accounts){
            if (data[0] == bank_num){
                name = $"{data[1]} {data[2]}";
            }
        }
        return name;
    }

    static void TransactionCreationTransfer(string prev_bal, string recipient, double amt_transferred){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {current_user.Value[0], "Transfer", datestr, recipient, NumToName(recipient), prev_bal, amt_transferred.ToString(), current_user.Value[4]};
        transactions.AddLast(trans);
    }

    static void TransactionCreationReceive(string recipient, string prev_bal, string sender, double amt_transferred, double new_recipient_bal){
        string datestr = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        string[] trans = {recipient, "Received", datestr, sender, NumToName(sender), prev_bal, amt_transferred.ToString(), new_recipient_bal.ToString()};
        transactions.AddLast(trans);
    }

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

    static void LoggedInSystem(){
        string user_choice;

        while (true){
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
            user_choice = Console.ReadLine();

            if (user_choice == "1"){
                Console.WriteLine("");
                Console.WriteLine($"Current Balance: Php {current_user.Value[4]}");
                Console.WriteLine("");
            } else if (user_choice == "2"){
                string withdraw_amt;
                double withdraw_converted;
                Console.WriteLine("");

                Console.Write($"How much do you want to withdraw: ");
                withdraw_amt = Console.ReadLine();

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
            } else if (user_choice == "3"){
                string deposit_amt;
                double deposit_converted;
                Console.WriteLine("");
                Console.Write($"How much do you want to deposit: ");

                deposit_amt = Console.ReadLine();
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

            } else if (user_choice == "4"){
                double transfer_amt_converted;

                Console.Write("Enter recipient's number: ");
                string recipient = Console.ReadLine();

                if(!IsAccountExisting(recipient)){
                    Console.WriteLine("Account does not exist. Going back to menu.");
                    continue;
                }

                Console.Write("Enter transfer amount: ");
                string transfer_amt = Console.ReadLine();

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

            } else if (user_choice == "5"){
                Console.WriteLine("");
                ShowTransaction();
                Console.WriteLine("");
            } else if(user_choice == "6"){
                Console.WriteLine("Thank you for using our system!");
                Console.WriteLine("");
                break;
            }
        }
        
    }

    // Add a New Bank Account on the bank_accounts linkedlist
    
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


    public static void Main(string[] args){
        Register("1230120313012", "Nino", "Dulay", "106 Ricabo St. Zamora, Meycauayan, Bulacan", 15000, "HoaxSnowden", "Password123");
        Register("1230120313052", "Loy", "Bayhon", "Northville 3, Bayugo, Meycauayan, Bulacan", 12000, "Loyloy", "Password3!@#");

        SeeRegisteredAccounts();


        Console.WriteLine("Welcome to Group 2 Banking System!");
        while(true){
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.WriteLine();
            Console.Write(">>> ");

            string user_choice = Console.ReadLine();

            if (user_choice == "1"){
                string username;
                string password;

                while(true){
                    Console.WriteLine("\n");
                    Console.Write("Username: ");
                    username = Console.ReadLine();

                    Console.Write("Password: ");
                    password = Console.ReadLine();

                    if(IsAccountExisting(username)){
                        if(Login(username, password)){
                            Console.WriteLine("");
                            Console.WriteLine("");
                            LoggedInSystem();
                            break;
                        } else {
                            Console.WriteLine("Password is not correct.");
                        }
                    } else{
                        Console.WriteLine("Username does not exist in the database.");
                    }
                    
                }
            } else if(user_choice == "2"){
                string firstname;
                string lastname;
                string address;
                string initial_deposit;
                double initial_deposit_converted;
                string username;
                string password;
                string confirm_password;

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
                        if(!Double.TryParse(initial_deposit, out initial_deposit_converted)){
                            Console.Write("Please enter a valid amount.");
                        } else{
                            break;
                        }
                    }
                    
                    Console.Write("Username: ");
                    username = Console.ReadLine();

                    while(true){
                        Console.Write("Password: ");
                        password = Console.ReadLine();

                        Console.Write("Confirm Password: ");
                        confirm_password = Console.ReadLine();
                        if(password != confirm_password){
                            Console.WriteLine("Please Try Again.");
                        }else{
                            break;
                        }
                        
                    }

                    Console.WriteLine();
                    Console.WriteLine("Your provided information.");
                    Console.WriteLine($"Name: {firstname} {lastname}");
                    Console.WriteLine($"Address: {address}");
                    Console.WriteLine($"Initial Deposit: Php {initial_deposit} ");
                    Console.WriteLine($"Username: {username}");
                    Console.WriteLine($"Password: {password}");
                    
                    Console.Write("\nIs your Information correct (Y/N)?");
                    string yesorno = Console.ReadLine();
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
               
            } else if(user_choice == "3"){
                Console.Write("See you!");
                break;
            }

        }
    }
}