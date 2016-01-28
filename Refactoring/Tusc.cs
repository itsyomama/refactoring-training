using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        const int ExitApp = 7;
        public static void Start(List<User> usrs, List<Product> prods)
        {
            
            AppWelcomeMessage();

                

           
          if (LogInToApp(usrs))
           {
                  double bal = RemainingAccountBalance(usrs, _username, _password);

                  PromptForProductPurchase(usrs, prods, _username, _password, ref bal);
            }
          else
          { // Prevent console from closing
              ExitPromptNotification();
          }


            
         
        }

        private static void AppSuccessfulLoginMessage(string name)
        {
            // Show welcome message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + name + "!");
            Console.ResetColor();
        }

        private static string PromptForUserName()
        {
            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            string name = Console.ReadLine();
            return name;
        }

        private static string PromptForUserPassword()
        {
            // Prompt for user input
            Console.WriteLine("Enter Password:");
            string pwd = Console.ReadLine();
            return pwd;
        }

        private static void PromptForProductPurchase(List<User> usrs, List<Product> prods, string name, string pwd, ref double bal)
        {
            // Show product list
            while (true)
            {
                // Prompt for user input
                DisplayProductsForPurchase(prods);

                string answer;
                int purchaseQty;
                PromptToEnterProductToPurchase(out answer, out purchaseQty);

                // Check if user entered number that equals product count
                if (purchaseQty == ExitApp)
                {
                    // Update balance
                    foreach (var usr in usrs)
                    {
                        // Check that name and password match
                        if (usr.Name == name && usr.Pwd == pwd)
                        {
                            usr.Bal = bal;
                        }
                    }

                    UpdateUserAccountBalance(usrs);

                    UpdateInventoryBalance(prods);


                    ExitPromptNotification();
                    return;
                }
                else
                {
                    ShowProductsPurchased(prods, bal, purchaseQty);

                    int qty = PromptForPurchaseQuantity(ref answer);

                    // Check if balance - quantity * price is less than 0
                    if (bal - prods[purchaseQty].Price * qty < 0)
                    {
                        InsufficientFundsNotification();
                        continue;
                    }

                    // Check if quantity is less than quantity
                    if (prods[purchaseQty].Qty <= qty)
                    {
                        OutofStockNotification(prods, purchaseQty);
                        continue;
                    }

                    // Check if quantity is greater than zero
                    if (qty > 0)
                    {
                        // Balance = Balance - Price * Quantity
                        bal = bal - prods[purchaseQty].Price * qty;

                        // Quanity = Quantity - Quantity
                        prods[purchaseQty].Qty = prods[purchaseQty].Qty - qty;

                        ProductPurchasedNotification(prods, bal, purchaseQty, qty);
                    }
                    else
                    {
                        QuantityLessThanZeroNotification();
                    }
                }
            }
        }

        private static void PromptToEnterProductToPurchase(out string answer, out int purchaseQty)
        {
            // Prompt for user input
            Console.WriteLine("Enter a number:");
            answer = Console.ReadLine();
            int num = Convert.ToInt32(answer);
            purchaseQty = num - 1; /* Subtract 1 from number
                            num = num + 1 // Add 1 to number */
        }

        private static void ProductPurchasedNotification(List<Product> prods, double bal, int num, int qty)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You bought " + qty + " " + prods[num].Name);
            Console.WriteLine("Your new balance is " + bal.ToString("C"));
            Console.ResetColor();
        }

        private static Boolean IsValidLoginName(List<User> usrs,string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                for (int i = 0; i < 5; i++)
                {
                    User user = usrs[i];
                    // Check that name matches
                    if (user.Name == username)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static Boolean IsValidUserPassword(List<User> usrs, string username, string pwd)
        {

            
            for (int i = 0; i < 5; i++)
            {
                User user = usrs[i];

                // Check that name and password match
                if (user.Name == username && user.Pwd == pwd)
                {
                    return true;
                }
            }

            return false;
        }

        private static Boolean LogInToApp(List<User> usrs)
        {
            var valUsr = false;
            string pwd = "";

            string username = PromptForUserName();
            if (IsValidLoginName(usrs,username))
            { 
                valUsr = true;
                _username = username;
                pwd = PromptForUserPassword();

            }
            else
            {
                InvalidUserNotification();
                return false;
            }

            if (valUsr && IsValidUserPassword(usrs, username, pwd))
            {
                _password = pwd;
                AppSuccessfulLoginMessage(username);
                return true;
            }
            else
            {
                InvalidPasswordNotification();
                return false;
            }



            return false;
        }

        private static int PromptForPurchaseQuantity(ref string answer)
        {
            // Prompt for user input
            Console.WriteLine("Enter amount to purchase:");
            answer = Console.ReadLine();
            int qty = Convert.ToInt32(answer);
            return qty;
        }

        private static void ShowProductsPurchased(List<Product> prods, double bal, int num)
        {
            Console.WriteLine();
            Console.WriteLine("You want to buy: " + prods[num].Name);
            Console.WriteLine("Your balance is " + bal.ToString("C"));
        }

        private static void DisplayProductsForPurchase(List<Product> prods)
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");
            for (int i = 0; i < 7; i++)
            {
                Product prod = prods[i];
                Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
            }
            Console.WriteLine(prods.Count + 1 + ": Exit");
        }

        private static void ExitPromptNotification()
        {
            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static void InsufficientFundsNotification()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You do not have enough money to buy that.");
            Console.ResetColor();
        }

        private static void OutofStockNotification(List<Product> prods, int num)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Sorry, " + prods[num].Name + " is out of stock");
            Console.ResetColor();
        }

        private static void QuantityLessThanZeroNotification()
        {
            // Quantity is less than zero
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Purchase cancelled");
            Console.ResetColor();
        }

        private static void InvalidPasswordNotification()
        {
            // Invalid Password
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid password.");
            Console.ResetColor();
        }

        private static void InvalidUserNotification()
        {
            // Invalid User
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid user.");
            Console.ResetColor();
        }

        private static void UpdateInventoryBalance(List<Product> prods)
        {
            // Write out new quantities
            string json2 = JsonConvert.SerializeObject(prods, Formatting.Indented);
            File.WriteAllText(@"Data\Products.json", json2);
        }

        private static void UpdateUserAccountBalance(List<User> usrs)
        {
            // Write out new balance
            string json = JsonConvert.SerializeObject(usrs, Formatting.Indented);
            File.WriteAllText(@"Data\Users.json", json);
        }

        private static double RemainingAccountBalance(List<User> usrs, string name, string pwd)
        {
            // Show remaining balance
            double bal = 0;
            for (int i = 0; i < 5; i++)
            {
                User usr = usrs[i];

                // Check that name and password match
                if (usr.Name == name && usr.Pwd == pwd)
                {
                    bal = usr.Bal;

                    // Show balance 
                    Console.WriteLine();
                    Console.WriteLine("Your balance is " + usr.Bal.ToString("C"));
                }
            }
            return bal;
        }

        private static void AppWelcomeMessage()
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        public static string _username { get; set; }

        public static string _password { get; set; }
    }
}
