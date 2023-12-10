using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;

public class BankAccount{
    public string bank_account_num {get; set;}
    public string first {get; set;}
    public string last {get; set;}
    public string address {get; set;}
    public double balance {get; set;}
    public string password {get; set;}
}

public class Program{
    static void ReadCSVFile(string filename){
        // https://www.youtube.com/watch?v=3mjJPptpjw4
        var lines = File.ReadAllLines(filename);
        foreach (var line in lines){
            String[] data = line.Split(";");
            Console.WriteLine(data[0]);
            Console.WriteLine(data[1]);
            Console.WriteLine(data[2]);
            Console.WriteLine(data[3]);
            Console.WriteLine(data[4]);
            Console.WriteLine(data[5]);
            Console.WriteLine("-------------------------");
        }
    }

    static void Login(string username, string password){

    }

    static void Register(string bank_num, string first, string last, string address, double deposit, string password){
        string line = $"{bank_num};{first};{last};{address};{deposit};{password};\n";
        File.AppendAllText("users.csv", line);
    }


    public static void Main(string[] args){
        // ReadCSVFile("users.csv");

        // Register("1230120313012", "Nino", "Dulay", "106 Ricabo St. Zamora, Meycauayan, Bulacan", 12312312, "Password123");
        // Register("1230120313052", "Loy", "Bayhon", "Northville 3, Bayugo, Meycauayan, Bulacan", 12000, "Password3!@#");
        // var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        // {
        //     HasHeaderRecord = false,
        //     Delimiter = ";",
        // };

        // using var streamReader = File.OpenText("users.csv");
        // using var csvReader = new CsvReader(streamReader, csvConfig);

        // string value;

        // while (csvReader.Read())
        // {
        //     Console.WriteLine("------------------------");
        //     for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
        //     {
        //         Console.WriteLine($"{value}");
        //     }
        //     Console.WriteLine("------------------------");
        // }


        // Comma Separated Values



        
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
                }
            } else if(user_choice == "2"){
                string firstname;
                string lastname;
                string address;
                string initial_deposit;
                double initial_deposit_converted;
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
                    Console.WriteLine($"Password: {password}");
                    
                    Console.Write("\nIs your Information correct (Y/N)?");
                    string yesorno = Console.ReadLine();
                    if(yesorno.ToUpper() == "Y"){
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Console.WriteLine($"\n\nYour Bank Account Number: {date}");
                        Console.WriteLine("Registration Successful.\n\n");
                        Register(date, firstname, lastname, address, initial_deposit_converted, password);
                        break;
                    } else if(yesorno.ToUpper() == "N"){
                        continue;
                    }
                }
               
            }

        }



    }
}


// 