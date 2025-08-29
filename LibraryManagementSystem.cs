using System;
using System.Collections.Generic;
using System.IO;

public class LibraryApp
{
    // book class
    class Book
    {
     public int Id {get; set;}
     public string Name {get; set;}
     public string Author {get; set;}
     public bool IsAvailable {get; set;} =true;
     public string BorrowedBy {get; set;} =null;
     public DateTime? BorrowDate {get; set;}
     public DateTime? DueDate {get; set;}
    }
    
    // Book List
    static List<Book> books = new List<Book>()
    {
        new Book {Id= 1, Name = "C# in Depth",Author ="Skeet"},
        new Book {Id= 2, Name = "ASP.NET Core Guide",Author ="Adam"},
        new Book {Id= 3, Name = "Clean Code",Author="Robert C"},
        new Book {Id= 4, Name = "EF Core" , Author="Jon "},
        new Book {Id= 5, Name = "Design Patten", Author="Erich Gamma"}
    };
    
    // Main Method
    public static void Main(string[] args)
    {
        bool repeat = true;
        string username="";
        string password;
        bool useractive =false;
    
//================== Login Console =============================
     while(repeat)
     {
     
        while(!useractive)
        {
          Console.WriteLine("=== # Login ===");
          Console.Write("UserName : ");
          username = Console.ReadLine();
          Console.Write("Password : ");
          password =Console.ReadLine();
        
        if(username =="User" && password =="1234")
        {
            useractive =true;
            Console.WriteLine("Login Successful !");
        }
        else
        {
            Console.WriteLine("Invaild Credentials, try Again. \n");
        }
        }
        
        // Login ---> Menu  
//======================= Menu ===================================
        int choice;
        do
        {
        Console.WriteLine ("\n===== # Library Management System =====");
        Console.WriteLine($"Hello {username}!");
        Console.ResetColor();
        Console.WriteLine("1. View Books");
        Console.WriteLine("2. Search Book");
        Console.WriteLine("3. Add Book");
        Console.WriteLine("4. Remove Book");
        Console.WriteLine("5. Borrow Book");
        Console.WriteLine("6. Return Book");
        Console.WriteLine("7. Library Statistics");
        Console.WriteLine("8. Logout");
        Console.WriteLine("Enter choice");
        
        if(int.TryParse(Console.ReadLine(),out choice))
        {
            switch(choice)
            {
                case 1:ViewBook();break;
                case 2:SearchBook();break;
                case 3:AddBook();break;
                case 4:RemoveBook();break;
                case 5:BorrowBook();break;
                case 6:ReturnBook();break;
                case 7:ShowStatistics();break;
                case 8:Console.WriteLine("Exiting.... Goodbye !");
                useractive=false;
                break;
                default :Console.WriteLine("Invaild choice.");break;
            }
        }
        else
        {
            Console.WriteLine("Please enter a valid number !");
            choice =0;
        }
        }
        while(choice !=8);
        
//========================== Feature =============================
//===========> View All Book
        static void ViewBook()
        {
            Console.WriteLine($"\n Books in Library (Total: {books.Count}):");
            
            if(books.Count == 0)
            {
                Console.WriteLine("No Books Available.");
                return;
            }
            foreach(var book in books)
            {
              string status = book.IsAvailable ? "Available" : $"Borrowed by {book.BorrowedBy}, Due:{book.DueDate?.ToShortDateString()}";
              Console.WriteLine($"[{book.Id}] {book.Name} by {book.Author} - {status}");
            }
        }
//===============> Search Box 
        static void SearchBook()
        {
            Console.WriteLine($"\n Enter book name to search: ");
            string name = Console.ReadLine();
            
        var results = books.FindAll(b=>b.Name.Contains(name,StringComparison.OrdinalIgnoreCase));
        
            if(results.Count > 0)
            {
              Console.WriteLine("\n Search Results :");
              foreach(var book in results)
              {
                  string status = book.IsAvailable ? "Available" :
                  "Borrowed";
                  
                  Console.WriteLine($"[{book.Id}] {book.Name} by {book.Author} - {status}");   
              }
            }
            else
            {
                Console.WriteLine($"No Books found matching '{name}'");
            }
        }
//==============> Add Box 
        static void AddBook()
        {
         Console.Write("\nEnter book Name :");
         string AddBookName=Console.ReadLine();
         
         Console.Write("\nEnter Author Name :");
         string AddBookAuthor =Console.ReadLine();
         
         if(!string.IsNullOrWhiteSpace(AddBookName) && !string.IsNullOrWhiteSpace(AddBookAuthor))
         {
             int newId = books.Count > 0 ? books[^1].Id + 1 : 1;
             books.Add(new Book {Id=newId, Name=AddBookName,Author=AddBookAuthor});
             Console.WriteLine($" Book '{AddBookName}' added sucessfully With Id {newId}.");
         }
         else
         {
             Console.WriteLine("Book Name/ Author cannot be Empty !");
         }
        }
//==============> Remove Book
        static void RemoveBook()
        {
         Console.WriteLine("\nEnter Book Id to Remove");
         
         if(int.TryParse(Console.ReadLine(),out int id ))
         {
             var book = books.Find(b=>b.Id ==id);
             if(book !=null)
             {
                 books.Remove(book);
                 Console.WriteLine($"!Book '{book.Name}' Removed Successfully !");
             }
             else
             {
                 Console.WriteLine("Book not found !");
             }
         }
         else
         {
             Console.WriteLine("Invalid Id");
         }
        }
//==============> Borrow Book 
        static void BorrowBook()
        {
            Console.Write("\nEnter Book ID to Borrow :");
            
            if(int.TryParse(Console.ReadLine(),out int id))
            {
                var book =books.Find(b=>b.Id == id);
                if(book != null && book.IsAvailable)
                {
                    Console.WriteLine("Enter Borrower Name");
                    string BName =Console.ReadLine();
                    
                    book.IsAvailable =false;
                    book.BorrowedBy = BName;
                    book.BorrowDate = DateTime.Now;
                    book.DueDate = DateTime.Now.AddDays(14);
                    
                    Console.WriteLine($"You borrowed '{book.Name}'.Return by {book.DueDate?.ToShortDateString()}.");
                }
                else
                {
                    Console.WriteLine("Book not Available or invalid ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id.");
            }
        }
//=============> Return Book
        static void ReturnBook()
        {
            Console.WriteLine("\nEnter Book Id to Return: ");
            if(int.TryParse(Console.ReadLine(),out int id ))
            {
                var book = books.Find(b=>b.Id == id);
                
                if(book !=null && !book.IsAvailable)
                {
                    book.IsAvailable =true;
                    book.BorrowedBy =null;
                    book.BorrowDate=null;
                    book.DueDate=null;
                    
                    Console.WriteLine($"You Returned '{book.Name}'. Thank You!");
                }
                else
                {
                    Console.WriteLine("Book is already available or invalid Id");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id.");
            }
        }
//=============> Show Statistics
        static void ShowStatistics()
        {
            int available = books.FindAll(b=>b.IsAvailable).Count;
            int borrowed =books.Count - available;
            
            Console.WriteLine("\nLibrary Statistics:");
            Console.WriteLine($"Total Books : {books.Count}");
            Console.WriteLine($"Available   : {available}");
            Console.WriteLine($"Borrowed    : {borrowed}");
        }
    }
    }
}
