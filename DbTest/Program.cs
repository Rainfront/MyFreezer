// See https://aka.ms/new-console-template for more information

using DBTest;

using (ApplicationContext db = new ApplicationContext())
{
    // создаем два объекта User
    User tom = new User { Id=1, Login = "Tom", Password = "33" };
    User alice = new User { Login = "Alice2", Password = "50" };
 
    // добавляем их в бд
    //db.Users.Add(tom);
    //db.Users.Add(alice);
    db.Users.Add(new User(){Login="Test1", Password="qwerty1"});
    db.Users.Add(new User(){Login="Test2", Password="qwerty2"});
    db.Users.Add(new User(){Login="Test3", Password="qwerty3"});
    db.Users.Add(new User(){Login="Test4", Password="qwerty4"});
    db.Users.Add(new User(){Login="Test5", Password="qwerty5"});
    db.Users.Add(new User(){Login="Test6", Password="qwerty6"});
    db.Users.Add(new User(){Login="Test7", Password="qwerty7"});
    db.Users.Add(new User(){Login="Test8", Password="qwerty8"});
    db.Users.Add(new User(){Login="Test9", Password="qwerty9"});
    db.Users.Add(new User(){Login="Test10", Password="qwerty10"});
    db.Users.Add(new User(){Login="Test11", Password="qwerty11"});
    db.SaveChanges();
    Console.WriteLine("Объекты успешно сохранены");
 
    // получаем объекты из бд и выводим на консоль
    var users = db.Users.ToList();
    Console.WriteLine("Список объектов:");
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Login} - {u.Password}");
    }
}

/*while (true)
{
    Console.WriteLine("enter");
    

    Console.WriteLine("0 - exit, other - go");
    string choise = Console.ReadLine();
    if (choise == "0")
    {
        break;
    } 
}*/
