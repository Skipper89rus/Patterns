using System;

/// <summary>
/// Prototype (Прототип) — это порождающий паттерн, который позволяет копировать объекты любой сложности без привязки к их конкретным классам.
/// https://refactoring.guru/ru/design-patterns/prototype
/// https://docs.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone
/// </summary>
namespace Patterns.Creational.Prototype
{
   internal class IdInfo
   {
      public IdInfo(int id)
      {
         Id = id;
      }

      public int Id;
   }

   internal class Person
   {
      public IdInfo   IdInfo;
      public string   Name;
      public DateTime Birthdate;
      public int      Age;

      /// <summary>https://en.wikipedia.org/wiki/Object_copying#Shallow_copy</summary>
      public Person ShallowCopy()
         => (Person)MemberwiseClone();

      /// <summary>https://en.wikipedia.org/wiki/Object_copying#Deep_copy</summary>
      public Person DeepCopy()
      {
         Person clone = (Person)MemberwiseClone();
         clone.IdInfo = new IdInfo(IdInfo.Id);
         clone.Name = String.Copy(Name);
         return clone;
      }

      public void Display(string title)
      {
         Console.WriteLine($"   {title}:");
         Console.WriteLine($"      ID:   {IdInfo.Id:d}", IdInfo.Id);
         Console.WriteLine($"      Name: {Name}, Birthdate: {Birthdate:MM/dd/yy}, Age: {Age}");
      }
   }

   internal static class TestClient
   {
      public static void DoDirt()
      {
         var p1 = new Person
         {
            Age = 42,
            Birthdate = Convert.ToDateTime("1977-01-01"),
            Name = "Jack Daniels",
            IdInfo = new IdInfo(666)
         };

         // Выполнить поверхностное копирование p1 и присвоить её p2.
         Person p2 = p1.ShallowCopy();
         // Сделать глубокую копию p1 и присвоить её p3.
         Person p3 = p1.DeepCopy();

         // Вывести значения p1, p2 и p3.
         Console.WriteLine("Original values of p1, p2, p3:");
         p1.Display("p1 instance values");
         p2.Display("p2 instance values");
         p3.Display("p3 instance values");

         // Изменить значение свойств p1 и отобразить значения p1, p2 и p3.
         p1.IdInfo.Id = 7878;
         p1.Name = "Frank";
         p1.Birthdate = Convert.ToDateTime("1900-01-01");
         p1.Age = 32;

         Console.WriteLine("\nValues of p1, p2 and p3 after changes to p1:");
         p1.Display("p1 instance values");
         p2.Display("p2 instance values (reference values have changed)");
         p3.Display("p3 instance values (everything was kept the same)");
      }
   }
}
