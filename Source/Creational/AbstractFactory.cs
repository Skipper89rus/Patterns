using System;

/// <summary>Abstract Factory (Абстрактная фабрика) — это порождающий паттерн проектирования,
/// который позволяет создавать семейства связанных объектов,
/// не привязываясь к конкретным классам создаваемых объектов.
/// https://refactoring.guru/ru/design-patterns/abstract-factory
/// </summary>
namespace Patterns.Creational.AbstractFactory
{
   /// <summary>
   /// Каждый отдельный продукт семейства продуктов должен иметь базовый интерфейс.
   /// Все вариации продукта должны реализовывать этот интерфейс.
   /// </summary>
   internal interface IAbstractProductA
   {
      string UsefulFunctionA();
   }

   /// <summary>
   /// Конкретные продукты создаются соответствующими Конкретными Фабриками.
   /// </summary>
   internal class ConcreteProductA1 : IAbstractProductA
   {
      public string UsefulFunctionA() => "The result of the product A1.";
   }

   internal class ConcreteProductA2 : IAbstractProductA
   {
      public string UsefulFunctionA() => "The result of the product A2.";
   }

   /// <summary>
   /// Базовый интерфейс другого продукта.
   /// Все продукты могут взаимодействовать друг с другом,
   /// но правильное взаимодействие возможно только между продуктами одной и той же конкретной вариации.
   /// </summary>
   internal interface IAbstractProductB
   {
      // Продукт B способен работать самостоятельно...
      string UsefulFunctionB();

      // ...а также взаимодействовать с Продуктами А той же вариации.

      /// <summary>
      /// Абстрактная Фабрика гарантирует, что все продукты, которые она создает,
      /// имеют одинаковую вариацию и, следовательно, совместимы.
      /// </summary>
      string AnotherUsefulFunctionB(IAbstractProductA collaborator);
   }

   // Конкретные Продукты создаются соответствующими Конкретными Фабриками.
   internal class ConcreteProductB1 : IAbstractProductB
   {
      public string UsefulFunctionB() => "The result of the product B1.";

      /// <summary>
      /// Продукт B1 может корректно работать только с Продуктом A1.
      /// Тем не менее, он принимает любой экземпляр Абстрактного Продукта А в качестве аргумента.
      /// </summary>
      public string AnotherUsefulFunctionB(IAbstractProductA collaborator)
         => $"The result of the B1 collaborating with the ({collaborator.UsefulFunctionA()})";
   }

   internal class ConcreteProductB2 : IAbstractProductB
   {
      public string UsefulFunctionB() => "The result of the product B2.";

      /// <summary>
      /// Продукт B2 может корректно работать только с Продуктом A2.
      /// Тем не менее, он принимает любой экземпляр Абстрактного Продукта А в качестве аргумента.
      /// </summary>
      public string AnotherUsefulFunctionB(IAbstractProductA collaborator)
         => $"The result of the B2 collaborating with the ({collaborator.UsefulFunctionA()})";
   }

   /// <summary>
   /// Интерфейс Абстрактной Фабрики объявляет набор методов, которые возвращают различные абстрактные продукты.
   /// Эти продукты называются семейством и связаны темой или концепцией высокого уровня.
   /// Продукты одного семейства обычно могут взаимодействовать между собой.
   /// Семейство продуктов может иметь несколько вариаций, но продукты одной вариации несовместимы с продуктами другой.
   /// </summary>
   internal interface IAbstractFactory
   {
      IAbstractProductA CreateProductA();

      IAbstractProductB CreateProductB();
   }

   /// <summary>
   /// Конкретная Фабрика производит семейство продуктов одной вариации.
   /// Фабрика гарантирует совместимость полученных продуктов.
   /// Обратите внимание, что сигнатуры методов Конкретной Фабрики возвращают абстрактный продукт,
   /// в то время как внутри метода создается экземпляр конкретного продукта.
   /// </summary>
   internal class ConcreteFactory1 : IAbstractFactory
   {
      public IAbstractProductA CreateProductA() => new ConcreteProductA1();
      public IAbstractProductB CreateProductB() => new ConcreteProductB1();
   }

   // Каждая Конкретная Фабрика имеет соответствующую вариацию продукта.
   internal class ConcreteFactory2 : IAbstractFactory
   {
      public IAbstractProductA CreateProductA() => new ConcreteProductA2();
      public IAbstractProductB CreateProductB() => new ConcreteProductB2();
   }

   internal static class TestClient
   {
      public static void DoDirt()
      {
         // Клиентский код может работать с любым конкретным классом фабрики.
         Console.WriteLine("Client: Testing client code with the first factory type...");
         clientCode(new ConcreteFactory1());
         Console.WriteLine();

         Console.WriteLine("Client: Testing the same client code with the second factory type...");
         clientCode(new ConcreteFactory2());
      }

      /// <summary>
      /// Клиентский код работает с фабриками и продуктами только через абстрактные типы: Абстрактная Фабрика и Абстрактный Продукт.
      /// Это позволяет передавать любой подкласс фабрики или продукта клиентскому коду, не нарушая его.
      /// </summary>
      private static void clientCode(IAbstractFactory factory)
      {
         var productA = factory.CreateProductA();
         var productB = factory.CreateProductB();

         Console.WriteLine(productB.UsefulFunctionB());
         Console.WriteLine(productB.AnotherUsefulFunctionB(productA));
      }
   }
}
