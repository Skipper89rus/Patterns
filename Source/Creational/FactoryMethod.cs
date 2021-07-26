using System;

/// <summary>
/// FactoryMethod (Фабричный метод) — это порождающий паттерн проектирования,
/// который определяет общий интерфейс для создания объектов в суперклассе,
/// позволяя подклассам изменять тип создаваемых объектов.
/// https://refactoring.guru/ru/design-patterns/factory-method
/// </summary>
namespace Patterns.Creational.FactoryMethod
{
   /// <summary>Интерфейс Продукта объявляет операции, которые должны выполнять все конкретные продукты.</summary>
   internal interface IProduct
   {
      string Operation();
   }

   internal class ConcreteProduct1 : IProduct
   {
      public string Operation() => "{Result of ConcreteProduct1}";
   }

   internal class ConcreteProduct2 : IProduct
   {
      public string Operation() => "{Result of ConcreteProduct2}";
   }

   /// <summary>
   /// Класс Создатель объявляет фабричный метод, который должен возвращать объект класса Продукт.
   /// Подклассы Создателя обычно предоставляют реализацию этого метода.
   /// </summary>
   internal abstract class ProductCreator
   {
      /// <summary>
      /// Создатель может также обеспечить реализацию фабричного метода по умолчанию.
      /// </summary>
      public abstract IProduct FactoryMethod();

      /// <summary>
      /// Несмотря на название, основная обязанность Создателя не заключается в создании продуктов.
      /// Обычно он содержит некоторую базовую бизнес-логику, которая основана на объектах Продуктов, возвращаемых фабричным методом.
      /// Подклассы могут косвенно изменять эту бизнес-логику, переопределяя фабричный метод и возвращая из него другой тип продукта.
      /// </summary>
      public string SomeOperation()
      {
         // Вызываем фабричный метод, чтобы получить объект-продукт.
         var product = FactoryMethod();
         // Далее, работаем с этим продуктом.
         return "Creator: The same creator's code has just worked with " + product.Operation();
      }
   }

   internal class ConcreteCreator1 : ProductCreator
   {
      /// <summary>
      /// Сигнатура метода по-прежнему использует тип абстрактного продукта, хотя  фактически из метода возвращается конкретный продукт.
      /// Таким образом, Создатель может оставаться независимым от конкретных классов продуктов.
      /// </summary>
      public override IProduct FactoryMethod() => new ConcreteProduct1();
   }

   internal class ConcreteCreator2 : ProductCreator
   {
      public override IProduct FactoryMethod() => new ConcreteProduct2();
   }

   internal static class TestClient
   {
      public static void DoDirt()
      {
         Console.WriteLine("App: Launched with the ConcreteCreator1.");
         clientCode(new ConcreteCreator1());
         Console.WriteLine("App: Launched with the ConcreteCreator2.");
         clientCode(new ConcreteCreator2());
      }

      /// <summary>
      /// Клиентский код работает с экземпляром конкретного создателя, хотя и через его базовый интерфейс.
      /// Пока клиент продолжает работать с создателем через базовый интерфейс, вы можете передать ему любой подкласс создателя.
      /// </summary>
      private static void clientCode(ProductCreator creator)
         => Console.WriteLine("Client: I'm not aware of the creator's class, but it still works.\n" + creator.SomeOperation());
   }
}
