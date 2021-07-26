using System;
using System.Collections.Generic;

/// <summary>Builder (Строитель) — это порождающий паттерн проектирования,
/// который позволяет создавать сложные объекты пошагово.
/// Строитель даёт возможность использовать один и тот же код строительства для получения разных представлений объектов.
/// https://refactoring.guru/ru/design-patterns/builder
/// </summary>
namespace Patterns.Creational.Builder
{
   /// <summary>
   /// Интерфейс Строителя объявляет создающие методы для различных частей объектов Продуктов.
   /// </summary>
   internal interface IBuilder
   {
      void BuildPartA();
      void BuildPartB();
      void BuildPartC();
   }

   /// <summary>
   /// Имеет смысл использовать паттерн Строитель только тогда, когда ваши продукты достаточно сложны и требуют обширной конфигурации.
   /// В отличие от других порождающих паттернов, различные конкретные строители могут производить несвязанные продукты.
   /// Другими словами, результаты различных строителей могут не всегда следовать одному и тому же интерфейсу.
   /// </summary>
   internal class Product
   {
      private readonly List<object> _parts = new List<object>();

      public void Add(string part) => _parts.Add(part);

      public string ListParts() => $"Product parts: {String.Join(", ", _parts)}\n";
   }

   /// <summary>
   /// Классы Конкретного Строителя следуют интерфейсу Строителя и предоставляют конкретные реализации шагов построения.
   /// Ваша программа может иметь несколько вариантов Строителей, реализованных по-разному.
   /// </summary>
   internal class ConcreteBuilder : IBuilder
   {
      private Product _product = new Product();

      // Новый экземпляр строителя должен содержать пустой объект продукта, который используется в дальнейшей сборке.
      public ConcreteBuilder()
      {
         Reset();
      }

      public void Reset() => _product = new Product();

      // Все этапы производства работают с одним и тем же экземпляром продукта.
      public void BuildPartA() => _product.Add("PartA1");
      public void BuildPartB() => _product.Add("PartB1");
      public void BuildPartC() => _product.Add("PartC1");

      // Конкретные Строители должны предоставить свои собственные методы получения результатов.
      // Это связано с тем, что различные типы строителей могут создавать совершенно разные продукты с разными интерфейсами.
      // Поэтому такие методы не могут быть объявлены в базовом интерфейсе Строителя (по крайней мере, в статически типизированном языке программирования).
      //
      // Как правило, после возвращения конечного результата клиенту, экземпляр строителя должен быть готов к началу производства следующего продукта.
      // Поэтому обычной практикой является вызов метода сброса в конце тела метода GetProduct.
      // Однако такое поведение не является обязательным, вы можете заставить своих строителей ждать явного запроса на сброс из кода клиента,
      // прежде чем избавиться от предыдущего результата.
      public Product GetProduct()
      {
         Product result = _product;
         Reset();
         return result;
      }
   }

   /// <summary>
   /// Директор отвечает только за выполнение шагов построения в определённой последовательности.
   /// Это полезно при производстве продуктов в определённом порядке или особой конфигурации.
   /// Строго говоря, класс Директор необязателен, так как клиент может напрямую управлять строителями.
   /// </summary>
   internal class Director
   {
      private IBuilder _builder;

      public IBuilder Builder
      {
         set { _builder = value; }
      }

      // Директор может строить несколько вариаций продукта, используя одинаковые шаги построения.
      public void BuildMinimalViableProduct()
      {
         _builder.BuildPartA();
      }

      public void BuildFullFeaturedProduct()
      {
         _builder.BuildPartA();
         _builder.BuildPartB();
         _builder.BuildPartC();
      }
   }

   internal static class TestClient
   {
      public static void DoDirt()
      {
         // Клиентский код создаёт объект-строитель, передаёт его директору, а затем инициирует процесс построения.
         // Конечный результат извлекается из объекта-строителя.
         var director = new Director();
         var builder = new ConcreteBuilder();
         director.Builder = builder;

         Console.WriteLine("Standard basic product:");
         director.BuildMinimalViableProduct();
         Console.WriteLine(builder.GetProduct().ListParts());

         Console.WriteLine("Standard full featured product:");
         director.BuildFullFeaturedProduct();
         Console.WriteLine(builder.GetProduct().ListParts());

         // Помните, что паттерн Строитель можно использовать без класса Директор.
         Console.WriteLine("Custom product:");
         builder.BuildPartA();
         builder.BuildPartC();
         Console.Write(builder.GetProduct().ListParts());
      }
   }
}
