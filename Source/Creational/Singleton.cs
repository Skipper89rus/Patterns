using System;
using System.Threading;

/// <summary>Singleton (Одиночка) — это порождающий паттерн проектирования,
/// который гарантирует, что у класса есть только один экземпляр, и предоставляет к нему глобальную точку доступа.
/// https://refactoring.guru/ru/design-patterns/singleton
/// </summary>
namespace Patterns.Creational.Singleton
{
   /// <summary>
   /// Класс Одиночка предоставляет Instance,
   /// который ведёт себя как альтернативный конструктор
   /// и позволяет клиентам получать один и тот же экземпляр класса при каждом вызове.
   ///
   /// Эта реализация Одиночки называется "блокировка с двойной проверкой" (double check lock).
   /// Она безопасна в многопоточной среде, а также позволяет отложенную инициализацию объекта Одиночки.
   /// </summary>
   internal class Singleton
   {
      // Конструктор Одиночки всегда должен быть скрытым, чтобы предотвратить создание объекта через оператор new.
      private Singleton() { }

      // Объект одиночки храниться в статичном поле класса.
      private static Singleton _instance;

      // Объект-блокировка для синхронизации потоков во время первого доступа к Одиночке.
      private static readonly object _lock = new object();

      // Это статический метод, управляющий доступом к экземпляру одиночки.
      // При первом запуске, он создаёт экземпляр одиночки и помещает его в статическое поле.
      // При последующих запусках, он возвращает клиенту объект, хранящийся в статическом поле.
      public static Singleton GetInst(string val)
      {
         // Это условие нужно для того, чтобы не стопорить потоки блокировкой после того как объект-одиночка уже создан.
         if (_instance == null)
         {
            // Теперь представьте, что программа была только-только запущена.
            // Объекта-одиночки ещё никто не создавал, поэтому несколько потоков вполне могли одновременно пройти через предыдущее условие и достигнуть блокировки.
            // Самый быстрый поток поставит блокировку и двинется внутрь секции, пока другие будут здесь его ожидать.
            lock (_lock)
            {
               // Первый поток достигает этого условия и проходит внутрь, создавая объект-одиночку.
               // Как только этот поток покинет секцию и освободит блокировку, следующий поток может снова установить блокировку и зайти внутрь.
               // Однако теперь экземпляр одиночки уже будет создан и поток не сможет пройти через это условие, а значит новый объект не будет создан.
               if (_instance == null)
                  _instance = new Singleton() { Val = val };
            }
         }
         return _instance;
      }

      // Мы используем это поле, чтобы доказать, что наш Одиночка действительно работает.
      public string Val { get; set; }

      // Любой одиночка должен содержать некоторую бизнес-логику, которая может быть выполнена на его экземпляре.
      public static void SomeBusinessLogic(string val)
      {
         Console.WriteLine($"Holy shit! This is {GetInst(val).Val}!");
      }
   }

   internal static class TestClient
   {
      public static void DoDirt()
      {
         Thread process1 = new Thread(() => Singleton.SomeBusinessLogic("Proc 1"));
         Thread process2 = new Thread(() => Singleton.SomeBusinessLogic("Proc 2"));

         process1.Start();
         process2.Start();

         process1.Join();
         process2.Join();

         Singleton s1 = Singleton.GetInst("Proc 3");
         Singleton s2 = Singleton.GetInst("Proc 4");
         if (s1 == s2)
            Console.WriteLine($"Singleton works, both variables contain the same instance.");
         else
            Console.WriteLine($"Singleton failed, variables contain different instances.");
         Console.WriteLine($"Val 1 = {s1.Val}");
         Console.WriteLine($"Val 2 = {s2.Val}");
      }
   }
}
